using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProvidersResponse>> GetProviderNames()
        {
            var providerNames = (await providerService
                .GetProviders()
                .OrderBy(provider => provider.Name)
                .Select(provider => new { provider.Id, provider.Name })
                .ToArrayAsync())
                .Select(provider => new ProvidersResponse.ProviderModel { Id = provider.Id, Name = provider.Name })
                .ToArray();

            return Ok(new ProvidersResponse() { Providers = providerNames });
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CreateProviderRequest request)
        {
            var result = providerService.Create(new Data.Entities.ProviderEntity
            {
                Name = request.Name
            });

            if(!result.Success)
                return BadRequest(result.Message);
            return NoContent();
        }
    }
}