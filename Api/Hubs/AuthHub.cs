using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using Api.DtoModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Ping.Commons.Dtos.Models.Auth;
using Ping.Commons.Dtos.Models.Wrappers.Response;

namespace Api.Hubs
{
    public class AuthHub : Hub
    {
        private static readonly string MicroserviceHandlerIdentifier = "AccountMicroservice";

        #region Authenticate
        public Task RequestAuthentication(AccountDto request)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestAuthentication", request);
        }

        public Task AuthenticationDone(AccountDto request)
        {
            return Clients.All
                .SendAsync($"AuthenticationDone{request.PhoneNumber}", request);
        }

        public Task AuthenticationFailed(ResponseDto<AccountDto> responseDto) // TODO: Make all response ResponseDto
        {
            return Clients.All
                .SendAsync($"AuthenticationFailed{responseDto.Dto.PhoneNumber}", responseDto);
        }
        #endregion

        #region Register
        public Task RequestRegistration(AccountDto request)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestRegistration", request);
        }

        public Task RegistrationDone(string phoneNumber, AccountDto request)
        {
            return Clients.All
                .SendAsync($"RegistrationDone{phoneNumber}", request);
        }

        public Task RegistrationFailed(string phoneNumber, string reasonMsg)
        {
            return Clients.All
                .SendAsync($"RegistrationFailed{phoneNumber}", reasonMsg);
        }
        #endregion

        #region ContactCodes
        public Task RequestCallingCodes(string phoneNumber)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestCallingCodes", phoneNumber);
        }

        public Task ResponseCallingCodes(string phoneNumber, List<CallingCodeDto> callingCodes)
        {
            return Clients.All
                .SendAsync($"ResponseCallingCodes{phoneNumber}", callingCodes);
        }
        #endregion

        #region Connected/Disco.
        public override async Task OnConnectedAsync()
        {
            // TODO: check here - this non-authorized user's ID and save for later use
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
