using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BloggingSystemRepository
{
	public class Post
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }
		public string Author { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<string> Images { get; set; } = new List<string>();
		public List<Comment> Comments { get; set; } = new List<Comment>();
	}
}