using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiFunctions.Models
{
    public class Painting : BaseEntity
    {
        [JsonPropertyName("paintingName")]
        public string PaintingName { get; set; }

        [JsonPropertyName("imageURL")]
        public string ImageURL { get; set; }

        [JsonPropertyName("contributedUsers")]
        public List<string> ContributedUsers { get; set; }

        [JsonPropertyName("roomDetails")]
        public Room RoomDetails { get; set; }

        [JsonPropertyName("creatorId")]
        public string CreatorId { get; set; }

        [JsonPropertyName("shapesDetails")]
        public ArrayList ShapesDetails { get; set; }
    }
}
