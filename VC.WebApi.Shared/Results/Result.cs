using VC.WebApi.Shared.Errors;

namespace VC.WebApi.Shared.Results
{
    /// <summary>
    /// All sync/async methods should use this general Result class.
    /// <para>Available Constructors are:</para>
    /// Success()<br />
    /// Failure(Error error)<br />
    /// Failure(ErrorList errors)<br />
    /// Failure(Result result)<br />
    /// </summary>
    public class Result
    {
        protected ResultState _state;
        public bool IsFailure => _state == ResultState.Failure;
        public bool IsSuccess => _state == ResultState.Success;
        private List<Error> _errors { get; set; } = new();
        public List<Error> Errors
        {
            get { return IsSuccess ? throw new ArgumentOutOfRangeException(nameof(Errors), "A positive result has no Errors.") : _errors; }
        }

        protected Result(ResultState state, List<Error> errors)
        {
            if (state == ResultState.Failure && errors.Count == 0) throw new ArgumentOutOfRangeException(nameof(errors), "A Failure Result can't have empty error list.");
            if (state == ResultState.Success && errors.Count != 0) throw new ArgumentOutOfRangeException(nameof(errors), "A Success Result can't have error list.");

            _state = state;
            _errors = errors;
        }

        public static Result Failure(Error error)
        {
            return new Result(ResultState.Failure, new List<Error> { error });
        }
        public static Result Failure(ErrorList errors)
        {
            return new Result(ResultState.Failure, errors.Errors);
        }
        public static Result Failure(Result result)
        {
            return new Result(ResultState.Failure, result.Errors);
        }
        public static Result Success()
        {
            return new Result(ResultState.Success, new List<Error>());
        }

        private void AddErrors(List<Error> errorList)
        {
            if (errorList.Count == 0)
                throw new ArgumentNullException(nameof(errorList), "Result: Can't add empty ErrorList.");

            _errors.AddRange(errorList);
            _state = ResultState.Failure;

        }
        public static Result Combine(Result result, Result result2, params Result[] results) // at least two results are required to be combined
        {
            Result combinedResult = Success();
            if (!result.IsSuccess) //!IsSucces covers IsFailure and IsPartialSuccess
            {
                combinedResult.AddErrors(result._errors);
            }
            if (!result2.IsSuccess) //!IsSucces covers IsFailure and IsPartialSuccess 
            {
                combinedResult.AddErrors(result2._errors);
            }

            foreach (Result r in results)
            {
                if (!r.IsSuccess)
                {
                    combinedResult.AddErrors(r._errors);
                }
            }

            return combinedResult;
        }
        public Result Combine(Result result2, params Result[] results) // at least two results are required to be combined
        {
            return Combine(this, result2, results);
        }

        public virtual Result AddParam(string parameterName, string? parameterValue)
        {
            foreach (Error error in Errors)
            {
                error.AddExtension(parameterName, parameterValue);
            }
            return this;
        }

        public virtual Result AddParamCommand(string command)
        {
            foreach (Error error in Errors)
            {
                error.AddExtension("command", command);
            }
            return this;
        }

        public void PrefixPointer(string prefix)
        {
            foreach (Error error in Errors)
            {
                error.PrefixPointer(prefix);
            }
        }
        protected enum ResultState
        {
            Success,
            PartialSuccess,
            Failure
        }
    }
}
