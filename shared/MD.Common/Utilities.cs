using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MD.Common;

/// <summary>
/// Static utility class for miscellaneous helper functions.
/// </summary>
public static class Utilities
{
    /// <summary>
    /// Splits a string formatted as 'key:value' into a KeyValuePair.
    /// </summary>
    /// <param name="input">The string to parse, which must contain exactly one colon character.</param>
    /// <returns>A KeyValuePair constructed from the input string, where the key is the part of the string before the colon, and the value is the part of the string after the colon.</returns>
    /// <exception cref="ArgumentException">Thrown when the input string does not contain exactly one colon character.</exception>
    public static KeyValuePair<string, string> ExtractKeyValueFromColonSeparatedString(string input)
    {
        string[] parts = input.Split(new[] {':'}, 2);
        if (parts.Length != 2)
            throw new ArgumentException("Input must be in the format 'key:value'");

        return new KeyValuePair<string, string>(parts[0], parts[1]);
    }
    
    /// <summary>
    /// Formats a number to string and formats it so that 1000 = k, etc.
    /// </summary>
    /// <param name="number">The number to format.</param>
    /// <returns>A formatted string representing the number.</returns>
    public static string NumberToStringFormatted(int number)
    {
        return number switch
        {
            >= 100000000 => (number / 1000000).ToString("#,0M"),
            >= 10000000 => (number / 1000000).ToString("0.#") + "M",
            >= 100000 => (number / 1000).ToString("#,0K"),
            >= 10000 => (number / 1000).ToString("0.#") + "K",
            _ => number.ToString("#,0")
        };
    }
    
    /// <summary>
    /// Moves files from one directory to another recursively.
    /// After successful transfer of files, the source directory will be deleted.
    /// </summary>
    /// <param name="sourcePath">The source directory from which files will be moved.</param>
    /// <param name="targetPath">The target directory to which files will be moved.</param>
    /// <param name="debug">Boolean flag indicating whether debug mode is active. Defaults to 'false'.</param>
    public static void MoveFilesRecursively(string sourcePath, string targetPath, bool debug = false)
    {
        CopyFilesRecursively(sourcePath, targetPath);
        DeleteDirectoryRecursively(sourcePath, debug);
    }

    /// <summary>
    /// Copies files from one directory to another (recursively).
    /// </summary>
    /// <param name="sourcePath">The source directory path.</param>
    /// <param name="targetPath">The target directory path.</param>
    public static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        // Clean the path
        targetPath = RemoveIllegalCharsFromPath(targetPath);

        // Create the base target directory
        Directory.CreateDirectory(targetPath);

        // Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
    
        // Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
    }

    /// <summary>
    /// Removes invalid characters from the provided file or folder path.
    /// </summary>
    /// <param name="path">The file or folder path that needs to be cleaned.</param>
    /// <returns>Return a cleaned file or folder path. Existing directories are not modified.</returns>
    private static string RemoveIllegalCharsFromPath(string path)
    {
        char directorySeparator = Path.DirectorySeparatorChar;
        string invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()) + ":" + "*" + "/");
        string invalidReStr = $@"[{invalidChars}]";

        string[] pathSegments = path.Split(directorySeparator);
        for (int i = 0; i < pathSegments.Length; i++)
        {
            string currentPath = string.Join(directorySeparator.ToString(), pathSegments.Take(i + 1));
            if (!Directory.Exists(currentPath))
                pathSegments[i] = Regex.Replace(pathSegments[i], invalidReStr, "-");
        }

        return string.Join(directorySeparator.ToString(), pathSegments);
    }

    /// <summary>
    /// Copies a file from the source path to the target directory.
    /// If a file with the same name exists in the target directory, it will be deleted prior to the copying process.
    /// </summary>
    /// <param name="sourcePath">The path of the file that needs to be copied.</param>
    /// <param name="targetDir">The directory to copy the file to.</param>
    /// <param name="debug">Whether or not to print debug messages.</param>
    public static void CopyFile(string sourcePath, string targetDir, bool debug = false)
    {
        try
        {
            // Prepare the path in the target directory
            string targetPath = Path.Combine(targetDir, Path.GetFileName(sourcePath));
        
            // Ensure that the target does not exist.
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            // Copy the file.
            File.Copy(sourcePath, targetPath);
            
            if (debug)
                Console.WriteLine($"The file was copied from {sourcePath} to {targetDir} successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"The process failed: {e}");
        }
    }

    /// <summary>
    /// Deletes a specified file.
    /// </summary>
    /// <param name="targetFile">The full path of the file that you want to delete.</param>
    /// <param name="debug">Whether or not to print debug messages.</param>
    public static void DeleteFile(string targetFile, bool debug = false)
    {
        // Check if file exists
        if (File.Exists(targetFile))
        {
            // Delete the file
            File.Delete(targetFile);
            
            if (debug)
                Console.WriteLine($"Deleted: {targetFile}");
        }
        else
        {
            Console.WriteLine($"The specified file {targetFile} does not exist.");
        }
    }

    /// <summary>
    /// Deletes the specified directory and all its content, including all its subdirectories.
    /// </summary>
    /// <param name="targetDirectory">The full path of the directory that you want to delete.</param>
    /// <param name="debug">Whether or not to print debug messages.</param>
    public static void DeleteDirectoryRecursively(string targetDirectory, bool debug = false)
    {
        // Check if directory exists
        if (Directory.Exists(targetDirectory))
        {
            // Delete the directory and all its contents
            Directory.Delete(targetDirectory, recursive: true);
            
            if (debug)
                Console.WriteLine($"Deleted: {targetDirectory}");
        }
        else
            Console.WriteLine($"The specified directory {targetDirectory} does not exist.");
    }

    /// <summary>
    /// Deletes the files satisfying the specified searchPattern in the given directory and all its subdirectories.
    /// </summary>
    /// <param name="targetDirectory">The full path of the directory that you want to delete files from.</param>
    /// <param name="searchPattern">The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but doesn't support regular expressions.</param>
    /// <param name="debug">Whether or not to print debug messages.</param>
    public static void DeleteFilesRecursively(string targetDirectory, string searchPattern, bool debug = false)
    {
        // Check if directory exists
        if (Directory.Exists(targetDirectory))
        {
            // Get all files matching searchPattern in current directory and all its subdirectories
            string[] files = Directory.GetFiles(targetDirectory, searchPattern, SearchOption.AllDirectories);

            // Delete all found files
            foreach (string file in files)
            {
                File.Delete(file);
                
                if (debug)
                    Console.WriteLine($"Deleted: {file}");
            }
        }
        else
        {
            Console.WriteLine($"The specified directory {targetDirectory} does not exist.");
        }
    }
}