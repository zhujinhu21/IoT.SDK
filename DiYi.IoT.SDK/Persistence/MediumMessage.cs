using System;
using System.Collections.Generic;
using System.Text;
using DiYi.IoT.SDK.Messages;

namespace DiYi.IoT.SDK.Persistence
{
    public class MediumMessage
    {
        public string DbId { get; set; }

        public Message Origin { get; set; }

        public string Content { get; set; }

        public DateTime Added { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int Retries { get; set; }
    }
}
