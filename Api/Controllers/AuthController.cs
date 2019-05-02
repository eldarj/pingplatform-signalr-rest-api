using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DtoModels.Auth;
using Api.Controllers.Base;
using Api.SignalR;
using Api.SignalR.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ApiBaseController
    {
        public AuthController(IAccountSignalRClient accountSignalRClient) : base(accountSignalRClient) { }

        // POST: api/authenticate
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AccountLoginDto RequestModel)
        {
            // provjeri postoji li s ovim telefonom
            // ako postoji provjeri uradi auth
            // ako ne postoji bad request

            AccountDto pox = await accountSignalRClient.Authenticate<AccountDto, AccountLoginDto>(RequestModel);


            return Ok(pox);

            //try
            //{
            //    if (!await IdentityService.IsMember(RequestModel.Telefon))
            //        return NotFound();

            //    User user = await IdentityService.Authenticate(RequestModel.Telefon);
            //    return RequestModel.CreateSession ?
            //        UserRequest.GetInstance(user, await IdentityService.CreateSession(user)) :
            //        UserRequest.GetInstance(user);
            //}
            //catch (Exception e)
            //{
            //    var x = e;
            //    return BadRequest();
            //}
        }
    }
}