using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CustomAuthenticationExtensions.API.Models
{
    public class ResponseAction
    {
        [JsonPropertyName("@odata.type")]
        public string DataType { get; set; }

        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }

        public ResponseAction(string dataType, string? message = null, string? title = null)
        {
            DataType = dataType;
            Message = message;
            Title = title;
        }
    }
}
