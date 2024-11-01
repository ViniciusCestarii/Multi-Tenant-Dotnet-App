using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Services.TenantService;
using MultiTenantApp.Services.TenantService.DTOs;

namespace MultiTenantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // Create a new tenant
        [HttpPost]
        public IActionResult Post(CreateTenantRequest request)
        {
            var result = _tenantService.CreateTenant(request);
            return Ok(result);
        }
    }
}
