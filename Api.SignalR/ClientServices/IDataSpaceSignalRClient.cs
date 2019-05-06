using Ping.Commons.Dtos.Models.DataSpace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.SignalR.ClientServices
{
    public interface IDataSpaceSignalRClient
    {
        void SaveFileMetadata(string appId, FileUploadDto filePath);
    }
}
