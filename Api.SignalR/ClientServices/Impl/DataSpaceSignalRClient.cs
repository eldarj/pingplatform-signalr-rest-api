using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Ping.Commons.Dtos.Models.DataSpace;
using Ping.Commons.Settings;
using Ping.Commons.SignalR.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.SignalR.ClientServices.Impl
{
    public class DataSpaceSignalRClient : BaseHubClientService, IDataSpaceSignalRClient
    {
        private static readonly string HUB_ENDPOINT = "dataspacehub";

        public DataSpaceSignalRClient(IOptions<GatewayBaseSettings> gatewayBaseOptions,
            IOptions<SecuritySettings> securityOptions)
            : base(gatewayBaseOptions, securityOptions, HUB_ENDPOINT)
        {
            this.Connect();
        }

        public async void Connect()
        {
            await hubConnection.StartAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    return;
                }
            });
        }

        public void SaveFileMetadata(string phonenumber, FileDto fileUploadDto)
        {
            hubConnection.SendAsync("SaveFileMetadata", phonenumber, fileUploadDto);
        }

        public void DeleteFileMetadata(string phonenumber, string filename)
        {
            hubConnection.SendAsync("DeleteFileMetadata", phonenumber, filename);
        }

        public void DeleteDirectoryMetadata(string phoneNumber, string directoryPath)
        {
            hubConnection.SendAsync("DeleteDirectoryMetadata", phoneNumber, directoryPath);
        }

        public void SaveDirectoryMetadata(string phonenumber, DirectoryDto directoryDto)
        {
            hubConnection.SendAsync("SaveDirectoryMetadata", phonenumber, directoryDto);
        }

        public void DeleteMultipleNodesMetadata(string phonenumber, List<SimpleNodeDto> nodes)
        {
            hubConnection.SendAsync("DeleteMultipleNodesMetadata", phonenumber, nodes);
        }
    }
}
