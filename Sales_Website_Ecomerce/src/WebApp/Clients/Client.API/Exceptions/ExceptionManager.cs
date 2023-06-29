namespace Client.API.Exceptions
{
    public static class ExceptionManager
    {
        private static readonly long MaxLogFileSize = 100 * 1024 * 1024; // 100 MB in bytes

        public static void HandleException(Exception ex, string LogDirectory)
        {
            string LogfileName = "exceptionlog.txt";
            string LogFilePath = Path.Combine(LogDirectory, LogfileName);

            // Log the exception to a text file
            using (var writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nMessage: {ex.Message}\nStackTrace: {ex.StackTrace}\n");
            }

            // Check if the log file size exceeds the limit and delete it if necessary
            var fileInfo = new FileInfo(LogFilePath);
            if (fileInfo.Exists && fileInfo.Length > MaxLogFileSize)
            {
                fileInfo.Delete();
            }
        }
    }
}
