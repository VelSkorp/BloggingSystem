namespace BloggingSystemRepository
{
	public class BlogStoreDatabaseSettings
	{
		public string DatabaseName { get; set; } = null!;
		public string UsersCollectionName { get; set; } = null!;
		public string PostsCollectionName { get; set; } = null!;
	}
}