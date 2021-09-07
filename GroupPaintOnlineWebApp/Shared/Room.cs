using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GroupPaintOnlineWebApp.Shared
{
    public class Room
    {
        public string Id { get; set; }
        [Required]
        [MinLength(0)]
        [MaxLength(20)]
        public string RoomName { get; set; }
        public int CurrentUsers { get; set; }
        public bool IsPublic { get; set; }
        [MaxLength(15)]
        public string Password { get; set; }
    }
}
