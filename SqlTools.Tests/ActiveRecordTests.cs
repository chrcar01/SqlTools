using NUnit.Framework;
using SqlTools.Tests.Models;
using System;

namespace SqlTools.Tests
{
	[TestFixture]
	public class ActiveRecordTests
	{
		[Test]
		public void Can_find()
		{
			var blog = new Blog { Name = "Carter Rocks", TagLine = "Whatever" };
			
		}
	}
}
