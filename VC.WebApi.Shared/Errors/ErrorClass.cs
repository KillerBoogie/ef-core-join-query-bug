namespace VC.WebApi.Shared.Errors
{
    public enum ErrorClass
    {
        //OK = 200,
        //Warning = 200,
        //Created = 201,
        //NoContent = 204,
        BadRequest = 400,
        NotAuthenticated = 401,
        Forbidden = 403,//  Not Authorized
        NotFound = 404,
        MethodNotAllowd = 405,
        Conflict = 409,
        PreconditionFailed = 412,
        ValidationError = 422,
        PreconditionRequired = 428,
        InternalServerError = 500,
        ServiceUnavailable = 503,
        GatewayTimeout = 504
    }
}
