namespace VC.WebApi.Shared.Errors
{
    /// <summary>
    /// Error class.
    /// <para>Available Constructors are:</para>
    /// Error(ErrorClass @class, string code)<br />
    /// Error(ErrorType errorType, ErrorClass @class, string code)<br />
    /// Error(ErrorClass @class, string code, List<IParameter> parameters)<br />
    /// Error(ErrorType errorType, ErrorClass @class, string code, List<IParameter> parameters)<br />
    /// </summary>
    public sealed partial class Error
    {
        public ErrorClass Class { get; init; }
        public string Code { get; init; }
        public string? Pointer { get; private set; }
        public IDictionary<string, object?> DetailParameters { get; init; } = new Dictionary<string, object?>(StringComparer.Ordinal);
        public IDictionary<string, object?> Extensions { get; init; } = new Dictionary<string, object?>(StringComparer.Ordinal);
        public IDictionary<string, object?> LogInfo { get; init; } = new Dictionary<string, object?>(StringComparer.Ordinal);


        #region Constructors

        public Error(ErrorClass @class, string code, string? pointer = null, IDictionary<string, object?>? detailParameters = null, IDictionary<string, object?>? extensions = null, IDictionary<string, object?>? logInfo = null)
        {
            Code = code;
            Class = @class;
            Pointer = pointer;
            if (detailParameters is not null) DetailParameters = detailParameters;
            if (extensions is not null) Extensions = extensions;
            if (logInfo is not null) LogInfo = logInfo;
        }

        #endregion

        #region Commands
        public Error AddExtension(string parameterName, object? parameterValue)
        {
            Extensions.Add(parameterName, parameterValue);
            return this;
        }

        public Error PrefixPointer(string prefix)
        {
            // sanitize backslashes
            prefix = prefix.Trim('\\');

            if (Pointer is null)
            {
                Pointer = prefix;
            }
            else
            {
                Pointer = prefix + "\\" + Pointer;
            }

            return this;
        }
        #endregion

    }
}
