using System.Text.Json.Serialization;

namespace ApiFunctions.Models
{
    public class Room : BaseEntity
    {

        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }

        [JsonPropertyName("currentUsers")]
        public int CurrentUsers { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
