using MovieTracker_API.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MovieTracker_API.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
    }
}
