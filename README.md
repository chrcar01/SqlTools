# SqlTools

## Purpose

Even if you're using NHibernate or Entity Framework, or your favorite ORM, sometimes, it's the right decision to execute inline sql in your application.  SqlTools simplifies executing inline sql by handling most of the ADO.NET code needed to perform the task.

## SqlTools is not an ORM

It executes sql, does some simple type mapping, and that's it.  It's kept pretty simple by design.

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
var numberOfStates = _helper.ExecuteScalar<int>("select count(*) from state");
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
var states = _helper.ExecuteMultiple<State>("select * from state");
</pre>

