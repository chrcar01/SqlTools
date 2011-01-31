using NUnit.Framework;
using SqlTools.Tests.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SqlTools.Tests
{
  //  [TestFixture]
  //  public class SqlDbHelperTests
  //  {
		//
  //      private IDbHelper _helper;

  //      [TestFixtureSetUp]
  //      public void InitializeAllTests()
  //      {
  //          _helper = new SqlDbHelper(ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString);
  //      }

  //      [Test]
  //      public void can_get_datatable_from_sql()
  //      {
  //          var data = _helper.ExecuteDataTable("select name from blog");
  //          Assert.AreEqual(1, data.Rows.Count);
  //          Assert.AreEqual("Killer Thoughts", data.Rows[0][0].ToString());
  //      }

  //      [Test]
  //      public void can_get_datatable_from_command()
  //      {
  //          string sql = "select * from post where id=@id";
  //          using (var command = new SqlCommand(sql))
  //          {
  //              command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = 3;
  //              var data = _helper.ExecuteDataTable(command);
  //              Assert.AreEqual(1, data.Rows.Count);
  //              Assert.AreEqual("Future post2", data.Rows[0]["Title"].ToString());
  //          }
  //      }

  //      [Test]
  //      public void can_get_single_value_from_sql()
  //      {
  //          Assert.AreEqual(2, _helper.ExecuteScalar<int>("select count(*) from post where DatePublished is null"));
  //      }

  //      [Test]
  //      public void can_get_single_value_from_command()
  //      {
  //          string sql = "select title from post where id=@id";
  //          using (var command = new SqlCommand(sql))
  //          {
  //              command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = 2;
  //              Assert.AreEqual("Future post1", _helper.ExecuteScalar<string>(command));
  //          }
  //      }

  //      [Test]
  //      public void can_map_single_model()
  //      {
  //          var post = _helper.ExecuteSingle<Post>("select * from post");
  //          Assert.AreEqual("How to do something cool", post.Title);
  //          Assert.AreEqual(1, post.BlogID);
  //          Assert.AreEqual(1, post.ID);
  //          Assert.AreEqual(Convert.ToDateTime("2010-01-10 09:34:00.000"), post.DatePublished);
  //          Assert.AreEqual("cool content", post.PostContent);
  //      }

  //      [Test]
  //      public void can_map_multiple_models()
  //      {
  //          var posts = _helper.ExecuteMultiple<Post>("select * from post");
  //          Assert.AreEqual(4, posts.Count());
  //      }

  //      [Test]
  //      public void can_execute_tuples()
  //      {
  //          var tuple1 = _helper.ExecuteTuple<string>("select title from post");
  //          Assert.AreEqual(4, tuple1.Count());
  //          Assert.AreEqual("Future post2", tuple1.ElementAt(2).First);
  //          var tuple2 = _helper.ExecuteTuple<int, string>("select top 1 id, title from post");
  //          Assert.AreEqual(1, tuple2.Count());
  //          Assert.AreEqual(1, tuple2.ElementAt(0).First);
  //          Assert.AreEqual("How to do something cool", tuple2.ElementAt(0).Second);
  //          var tuple3 = _helper.ExecuteTuple<string, DateTime?, int>("select title, DatePublished, id from post where DatePublished is null");
  //          Assert.AreEqual(2, tuple3.Count());
  //          Assert.IsNull(tuple3.ElementAt(0).Second);
  //      }

  //      [Test]
  //      public void can_get_datareader_and_use_extension()
  //      {
  //          using (var reader = _helper.ExecuteReader("select * from post"))
  //          {
  //              while (reader.Read())
  //              {
  //                  Assert.IsNotNull(reader.GetValue<int>("ID"));
  //              }
  //          }
  //      }
  //  }
}
