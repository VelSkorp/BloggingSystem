using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace BloggingSystemRepository
{
	public class UserRepository : IUserRepository
	{
		private readonly IMongoCollection<User> _usersCollection;

		public UserRepository(IMongoDatabase mongoDatabase, IOptions<BlogStoreDatabaseSettings> settings)
		{
			_usersCollection = mongoDatabase.GetCollection<User>(settings.Value.UsersCollectionName);
		}

		public async Task<User> AuthenticateUserAsync(LoginCredentials credentials)
		{
			var user = await _usersCollection.Find(u => u.Username == credentials.Username).FirstOrDefaultAsync();
			if (user is null || user.Password != HashPassword(credentials.Password))
			{
				throw new Exception("Invalid username or password");
			}

			return user;
		}

		public async Task<User> RegisterUserAsync(RegisterCredentials credentials)
		{
			var existingUser = await _usersCollection.Find(u => u.Username.Equals(credentials.Username)).FirstOrDefaultAsync();
			if (existingUser is not null)
			{
				throw new Exception("User already exists");
			}

			var user = new User
			{
				Id = ObjectId.GenerateNewId(DateTime.Now),
				Username = credentials.Username,
				FirstName = credentials.FirstName,
				LastName = credentials.LastName,
				Password = HashPassword(credentials.Password)
			};

			await _usersCollection.InsertOneAsync(user);
			return user;
		}

		private string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				var builder = new StringBuilder();
				foreach (var b in bytes)
				{
					builder.Append(b.ToString("x2"));
				}
				return builder.ToString();
			}
		}
	}
}