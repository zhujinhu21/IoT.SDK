using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK.Diagnostics
{
    /// <summary>
    /// Extension methods on the DiagnosticListener class to log IoT data
    /// </summary>
    public class IoTDiagnosticListenerNames
    {
        private const string CapPrefix = "DiYi.IoT.";

        public const string DiagnosticListenerName = "CapDiagnosticListener";

        public const string BeforePublishMessageStore = CapPrefix + "WritePublishMessageStoreBefore";
        public const string AfterPublishMessageStore = CapPrefix + "WritePublishMessageStoreAfter";
        public const string ErrorPublishMessageStore = CapPrefix + "WritePublishMessageStoreError";

        public const string BeforePublish = CapPrefix + "WritePublishBefore";
        public const string AfterPublish = CapPrefix + "WritePublishAfter";
        public const string ErrorPublish = CapPrefix + "WritePublishError";

        public const string BeforeConsume = CapPrefix + "WriteConsumeBefore";
        public const string AfterConsume = CapPrefix + "WriteConsumeAfter";
        public const string ErrorConsume = CapPrefix + "WriteConsumeError";

        public const string BeforeSubscriberInvoke = CapPrefix + "WriteSubscriberInvokeBefore";
        public const string AfterSubscriberInvoke = CapPrefix + "WriteSubscriberInvokeAfter";
        public const string ErrorSubscriberInvoke = CapPrefix + "WriteSubscriberInvokeError";
    }
}
