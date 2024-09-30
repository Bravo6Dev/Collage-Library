using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;

namespace CollageControllers.Controllers
{
    [Authorize]
    [Route("Specialties")]
    [ApiController]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtiesRepo _SpecialtiesService;

        public SpecialtiesController(ISpecialtiesRepo SpecialtyServer)
        {
            this._SpecialtiesService = SpecialtyServer;
        }

        [HttpGet("GetAll", Order = 1)]
        public ActionResult<List<SpecialtiesEF>> GetAll()
        {
            try
            {
                return Ok(_SpecialtiesService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll/CollageId", Order = 2)]
        public ActionResult<List<SpecialtiesEF>> GetAll(int CollageId)
        {
            try
            {
                return Ok(_SpecialtiesService.GetAll(CollageId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex?.Message);
            }
        }

        [HttpGet("Get/id", Order = 3)]
        public ActionResult<SpecialtiesEF> GetById(int Id)
        {
            try
            {
                SpecialtiesEF Specialty = _SpecialtiesService.GetById(Id);
                if (Specialty is null)
                    return BadRequest("Specialty doesn't found");
                else
                    return Ok(Specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddNew", Order = 4)]
        public ActionResult AddNew(SpecialtiesEF SpecialtyAvg)
        {
            try
            {
                if (SpecialtyAvg is null)
                    return BadRequest("Specialty object was null");
                if (_SpecialtiesService.Save(SpecialtyAvg, enMode.AddNew))
                    return NoContent();
                else
                    return BadRequest("Faild to add new specialty");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/id", Order = 5)]
        public ActionResult Update(int Id, SpecialtiesEF SpecialtyArg)
        {
            try
            {
                SpecialtiesEF Specialty = _SpecialtiesService.GetById(Id);
                if (Specialty is null)
                    return BadRequest("Specialty not found");

                Specialty.SpecialtyName = SpecialtyArg.SpecialtyName;
                Specialty.CollageID = SpecialtyArg.CollageID;
                if (_SpecialtiesService.Save(Specialty, enMode.Update))
                    return NoContent();
                else
                    return BadRequest("Faild to update");
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
                if (_SpecialtiesService.Delete(Id))
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
