using System;
using System.Collections.Generic;

namespace Nuclear.Data.PlugIn.Contracts
{
    /// <summary>
    /// Contract that is neccessary to register additional channels from third party plugins
    /// </summary>
    public interface IKernelPlugIn
    {
        List<Type> LoadChannels();

    }
}
