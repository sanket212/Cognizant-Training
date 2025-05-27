using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Domain
{
    public class Tag
    {
        [Key]
        public Guid TagId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
