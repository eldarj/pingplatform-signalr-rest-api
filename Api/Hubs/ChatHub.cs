using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Ping.Commons.Dtos.Models.Wrappers.Response;
using Ping.Commons.SignalR.Extensions;
using Ping.Commons.Dtos.Models.Auth;
using Ping.Commons.Dtos.Models.Chat;
using System.Security.Claims;
using Ping.Commons.Dtos.Models.Emojis;
using Ping.Commons.Dtos.Models.Various;

namespace Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly string MicroserviceHandlerIdentifier = "ChatMicroservice";

        #region Messages
        public Task SendMessage(MessageDto newMessageDto)
        {
            newMessageDto.Sender = this.NameIdentifier();

            Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("SendMessage", newMessageDto);

            return Clients.User(newMessageDto.Receiver)
                .SendAsync($"ReceiveMessage", newMessageDto);
        }

        public Task GetEmojis()
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("GetEmojis", this.NameIdentifier());
        }

        public Task EmojisResponse(string phoneNumber, List<EmojiCategoryDto> emojiCategories)
        {
            return Clients.User(phoneNumber)
                .SendAsync("EmojisResponse", emojiCategories);
        }
        #endregion

        #region CrudContacts
        public Task UpdateContact(ContactDto response)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("UpdateContact", this.NameIdentifier(), response);
        }

        public Task UpdateContactResponse(string phoneNumber, ResponseDto<ContactDto> response)
        {
            return Clients.User(phoneNumber)
                .SendAsync("UpdateContact", response);
        }

        public Task AddContact(ContactDto newContactDto)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("AddContact", this.NameIdentifier(), newContactDto);
        }

        public Task AddContactResponse(string phoneNumber, ResponseDto<ContactDto> response)
        {
            return Clients.User(phoneNumber)
                .SendAsync("AddContactResponse", response);
        }

        public Task RequestContacts()
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestContacts", this.NameIdentifier());
        }

        public Task RequestContactsResponse(string phoneNumber, ResponseDto<List<ContactDto>> response) // TODO: Check whether we should use responseDto wrapper
        {
            return Clients.User(phoneNumber)
                .SendAsync($"RequestContactsResponse", response);
        }

        // TODO: Unused - either this or Line:79
        public Task RequestContactsSuccess(string phoneNumber, List<ContactDto> contacts) 
        {
            return Clients.User(phoneNumber)
                .SendAsync($"RequestContactsSuccess", contacts);
        }

        public Task RequestContactsFail(string phoneNumber, string errorMessage)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"RequestContactsFail", errorMessage);
        }

        public Task LoadMessages(int pageNumber)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("LoadMessages", this.NameIdentifier(), pageNumber);
        }

        public Task LoadMessagesSuccess(string phoneNumber, PagedList<MessageDto> response)
        {
            return Clients.User(phoneNumber)
                .SendAsync("LoadMessagesSuccess", response);
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
