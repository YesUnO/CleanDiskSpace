string cDrivePath = "C:\\";

try
{
    DriveInfo cDrive = new DriveInfo(cDrivePath);
    long freeSpace = cDrive.AvailableFreeSpace;

    // Set your free space limit here (in bytes)
    long spaceLimit = 1_073_741_824; // 1 GB
    if (freeSpace < spaceLimit)
    {
        Console.WriteLine($"Free space on C drive is less than 1GB. Deleting folder contents.");
        // Add your list of folder paths here


        foreach (var folderPath in args)
        {
            DeleteFolderContent(folderPath);
            Console.WriteLine($"Content of {folderPath} deleted.");
        }
    }
    else
    {
        decimal freeSpaceForLogInGB = freeSpace / 1024 / 1024 / 1024;
        Console.WriteLine($"Free space on C drive: {freeSpaceForLogInGB} GB. No action needed.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error checking free space: {ex.Message}");
}


void DeleteFolderContent(string folderPath)
{
    foreach (var file in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories))
    {
        FileInfo fileInfo = new FileInfo(file);
        DateTime lastUpdatedDate = fileInfo.LastWriteTime;
        bool isLessThanOneDayOld = (DateTime.Now - lastUpdatedDate).TotalHours < 24;
        if (isLessThanOneDayOld) continue;

        try
        {
            File.Delete(file);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
        }
    }

    foreach (var dir in Directory.EnumerateDirectories(folderPath))
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(dir);
        DateTime lastUpdatedDate = directoryInfo.LastWriteTime;
        bool isLessThanOneDayOld = (DateTime.Now - lastUpdatedDate).TotalHours < 24;
        if (isLessThanOneDayOld) continue;

        try
        {
            Directory.Delete(dir, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting folder: {ex.Message}");
        }
    }
}
