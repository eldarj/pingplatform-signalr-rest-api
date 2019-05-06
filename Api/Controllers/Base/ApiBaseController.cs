using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.SignalR.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    public class ApiBaseController : ControllerBase
    {
        protected IAccountSignalRClient accountSignalRClient;
        protected IDataSpaceSignalRClient dataSpaceSignalRClient;

        public ApiBaseController(IAccountSignalRClient accountSignalRClient)
        {
            this.accountSignalRClient = accountSignalRClient;
        }

        public ApiBaseController(IDataSpaceSignalRClient dataSpaceSignalRClient)
        {
            this.dataSpaceSignalRClient = dataSpaceSignalRClient;
        }

        public ApiBaseController()
        {

        }
    }
}