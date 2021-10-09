using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK.Messages
{
    public static class Headers
    {
        /// <summary>
        /// Id of the message. Either set the ID explicitly when sending a message, or assign one to the message.
        /// </summary>
        public const string MessageId = "iot-msg-id";

        public const string MessageName = "iot-msg-name";

        public const string Group = "iot-msg-group";

        /// <summary>
        /// Message value .NET type
        /// </summary>
        public const string Type = "iot-msg-type";

        public const string CorrelationId = "iot-corr-id";

        public const string CorrelationSequence = "cap-corr-seq";

        public const string CallbackName = "iot-callback-name";

        public const string SentTime = "iot-senttime";

        public const string Exception = "iot-exception";
    }
}
