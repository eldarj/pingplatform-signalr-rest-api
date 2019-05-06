using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Controllers.Base;
using Api.SignalR.ClientServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ping.Commons.Dtos.Models.Various;

namespace Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ApiBaseController
    {
        private readonly IHostingEnvironment appEnv;

        public AccountController(IHostingEnvironment hostingEnvironment, IAccountSignalRClient accountSignalRClient) 
            : base(accountSignalRClient)
        {
            appEnv = hostingEnvironment;
        }
        // UPLOAD COVER IMAGE: api/account/profile/cover

        [HttpPost]
        [Route("profile/cover")]
        [HttpPost]
        public IActionResult CoverUpload([FromBody] ImageUploadRequest request) // TODO: Chance ImageUploadRequest to integrate IFormFile
        {
            if (request.PhoneNumber == null || !(request.PhoneNumber.Length > 0))
            {
                return BadRequest("Missing phone number (auth.)");
            }
            if (request.Base64Image == null || !(request.Base64Image.Length > 0))
            {
                return BadRequest("Missing base64 encoded image string.");
            }

            try
            {
                string Filename = GenerateFileName(request);
                string UploadsDir = Path.Combine(appEnv.WebRootPath, "users/covers");
                string WritePath = Path.Combine(UploadsDir, Filename); // Pripremi path i ime slike

                byte[] imageBytes = Convert.FromBase64String(request.Base64Image);
                System.IO.File.WriteAllBytes(WritePath, imageBytes);

                string imgUploadedTo = "https://localhost:44380/users/covers/" + Filename;

                // save the url
                accountSignalRClient.CoverUpload(request.appId, request.PhoneNumber, imgUploadedTo);

                return NoContent();
            }
            catch (Exception e)
            {
                // handle ili samo pusti da akcija vrati bad request?
                return BadRequest("Dogodila se greska pri konverziji base64 u sliku.");
            }
        }

        // UPLOAD AVATAR IMAGE: api/account/profile/avatar
        [HttpPost]
        [Route("profile/avatar")]
        [HttpPost]
        public IActionResult AvatarUpload([FromBody] ImageUploadRequest request)
        {
            if (request.PhoneNumber == null || !(request.PhoneNumber.Length > 0))
            {
                return BadRequest("Missing phone number (auth.)");
            }
            if (request.Base64Image == null || !(request.Base64Image.Length > 0))
            {
                return BadRequest("Missing base64 encoded image string.");
            }

            try
            {
                string Filename = GenerateFileName(request);
                string UploadsDir = Path.Combine(appEnv.WebRootPath, "users/avatars");
                string WritePath = Path.Combine(UploadsDir, Filename); // Pripremi path i ime slike

                byte[] imageBytes = Convert.FromBase64String(request.Base64Image);
                System.IO.File.WriteAllBytes(WritePath, imageBytes);

                string imgUploadedTo = "https://localhost:44380/users/avatars/" + Filename;

                // save the url
                accountSignalRClient.AvatarUpload(request.appId, request.PhoneNumber, imgUploadedTo);

                return NoContent();
            }
            catch (Exception e)
            {
                // handle ili samo pusti da akcija vrati bad request?
                return BadRequest("Dogodila se greska pri konverziji base64 u sliku.");
            }
        }

        private static string GenerateFileName(ImageUploadRequest request)
        {
            // Example: EldarJahijagic_filename_GUID.jpeg
            return request.Firstname + request.Lastname + "_" + request.FileName + "_" +
                Guid.NewGuid().ToString().Substring(0, 4) + "." + request.FileExtension;
        }
    }
}