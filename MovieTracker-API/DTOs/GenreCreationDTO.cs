using MovieTracker_API.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MovieTracker_API.DTOs
{
    public class GenreCreationDTO
    {
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
    }
}
