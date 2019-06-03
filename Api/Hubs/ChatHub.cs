using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Ping.Commons.Dtos.Models.Auth;
using Ping.Commons.Dtos.Models.Chat;
using Ping.Commons.Dtos.Models.Wrappers.Response;
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
        #region Messages
        public Task SendMessage(string phoneNumber, MessageDto newMessageDto)
        {
            newMessageDto.Sender = phoneNumber;
            if (Clients.Group("chatMicroservice") != null)
            {
                Clients.Group("chatMicroservice").SendAsync("SendMessage", newMessageDto);
            }
            return Clients.All.SendAsync($"ReceiveMessage{newMessageDto.Receiver}", newMessageDto);
        }

        #endregion
        #region Contacts
        public Task UpdateContact(string appId, string phoneNumber, ContactDto contactDto)
        {
            if (Clients.Group("chatMicroservice") != null)
            {
                return Clients.Group("chatMicroservice").SendAsync("UpdateContact", appId, phoneNumber, contactDto);
            }
            else
            {
                return Clients.All.SendAsync("AddContact", appId, phoneNumber);
            }
        }
        public Task AddContact(string appId, string phoneNumber, ContactDto newContactDto)
        {
            if (Clients.Group("chatMicroservice") != null)
            {
                return Clients.Group("chatMicroservice").SendAsync("AddContact", appId, phoneNumber, newContactDto);
            }
            else
            {
                return Clients.All.SendAsync("AddContact", appId, phoneNumber);
            }
        }

        public Task AddContactResponse(string appId, ResponseDto<ContactDto> contactDto)
        {
            return Clients.All.SendAsync($"AddContactResponse{appId}", contactDto);
        }

        public Task AddContactFail(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"AddContactFail{appId}", reasonMsg);
        }
        // NOT USED::THINK ABOUT USING THIS FOR SENDING OUT NOTIFICATIONS AFTER INVITES
        public Task ContactRegisteredOnPing(string phoneNumber, ContactDto contactDto)
        {
            return Clients.All.SendAsync($"ContactRegisteredOnPing{phoneNumber}", contactDto);
        }

        //
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

        public Task RequestContactsSuccess(string appId, List<ContactDto> contactDto)
        {
            return Clients.All.SendAsync($"RequestContactsSuccess{appId}", contactDto);
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
