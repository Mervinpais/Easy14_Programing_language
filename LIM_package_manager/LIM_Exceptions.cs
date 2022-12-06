namespace LIM_package_manager
{
    class LIM_Exceptions
    {
        [Serializable]
        public class LIM_UnknownError : Exception
        {
            public LIM_UnknownError() : base() { }
            public LIM_UnknownError(string message) : base(message) { }
            public LIM_UnknownError(string message, Exception inner) : base(message, inner) { }
        }
        [Serializable]
        public class UnableToDownloadFileNet : Exception
        {
            public UnableToDownloadFileNet() : base() { }
            public UnableToDownloadFileNet(string message) : base(message) { }
            public UnableToDownloadFileNet(string message, Exception inner) : base(message, inner) { }
        }
        [Serializable]
        public class UnableToDownloadFileIO : Exception
        {
            public UnableToDownloadFileIO() : base() { }
            public UnableToDownloadFileIO(string message) : base(message) { }
            public UnableToDownloadFileIO(string message, Exception inner) : base(message, inner) { }
        }
        [Serializable]
        public class FileDownloadError : Exception
        {
            public FileDownloadError() : base() { }
            public FileDownloadError(string message) : base(message) { }
            public FileDownloadError(string message, Exception inner) : base(message, inner) { }
        }
        [Serializable]
        public class UnknownOrInvalidPackage : Exception
        {
            public UnknownOrInvalidPackage() : base()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The package passed is an invalid/unknown package\n\r");
            }
            public UnknownOrInvalidPackage(string message) : base(message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The package \'" + message + "\' is an invalid/unknown package\n\r");
            }
            public UnknownOrInvalidPackage(string message, Exception inner) : base(message, inner)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The package \'" + message + "\' is an invalid/unknown package\n\r");
            }
        }
        [Serializable]
        public class InvalidURL : Exception
        {
            public InvalidURL() : base()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The URL passed is not a real URL\n\r");
            }
            public InvalidURL(string message) : base(message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The URL \'" + message + "\' passed is not a real URL\n\r");
            }
            public InvalidURL(string message, Exception inner) : base(message, inner)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The URL \'" + message + "\' passed is not a real URL\n\r");
            }
        }
    }
}
