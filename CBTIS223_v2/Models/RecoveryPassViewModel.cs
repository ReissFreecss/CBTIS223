using System.ComponentModel.DataAnnotations;

namespace CBTIS223_v2.Models
{
    public class RecoveryPasswordViewModel
    {
        public string token { get; set; }

        [Required]
        public string password { get; set; }

        [Compare("password")]
        [Required]
        public string password2 { get; set; }
    }
}