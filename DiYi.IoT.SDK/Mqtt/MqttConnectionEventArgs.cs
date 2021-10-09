using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK.Mqtt
{
    /// <summary>
    /// 重连事件参数
    /// </summary>
    public class MqttConnectionEventArgs
    {

        /// <summary>
        /// 重连次数
        /// </summary>
        public long ReconnectingCount { get; }

        /// <summary>
        /// 重连异常
        /// </summary>
        public Exception ReconnectingException { get; }

        public MqttConnectionEventArgs(long reconnectingCount, Exception reconnectingException = null)
        {
            ReconnectingCount = reconnectingCount;
            ReconnectingException = reconnectingException;
        }
    }
}
