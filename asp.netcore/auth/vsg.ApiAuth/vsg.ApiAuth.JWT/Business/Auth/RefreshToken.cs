using System;
using System.Text.Json.Serialization;

namespace vsg.ApiAuth.JWT.Business.Auth
{
    public class RefreshToken
    {
        // can be used for usage tracking
        // can optionally include other metadata,
        // such as user agent, ip address, device name, and so on
        [JsonPropertyName("username")]
        public string UserName { get; set; }        

        [JsonPropertyName("tokenString")]
        public string TokenString { get; set; }

        [JsonPropertyName("expireAt")]
        public DateTime ExpireAt { get; set; }
    }
}
