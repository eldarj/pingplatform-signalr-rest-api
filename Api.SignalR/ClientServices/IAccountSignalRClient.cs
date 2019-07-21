using Ping.Commons.Dtos.Models.Various;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.SignalR.ClientServices
{
    public interface IAccountSignalRClient
    {
        void AvatarUpload(string accountPhoneNumber, string avatarImgUrl);
        void CoverUpload(string accountPhoneNumber, string coverImgUrl);

        Task<T> GetAccounts<T>();

        Task<T> Authenticate<T, K>(K Model);
    }
}
