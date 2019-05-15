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

namespace Api.Controllers
{
    [Route("api/dataspace")]
    [ApiController]
    public class DirectoriesController : ApiBaseController
    {
        private readonly IHostingEnvironment appEnv;

        public DirectoriesController(IHostingEnvironment hostingEnvironment, IDataSpaceSignalRClient dataSpaceSignalRClient)
            : base(dataSpaceSignalRClient)
        {
            appEnv = hostingEnvironment;
        }

        // POST: eldarja/directories/?dir1/dir2/mydir/mysubdir...

        [Route("{username}/directories/{*directoryPath}")]
        [HttpPost]
        public IActionResult CreateDirectory([FromRoute] string username, [FromRoute] string directoryPath, [FromBody] DirectoryDto directoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // TODO: Check for what is 'Path' used and introduce new property 'Url'
            string physicalPath;
            if (String.IsNullOrWhiteSpace(directoryPath))
            {
                physicalPath = Path.Combine(appEnv.WebRootPath,
                    String.Format(@"dataspace/{0}/{1}", username, directoryDto.DirName)); // TODO FIX THIS AND ADD URL FIELD TO DTO MODEL
                directoryDto.Path = $"{Request.Scheme}://{Request.Host}/dataspace/{username}/{directoryDto.DirName}";
            }
            else
            {
                physicalPath = Path.Combine(appEnv.WebRootPath,
                    String.Format(@"dataspace/{0}/{1}/{2}", username, directoryPath, directoryDto.DirName));
                directoryDto.Path = $"{Request.Scheme}://{Request.Host}/dataspace/{username}/{directoryPath}/{directoryDto.DirName}";
            }


            var appId = Request.Headers["AppId"];
            var phonenumber = Request.Headers["OwnerPhoneNumber"];

            if (System.IO.Directory.Exists(directoryDto.Path))
            {
                return BadRequest();
            }

            System.IO.Directory.CreateDirectory(physicalPath);

            dataSpaceSignalRClient.SaveDirectoryMetadata(appId, phonenumber, directoryDto);
            return Ok();
        }


        [Route("{username}/directories/{*directoryPath}")]
        [HttpDelete]
        public IActionResult DeleteDirectory([FromRoute] string username, [FromRoute] string directoryPath)
        {
            if (String.IsNullOrWhiteSpace(directoryPath))
            {
                return BadRequest();
            }

            string physicalPath = Path.Combine(appEnv.WebRootPath, String.Format(@"dataspace/{0}/{1}", username, directoryPath));
            var appId = Request.Headers["AppId"];
            var phonenumber = Request.Headers["OwnerPhoneNumber"];

            // ((( NOT TRUE: we have a try/catch ))) We won't check whether the file exists on filesystem, because we want to delete any metadata from DB anyway
            try
            {
                System.IO.Directory.Delete(physicalPath);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            dataSpaceSignalRClient.DeleteDirectoryMetadata(appId, phonenumber, directoryPath);
            return NoContent();
        }
    }
}