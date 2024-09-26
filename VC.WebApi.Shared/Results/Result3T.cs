using VC.WebApi.Shared.Errors;

namespace VC.WebApi.Shared.Results
{
    public class Result3<T> : Result<T>
    {
        public bool IsPartialSuccess => _state == ResultState.PartialSuccess;

        private Result3(T? Value, ResultState state, List<Error> errors) : base(Value, state, errors)
        {

        }

        public static Result3<T> PartialSuccess(T value, Error error) //e.g. if list of errors is returned, but some objects produced errors
        {
            return new Result3<T>(value, ResultState.PartialSuccess, new List<Error>() { error }); //TOCHECK: mixing of errorTypes for Partial Errors
        }
        public static Result3<T> PartialSuccess(T value, List<Error> errors) //e.g. if list of errors is returned, but some objects produced errors
        {
            return new Result3<T>(value, ResultState.PartialSuccess, errors);
        }

        public static new Result3<T> Failure(Error error)
        {
            return new Result3<T>(default, ResultState.Failure, new List<Error> { error });
        }
        public static new Result3<T> Failure(ErrorList errors)
        {
            return new Result3<T>(default, ResultState.Failure, errors.Errors);
        }

        public static new Result3<T> Success(T value)
        {
            return new Result3<T>(value, ResultState.Success, new List<Error>());
        }

    }
}
