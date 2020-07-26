using Nuclear.Channels.Base.Decorators;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Server.Web.Commands.Restart;
using Nuclear.Channels.Server.Web.Commands.Start;
using Nuclear.Channels.Server.Web.Commands.Stop;
using Nuclear.Channels.Server.Web.Common;
using Nuclear.Channels.Server.Web.Exceptions;
using Nuclear.Channels.Server.Web.Queries.HandlerHistory;
using Nuclear.Channels.Server.Web.Queries.ListHandlerByState;
using Nuclear.Channels.Server.Web.Queries.ListHandlers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nuclear.Channels.Server.Web.Api
{
    [Channel]
    public class ServerChannel : ChannelBase
    {
        [ImportedService]
        public IEventHandler<ListHandlerByStateQuery, IEnumerable<HandlerInformation>> StateQueryHandler { get; set; }

        [ImportedService]
        public IEventHandler<ListHandlersQuery,IReadOnlyCollection<HandlerInformation>> ListHandlersQueryHandler { get; set; }
        
        [ImportedService]
        public IEventHandler<HandlerHistoryQuery, List<string>> HistoryQueryHandler { get; set; }

        [ImportedService]
        public IEventHandler<StartChannelMethodCommand> StartCommandHandler { get; set; }

        [ImportedService]
        public IEventHandler<RestartChannelMethodCommand> RestartCommandHandler { get; set; }

        [ImportedService]
        public IEventHandler<StopChannelMethodCommand> StopCommandHandler { get; set; }

        
        [ChannelMethod]
        public void GetAllMethods()
        {
            var queryAll = new ListHandlersQuery();
            var result = ListHandlersQueryHandler.Handle(queryAll);
            var message = new ChannelMessage
            {
                Success = true,
                Message = string.Empty,
                Output = result
            };

            ChannelMessageWriter.Write(message, Context.Response);
        }

        [ChannelMethod]
        public void GetMethodsByState(string state)
        {
            var queryByState = new ListHandlerByStateQuery(state);
            var result = StateQueryHandler.Handle(queryByState);
            var message = new ChannelMessage
            {
                Success = true,
                Message = string.Empty,
                Output = result
            };
            ChannelMessageWriter.Write(message, Context.Response);
        }

        [ChannelMethod]
        public void GetHandlerHistory(string handlerId)
        {
            var queryHistory = new HandlerHistoryQuery(handlerId);
            IChannelMessage message;
            try
            {
                var result = HistoryQueryHandler.Handle(queryHistory);
                message = new ChannelMessage() 
                {
                    Success = true,
                    Message = string.Empty,
                    Output = result
                };

            }
            catch(HandlerNotFoundException)
            {
                message = new ChannelMessage
                {
                    Success = false,
                    Message = "ChannelMethod Handler not found"
                };
            }
            ChannelMessageWriter.Write(message, Context.Response);
        }

        [ChannelMethod]
        public void StartMethod(string handlerId)
        {
            var startCommand = new StartChannelMethodCommand(handlerId);
            IChannelMessage message;
            try
            {
                StartCommandHandler.Handle(startCommand);
                message = new ChannelMessage()
                {
                    Success = true,
                    Message = "ChannelMethod Started"
                };

            }
            catch (HandlerNotFoundException)
            {
                message = new ChannelMessage
                {
                    Success = false,
                    Message = "ChannelMethod Handler not found"
                };
            }
            ChannelMessageWriter.Write(message, Context.Response);
        }

        [ChannelMethod]
        public void RestartMethod(string handlerId)
        {
            var restartCommand = new RestartChannelMethodCommand(handlerId);
            IChannelMessage message;
            try
            {
                RestartCommandHandler.Handle(restartCommand);
                message = new ChannelMessage()
                {
                    Success = true,
                    Message = "ChannelMethod restarted"
                };

            }
            catch (HandlerNotFoundException)
            {
                message = new ChannelMessage
                {
                    Success = false,
                    Message = "Handler not found"
                };
            }
            ChannelMessageWriter.Write(message, Context.Response);
        }


        [ChannelMethod]
        public void StopMethod(string handlerId)
        {
            var stopCommand = new StopChannelMethodCommand(handlerId);
            IChannelMessage message;
            try
            {
                StopCommandHandler.Handle(stopCommand);
                message = new ChannelMessage()
                {
                    Success = true,
                    Message = "ChannelMethod stopped"
                };

            }
            catch (HandlerNotFoundException)
            {
                message = new ChannelMessage
                {
                    Success = false,
                    Message = "ChannelMethod Handler not found"
                };
            }
            ChannelMessageWriter.Write(message, Context.Response);
        }
    }
}
