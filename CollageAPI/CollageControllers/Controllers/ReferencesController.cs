using BuisnessLayer.Services;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.IO;

using Ref = DataLayer.Entites.ReferencesEF;
using DataLayer.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CollageControllers.Controllers
{
    [Authorize]
    [Route("References")]
    [ApiController]
    public class ReferencesController : ControllerBase
    {
        private IReferencesRepo _ReferencesService;
        private readonly string _ImageDirectory = @"C:\Users\DELL\Desktop\Projects\APIs\Collage-Web\ReferencesImages";
        private readonly string _PdfDirectory = @"C:\Users\DELL\Desktop\Projects\APIs\Collage-Web\ReferencesPdf";

        public ReferencesController(IReferencesRepo Ref)
        {
            _ReferencesService = Ref;
        }

        private async Task<string> SaveImageController(IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
                return null;
            try
            {
                string FileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);

                if (!Directory.Exists(_ImageDirectory))
                {
                    Directory.CreateDirectory(_ImageDirectory);
                }

                string path = Path.Combine(_ImageDirectory, FileName);

                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(Stream);
                }
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> SavePdfFileController(IFormFile PdfFile)
        {
            try
            {
                if (PdfFile == null || PdfFile.Length == 0)
                    return null;
                string FileName = Guid.NewGuid() + Path.GetExtension(PdfFile.FileName);

                if (!Directory.Exists(_PdfDirectory))
                    Directory.CreateDirectory(_PdfDirectory);

                string path = Path.Combine(_PdfDirectory, FileName);

                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    await PdfFile.CopyToAsync(Stream);
                }
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMimeType(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();  // Use lowercase to handle both cases
            return ext switch
            {
                ".jpeg" or ".jpg" => "image/jpeg",  // Fixed MIME type and unified jpg and jpeg
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"  // Fixed typo in MIME type
            };
        }

        [HttpGet("GetAll", Order = 1)]
        public ActionResult<List<RefDTO>> GetAll()
        {
            try
            {
                return Ok(_ReferencesService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll/{Subjectid}", Order = 2)]
        public ActionResult<List<RefDTO>> GetAll(int SubId)
        {
            try
            {
                return Ok(_ReferencesService.GetAll(SubId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Order = 3)]
        public ActionResult<RefDTO> GetById(int Id)
        {
            try
            {
                return _ReferencesService.GetById(Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetImage/{filename}", Order = 4)]
        public ActionResult GetImage(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename was empty");
            string path = Path.Combine(_ImageDirectory, filename);

            if (!System.IO.File.Exists(path))
                return NotFound("Image not found");

            FileStream image = System.IO.File.OpenRead(path);
            string MimeType = GetMimeType(path);

            return File(image, MimeType);
        }

        [HttpPost("AddNew", Order = 5)]
        public async Task<ActionResult> AddNew(IFormFile ImageFile, IFormFile PdfFile, [FromForm]RefDTO Reference)
        {
            try
            {
                if (!_ReferencesService.Valid(Reference))
                    return BadRequest();

                Reference.ImagePath = await SaveImageController(ImageFile);
                Reference.RefPath = await SavePdfFileController(PdfFile);

                if (_ReferencesService.Save(Reference, enMode.AddNew))
                    return NoContent();
                else
                    return BadRequest("Faild to add new reference");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/id", Order = 6)]
        public async Task<ActionResult> UpdateAsync(int Id, [FromForm]RefDTO ReferenceArg, IFormFile ImageFile, IFormFile PdfFile)
        {
            try
            {
                if (!_ReferencesService.Valid(ReferenceArg))
                    return BadRequest();
                RefDTO Reference = _ReferencesService.GetById(Id);

                if (ReferenceArg == null)
                    return BadRequest("Reference object was null");

                if (Reference == null)
                    return NotFound();

                Reference.RefName = ReferenceArg.RefName;
                Reference.SubId = ReferenceArg.SubId;
                Reference.ImagePath = await SaveImageController(ImageFile);
                Reference.RefPath = await SavePdfFileController(PdfFile);

                if (_ReferencesService.Save(Reference, enMode.Update))
                    return NoContent();
                else
                    return BadRequest("Faild to update");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/id", Order = 7)]
        public ActionResult Delete(int Id)
        {
            try
            {
                if (_ReferencesService.Delete(Id))
                    return NoContent();
                else
                    return BadRequest("Faild to delete");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
