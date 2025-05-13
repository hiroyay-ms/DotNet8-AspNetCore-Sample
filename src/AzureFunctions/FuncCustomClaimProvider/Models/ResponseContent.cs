using System.Text.Json;
using System.Text.Json.Serialization;

namespace FuncCustomClaimProvider.Models;

public class Claims
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? tenantGuid { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? servicePlan { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? tenantEnablement { get; set; }
}

public class Action
{
    [JsonPropertyName("@odata.type")]
    public string odatatype { get; set; }
    public Claims claims { get; set; }
    public Action()
    {
        odatatype = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";
        claims = new Claims();
    }
}

public class Data
{
    [JsonPropertyName("@odata.type")]
    public string odatatype { get; set; }
    public List<Action> actions { get; set; }
    public Data()
    {
        odatatype = "microsoft.graph.onTokenIssuanceStartResponseData";
        actions = new List<Action>();
        actions.Add(new Action());
    }
}

public class ResponseContent
{
    [JsonPropertyName("data")]
    public Data data { get; set; }
    public ResponseContent()
    {
        data = new Data();
    }
}