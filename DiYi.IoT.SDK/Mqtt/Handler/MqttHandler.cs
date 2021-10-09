using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.IoT.SDK.Mqtt
{
    /// <summary>
    /// Mqtt处理者
    /// </summary>
    public class MqttHandler : IMqttHandler
    {
        public Task<SendOpenCellOut> PublishOpenCellMessage(SendOpenCellData openCellSendData, bool isAsync = false, int timeoutSeconds = 2)
        {
            throw new NotImplementedException();
        }
    }
}
