using System;
using System.Text;

namespace DBA.FreshdeskSharp
{
    public class FreshdeskCredentials
    {
        private readonly string _userName;
        private readonly string _password;

        public FreshdeskCredentials(string apiKey)
        {
            _userName = apiKey;
            _password = "X";
        }

        public FreshdeskCredentials(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public string GetEncodedCredentials()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userName}:{_password}"));
        }
    }
}