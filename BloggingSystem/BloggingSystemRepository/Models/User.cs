using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BloggingSystemRepository
{
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Photo { get; set; }
		public string Password { get; set; }
		public HashSet<UserFollowInfo> Followers { get; set; } = new HashSet<UserFollowInfo>();
		public HashSet<UserFollowInfo> Following { get; set; } = new HashSet<UserFollowInfo>();
		public List<string> Notifications { get; set; } = new List<string>();
	}
}