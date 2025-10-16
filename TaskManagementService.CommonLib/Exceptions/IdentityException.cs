namespace TaskManagementService.CommonLib.Exceptions
{
    public class IdentityException : Exception
    {
        public IdentityException()
            : base()
        {
        }

        public IdentityException(string message)
            : base(message)
        {
        }

        public IdentityException(Exception innerException)
            : base($"Identity isn't working.", innerException)
        {
        }

        public IdentityException(string name, object key)
            : base($"Identity  \"{name}\" ({key}) isn't working.")
        {
        }
    }
}
