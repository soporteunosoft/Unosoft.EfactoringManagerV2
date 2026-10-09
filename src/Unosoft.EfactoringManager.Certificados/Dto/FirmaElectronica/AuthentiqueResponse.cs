using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto.FirmaElectronica
{
    public class AuthentiqueResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
         
        [JsonProperty("lastName")] 
        public string LastName { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; } 
    }
}
