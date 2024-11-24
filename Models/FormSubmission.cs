using System.ComponentModel.DataAnnotations;

namespace mtHopeApiProject.Models
{
    public class FormSubmission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SubmissionDate { get; set; }
        [Required]
        public int RecipientId { get; set; }
        public Recipient Recipient { get; set; }
    }
}
