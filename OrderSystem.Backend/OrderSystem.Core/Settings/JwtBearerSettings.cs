using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OrderSystem.API.Settings
{
    public class JwtBearerSettings
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}