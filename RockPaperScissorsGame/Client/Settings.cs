using System.Text.Json.Serialization;

namespace Client
{
    public class Settings
    {
        [JsonPropertyName("baseAddress")]
        public string BaseAddress { get; set; }
    }
}
