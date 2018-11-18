using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace PhotoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private const String UploadsFolderName = "uploads";

        private IHostingEnvironment hostingEnvironment;

        public FilesController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post()
        {
            //! IFormFileCollection and IFormFile types alternatively can be used for method argument.
            //! In that case separate methods with arguments of IFormFileCollection or IFormFile type must be implemented

            // var filePath = Path.GetTempFileName();
            var formFiles = Request.Form.Files;
            if (formFiles == null || !formFiles.Any())
            {
                return BadRequest("Empty input files collection");
            }

            var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, UploadsFolderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // TODO: Upload files in transaction (UnitOfWork ?)
            var filesUploaded = new List<String>();
            foreach (var formFile in formFiles.Where(f => f.Length > 0))
            {
                try
                {
                    // TODO: Add uploaded file names validation (use Path.GetTempFileName() ?)
                    var filePath = Path.Combine(uploadsFolder, formFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    filesUploaded.Add(filePath);
                }
                catch (Exception ex)
                {
                    // Remove all auploaded files to rollback action
                    foreach (var file in filesUploaded)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        error = $"Upload failed. Reason: {ex.Message}"
                    });
                }
            }

            return Ok(new
            {
                totalSize = formFiles.Sum(f => f.Length),
                filesUploaded = filesUploaded.Select(f => Path.GetFileName(f)).ToArray(),
                uploadsFolder
            });
        }

        #region Sample actions
        // // GET api/values
        // [HttpGet]
        // public ActionResult<IEnumerable<String>> Get()
        // {
        //     return Ok(new string[] { "value1", "value2" });
        // }

        // // GET api/values/5
        // [HttpGet("{id}")]
        // public ActionResult<String> Get(Int32 id)
        // {
        //     return "value";
        // }

        // // POST api/values
        // [HttpPost]
        // public void Post([FromBody] String value)
        // {
        //     Console.WriteLine(value);
        // }

        // // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(Int32 id, [FromBody] String value)
        // {
        // }

        // // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public void Delete(Int32 id)
        // {
        // }
        #endregion Sample actions
    }
}
