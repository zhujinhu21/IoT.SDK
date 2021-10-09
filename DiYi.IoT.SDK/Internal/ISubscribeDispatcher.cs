// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using DiYi.IoT.SDK.Diagnostics;
using DiYi.IoT.SDK.Persistence;

namespace DiYi.IoT.SDK.Internal
{
    /// <summary>
    /// Consumer executor
    /// </summary>
    public interface ISubscribeDispatcher
    {
        Task<OperateResult> DispatchAsync(MediumMessage message, CancellationToken cancellationToken = default);

        Task<OperateResult> DispatchAsync(MediumMessage message, ConsumerExecutorDescriptor descriptor, CancellationToken cancellationToken = default);
    }
}