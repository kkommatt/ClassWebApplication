using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassWebAPI.Models
{
    public class Lector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "ПІБ")]
        public string FullName { get; set; }

        [Display(Name = "Електронна адреса")]
        [EmailAddress(ErrorMessage = "Неправильний формат")]
        public string Email { get; set; }

        [Display(Name = "Універистет")]
        public string University { get; set; } 

        public ICollection<LectorCourse> LectorCourses { get; set; } = new List<LectorCourse>();
    }
}
