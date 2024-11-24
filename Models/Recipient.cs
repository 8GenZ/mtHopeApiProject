using System.ComponentModel.DataAnnotations;

namespace mtHopeApiProject.Models
{
    public class Recipient
    {
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Sponsored { get; set; }

    }
}
