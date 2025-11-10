
using System.ComponentModel.DataAnnotations;

namespace DogsHouse.Models
{
    public class Dog

    {
        [Key]
        public string name { get; set; }
        public string color { get; set; }
        public Int16 tail_length { get; set; }
        public Int16 weight { get; set; }
    }
}