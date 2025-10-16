namespace TaskManagementService.CommonLib.Services;

public interface INamesProvider
{
    public string ServiceName { get; set; } 
    public string ServiceNamespace  { get; set; } 
    public string CompanyNamespace { get; set; } 
    public string RoutePrefix { get; set; } 
}
