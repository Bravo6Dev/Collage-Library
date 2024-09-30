using DataLayer.DTOs;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CollageControllers.Controllers
{
    [Authorize]
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
        public ActionResult<List<SemesterDTO>> GetAll()
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
        public ActionResult<List<SemesterDTO>> GetAll(int SpecialtyId)
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
        public ActionResult<SemesterDTO> GetById(int ID)
        {
            try
            {
                SemesterDTO Semester = _SemestersService.GetById(ID);
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
        public ActionResult AddNew(SemesterDTO Semesters)
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

        // On working......
        //[HttpPut("Update/id", Order = 5)]
        //public ActionResult Update(int Id, SemesterDTO SemesterArg)
        //{
        //    try
        //    {
        //        SemesterDTO Semester = _SemestersService.GetById(Id);
        //        if (Semester == null)
        //            return BadRequest("Semester not found");
        //        if (SemesterArg == null)
        //            throw new ArgumentNullException("Semester Argument was null");


        //        Semester.SemNumber = SemesterArg.SemNumber;
        //        Semester.SpecId = SemesterArg.SpecId;

        //        if (_SemestersService.Save(Semester, enMode.Update))
        //            return NoContent();
        //        else
        //            return BadRequest("Faild to update semester");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

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
