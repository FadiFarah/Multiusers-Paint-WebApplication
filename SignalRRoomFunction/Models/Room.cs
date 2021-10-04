using System.Text.Json.Serialization;

namespace SignalRRoomFunction.Models
{
    public class Room : BaseEntity
    {

        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }

        [JsonPropertyName("currentUsers")]
        public int CurrentUsers { get; set; }

        [JsonPropertyName("maxUsers")]
        public int MaxUsers { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
