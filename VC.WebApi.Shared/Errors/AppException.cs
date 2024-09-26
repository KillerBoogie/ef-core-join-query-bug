namespace VC.WebApi.Shared.Errors
{
    public class AppException : Exception
    {
        public AppException()
        { }

        public AppException(string msg)
            : base(msg)
        { }

        public AppException(string msg, Exception ex)
            : base(msg, ex)
        { }
    }
}
