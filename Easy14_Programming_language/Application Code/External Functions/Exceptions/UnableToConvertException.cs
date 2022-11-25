using System;

namespace Easy14_Programming_Language
{
    [Serializable]
    public class UnableToConvertException : Exception
    {
        public UnableToConvertException() { }

        public UnableToConvertException(string message)
            : base(message) { }

        public UnableToConvertException(string message, Exception inner)
            : base(message, inner) { }
    }
}
