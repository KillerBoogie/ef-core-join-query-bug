using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Errors
{
    public class ErrorList
    {
        public List<Error> Errors { get; private set; } = new List<Error>();

        public bool HasErrors
        {
            get { return Errors.Count != 0; }
        }
        public bool HasNoErrors
        {
            get { return Errors.Count == 0; }
        }
        public ErrorList()
        {

        }

        public void Add(Error error)
        {
            Add(new List<Error>() { error });
        }

        private void Add(List<Error> errors)
        {
            Errors.AddRange(errors);
        }

        public void Add(ErrorList errors)
        {
            Add(errors.Errors);
        }
        public void Add(Result result)
        {
            Add(result.Errors);
        }

        public void AddIfFailure(Result result)
        {
            if (result.IsFailure)
            {
                Add(result.Errors);
            }
        }

        public void AddIfFailure(Result result, string prefix)
        {
            if (result.IsFailure)
            {
                result.PrefixPointer(prefix);
                Add(result.Errors);
            }
        }

    }
}
