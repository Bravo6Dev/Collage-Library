using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CollageControllers.Controllers
{
    [Route("Semesters")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private ISemesterRepo _SemestersService;

        public SemestersController(ISemesterRepo SemRepo)
        {
            _SemestersService = SemRepo;
        }

        [HttpGet("GetAll", Order = 1)]
        public ActionResult<List<SemestersEF>> GetAll()
        {
            try
            {
                return Ok(_SemestersService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll/SepicaltyId", Order = 2)]
        public ActionResult<List<SemestersEF>> GetAll(int SpecialtyId)
        {
            try
            {
                return Ok(_SemestersService.GetAll(SpecialtyId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Get/id", Order = 3)]
        public ActionResult<SemestersEF> GetById(int ID)
        {
            try
            {
                SemestersEF Semester = _SemestersService.GetById(ID);
                if (Semester is null)
                    return BadRequest("Semester not found");
                else
                    return Ok(Semester);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddNew", Order = 4)]
        public ActionResult AddNew(SemestersEF Semesters)
        {
            try
            {
                if (_SemestersService.Save(Semesters, enMode.AddNew))
                    return NoContent();
                else
                    return BadRequest("Faild to add new semeter");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/id", Order = 5)]
        public ActionResult Update(int Id, SemestersEF SemesterArg)
        {
            try
            {
                SemestersEF Semester = _SemestersService.GetById(Id);
                if (Semester is null)
                    return BadRequest("Semester not found");
                Semester.NumOfSem = SemesterArg.NumOfSem;
                Semester.SpeicaltyID = SemesterArg.SpeicaltyID;

                if (_SemestersService.Save(Semester, enMode.Update))
                    return NoContent();
                else
                    return BadRequest("Faild to update semester");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/id", Order = 6)]
        public ActionResult Delete(int Id)
        {
            try
            {
                if (_SemestersService.Delete(Id))
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
