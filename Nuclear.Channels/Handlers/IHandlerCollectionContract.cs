// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Collections.Generic;

namespace Nuclear.Channels.Handlers
{
    public interface IHandlerCollectionContract<THandler>
    {
        void AddHandler(THandler handler);
        void RemoveHandler(THandler handler);
        THandler GetHandler(string HandlerId);
        List<THandler> AsList();
        THandler[] AsArray();
    }  
}

