using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CustomAuthenticationExtensions.API.Models
{
    public class Data
    {
        [JsonPropertyName("@odata.type")]
        public string DataType { get; set; }
        [JsonPropertyName("actions")]
        public List<ResponseAction> Actions { get; set; }

        public Data(string dataType)
        {
            DataType = dataType;
        }
    }
}
