
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Dog

    {
        [Key]
        public string name { get; set; }

        [Required]
        [RegularExpression(@"^[\p{L}&\s]+$", ErrorMessage = "The field {0} may contain only letters and the '&' sign.")]
        public string color { get; set; }

        [Required]
        [Range(1, Int16.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]

        public Int16 tail_length { get; set; }
        [Required]
        [Range(1, Int16.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        
        public Int16 weight { get; set; }
    }
}