# FileSystem Class Documentation

The `FileSystem` class in Easy14 provides functions for editing the file system.

## Table of Contents

## FileSystem.MakeFile(x)

The `FileSystem.MakeFile(x)` function creates a new file with the specified name.

  ### Parameters

`x`: The name of the file to create.

  ### Example

```easy14
      // Create a new file named "example.txt"
      FileSystem.MakeFile("example.txt");
```

## FileSystem.MoveFile(x)

  The `FileSystem.MoveFile(x)` function moves a file to a different location or renames it.

  ### Parameters

`x`: The current path of the file to move or rename.

  ### Example

```easy14
      // Rename a file from "oldfile.txt" to "newfile.txt"
      FileSystem.MoveFile("oldfile.txt", "newfile.txt");
```

## FileSystem.WriteFile(x)

  The `FileSystem.WriteFile(x)` function writes content to a file.

  ### Parameters

`x`: The path of the file to write content to.
`content`: The content to write to the file.

  ### Example

```easy14
      // Write the text "Hello, World!" to a file named "example.txt"
      FileSystem.WriteFile("example.txt", "Hello, World!");
```

## FileSystem.DeleteFile(x)

  The `FileSystem.DeleteFile(x)` function deletes a file.

  ### Parameters

`x`: The path of the file to delete.

  ### Example

```easy14
      // Delete a file named "example.txt"
      FileSystem.DeleteFile("example.txt");
```

## FileSystem.ReadAllLines(x)

  The `FileSystem.ReadAllLines(x)` function reads all lines from a text file and returns them as a list of strings.

  ### Parameters

`x`: The path of the file to read.

  ### Example

```easy14
      // Read all lines from a file named "example.txt"
      List<string> lines = FileSystem.ReadAllLines("example.txt");
```

## FileSystem.ReadFile(x)

  The `FileSystem.ReadFile(x)` function reads the entire content of a file as a single string.

  ### Parameters

`x`: The path of the file to read.

  ### Example

```easy14
      // Read the entire content of a file named "example.txt"
      string content = FileSystem.ReadFile("example.txt");
```

## FileSystem.RenameFile(x)

  The `FileSystem.RenameFile(x)` function renames a file.

  ### Parameters

`x`: The current path of the file to rename.
`newName`: The new name for the file.

  ### Example

```easy14
      // Rename a file from "oldfile.txt" to "newfile.txt"
      FileSystem.RenameFile("oldfile.txt", "newfile.txt");
```

## FileSystem.MakeDirectory(x)

  The `FileSystem.MakeDirectory(x)` function creates a new directory with the specified name.

  ### Parameters

`x`: The name of the directory to create.

  ### Example

```easy14
      // Create a new directory named "myfolder"
      FileSystem.MakeDirectory("myfolder");
```

## FileSystem.DeleteDirectory(x)

  The `FileSystem.DeleteDirectory(x)` function deletes a directory and its contents.

  ### Parameters

`x`: The path of the directory to delete.

  ### Example

```easy14
      // Delete a directory named "myfolder" and all its contents
      FileSystem.DeleteDirectory("myfolder");
```

That's a comprehensive overview of the `FileSystem` class in Easy14, including its functions for file and directory manipulation. These functions allow you to perform various file system operations within your Easy14 programs.
