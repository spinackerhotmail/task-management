namespace TaskManagementService.CommonLib.Exceptions
{
    public class RequiredAttributeNotFoundException : Exception
    {
        public RequiredAttributeNotFoundException()
            : base()
        {
        }

        public RequiredAttributeNotFoundException(string message)
            : base(message)
        {
        }

        public RequiredAttributeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
