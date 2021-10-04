using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GroupPaintOnlineWebApp.Client.Models
{
    public class Room : BaseEntity
    {

        [JsonPropertyName("roomName")]
        [Required]
        [MinLength(0)]
        [MaxLength(20)]
        public string RoomName { get; set; }

        [JsonPropertyName("currentUsers")]
        public int CurrentUsers { get; set; }

        [Range(1, 10)]
        [JsonPropertyName("maxUsers")]
        public int MaxUsers { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        [MaxLength(15)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
