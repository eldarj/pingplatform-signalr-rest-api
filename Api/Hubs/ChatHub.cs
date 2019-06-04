using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Ping.Commons.Dtos.Models.Wrappers.Response;
using Ping.Commons.Dtos.Models.Auth;
using Ping.Commons.Dtos.Models.Chat;

namespace Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly string MicroserviceHandlerIdentifier = "ChatMicroservice"; // Change this to accept role Claim identifiers

        #region Messages
        public Task SendMessage(MessageDto newMessageDto)
        {
            newMessageDto.Sender = Context.User.Identity.Name;

            Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("SendMessage", Context.User.Identity.Name, newMessageDto);

            return Clients.User(newMessageDto.Receiver)
                .SendAsync($"ReceiveMessage", newMessageDto);
        }

        #endregion
        #region CrudContacts
        public Task UpdateContact(ContactDto contactDto) // TODO: Wrap this up
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("UpdateContact", Context.User.Identity.Name, contactDto);
        }
        public Task AddContact(ContactDto newContactDto)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("AddContact", Context.User.Identity.Name, newContactDto);
        }

        public Task AddContactResponse(string phoneNumber, ResponseDto<ContactDto> contactDto)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"AddContactResponse", contactDto);
        }

        public Task AddContactFail(string phoneNumber, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"AddContactFail", reasonMsg);
        }
        #endregion

        #region GetAllContacts
        public Task RequestContacts()
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestContacts", Context.User.Identity.Name);
        }

        public Task RequestContactsSuccess(string phoneNumber, List<ContactDto> contactDto)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"RequestContactsSuccess", contactDto);
        }

        public Task RequestContactsFail(string phoneNumber, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"RequestContactsFail", reasonMsg);
        }
        #endregion
        
        // NOT USED::THINK ABOUT USING THIS FOR SENDING OUT NOTIFICATIONS AFTER INVITES
        public Task ContactRegisteredOnPing(string phoneNumber, ContactDto contactDto)
        {
            return Clients.All
                .SendAsync($"ContactRegisteredOnPing{phoneNumber}", contactDto);
        }
    }
}
