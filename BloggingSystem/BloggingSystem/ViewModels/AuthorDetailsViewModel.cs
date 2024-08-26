using BloggingSystemRepository;

namespace BloggingSystem
{
    public class AuthorDetailsViewModel
    {
        public User Author { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}