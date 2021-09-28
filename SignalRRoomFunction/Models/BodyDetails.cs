using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SignalRRoomFunction.Models
{
    public class BodyDetails
    {
        [JsonPropertyName("connectionId")]
        public string ConnectionId { get; set; }
        [JsonPropertyName("groupName")]
        public string GroupName { get; set; }
        [JsonPropertyName("imageURL")]
        public string ImageURL { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("isAccepted")]
        public string IsAccepted { get; set; }
    }
}
