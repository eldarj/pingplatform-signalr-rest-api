using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Api.SignalR.ClientServices.Impl
{
    public class AccountSignalRClient : BaseServiceClient, IAccountSignalRClient
    {
        public AccountSignalRClient()
        {
            this.Connect();
        }

        protected override void Connect()
        {
            base.Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44380/accounthub")
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

        // TODO: Remove this or leave for API reference (eg. auth. ove api and not signalR hubs)
        public Task<T> Authenticate<T, K>(K Model)
        {
            return Connection.InvokeAsync<T>("AuthAccount", Model);
        }

        public Task<T> GetAccounts<T>()
        {
            return Connection.InvokeAsync<T>("GetAccounts");
        }

        public void AvatarUpload(string appId, string accountPhoneNumber, string avatarImgUrl)
        {
            Connection.SendAsync("AvatarUpload", appId, accountPhoneNumber, avatarImgUrl);
        }
        public void CoverUpload(string appId, string accountPhoneNumber, string coverImgUrl)
        {
            Connection.SendAsync("CoverUpload", appId, accountPhoneNumber, coverImgUrl);
        }
    }
}
