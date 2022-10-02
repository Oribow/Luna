using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Services
{
    public interface IEmailService
    {
        public void OpenEmailClient(string to, string subject, string body);
    }
}
