using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using OpenTelemetry;
using System.Diagnostics;

internal class LogsWithTenantIdProcessor : BaseProcessor<Activity>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogsWithTenantIdProcessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override void OnEnd(Activity activity)
    {
        //var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["x-tenant-id"];
        string tenantId = _httpContextAccessor.HttpContext?.User.Claims.ToList().Find(c => c.Type == "tenantId")?.Value ?? "Unknown";
        activity.SetTag("TenantId", tenantId);
    }
}
