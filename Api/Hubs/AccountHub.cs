using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Api.DtoModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    [Authorize]
    public class AccountHub : Hub
    {
        private static readonly string MicroserviceHandlerIdentifier = "AccountMicroservice";

        #region Update
        public Task UpdateProfile(AccountDto request)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("UpdateProfile", Context.User.Identity.Name, request);
        }

        public Task AvatarUpload(string imgUrl)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("AvatarUpload", Context.User.Identity.Name, imgUrl);
        }

        public Task CoverUpload(string imgUrl)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("CoverUpload", Context.User.Identity.Name, imgUrl);
        }

        public Task UpdateProfileSuccess(string phoneNumber, AccountDto response)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"UpdateProfileSuccess", response);
        }

        public Task UpdateProfileFailed(string phoneNumber, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"UpdateProfileFailed", reasonMsg);
        }
        #endregion

        #region Connected/Disco.
        public override async Task OnConnectedAsync()
        {
            // TODO: check here - this anonymous non-authorized user's ID and save for later use
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // TODO: check here - this non-authorized user's ID and save for later use
            await base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}
