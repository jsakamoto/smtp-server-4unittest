using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbelt.Net.Smtp.Test
{
    [TestClass]
    public class SmtpServerForUnitTestTest
    {
        [TestMethod]
        public void Send_with_Auth_Success_Test()
        {
            using (var server = new SmtpServerForUnitTest(
                address: IPAddress.Loopback,
                port: 2525,
                credentials: new[] { new NetworkCredential("john@example.com", "p@$$w0rd") }))
            {
                server.Start();

                var client = new SmtpClient("localhost", 2525);
                client.Credentials = new NetworkCredential("john@example.com", "p@$$w0rd");
                client.Send(
                    "john@example.com",
                    "mike@example.com,alice@example.com",
                    "[HELLO WORLD]",
                    "Hello, World.");

                server.ReceivedMessages.Count().Is(1);
                var msg = server.ReceivedMessages.Single();
                msg.MailFrom.Is("<john@example.com>");
                msg.RcptTo.OrderBy(_ => _).Is("<alice@example.com>", "<mike@example.com>");
                msg.From.Address.Is("john@example.com");
                msg.To.Select(_ => _.Address).OrderBy(_ => _).Is("alice@example.com", "mike@example.com");
                msg.CC.Count().Is(0);
                msg.Subject.Is("[HELLO WORLD]");
                msg.Body.Is("Hello, World.");
            }
        }
    }
}
