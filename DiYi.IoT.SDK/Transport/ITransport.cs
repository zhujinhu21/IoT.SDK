using System.Threading.Tasks;
using DiYi.IoT.SDK;
using DiYi.IoT.SDK.Messages;

namespace DiYi.IoT.SDK.Transport
{
    public interface ITransport
    {
        //BrokerAddress BrokerAddress { get; }

        Task<OperateResult> SendAsync(TransportMessage message);
    }
}
