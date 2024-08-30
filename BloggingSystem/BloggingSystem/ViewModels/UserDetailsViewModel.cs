using BloggingSystemRepository;

namespace BloggingSystem
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}