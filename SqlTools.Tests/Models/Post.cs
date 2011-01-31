using System;

namespace SqlTools.Tests.Models
{
    public class Post
    {
        public int ID { get; set; }
        public int BlogID { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime? DatePublished { get; set; }
    }
}
