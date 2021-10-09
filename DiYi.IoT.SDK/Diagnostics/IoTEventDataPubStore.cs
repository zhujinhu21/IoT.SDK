using System;
using System.Collections.Generic;
using System.Text;
using DiYi.IoT.SDK.Messages;

namespace DiYi.IoT.SDK.Diagnostics
{
    public class IoTEventDataPubStore
    {
        public long? OperationTimestamp { get; set; }

        public string Operation { get; set; }

        public Message Message { get; set; }

        public long? ElapsedTimeMs { get; set; }

        public Exception Exception { get; set; }
    }
}
