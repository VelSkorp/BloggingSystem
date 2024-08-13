namespace BloggingSystemRepository
{
	public interface ISearchService
	{
		Task<List<Post>> SearchPostsByAuthorAsync(string author);
	}
}