namespace AccessControlSystem.Dtos;

public class AccessResult
{
    public bool IsValid { get; set; }
    public string? EmployeeName { get; set; }
    public string? Reason { get; set; }
    public DateTime Timestamp { get; set; }
        
    public static AccessResult Granted(string employeeName)
    {
        return new AccessResult
        {
            IsValid = true,
            EmployeeName = employeeName,
            Reason = "Доступ разрешен",
            Timestamp = DateTime.Now
        };
    }
        
    public static AccessResult Denied(string reason, string? employeeName = null)
    {
        return new AccessResult
        {
            IsValid = false,
            EmployeeName = employeeName,
            Reason = reason,
            Timestamp = DateTime.Now
        };
    }
}