using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.API.Models.Providers;
using OrderSystem.Core.Services;

namespace OrderSystem.API.Controllers
{
    [ApiController]
    [Route("/providers")]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService providerService;

        public ProvidersController(IProviderService providerService)
        {
            ArgumentNullException.ThrowIfNull(providerService, nameof(providerService));
            this.providerService = providerService;
        }

        [HttpGet("names")]
        [Authorize]
        public ActionResult<ProviderNamesResponse> GetProviderNames()
        {
            var providerNames = providerService.GetProviders().Select(provider => provider.Name).ToArray();
            return Ok(new ProviderNamesResponse() { Names = providerNames });
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult Create([FromBody] CreateProviderRequest request)
        {
            var result = providerService.Create(new Data.Entities.ProviderEntity()
            {
                Name = request.Name
            });

            if(!result.Success)
                return BadRequest(result.Message);
            return NoContent();
        }
    }
}