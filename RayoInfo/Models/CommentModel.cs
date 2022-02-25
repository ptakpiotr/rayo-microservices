using System.ComponentModel.DataAnnotations;

namespace RayoInfo.Models
{
    public class CommentModel
    {
        [Key]
        public int Id { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        
        [EmailAddress]
        public string Author { get; set; }

        [Required]
        [MaxLength(150)]
        public string Content { get; set; }


        public int NewsId { get; set; }
        public NewsModel News{ get; set; }
    }
}
