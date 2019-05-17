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
        #region SaveDirectoryMetadata / upload file
        public Task SaveDirectoryMetadata(string appId, string phoneNumber, DirectoryDto directoryDto)
        {
            if (Clients.Group("dataspaceMicroservice") != null)
            {
                return Clients.Group("dataspaceMicroservice").SendAsync("SaveDirectoryMetadata", appId, phoneNumber, directoryDto);
            }
            else
            {
                return Clients.All.SendAsync("SaveDirectoryMetadata", appId, phoneNumber, directoryDto);
            }
        }

        // TODO, return metadata not only response-string
        public Task SaveDirectoryMetadataSuccess(string appId, NodeDto dirDto)
        {
            return Clients.All.SendAsync($"SaveDirectoryMetadataSuccess{appId}", dirDto);
        }

        public Task SaveDirectoryMetadataFail(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"SaveDirectoryMetadataFail{appId}", reasonMsg);
        }
        public Task DeleteDirectoryMetadata(string appId, string phoneNumber, string directoryPath)
        {
            if (Clients.Group("dataspaceMicroservice") != null)
            {
                return Clients.Group("dataspaceMicroservice").SendAsync("DeleteDirectoryMetadata", appId, phoneNumber, directoryPath);
            }
            else
            {
                return Clients.All.SendAsync("DeleteDirectoryMetadata", appId, phoneNumber, directoryPath);
            }
        }
        public Task DeleteDirectoryMetadataSuccess(string appId, string directoryPath)
        {
            return Clients.All.SendAsync($"DeleteDirectoryMetadataSuccess{appId}", directoryPath);
        }

        public Task DeleteDirectoryMetadataFail(string appId, string directoryPath, string reasonMsg)
        {
            return Clients.All.SendAsync($"DeleteDirectoryMetadataFail{appId}", directoryPath, reasonMsg);
        }
        #endregion

        #region DeleteFileMetadata
        public Task DeleteFileMetadata(string appId, string phonenumber, string filePath)
        {
            if (Clients.Group("dataspaceMicroservice") != null)
            {
                return Clients.Group("dataspaceMicroservice").SendAsync("DeleteFileMetadata", appId, phonenumber, filePath);
            }
            else
            {
                return Clients.All.SendAsync("DeleteFileMetadata", appId, phonenumber, filePath);
            }
        }
        public Task DeleteFileMetadataSuccess(string appId, string filePath)
        {
            return Clients.All.SendAsync($"DeleteFileMetadataSuccess{appId}", filePath);
        }

        public Task DeleteFileMetadataFail(string appId, string filePath, string reasonMsg)
        {
            return Clients.All.SendAsync($"DeleteFileMetadataFail{appId}", filePath, reasonMsg);
        }
        #endregion

        #region SaveFileMetadata / upload file
        public Task SaveFileMetadata(string appId, string phonenumber, FileDto fileDto)
        {
            if (Clients.Group("dataspaceMicroservice") != null)
            {
                return Clients.Group("dataspaceMicroservice").SendAsync("SaveFileMetadata", appId, phonenumber, fileDto);
            }
            else
            {
                return Clients.All.SendAsync("SaveFileMetadata", appId, phonenumber, fileDto);
            }
        }

        // TODO, return metadata not only response-string
        public Task SaveFileMetadataSuccess(string appId, NodeDto fileDto)
        {
            return Clients.All.SendAsync($"UploadFileSuccess{appId}", fileDto);
        }

        public Task SaveFileMetadataFail(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"UploadFileFail{appId}", reasonMsg);
        }
        #endregion

        #region RequestFilesMetadata / get list of files and dirs
        public Task RequestFilesMetaData(string appId, string phoneNumber)
        {
            if (Clients.Group("dataspaceMicroservice") != null)
            {
                return Clients.Group("dataspaceMicroservice").SendAsync("RequestFilesMetaData", appId, phoneNumber);
            }
            else
            {
                return Clients.All.SendAsync("RequestFilesMetaData", appId, phoneNumber);
            }
        }

        // TODO, return metadata not only response-string
        public Task RequestFilesMetaDataSuccess(string appId, DataSpaceMetadata fileDto)
        {
            return Clients.All.SendAsync($"RequestFilesMetaDataSuccess{appId}", fileDto);
        }

        public Task RequestFileMetaDataFail(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"RequestFilesMetaDataFail{appId}", reasonMsg);
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
