namespace VC.WebApi.Shared.Optional
{
    public readonly struct Optional<T>
    {
        public Optional(T? value)
        {
            HasValue = true;
            Value = value;
        }

        public bool HasValue { get; }
        public T? Value { get; }
        public static implicit operator Optional<T>(T value) => new Optional<T>(value);
        public override string ToString() => HasValue ? Value?.ToString() ?? "null" : "unspecified";
    }
}
