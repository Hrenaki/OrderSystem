using OrderSystem.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
    public class HashService
    {
        private readonly HashServiceSettings settings;

        public HashService(HashServiceSettings settings)
        {
            this.settings = settings;
        }

        public string GetHash(string value)
        {
            var stringToHash = string.Concat(value, settings.Salt);
            var hashBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}