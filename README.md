SqlTools
========

A collection of useful methods for executing simple, non-transactional, sql statements.  Reduces the need to worry about opening and closing connections.  Allows for simple, strongly typed, queries.  Using this library allows developers to worry about the sql and not about lots of plumbing needed to execute the sql.  Stongly typed results are also greatly simplified with automatic case-INsensitive property mapping; specifying a column or alias that matches any property on a custom type, results in the automatic type conversion and assignment of value to that property.
Test.