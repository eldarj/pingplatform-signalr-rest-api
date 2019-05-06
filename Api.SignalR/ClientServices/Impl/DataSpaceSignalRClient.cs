using Microsoft.AspNetCore.SignalR.Client;
using Ping.Commons.Dtos.Models.DataSpace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.SignalR.ClientServices.Impl
{
    public class DataSpaceSignalRClient : BaseServiceClient, IDataSpaceSignalRClient
    {
        public DataSpaceSignalRClient()
        {
            this.Connect();
        }

        protected override void Connect()
        {
            base.Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44380/dataspacehub")
                .Build();

            try
            {
                base.Connection.StartAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        //
                    }
                    else
                    {
                        //
                    }
                }).Wait();
            }
            catch (Exception ex)
            {
                //
            }
        }

        public void SaveFileMetadata(string appId, FileUploadDto fileUploadDto)
        {
            Connection.SendAsync("SaveFileMetadata", appId, fileUploadDto);
        }
    }
}
