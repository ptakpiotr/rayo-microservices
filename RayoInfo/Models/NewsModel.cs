using System.ComponentModel.DataAnnotations;

namespace RayoInfo.Models
{
    public class NewsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Url]
        public string PhotoUrl { get; set; }

        [EmailAddress]
        public string Author { get; set; }

        public DateTime DateOfCreation { get; set; }

        public ICollection<CommentModel> Comments{ get; set; }
    }
}
