﻿using Microsoft.AspNetCore.Http;
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
        public Task SaveDirectoryMetadataSuccess(string appId, DirectoryDto dirDto)
        {
            return Clients.All.SendAsync($"SaveDirectoryMetadataSuccess{appId}", dirDto);
        }

        public Task SaveDirectoryMetadataFail(string appId, string reasonMsg)
        {
            return Clients.All.SendAsync($"SaveDirectoryMetadataFail{appId}", reasonMsg);
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

        #region DeleteFileMetadata
        public Task DeleteFileMetadata(string appId, string phonenumber, string filename)
        {
            if (Clients.Group("dataspaceMicroservice") != null)
            {
                return Clients.Group("dataspaceMicroservice").SendAsync("DeleteFileMetadata", appId, phonenumber, filename);
            }
            else
            {
                return Clients.All.SendAsync("DeleteFileMetadata", appId, phonenumber, filename);
            }
        }
        public Task DeleteFileMetadataSuccess(string appId, string fileName)
        {
            return Clients.All.SendAsync($"DeleteFileMetadataSuccess{appId}", fileName);
        }

        public Task DeleteFileMetadataFail(string appId, string fileName, string reasonMsg)
        {
            return Clients.All.SendAsync($"DeleteFileMetadataFail{appId}", fileName, reasonMsg);
        }
        #endregion

        #region SaveFileMetadata / upload file
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
