using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CollageControllers.Controllers
{
    [Route("Collages")]
    [ApiController]
    public class CollageController : ControllerBase
    {
        private readonly ICollageRepo _CollageService;

        public CollageController(ICollageRepo CollageRepo)
        {
            _CollageService = CollageRepo;
        }

        [HttpGet("GetAll", Order = 1)]
        [ProducesResponseType(typeof(List<CollagesEF>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<CollagesEF>> GetAll()
        {
            try
            {
                return Ok(_CollageService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Get/id", Order = 2)]
        [ProducesResponseType(typeof(CollagesEF), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CollagesEF> GetById(int Id)
        {
            try
            {
                CollagesEF Collage = _CollageService.GetById(Id);
                if (Collage is null)
                    return NotFound("Collage Not Found");
                else
                    return Ok(Collage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("Get/Name", Order = 3)]
        [ProducesResponseType(typeof(CollagesEF), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CollagesEF> GetByName(string Name)
        {
            try
            {
                CollagesEF Collage = _CollageService.GetByName(Name);
                if (Collage is null)
                    return NotFound("Collage Not Found");
                else
                    return Ok(Collage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddNew", Order = 4)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNew(CollagesEF Collage)
        {
            if (Collage is null)
                return BadRequest();
            if (_CollageService.Save(Collage, enMode.AddNew))
                return Ok("Collage has been created");
            else
                return BadRequest("Faild to add new collage");
        }

        [HttpPost("Update", Order = 5)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Update(int Id, CollagesEF CollageArg)
        {
            try
            {
                CollagesEF Collage = _CollageService.GetById(Id);
                if (Collage is null)
                    return NotFound("Collage not found");
                if (CollageArg is null)
                    return BadRequest("Collage argument is null");
                Collage.CollageName = CollageArg.CollageName;
                if (_CollageService.Save(Collage, enMode.Update))
                    return Ok("Collage has been updated");
                else
                    return BadRequest("Faild to update collage");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/id", Order = 6)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(int Id)
        {
            try
            {
                if (_CollageService.Delete(Id))
                    return Ok("Collage has been deleted");
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
