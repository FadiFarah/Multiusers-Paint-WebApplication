using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GroupPaintOnlineWebApp.Shared
{
    public class Painting 
    {
        [Key]
        public string PaintingId { get; set; }
        public string UserName { get; set; }
        public string ImageURL { get; set; }
        public string PaintingName { get; set; }
        public DateTime Date { get; set; }
    }
}
