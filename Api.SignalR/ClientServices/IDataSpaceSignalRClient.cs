using Ping.Commons.Dtos.Models.DataSpace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.SignalR.ClientServices
{
    public interface IDataSpaceSignalRClient
    {
        void SaveFileMetadata(string appId, FileUploadDto filePath);

        void DeleteFileMetadata(string appId, string phonenumber, string filename);

        void SaveDirectoryMetadata(string appId, string phoneNumber, DirectoryDto dirname);
    }
}
