using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK
{
    public class BrokerConnectionException : Exception
    {
        public BrokerConnectionException(Exception innerException)
            : base("Broker Unreachable", innerException)
        {

        }
    }
}
