using GraduationChecker.Config;
using GraduationChecker.Models;
using GraduationChecker.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationChecker.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly string _folderId;
        private readonly string _productId;
        private readonly DataService _dataService;
        private readonly FileService _fileService;

        public FilesController(DataService dataService, FileService fileService, AppSettings appSettings)
        {
            _folderId = appSettings.MantleKeeperFolderId;
            _productId = appSettings.MantleKeeperProductId;
            _dataService = dataService;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> CheckGraduation([FromBody] GraduationRequest graduationRequest)
        {
            var graduation = new Graduation
            {
                PermanentCode = graduationRequest.PermanentCode,
                University = await _dataService.GetUniversityById(graduationRequest.University),
                Program = await _dataService.GetProgramById(graduationRequest.Program),
                GraduationYear = graduationRequest.GraduationYear,
                Gpa = graduationRequest.Gpa
            };

            var fileString = graduation.ToString();
            using (var ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);
                try
                {
                    sw.Write(fileString);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    using (var fs = new FileStream($"{graduation.PermanentCode}.txt", FileMode.Create, FileAccess.ReadWrite))
                    {
                        ms.CopyTo(fs);

                        var keeperFiles = await _fileService.GetFiles();
                        var fileToCompare = keeperFiles.FirstOrDefault(file => file.DisplayName.Contains(graduation.PermanentCode));

                        if (fileToCompare != null)
                        {
                            KeeperCompareResult bestCompareResponse = null;
                            decimal bestCompareAccuracy = 0;
                            var fileVersions = await _fileService.GetVersions(fileToCompare.Id);

                            foreach (var fileVersion in fileVersions)
                            {
                                fs.Seek(0, SeekOrigin.Begin);
                                var file = new FormFile(fs, 0, fs.Length, "file", fs.Name)
                                {
                                    Headers = new HeaderDictionary(),
                                    ContentType = "application/pdf"
                                };
                                var compareResponse = await _fileService.CompareVersion(fileToCompare.Id, fileVersion.Id, file);

                                if (bestCompareAccuracy < compareResponse.Accuracy)
                                {
                                    bestCompareAccuracy = compareResponse.Accuracy;
                                    bestCompareResponse = compareResponse;
                                }
                            }

                            return bestCompareAccuracy == 1 ?
                                Ok(new KeeperResponse { IsValid = true }) :
                                Ok(new KeeperResponse { IsValid = false, FileDiff = bestCompareResponse.FileUrl });
                        }
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    sw.Dispose();
                }
            }

            return NotFound();
        }
    }
}
