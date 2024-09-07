using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace CollageControllers.Controllers
{
    [Route("Subjects")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private ISubjectRepo _SubjectsService;

        public SubjectsController(ISubjectRepo SubjectRepo)
        {
            _SubjectsService = SubjectRepo;
        }

        [HttpGet("GetAll", Order = 1)]
        public ActionResult<List<SubjectEF>> GetAll()
        {
            try
            {
                return Ok(_SubjectsService.GetSubjects());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll/SemeseterID", Order = 2)]
        public ActionResult<List<SubjectEF>> GetAll(int SemesterId)
        {
            try
            {
                return Ok(_SubjectsService.GetSubjects(SemesterId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Get/id", Order = 3)]
        public ActionResult<SubjectEF> GetById(int Id)
        {
            try
            {
                SubjectEF Subject = _SubjectsService.GetById(Id);
                if (Subject is null)
                    return BadRequest($"There is no Subject with {Id} ID");
                else
                    return Ok(Subject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddNew", Order = 4)]
        public ActionResult AddNew(SubjectEF Subject)
        {
            try
            {
                if (_SubjectsService.Save(Subject, enMode.AddNew))
                    return Created();
                else
                    return BadRequest("Faild to add new subject");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/id", Order = 5)]
        public ActionResult Update(int Id, SubjectEF SubjectArg)
        {
            try
            {
                SubjectEF Subject = _SubjectsService.GetById(Id);

                if (Subject is null)
                    return BadRequest($"There is no subject with {Id} ID");
                if (SubjectArg is null)
                    return BadRequest("Subject Argumant was null");
                Subject.SubjectName = SubjectArg.SubjectName;
                Subject.SemeseterID = SubjectArg.SemeseterID;

                if (_SubjectsService.Save(Subject, enMode.Update))
                    return NoContent();
                else
                    return BadRequest("Faild to update subject");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/Id", Order = 6)]
        public ActionResult Delete(int Id)
        {
            try
            {
                if (_SubjectsService.Delete(Id))
                    return NoContent();
                else
                    return BadRequest("Faild to delete subject");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
