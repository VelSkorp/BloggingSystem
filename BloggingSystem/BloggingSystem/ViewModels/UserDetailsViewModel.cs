using BloggingSystemRepository;

namespace BloggingSystem
{
    public class UserDetailsViewModel
    {
        public User Author { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}