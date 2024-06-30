using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BloggingSystem
{
	public class Comment
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
	}
}