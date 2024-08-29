namespace BloggingSystemRepository
{
	public interface IUserRepository
	{
		Task<User> AuthenticateUserAsync(LoginCredentials credentials);
		Task<User> RegisterUserAsync(RegisterCredentials credentials);
		Task<User> GetUserDetailsAsync(string username);
		Task UpdateUserDetailsAsync(User user);
	}
}