# SqlTools

A collection of useful methods for executing simple, non-transactional, sql statements.  Reduces the need to worry about opening and closing connections.  Allows for simple, strongly typed, queries.  Using this library allows developers to worry about the sql and not about lots of plumbing needed to execute the sql.  Stongly typed results are also greatly simplified with automatic case-INsensitive property mapping; specifying a column or alias that matches any property on a custom type, results in the automatic type conversion and assignment of value to that property.
Test.

## Without SqlTools

## The following code creates a string array of values in the **name** column in the **state** table.

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

## With SqlTools

<pre>
var stateNames = _helper.ExecuteArray<string>("select name from state");
</pre>

## How many rows are in the state table?

## Without SqlTools


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

## With SqlTools

<pre>
var numberOfStates = _helper.ExecuteScalar<int>("select count(*) from state");
</pre>

# Complex Type Mapping

## Sample Database

<img src="https://github.com/chrcar01/SqlTools/blob/master/dbdiagram.png" alt="Sample Database Diagram" />

