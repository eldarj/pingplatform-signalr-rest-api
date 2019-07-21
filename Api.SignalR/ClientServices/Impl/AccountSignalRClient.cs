using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Ping.Commons.Settings;
using Ping.Commons.SignalR.Base;
using System;
using System.Threading.Tasks;

namespace Api.SignalR.ClientServices.Impl
{
    public class AccountSignalRClient : BaseHubClientService, IAccountSignalRClient
    {
        private static readonly string HUB_ENDPOINT = "accounthub";

        public AccountSignalRClient(IOptions<GatewayBaseSettings> gatewayBaseOptions,
            IOptions<SecuritySettings> securityOptions)
            : base(gatewayBaseOptions, securityOptions, HUB_ENDPOINT)
        {
            this.Connect(); // Self-start connection
        }

        public async void Connect()
        {
            await hubConnection.StartAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    //logger.LogInformation("-- Couln't connect to SignalR AccountHub (OnStarted)");
                    string pxo = "yes";

                    return;
                }
                //logger.LogInformation("AccountMicroservice connected to AuthHub successfully (OnStarted)");
                //this.RegisterHandlers(); // We won't be registering any handlers from here
                string po = "yes";
            });
        }

        // TODO: Remove this or leave for API reference (eg. auth. ove api and not signalR hubs)
        public Task<T> Authenticate<T, K>(K Model)
        {
            return hubConnection.InvokeAsync<T>("AuthAccount", Model);
        }

        public Task<T> GetAccounts<T>()
        {
            return hubConnection.InvokeAsync<T>("GetAccounts");
        }

        public void AvatarUpload(string accountPhoneNumber, string avatarImgUrl)
        {
            hubConnection.SendAsync("AvatarUpload", accountPhoneNumber, avatarImgUrl);
        }
        public void CoverUpload(string accountPhoneNumber, string coverImgUrl)
        {
            hubConnection.SendAsync("CoverUpload", accountPhoneNumber, coverImgUrl);
        }
    }
}
