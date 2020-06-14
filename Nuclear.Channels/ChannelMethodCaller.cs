// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using Nuclear.Channels.Base.Contracts;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Base.Exceptions;
using Nuclear.Channels.Data.Deserializers;
using Nuclear.Channels.Data.Logging;
using Nuclear.Channels.Heuristics;
using Nuclear.Channels.InvokerServices.Contracts;
using Nuclear.Channels.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

namespace Nuclear.Channels
{
    internal class ChannelMethodCaller
    {
        private readonly IChannelMessageService _msgService;
        private readonly IChannelMethodRequestActivator _requestActivator;
        private readonly IChannelMethodContextProvider _contextProvider;
        private ChannelMethodDeserializerFactory _dsrFactory;
        public ChannelMethodCaller(IChannelMessageService msgService, IChannelMethodContextProvider contextProvider, IChannelMethodRequestActivator requestActivator)
        {
            _contextProvider = contextProvider;
            _requestActivator = requestActivator;
            _msgService = msgService;
        }

        internal bool TryInvokeGetRequest(Type channel,
        MethodInfo method,
        HttpListenerRequest request,
        HttpListenerResponse response,
        ChannelMethodInfo methodDescription,
        ChannelConfigurationInfo channelConfig,
        List<object> channelRequestBody,
        bool authenticated,
        CacheExecutionResult cacheExecutionResult)
        {
            try
            {
                if (request.QueryString.AllKeys.Length > 0)
                {
                    SetupAndInvokeGetRequest(channel, method, request, response, methodDescription, channelConfig, channelRequestBody, authenticated, true, cacheExecutionResult);
                }
                else if (request.QueryString.AllKeys.Length == 0)
                {
                    if (methodDescription.Parameters.Count > 0)
                    {
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _msgService.ExceptionHandler(writer, new TargetParameterCountException(), response);
                        writer.Close();
                        return false;
                    }

                    SetupAndInvokeGetRequest(channel, method, request, response, methodDescription, channelConfig, channelRequestBody, authenticated, false, cacheExecutionResult);
                }
                return true;
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(response.OutputStream))
                {
                    _msgService.ExceptionHandler(writer, ex, response);
                    LogChannel.Write(LogSeverity.Fatal, ex.Message);
                }

                return true;
            }

        }

        internal void SetupAndInvokeGetRequest(Type channel,
            MethodInfo method,
            HttpListenerRequest request,
            HttpListenerResponse response,
            ChannelMethodInfo methodDescription,
            ChannelConfigurationInfo channelConfig,
            List<object> channelRequestBody,
            bool authenticated,
            bool hasParams,
            CacheExecutionResult cacheExecutionResult)
        {
            //Context should be initialized before invoking the method because ChannelBase relies on Context
            InitChannelMethodContext(channelConfig.Endpoint, request, response, authenticated, channelConfig.HttpMethod, channelRequestBody);
            if (hasParams)
            {
                //Since request body will be processed in Heuristics if cache is enabled
                //Data is already stored in Data.Parameters property of CacheExecutionResult 
                if (!cacheExecutionResult.DataProcessed)
                {
                    _dsrFactory = new ChannelMethodDeserializerFactory(request.QueryString);
                    channelRequestBody = _dsrFactory.DeserializeFromQueryParameters(methodDescription);
                }
                else
                    channelRequestBody = cacheExecutionResult.Data.Parameters;

                _requestActivator.GetActivateWithParameters(channel, method, channelRequestBody, response);
            }
            else
                _requestActivator.GetActivateWithoutParameters(channel, method, response);

            _contextProvider.DestroyChannelMethodContext(channelConfig.Endpoint);
        }

        internal void TryInvokePostRequest(Type channel,
            MethodInfo method,
            HttpListenerRequest request,
            HttpListenerResponse response,
            List<object> channelRequestBody,
            ChannelMethodInfo methodDescription,
            ChannelConfigurationInfo channelConfig,
            bool authenticated,
            CacheExecutionResult cacheExecutionResult)
        {
            StreamWriter writer = new StreamWriter(response.OutputStream);
            try
            {
                //Since request body will be processed in Heuristics if cache is enabled
                //InputStream is flushed and data is already stored in Data.Parameters  
                //property of CacheExecutionResult
                if (!cacheExecutionResult.DataProcessed)
                {
                    _dsrFactory = new ChannelMethodDeserializerFactory(request.InputStream);
                    channelRequestBody = _dsrFactory.DeserializeFromBody(methodDescription, request.ContentType);
                }
                else
                    channelRequestBody = cacheExecutionResult.Data.Parameters;

                InitChannelMethodContext(channelConfig.Endpoint, request, response, authenticated, channelConfig.HttpMethod, channelRequestBody);
                _requestActivator.PostActivate(channel, method, channelRequestBody, response);
            }
            catch (ChannelMethodContentTypeException cEx)
            {
                response.StatusCode = 400;
                _msgService.ExceptionHandler(writer, cEx, response);
                LogChannel.Write(LogSeverity.Error, cEx.Message);
            }
            catch (ChannelMethodParameterException pEx)
            {
                response.StatusCode = 400;
                _msgService.ExceptionHandler(writer, pEx, response);
                LogChannel.Write(LogSeverity.Error, pEx.Message);
            }
            catch (TargetParameterCountException tEx)
            {
                response.StatusCode = 400;
                _msgService.ExceptionHandler(writer, tEx, response);
                LogChannel.Write(LogSeverity.Error, tEx.Message);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                _msgService.ExceptionHandler(writer, ex, response);
                LogChannel.Write(LogSeverity.Fatal, ex.Message);
            }
            finally
            {
                _contextProvider.DestroyChannelMethodContext(channelConfig.Endpoint);
                writer.Flush();
                writer.Close();
            }

        }

        private void InitChannelMethodContext(IChannelEndpoint endpoint, HttpListenerRequest request, HttpListenerResponse response, bool isAuthenticated, ChannelHttpMethod method, List<object> channelRequestBody)
        {
            ChannelMethodContext methodContext = new ChannelMethodContext(request, response, method, channelRequestBody, isAuthenticated);
            _contextProvider.SetChannelMethodContext(endpoint, methodContext);
        }

    }
}
