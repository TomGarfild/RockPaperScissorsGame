using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Account
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [Required]
        [StringLength(maximumLength: 64, MinimumLength = 6)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}