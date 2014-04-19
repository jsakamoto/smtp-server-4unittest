using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Toolbelt.Net.Smtp
{
    /// <summary>
    /// The class of mocking smtp server designed for Unit Test.
    /// </summary>
    public class SmtpServerForUnitTest : SmtpServerCore
    {
        protected List<SmtpMessage> _Messages;

        public SmtpMessage[] ReceivedMessages
        {
            get
            {
                lock (_Messages)
                {
                    return _Messages.ToArray();
                }
            }
        }

        public SmtpServerForUnitTest()
            : base()
        {
        }

        public SmtpServerForUnitTest(IPAddress address, int port)
            : base(address, port)
        {
        }

        public SmtpServerForUnitTest(IEnumerable<IPEndPoint> endPoints)
            : base(endPoints)
        {
        }

        protected override void Initialize(IEnumerable<IPEndPoint> endPoints, IEnumerable<NetworkCredential> credentials)
        {
            base.Initialize(endPoints, credentials);
            this._Messages = new List<SmtpMessage>();
            this.ReceiveMessage += OnReceiveMessage;
        }

        void OnReceiveMessage(object sender, ReceiveMessageEventArgs e)
        {
            lock (_Messages)
            {
                _Messages.Add(e.Message);
            }
        }

        public void ClearReceivedMessages()
        {
            lock (_Messages)
            {
                _Messages.Clear();
            }
        }
    }
}
