using System.Text.Json.Serialization;

namespace ApiFunctions.Models
{
    public abstract class BaseEntity
    {

        [JsonPropertyName("id")]
        public string id { get; set; }
    }
}