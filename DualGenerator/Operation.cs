namespace DualGenerator
{
    public enum Operation
    {
        Unknown = 0,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Equal,
        Unrestricted,
    }

    public static class OperationHelper
    {
        public static string ToString(Operation operation)
        {
            switch (operation)
            {
                case Operation.GreaterThanOrEqual:
                    return ">=";
                case Operation.LessThanOrEqual:
                    return "<=";
                case Operation.Equal:
                    return "=";
                case Operation.Unrestricted:
                    return "előjel kötetlen";
                case Operation.Unknown:
                default:
                    throw new ArgumentException();
            }
        }
    }
}
