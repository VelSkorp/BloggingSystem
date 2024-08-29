using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using static MongoDB.Driver.WriteConcern;

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
			var user = await _usersCollection.Find(u => u.Username.Equals(credentials.Username)).FirstOrDefaultAsync();
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

		public async Task<User> GetUserDetailsAsync(string username)
		{
			return await _usersCollection.Find(u => u.Username.Equals(username)).FirstOrDefaultAsync();
		}

		public async Task UpdateUserDetailsAsync(User user)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
			var update = Builders<User>.Update
				.Set(u => u.FirstName, user.FirstName)
				.Set(u => u.Username, user.Username)
				.Set(u => u.LastName, user.LastName)
				.Set(u => u.Photo, user.Photo);
			await _usersCollection.UpdateOneAsync(filter, update);
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