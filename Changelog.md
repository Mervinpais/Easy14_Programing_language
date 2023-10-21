14/Oct/2023

# ChangeLog

### I shouldnt be writing this on my birthday, but oh well.. there is work that has to be done

### Main Point(s)

(1  Removed old shitty code from yesteryear
```csharp
if (Configuration.GetBoolOptionValue("showOptionsINI_DataWhenE14_Loads") == true)
{
    List<string> configFileLIST = new List<string>();

    foreach (string currentLine in configFile)
    {
        if (currentLine.StartsWith(";") || currentLine == "" || currentLine == " ") continue;
        configFileLIST.Add(currentLine);
    }

    string[] configFile_modified = configFileLIST.ToArray();

    Console.WriteLine(string.Join(Environment.NewLine, configFile_modified));
    Console.WriteLine("\n========================\n\n");
}
```