using Ping.Commons.Dtos.Models.DataSpace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.SignalR.ClientServices
{
    public interface IDataSpaceSignalRClient
    {
        void SaveFileMetadata(string appId, string phonenumber, FileDto fileUploadDto);
        void SaveDirectoryMetadata(string appId, string phoneNumber, DirectoryDto directoryDto);

        void DeleteMultipleNodesMetadata(string appId, string phonenumber, List<SimpleNodeDto> nodes);

        void DeleteFileMetadata(string appId, string phonenumber, string filename);

        void DeleteDirectoryMetadata(string appId, string phoneNumber, string directoryPath);

    }
}
