namespace VC.WebApi.Shared.Errors
{
    public interface IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }
    }

    public class Parameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class NullParameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public NullParameter(string name)
        {
            Name = name;
            Value = "NULL";
        }
    }

    public class AttributeParameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public AttributeParameter(string value)
        {
            Name = "fieldName";
            Value = value;
        }
    }

    public class InputParameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public InputParameter(string value)
        {
            Name = "inputParameter";
            Value = value;
        }
    }

    public class ResourceParameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public ResourceParameter(string value)
        {
            Name = "resourceName";
            Value = value;
        }
    }

    public class CommandParameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public CommandParameter(string value)
        {
            Name = "commandName";
            Value = value;
        }
    }

    public class ConstraintParameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public ConstraintParameter(string value)
        {
            Name = "constraintName";
            Value = value;
        }
    }

    public class TableParameter : IParameter
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public TableParameter(string value)
        {
            Name = "tableName";
            Value = value;
        }
    }
}
