using System.ComponentModel.DataAnnotations;

namespace Sician_Diana_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public ICollection<Book>? Books { get; set; }
    }
}
