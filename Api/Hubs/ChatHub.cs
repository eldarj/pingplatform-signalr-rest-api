using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Ping.Commons.Dtos.Models.Auth;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Api.Hubs
{
    public class ChatHub : Hub
    {
        #region Contacts
        public Task RequestContacts(string appId, string phoneNumber)
        {
            if (Clients.Group("chatMicroservice") != null)
            {
                return Clients.Group("chatMicroservice").SendAsync("RequestContacts", appId, phoneNumber);
            }
            else
            {
                return Clients.All.SendAsync("RequestContacts", appId, phoneNumber);
            }
        }

        public Task RequestContactsSuccess(string appId, List<ContactDto> fileDto)
        {
            return Clients.All.SendAsync($"RequestContactsSuccess{appId}", fileDto);
        }

        public Task RequestContactsFail(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"RequestContactsFail{appId}", reasonMsg);
        }
        #endregion

        #region ConnectDisconnect
        public override async Task OnConnectedAsync()
        {
            QueryString queryString = Context.GetHttpContext().Request.QueryString;
            NameValueCollection qs = HttpUtility.ParseQueryString(queryString.ToString());
            String groupName = qs.Get("groupName");
            if (groupName != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            QueryString queryString = Context.GetHttpContext().Request.QueryString;
            NameValueCollection qs = HttpUtility.ParseQueryString(queryString.ToString());
            String groupName = qs.Get("groupName");

            if (groupName != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnDisconnectedAsync(exception);
        }
        #endregion  
    }
}
