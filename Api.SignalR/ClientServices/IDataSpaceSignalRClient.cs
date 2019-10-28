using Ping.Commons.Dtos.Models.DataSpace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.SignalR.ClientServices
{
    public interface IDataSpaceSignalRClient
    {
        void SaveFileMetadata(string phonenumber, FileDto fileUploadDto);
        void SaveDirectoryMetadata(string phoneNumber, DirectoryDto directoryDto);

        void DeleteMultipleNodesMetadata(string phonenumber, List<SimpleNodeDto> nodes);

        void DeleteFileMetadata(string phonenumber, string filename);

        void DeleteDirectoryMetadata(string phoneNumber, string directoryPath);

    }
}
