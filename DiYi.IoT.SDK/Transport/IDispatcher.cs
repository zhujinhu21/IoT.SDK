using System;
using System.Collections.Generic;
using System.Text;
using DiYi.IoT.SDK.Internal;
using DiYi.IoT.SDK.Persistence;

namespace DiYi.IoT.SDK.Transport
{
    public interface IDispatcher
    {
        void EnqueueToPublish(MediumMessage message);

        void EnqueueToExecute(MediumMessage message, ConsumerExecutorDescriptor descriptor);
    }
}
