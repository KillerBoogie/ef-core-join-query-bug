using VC.WebApi.Shared.Errors;

namespace VC.WebApi.Shared.Results
{
    public class Result<T> : Result
    {
        private T? _value { get; }
        public T Value => IsFailure || _value is null ? throw new ArgumentOutOfRangeException(nameof(Value), "A failure result has no value.") : _value;

        protected Result(T? Value, ResultState state, List<Error> errors) : base(state, errors)
        {
            _value = Value;
        }

        public static new Result<T> Failure(Error error)
        {
            return new Result<T>(default, ResultState.Failure, new List<Error> { error });
        }
        public static new Result<T> Failure(ErrorList errors)
        {
            return new Result<T>(default, ResultState.Failure, errors.Errors);
        }
        public static new Result<T> Failure(Result result)
        {
            return new Result<T>(default, ResultState.Failure, result.Errors);
        }
        public static Result<T> Success(T value)
        {
            return new Result<T>(value, ResultState.Success, new List<Error>());
        }
        public override Result<T> AddParamCommand(string command)
        {
            foreach (Error error in Errors)
            {
                error.AddExtension("command", command);
            }
            return this;
        }
    }
}
