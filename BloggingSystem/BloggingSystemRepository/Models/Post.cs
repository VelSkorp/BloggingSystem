using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace BloggingSystemRepository
{
	public class Post
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		[JsonProperty("_id")]
		public ObjectId Id { get; set; }
		public string Author { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<string> Images { get; set; } = new List<string>();
		public List<Comment> Comments { get; set; } = new List<Comment>();
	}
}