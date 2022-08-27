using System.ComponentModel.DataAnnotations;

namespace MVC_test.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;

        //nav prop
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        //foreign key
        public int? AuthorId { get; set; }
        //nav prop
        public User? Author { get; set; }
    }
}
