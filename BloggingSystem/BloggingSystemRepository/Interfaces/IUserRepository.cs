using System.Linq.Expressions;

namespace BloggingSystemRepository
{
	public interface IUserRepository
	{
		Task<User> AuthenticateUserAsync(LoginCredentials credentials);
		Task<User> RegisterUserAsync(RegisterCredentials credentials);
		Task<User> GetUserDetailsAsync(string username);
		Task UpdateUserDetailsAsync<TField>(Expression<Func<User, TField>> field, TField value, string username);
	}
}