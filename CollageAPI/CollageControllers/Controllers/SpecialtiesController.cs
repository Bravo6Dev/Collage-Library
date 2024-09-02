using DataLayer.Reposertory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollageControllers.Controllers
{
    [Route("Specialties")]
    [ApiController]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtiesRepo _SpecialtiesService;
    }
}
