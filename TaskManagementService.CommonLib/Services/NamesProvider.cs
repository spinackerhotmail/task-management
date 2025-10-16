namespace TaskManagementService.CommonLib.Services;

public sealed class NamesProvider : INamesProvider
{
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceNamespace { get; set; } = string.Empty;
    public string CompanyNamespace { get; set; } = string.Empty;
    public string RoutePrefix { get; set; } = string.Empty;
}
