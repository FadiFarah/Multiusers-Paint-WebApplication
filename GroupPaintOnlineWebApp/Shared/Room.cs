using System;
using System.Collections.Generic;
using System.Text;

namespace GroupPaintOnlineWebApp.Shared
{
    public class Room
    {
        public string Id { get; set; }
        public string RoomName { get; set; }
        public int CurrentUsers { get; set; }
        public bool IsPublic { get; set; }
        public string Password { get; set; }
    }
}
