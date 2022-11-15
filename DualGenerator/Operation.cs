namespace DualGenerator
{
    public enum Operation
    {
        Unknown = 0,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Equal,
        Arbitrary,
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
                case Operation.Arbitrary:
                    return "tetszőleges";
                case Operation.Unknown:
                default:
                    throw new ArgumentException();
            }
        }
    }
}
