namespace UnityExtras.Converters.Tests
{
    internal sealed class ConstructorMock<T>
    {
        public ConstructorMock(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
