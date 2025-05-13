using Microsoft.AspNetCore.Http;
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
        var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["x-tenant-id"];
        activity.SetTag("TenantId", string.IsNullOrEmpty(tenantId) ? "Unknown" : tenantId);
    }
}
