using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace dsignelixir.Models
{
    public class Contact
    {
            [Required(ErrorMessage = "You need to fill in a name")]
            [DisplayName("Your Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "You need to fill in an email address")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Your email address contains some errors")]
            [DisplayName("Your Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "You need to fill in a comment")]
            [DisplayName("Your Question / Comments")]
            [DataType(DataType.MultilineText)]
            public string Comment { get; set; }
        
    }
}