﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BloggingSystem
{
	public class Post
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public int Id { get; set; }
		public string Author { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<Comment> Comments { get; set; }
	}
}