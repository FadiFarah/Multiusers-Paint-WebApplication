using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRRoomFunction
{
    public class SignalRConnectionData
    {
        public DateTime Timestamp { get; set; }
        public string HubName { get; set; }
        public string ConnectionId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
