using System.Threading.Tasks;
using DiYi.IoT.SDK.Persistence;

namespace DiYi.IoT.SDK.Internal
{
    public interface IMessageSender
    {
        Task<OperateResult> SendAsync(MediumMessage message);
    }
}