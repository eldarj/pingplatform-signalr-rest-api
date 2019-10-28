using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Controllers.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Api.Filters.ControllerAttributes;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Api.SignalR.ClientServices;
using Ping.Commons.Dtos.Models.DataSpace;

namespace Api.Controllers
{
    [Route("api/dataspace")]
    [ApiController]
    public class FilesController : ApiBaseController
    {
        private readonly IHostingEnvironment appEnv;

        public FilesController(IHostingEnvironment hostingEnvironment, IDataSpaceSignalRClient dataSpaceSignalRClient)
            : base(dataSpaceSignalRClient)
        {
            appEnv = hostingEnvironment;
        }

        [Route("{username}/files/{*filePath}")]
        [HttpGet]
        public IActionResult DownloadFile([FromRoute] string username, [FromRoute] string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                return BadRequest();
            }

            string physicalPath = Path.Combine(appEnv.WebRootPath, String.Format(@"dataspace/{0}/{1}", username, filePath));
            string acceptHeader = Request.Headers["Accept"];

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(physicalPath, out contentType) || acceptHeader.Equals("application/octet-stream"))
            {
                contentType = "application/octet-stream";
            }

            // OPT 2
            FileStream fs = new FileStream(physicalPath, FileMode.Open, FileAccess.ReadWrite);
            return new FileStreamResult(fs, contentType) {
                EnableRangeProcessing = true
            };

            //// OPT 3
            //var response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(System.IO.File.OpenRead(physicalPath));
            //var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(FilePath));
            //response.Content.Headers.Add("Content-Type", contentType);
            //return response;

            //// OPT 1
            //return File(System.IO.File.OpenRead(physicalPath), contentType);
        }

        // Delete multiple files
        [Route("{username}/files")]
        [HttpDelete]
        public IActionResult Delete([FromRoute] string username, [FromBody] List<SimpleNodeDto> nodes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (nodes.Count <= 0)
            {
                return BadRequest("Requested list of files and directories to be deleted, must not be empty.");
            }

            if (!TryBatchDelete(username, nodes))
            {
                return BadRequest("Error while trying to delete files - check passed list of files or dirs.");
            }

            var appId = Request.Headers["AppId"];
            var phonenumber = Request.Headers["OwnerPhoneNumber"];

            dataSpaceSignalRClient.DeleteMultipleNodesMetadata(phonenumber, nodes);
            return NoContent();
        }

        // Delete file from filesystem and send a SignalR event, for DataSpaceMicroservice, to delete metadata within the db aswell
        [Route("{username}/files/{*filePath}")]
        [HttpDelete]
        public IActionResult DeleteFile([FromRoute] string username, [FromRoute] string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                return BadRequest();
            }

            string physicalPath = Path.Combine(appEnv.WebRootPath, String.Format(@"dataspace/{0}/{1}", username, filePath));
            var appId = Request.Headers["AppId"];
            var phonenumber = Request.Headers["OwnerPhoneNumber"];

            // ((( COMMENT BELOW IS NOT TRUE: we have a try/catch block ))) 
            // We won't check whether the file exists on filesystem, because we want to delete any metadata from DB anyway
            try
            {
                System.IO.File.Delete(physicalPath);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            dataSpaceSignalRClient.DeleteFileMetadata(phonenumber, filePath);
            return NoContent();
        }

        // TOODO - Add restful Directory endpoints, for uploading files within a specific directory


        // TODO - DONE: read a json string from byte[] into a string and write as a .txt file (System.IO.Write)
        //      - DONE: write a picture or document as byte[] into a file (System.IO.Write)
        //      - DONE: convert the receiving stream into another stream and write into a file, instead of loading the entire byte[] into memory
        //      - check memory usage for above scenarios and: MultipartForm, IFormFile, Request.Body.ReadAsync (stream)
        //      - handle multiple files
        //      - check MultipartForm
        //      - check axios:stream

        // POST: api/dataspace/eldar/files
        [Route("{username}/files/{*directoryPath}")]
        [HttpPost]
        [DisableRequestSizeLimit]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadFile([FromRoute] string username, [FromRoute] string directoryPath)
        {
            //string requestContentType = Request.ContentType;
            //bool hasFormContentType = Request.HasFormContentType;

            // Accumulate all form key-value pairs in the request (in case we want to do something with other form-fields that are not only files)
            KeyValueAccumulator formAccumulator = new KeyValueAccumulator();
            Dictionary<string, string> sectionDictionary = new Dictionary<string, string>();

            // get off the boundary appended by the form-post
            string boundary = GetBoundary(Request.ContentType);

            try // FIX THIS HUGE TRY BLOCK
            {
                var reader = new MultipartReader(boundary, Request.Body);
                MultipartSection section = null;

                section = await reader.ReadNextSectionAsync();
                while (section != null)
                {
                    // Get the conten header disposition for checking if we are handling a file-form-field or just any other type of field
                    ContentDispositionHeaderValue contentDisposition;
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                    if (hasContentDispositionHeader)
                    {
                        if (contentDisposition.IsFileDisposition())
                        {
                            // Fetch data from header (Delete this after we implement authentication)
                            var appId = Request.Headers["AppId"];
                            var phonenumber = Request.Headers["OwnerPhoneNumber"];
                            var firstname = Request.Headers["OwnerFirstName"];
                            var lastname = Request.Headers["OwnerLastName"];

                            // Here we handle file fields
                            string fileName = contentDisposition.FileName.ToString().Replace(" ","");
                            string UploadsDir = Path.Combine(appEnv.WebRootPath, $"dataspace/{username}");

                            // If the BASE dir doesn't exist, create it
                            Directory.CreateDirectory(UploadsDir);

                            string physicalPath;
                            string url;
                            // Check for subdir
                            if (String.IsNullOrWhiteSpace(directoryPath))
                            {
                                physicalPath = Path.Combine(UploadsDir, fileName); // Prepare file name and path for writing
                                url = $"{Request.Scheme}://{Request.Host}/dataspace/{username}/files/{fileName}";
                                directoryPath = "";
                            }
                            else
                            {
                                physicalPath = Path.Combine(UploadsDir, directoryPath, fileName); // Prepare file name and path for writing
                                url = $"{Request.Scheme}://{Request.Host}/dataspace/{username}/files/{directoryPath}/{fileName}";
                            }

                            //// We can also use a temp location for now eg. AppData on Windows
                            //var targetFilePath = Path.GetTempFileName();

                            // Createa a stream and write the request (section-field) body/data
                            using (var targetStream = System.IO.File.Create(physicalPath))
                            {
                                // Copy file to disk
                                await section.Body.CopyToAsync(targetStream);


                                // TODO: Generate metadata and save the path into a FileMicroservice with SignalR (db handler)
                                FileDto newFile = new FileDto
                                {
                                    Name = fileName,
                                    Path = directoryPath,
                                    Url = url,
                                    MimeType = section.ContentType,
                                    FileSizeInKB = (int)(targetStream.Length / 1024) // Test
                                };

                                dataSpaceSignalRClient.SaveFileMetadata(phonenumber, newFile); // rename stuff like this to nodePath
                            }
                        }
                        else if (contentDisposition.IsFormDisposition())
                        {
                            // Remove this (?)
                            // Here we handle other form fields
                            StringSegment fieldName = HeaderUtilities.RemoveQuotes(contentDisposition.Name);

                            FormMultipartSection formMultipartSection = section.AsFormDataSection();
                            string fieldValue = await formMultipartSection.GetValueAsync().ConfigureAwait(false);

                            sectionDictionary.Add(formMultipartSection.Name, fieldValue);

                            using (var streamReader = new StreamReader(section.Body))
                            {
                                if (String.Equals(fieldValue, "undefined", StringComparison.OrdinalIgnoreCase))
                                {
                                    fieldValue = String.Empty;
                                }
                                formAccumulator.Append(fieldName.ToString(), fieldValue);
                            }

                        }
                    }
                    // Read next section/field
                    section = await reader.ReadNextSectionAsync();
                }
            }
            catch (Exception e)
            {
                // handle any unread or errors on errors while streaming and writing received data
                return BadRequest();
            }

            //// Remove this (?)
            //// Handle all the non-file fields either here, or above while reading the streams already eg. chosen parent directory
            //var result = sectionDictionary;
            //var frmResults = formAccumulator.GetResults();

            return Ok();
        }

        private string GetBoundary(string contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }
            string boundary = contentType;
            try
            {
                string[] elements = contentType.Split(' ');
                string element = elements.First(entry => entry.StartsWith("boundary="));
                boundary = element.Substring("boundary=".Length);
            }
            catch (Exception)
            {

            }

            return HeaderUtilities.RemoveQuotes(boundary).Value;
        }

        // Batch deletes files and/or directories
        //  -- returns false if a file or dir doesn't exist, or if an exception was thrown
        private bool TryBatchDelete(string username, List<SimpleNodeDto> nodes)
        {
            foreach (var node in nodes)
            {
                string nodePath = node.Path.Length > 0 ? node.Path + "/" + node.Name : node.Name;
                string physicalPath = Path.Combine(appEnv.WebRootPath, String.Format(@"dataspace/{0}/{1}", username, nodePath));

                try
                {
                    if (node.NodeType == "File")
                    {
                        if (System.IO.File.Exists(physicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (node.NodeType == "Directory")
                    {
                        if (System.IO.Directory.Exists(physicalPath))
                        {
                            System.IO.Directory.Delete(physicalPath, true);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    // log error
                    return false;
                }
            }

            return true;
        }
    }
}