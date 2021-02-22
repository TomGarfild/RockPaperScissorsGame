using System.ComponentModel.DataAnnotations;

namespace Server
{
    public class Account
    {
        public string Id { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(maximumLength: 128, MinimumLength = 6)]
        public string Password { get; set; }
    }
}