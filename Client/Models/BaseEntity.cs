﻿using System.Text.Json.Serialization;

namespace GroupPaintOnlineWebApp.Client.Models
{
    public abstract class BaseEntity
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("_etag")]
        public string _etag { get; set; }
    }
}