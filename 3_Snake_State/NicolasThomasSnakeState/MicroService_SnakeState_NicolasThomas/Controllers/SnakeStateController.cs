using MicroService_SnakeState_NicolasThomas.Data.Models;
using MicroService_SnakeState_NicolasThomas.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MicroService_SnakeState_NicolasThomas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SnakeStateController : ControllerBase
    {
        private readonly  ISnakeStateRepository repository;

        public SnakeStateController(ISnakeStateRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("on-move")]
        public async Task<ActionResult<SnakeStateModel>> EnvoieListPosition(SnakeStateModel model)
        {
            var StateSnake = await repository.EnvoieListPosition(model);
            if (StateSnake == null)
                return StatusCode(500);
            return model;
        }
    }
}
