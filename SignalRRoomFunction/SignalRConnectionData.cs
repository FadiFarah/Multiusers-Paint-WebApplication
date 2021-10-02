using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SignalRRoomFunction
{
    public class SignalRConnectionData
    {
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        [JsonPropertyName("hubName")]
        public string hubName { get; set; }
        [JsonPropertyName("connectionId")]
        public string ConnectionId { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
