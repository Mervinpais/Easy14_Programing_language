# Console Class Documentation

 The `Console` class in Easy14 provides several useful functions for input, output, and interaction.

## Table of Contents

 - Console.Print(x);
 - Console.Input(x);
 - Console.Clear();
 - Console.Exec(x);
 - Console.Beep(x, y);
## Console.Print(x)

 The `Console.Print(x)` function allows you to display text or values on the console. It is used for output.

### Parameters

`x`: The text or value to be displayed on the console.

### Example

 ```easy14
 Console.Print("Hello, World!");
 ```

## Console.Input(x)

 The `Console.Input(x)` function allows you to receive user input from the console. It is used for input.

### Parameters

`x`: The prompt or message to display before waiting for user input.

 ### Example

 ```easy14
 Var.New(name, () => Console.Input("Please enter your name: "));
 Console.Print("Hello, "  name  "!");
 ```

## Console.Clear()

 The `Console.Clear()` function clears the console screen, removing all previous output.

### Example

 ```easy14
 Console.Clear();
 ```

## Console.Exec(x)

 The `Console.Exec(x)` function allows you to execute Easy14 code from a string.

### Parameters
`x`: The Easy14 code to be executed.

### Example

 ```easy14
 Console.Exec("Console.Print(\"This code was executed dynamically!\");");
 ```

## Console.Beep(x, y)

The `Console.Beep(x, y)` function produces a beep sound.

### Parameters

`x`: The frequency of the beep.
`y`: The duration of the beep in milliseconds.

### Example

 ```easy14
 Console.Beep(500, 1000); // Beep at 500Hz for 1 second.
 ```

That's a brief overview of the `Console` class in Easy14. These functions provide essential functionality for input and output in your Easy14 programs.
