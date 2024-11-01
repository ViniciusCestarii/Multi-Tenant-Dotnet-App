using MultiTenantApp.Models;
using MultiTenantApp.Services.TenantService.DTOs;

namespace MultiTenantApp.Services.TenantService
{
    public interface ITenantService
    {
        Tenant CreateTenant(CreateTenantRequest request);
    }
}
