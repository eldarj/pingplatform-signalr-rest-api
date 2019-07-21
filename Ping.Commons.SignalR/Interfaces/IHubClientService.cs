using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.SignalR.Interfaces
{
    public interface IHubClientService
    {
        void RegisterHandlers();

        void Connect();
    }
}
