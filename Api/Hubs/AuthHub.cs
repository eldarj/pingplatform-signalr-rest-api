﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using Api.DtoModels.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class AuthHub : Hub
    {
        #region Auth-Login Hub Endpoints
        public Task RequestAuthentication(string appId, AccountLoginDto request)
        {
            if (Clients.Group("accountMicroservice") != null)
            {
                return Clients.Group("accountMicroservice").SendAsync("RequestAuthentication", appId, request);
            }
            else
            {
                return Clients.All.SendAsync("RequestAuthentication", appId, request);
            }
        }

        public Task AuthenticationDone(string appId, AccountDto request)
        {
            return Clients.All.SendAsync($"AuthenticationDone{appId}", request);
        }

        public Task AuthenticationFailed(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"AuthenticationFailed{appId}", reasonMsg);
        }
        #endregion

        #region Auth-Register Hub Endpoints
        public Task RequestRegistration(string appId, AccountRegisterDto request)
        {
            if (Clients.Group("accountMicroservice") != null)
            {
                return Clients.Group("accountMicroservice").SendAsync("RequestRegistration", appId, request);
            }
            else
            {
                return Clients.All.SendAsync("RequestRegistration", appId, request);
            }
        }

        public Task RegistrationDone(string appId, AccountDto request)
        {
            return Clients.All.SendAsync($"RegistrationDone{appId}", request);
        }

        public Task RegistrationFailed(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"RegistrationFailed{appId}", reasonMsg);
        }
        #endregion

        public override async Task OnConnectedAsync()
        {
            QueryString queryString = Context.GetHttpContext().Request.QueryString;
            NameValueCollection qs = HttpUtility.ParseQueryString(queryString.ToString());
            String groupName = qs.Get("groupName");
            if(groupName != null)
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
    }
}
