using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Ping.Commons.Dtos.Models.DataSpace;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Api.Hubs
{
    public class DataSpaceHub : Hub
    {
        public Task SaveFileMetadata(string appId, FileUploadDto fileDto)
        {
            if (Clients.Group("dataspaceMicroservice") != null)
            {
                return Clients.Group("dataspaceMicroservice").SendAsync("SaveFileMetadata", appId, fileDto);
            }
            else
            {
                return Clients.All.SendAsync("SaveFileMetadata", appId, fileDto);
            }
        }

        // TODO, return metadata not only response-string
        public Task SaveFileMetadataSuccess(string appId, FileUploadDto fileDto)
        {
            return Clients.All.SendAsync($"UploadFileSuccess{appId}", fileDto);
        }

        public Task SaveFileMetadataFail(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"UploadFileFail{appId}", reasonMsg);
        }

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
