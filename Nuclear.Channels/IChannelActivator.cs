﻿// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base.Exceptions;
using Nuclear.ExportLocator.Services;
using System;

namespace Nuclear.Channels
{
    /// <summary>
    /// Service that will initialize all ChannelMethods as HTTP Endopints
    /// </summary>
    public interface IChannelActivator
    {
        /// <summary>
        /// Method that will do the initialization of Channels
        /// </summary>
        /// <param name="currentDomain">AppDomain with all assemblies</param>
        /// <param name="Services">IServiceLocator</param>
        /// <param name="baseURL">Base URL to be exposed for channels</param>
        /// <exception cref="HttpListenerNotSupportedException"></exception>
        void Execute(AppDomain currentDomain, IServiceLocator Services, string baseURL = null);

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="basicAuthenticationMethod">Function delegate to be used for basic authentication</param>
        void AuthenticationOptions(Func<string, string, bool> basicAuthenticationMethod);

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="tokenAuthenticationMethod">Function delegate to be used for token authentication</param>
        void AuthenticationOptions(Func<string, bool> tokenAuthenticationMethod);
    }
}