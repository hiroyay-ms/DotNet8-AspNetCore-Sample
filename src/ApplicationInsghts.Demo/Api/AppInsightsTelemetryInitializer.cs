using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

public class AppInsightsTelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AppInsightsTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
        {
            telemetry.Context.Cloud.RoleName = "AdventureWorks-Api";
        }

        var reqeustTelemetry = telemetry as RequestTelemetry;
        if (reqeustTelemetry != null)
            reqeustTelemetry.Properties["TenantId"] = _httpContextAccessor.HttpContext?.Request.Headers["x-tenant-id"];
    }
}