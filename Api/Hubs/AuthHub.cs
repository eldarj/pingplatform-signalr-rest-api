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
        public Task RequestAuthentication(string anonymousIdentifier, AccountDto request)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestAuthentication", anonymousIdentifier, request);
        }

        public Task AuthenticationDone(string anonymousIdentifier, ResponseDto<AccountDto> response)
        {
            return Clients.All
                .SendAsync($"AuthenticationDone{anonymousIdentifier}", response);
        }

        public Task AuthenticationFailed(string anonymousIdentifier, ResponseDto<AccountDto> response) // TODO: Make all response ResponseDto
        {
            //return Clients.User(responseDto.Dto.PhoneNumber)
            //    .SendAsync($"AuthenticationFailed", responseDto);
            return Clients.All
                .SendAsync($"AuthenticationFailed{anonymousIdentifier}", response);
        }
        #endregion

        #region Register
        public Task RequestRegistration(string anonymousIdentifier, AccountDto request)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestRegistration", anonymousIdentifier, request);
        }

        public Task RegistrationDone(string anonymousIdentifier, ResponseDto<AccountDto> response)
        {
            return Clients.All
                .SendAsync($"RegistrationDone{anonymousIdentifier}", response);
        }

        public Task RegistrationFailed(string anonymousIdentifier, ResponseDto<AccountDto> response)
        {
            return Clients.All
                .SendAsync($"RegistrationFailed{anonymousIdentifier}", response);
        }
        #endregion

        #region ContactCodes
        public Task RequestCallingCodes(string anonymousIdentifier)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestCallingCodes", anonymousIdentifier);
        }

        // TODO: Fix this to make use of Clients.User(phoneNumber) instead of Clients.All and anonymousIdentifier
        public Task ResponseCallingCodes(string anonymousIdentifier, List<CallingCodeDto> callingCodes)
        {
            return Clients.All
                .SendAsync($"ResponseCallingCodes{anonymousIdentifier}", callingCodes);
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
