using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
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

		public async Task UpdateUserDetailsAsync<TField>(Expression<Func<User, TField>> field, TField value, string username)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Username, username);
			var update = Builders<User>.Update.Set(field, value);
			await _usersCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddToUserCollectionAsync<TField>(Expression<Func<User, IEnumerable<TField>>> field, TField value, string username)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Username, username);
			var update = Builders<User>.Update.AddToSet(field, value);
			await _usersCollection.UpdateOneAsync(filter, update);
		}

		public async Task RemoveFromUserCollectionAsync<TField>(Expression<Func<User, IEnumerable<TField>>> field, TField value, string username)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Username, username);
			var update = Builders<User>.Update.Pull(field, value);
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