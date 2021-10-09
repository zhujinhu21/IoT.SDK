using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.IoT.SDK.Mqtt
{
    public interface IMqttHandler
    {

        /// <summary>
        /// 发送开箱命令
        /// </summary>
        /// <param name="openCellSendData"></param>
        /// <param name="isAsync"></param>
        /// <returns></returns>
        Task PublishOpenCellMessage(Boolean isAsync = false, Int32 timeoutSeconds = 2);
    }
}
