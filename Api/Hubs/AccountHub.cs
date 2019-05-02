using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Api.DtoModels.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class AccountHub : Hub
    {
        #region Update
        public Task UpdateProfile(string appId, AccountDto request)
        {
            if (Clients.Group("accountMicroservice") != null)
            {
                return Clients.Group("accountMicroservice").SendAsync("UpdateProfile", appId, request);
            }
            else
            {
                return Clients.All.SendAsync("UpdateProfile", appId, "test");
            }
        }
        public Task AvatarUpload(string appId, string accountPhoneNumber, string imgUrl)
        {
            if (Clients.Group("accountMicroservice") != null)
            {
                return Clients.Group("accountMicroservice").SendAsync("AvatarUpload", appId, accountPhoneNumber, imgUrl);
            }
            else
            {
                return Clients.All.SendAsync("AvatarUpload", appId, accountPhoneNumber, imgUrl);
            }
        }
        public Task CoverUpload(string appId, string accountPhoneNumber, string imgUrl)
        {
            if (Clients.Group("accountMicroservice") != null)
            {
                return Clients.Group("accountMicroservice").SendAsync("CoverUpload", appId, accountPhoneNumber, imgUrl);
            }
            else
            {
                return Clients.All.SendAsync("CoverUpload", appId, accountPhoneNumber, imgUrl);
            }
        }

        public Task UpdateProfileSuccess(string appId, AccountDto response)
        {
            return Clients.All.SendAsync($"UpdateProfileSuccess{appId}", response);
        }

        public Task UpdateProfileFailed(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"UpdateProfileFailed{appId}", reasonMsg);
        }
        #endregion

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
    }
}
