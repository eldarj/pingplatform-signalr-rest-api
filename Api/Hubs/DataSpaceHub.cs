using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Ping.Commons.Dtos.Models.DataSpace;
using System;
using System.Collections.Generic;
using Ping.Commons.SignalR.Extensions;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Api.Hubs
{
    [Authorize]
    public class DataSpaceHub : Hub
    {
        private static readonly string MicroserviceHandlerIdentifier = "DataspaceMicroservice";

        #region DeleteMultipleNodes
        public Task DeleteMultipleNodesMetadata(string phoneNumber, List<SimpleNodeDto> nodes)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("DeleteMultipleNodesMetadata", phoneNumber, nodes);
        }
        
        // Merge into one endpoint?
        public Task DeleteMultipleNodesMetadataSuccess(string phoneNumber, List<SimpleNodeDto> nodes)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"DeleteMultipleNodesMetadataSuccess", nodes);
        }

        public Task DeleteMultipleNodesMetadataFail(string phoneNumber, List<SimpleNodeDto> nodes, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"DeleteMultipleNodesMetadataFail", nodes, reasonMsg);
        }
        #endregion

        #region SaveDirectoryMetadata / upload file
        public Task SaveDirectoryMetadata(string phonenumber, DirectoryDto directoryDto)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("SaveDirectoryMetadata", phonenumber, directoryDto);
        }

        // TODO, return metadata not only response-string
        public Task SaveDirectoryMetadataSuccess(string phoneNumber, NodeDto dirDto)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"SaveDirectoryMetadataSuccess", dirDto);
        }

        public Task SaveDirectoryMetadataFail(string phoneNumber, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"SaveDirectoryMetadataFail", reasonMsg);
        }

        public Task DeleteDirectoryMetadata(string phoneNumber, string directoryPath)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("DeleteDirectoryMetadata", phoneNumber, directoryPath);
        }
        public Task DeleteDirectoryMetadataSuccess(string phoneNumber, string directoryPath)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"DeleteDirectoryMetadataSuccess", directoryPath);
        }

        public Task DeleteDirectoryMetadataFail(string phoneNumber, string directoryPath, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"DeleteDirectoryMetadataFail", directoryPath, reasonMsg);
        }
        #endregion

        #region DeleteFileMetadata
        public Task DeleteFileMetadata(string phoneNumber, string filePath)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("DeleteFileMetadata", phoneNumber, filePath);
        }
        public Task DeleteFileMetadataSuccess(string phoneNumber, string filePath)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"DeleteFileMetadataSuccess", filePath);
        }

        public Task DeleteFileMetadataFail(string phoneNumber, string filePath, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"DeleteFileMetadataFail", filePath, reasonMsg);
        }
        #endregion

        #region SaveFileMetadata / upload file
        public Task SaveFileMetadata(string phoneNumber, FileDto fileDto)
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("SaveFileMetadata", phoneNumber, fileDto);
        }

        // TODO, return metadata not only response-string
        public Task SaveFileMetadataSuccess(string phoneNumber, NodeDto fileDto)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"UploadFileSuccess", fileDto);
        }

        public Task SaveFileMetadataFail(string phoneNumber, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"UploadFileFail", reasonMsg);
        }
        #endregion

        #region RequestFilesMetadata / get list of files and dirs
        public Task RequestFilesMetaData()
        {
            return Clients.User(MicroserviceHandlerIdentifier)
                .SendAsync("RequestFilesMetaData", this.NameIdentifier());
        }

        // TODO, return metadata not only response-string
        public Task RequestFilesMetaDataSuccess(string phoneNumber, DataSpaceMetadata fileDto)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"RequestFilesMetaDataSuccess", fileDto);
        }

        public Task RequestFilesMetaDataFail(string phoneNumber, string reasonMsg)
        {
            return Clients.User(phoneNumber)
                .SendAsync($"RequestFilesMetaDataFail", reasonMsg);
        }
        #endregion
    }
}
