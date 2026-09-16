using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static CustomAuthenticationExtensions.API.ValidateEmailDomain;

namespace CustomAuthenticationExtensions.API.Models
{
    public class ResponseObject
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }

        public ResponseObject(string dataType)
        {
            Data = new Data(dataType);
        }
    }

}
