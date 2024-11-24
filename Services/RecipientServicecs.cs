using mtHopeApiProject.Models;

namespace mtHopeApiProject.Services
{
    public class RecipientServicecs
    {
        // A list of recipients to simulate a data store
        private static readonly List<Recipient> recipients = new List<Recipient>
        {
            new Recipient { Id = 1, Name = "John Doe", Sponsored = true },
            new Recipient { Id = 2, Name = "Jane Smith", Sponsored = false },
            new Recipient { Id = 3, Name = "Samuel Green", Sponsored = false}
        };

        // Method to get all recipients
        public List<Recipient> GetAllRecipients()
        {
            return recipients;
        }

        // Optional: Method to get a recipient by Id
        public Recipient GetRecipientById(int id)
        {
            return recipients.Find(r => r.Id == id);
        }
    }
}
