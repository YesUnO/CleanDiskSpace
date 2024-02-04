string cDrivePath = "C:\\";
string logRootPath = AppDomain.CurrentDomain.BaseDirectory;

DateTime now = DateTime.Now;
string year = now.ToString("yyyy");
string month = now.ToString("MM");
string day = now.ToString("dd");

string logFolderPath = Path.Combine(logRootPath, year, month);
string logFilePath = Path.Combine(logFolderPath, $"Log-{day}.txt");

try
{
    DriveInfo cDrive = new DriveInfo(cDrivePath);
    long freeSpace = cDrive.AvailableFreeSpace;

    // Set your free space limit here (in bytes)
    long spaceLimit = 1_073_741_824; // 1 GB
    if (freeSpace < spaceLimit)
    {
        Log($"Free space on C drive is less than 1GB. Deleting folder contents.");
        // Add your list of folder paths here

        string[] folderPaths =
        {
            @"C:\Deploy",
            @"C:\Backup",
            @"C:\inetpub\logs\LogFiles",
            @"C:\Windows\Temp\K2CMS\ContentPackages"
        };

        foreach (var folderPath in folderPaths)
        {
            DeleteFolderContent(folderPath);
            Log($"Content of {folderPath} deleted.");
        }
    }
    else
    {
        decimal freeSpaceForLog = freeSpace / 1024 / 1024 / 1024;
        Log($"Free space on C drive: {freeSpaceForLog} GB. No action needed.");
    }
}
catch (Exception ex)
{
    Log($"Error checking free space: {ex.Message}");
}


void DeleteFolderContent(string folderPath)
{
    foreach (var file in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories))
    {
        File.Delete(file);
    }

    foreach (var dir in Directory.EnumerateDirectories(folderPath))
    {
        Directory.Delete(dir, true);
    }
}

TextWriter GetLogWriter()
{
    if (!Directory.Exists(logFolderPath))
    {
        Directory.CreateDirectory(logFolderPath);
    }

    return new StreamWriter(logFilePath, true);
}

void Log(string message)
{
    Console.WriteLine(message);
    using var logWriter = GetLogWriter();
    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
}