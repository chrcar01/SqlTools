# SqlTools

[![Build status](https://ci.appveyor.com/api/projects/status/vch56oy3aj2eq37k/branch/master?svg=true)](https://ci.appveyor.com/project/chrcar01/sqltools/branch/master)

## Purpose

Even if you're using NHibernate or Entity Framework, or your favorite ORM, sometimes, it's the right decision to execute inline sql in your application.  SqlTools simplifies executing inline sql by handling most of the ADO.NET code needed to perform the task.

## SqlTools is not an ORM

It executes sql, does some simple type mapping, and that's it.  It's kept simple by design.

# Examples

## Sample Database

I have a sample database I use for testing this stuff out.  Below is a diagram of [the sample database](https://github.com/chrcar01/SqlTools/blob/master/SqlTools.sql).  

![Sample Database Diagram](https://github.com/chrcar01/SqlTools/raw/master/dbdiagram.png)

## Retrieve the values of a single column as an array

### Without SqlTools

<pre>
var connString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
var sql = "select name from state";
string[] stateNames = null;
var cn = new SqlConnection(connString);
try
{
	var cmd = new SqlCommand(sql, cn);
	try
	{
		cn.Open();
		using (var reader = cmd.ExecuteReader())
		{
			var list = new List<string>();
			while (reader.Read())
			{
				list.Add(reader.GetString(0));
			}
			stateNames = list.ToArray();
		}
	}
	finally
	{
		if (cmd != null)
			cmd.Dispose();
	}
}
finally
{
	if (cn != null)
		cn.Dispose();
}
</pre>

### With SqlTools

<pre>
var stateNames = _helper.ExecuteArray<string>("select name from state");
</pre>

## Retrieve a row count in a table

### Without SqlTools


<pre>
var connString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
var sql = "select count(*) from state";
var numberOfStates = 0;
var cn = new SqlConnection(connString);
try
{
	var cmd = new SqlCommand(sql, cn);
	try
	{
		cn.Open();
		var result = cmd.ExecuteScalar();
		if (result != System.DBNull.Value)
			numberOfStates = System.Convert.ToInt32(result);
	}
	finally
	{
		if (cmd != null)
			cmd.Dispose();
	}
}
finally
{
	if (cn != null)
		cn.Dispose();
}
</pre>

### With SqlTools

<pre>
var numberOfStates = _helper.ExecuteScalar&lt;int&gt;("select count(*) from state");
</pre>

# Type Mapping

Many times I'll have a class that maps directly to a table or a view.  When I don't need an ORM, mapping up the data in the database to a collection of objects is a little bit of a pain.  Let's say you have a simple class like this:

<pre>
public class State
{
	public string Code { get; set; }
	public string Abbreviation { get; set; }
	public string Name { get; set; }
	public string Display { get; set; }
	public DateTime? LastUpdated { get; set; }        
}
</pre>

and you want it populated with all of the data in the **state** table.

### Without SqlTools

<pre>
var connString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
var sql = "select [Code], [Abbreviation], [Name], [Display], [LastUpdated] from state";
IEnumerable<State> states = null;
using (var cn = new SqlConnection(connString))
using (var cmd = new SqlCommand(sql, cn))
{
	cn.Open();
	using (var reader = cmd.ExecuteReader())
	{
		var items = new List<State>();
		while (reader.Read())
		{
			var item = new State();
			if (!reader.IsDBNull(0)) item.Code = reader.GetString(0);
			if (!reader.IsDBNull(1)) item.Abbreviation = reader.GetString(1);
			if (!reader.IsDBNull(2)) item.Name = reader.GetString(2);
			if (!reader.IsDBNull(3)) item.Display = reader.GetString(3);
			if (!reader.IsDBNull(4)) item.LastUpdated = reader.GetDateTime(4);
			items.Add(item);
		}
		states = items;
	}
}
</pre>

### With SqlTools

<pre>
var states = _helper.ExecuteMultiple&lt;State&gt;("select * from state");
</pre>

# DbUtility

This class used to have methods that helped in writing sql dynamically, but on further inspection I decided that most of the methods were promoting bad code.  So, on that note, I've removed all but the Parameterize method and it's overloads because it's really useful, check it out.

## Expanding a collection of values into parameters in an IN CLAUSE

Let's say we want a list of all states except for Colorado, California, and Texas.  The sql for this might look like the following:

<pre>
select *
from state
where abbreviation not in ('CO','CA','TX')
</pre>

Now let's say that a user is allowed to pick randomly, what states to exclude.  And let's assume we want to be good developers and use a parameterized query(instead of string concatenating the values).  Using SqlTools here is how this is done.

<pre>
var excludeTheseStateAbbreviations = new string[] { "CO", "CA", "TX" };
var sql = "select * from state where abbreviation not in (@abbreviation)";
using (var cmd = new SqlCommand(sql))
{
	DbUtility.Parameterize(cmd, excludeTheseStateAbbreviations, "@abbreviation");
	var data = _helper.ExecuteDataTable(cmd);
	Assert.AreEqual(68, data.Rows.Count); //normally 71 states but we excluded 3 of them!
}
</pre>

The command that gets sent to sql server using the code above looks like this:

<pre>
exec sp_executesql 
N'select * from state where abbreviation not in (@abbreviation0,@abbreviation1,@abbreviation2)',
N'@abbreviation0 varchar(2),@abbreviation1 varchar(2),@abbreviation2 varchar(2)',
@abbreviation0='CO',@abbreviation1='CA',@abbreviation2='TX'
</pre>

## Extension Methods

When I first wrote the SqlTools code, extension methods were not available yet.  Since then I've also added extension methods that map to the same functionality of the DbUtility.Parameterize methods.  So the above example can be rewritten like this:

<pre>
var excludeTheseStateAbbreviations = new string[] { "CO", "CA", "TX" };
var sql = "select * from state where abbreviation not in (@abbreviation)";
using (var cmd = new SqlCommand(sql))
{
	cmd.AddParameters("@abbreviation", excludeTheseStateAbbreviations);
	var data = _helper.ExecuteDataTable(cmd);
	Assert.AreEqual(68, data.Rows.Count);
}
</pre>

## Dynamic Strongly Typed Queries

Let's say you want to execute a query and you want a strongly typed object.  You could create a separate model class to hold the results as shown above.  OR you could be super cool and use the new dynamic execute methods to retrieve a strongly typed result without creating a class at all.  

In the following example, I just want to get two attributes of the single customer who has an ID of 1.

<pre>
var sql = "select firstname, lastname from customer where id = 1";
dynamic customer = _helper.ExecuteDynamic(sql);
</pre>

In the above code, **customer** now has two **case-INsensitive** properties that you can use however you want, and they are strongly typed.


