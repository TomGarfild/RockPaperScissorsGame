using System.Text.Json.Serialization;

namespace Client
{
    class Settings
    {
        [JsonPropertyName("baseAddress")]
        public string BaseAddress { get; set; }
    }
}
