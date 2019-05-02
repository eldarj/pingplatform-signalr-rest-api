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

        public ApiBaseController(IAccountSignalRClient accountSignalRClient)
        {
            this.accountSignalRClient = accountSignalRClient;
        }
    }
}