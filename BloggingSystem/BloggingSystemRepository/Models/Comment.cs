using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BloggingSystemRepository
{
	public class Comment
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }
		public string Author { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}