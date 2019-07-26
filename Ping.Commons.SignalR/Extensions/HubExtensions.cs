using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Ping.Commons.SignalR.Extensions
{
    // Extension methods for the Hub classes as in e.g. ChatHub
    public static class HubExtensions
    {
        public static string NameIdentifier(this Hub hub) => hub.Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
