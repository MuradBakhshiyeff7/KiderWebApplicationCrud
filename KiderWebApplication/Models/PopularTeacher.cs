using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KiderWebApplication.Models
{
    public class PopularTeacher
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Profession { get; set; }

        public string? Image { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        
        public IFormFile? ImageFile { get; set; }
    }

}
