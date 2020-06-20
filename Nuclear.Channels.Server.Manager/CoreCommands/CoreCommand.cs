﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager.CoreCommands
{
    public class CoreCommand
    {
        public List<object> Services { get; private set; }
        public string CommandName { get; }

        public CoreCommand(string commandName)
        {
            Services = new List<object>();
            CommandName = commandName;
        }

        public void AddService(object service)
        {
            Services.Add(service);
        }

        public void AddServices(List<object> services)
        {
            Services.AddRange(services);
        }
    }
}
