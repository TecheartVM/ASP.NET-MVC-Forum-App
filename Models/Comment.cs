using System.ComponentModel.DataAnnotations;

namespace MVC_test.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;

        //foreign key
        public int TopicId { get; set; }
        //nav prop
        public Topic Topic { get; set; }

        //foreign key
        public int? AuthorId { get; set; }
        //nav prop
        public User? Author { get; set; }
    }
}
