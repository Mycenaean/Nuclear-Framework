// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Abstractions
{
    public interface IEventHandler<in TRequest, out TResult>
    {
        TResult Handle(TRequest request);
    }

    public interface IEventHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}
