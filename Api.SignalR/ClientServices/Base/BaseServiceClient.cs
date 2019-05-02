using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.SignalR.ClientServices
{
    public abstract class BaseServiceClient
    {
        protected HubConnection Connection { get; set; }
        protected abstract void Connect();
    }
}
