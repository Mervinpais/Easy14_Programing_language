# SDL2 Class Documentation

The `SDL2` class in Easy14 provides functions for playing and managing audio files.

## Table of Contents

## SDL2.CreateWindow(posX, posY, sizeX, sizeY, title)

  The `SDL2.CreateWindow(posX, posY, sizeX, sizeY, title);` function creates an SDL2 window at the given position (X, Y) with the specified size (Width, Height) and a title.

  ### Parameters

    `posX`: The X-coordinate position of the window.
    `posY`: The Y-coordinate position of the window.
    `sizeX`: The width of the window.
    `sizeY`: The height of the window.
    `title`: The title of the window.

  ### Example

    ```easy14
      // Create an SDL2 window at position (100, 100) with size (800, 600) and title "My Window"
      SDL2.CreateWindow(100, 100, 800, 600, "My Window");
      ```

## SDL2.CreateShape(windowLong, Xpos, Ypos, sizeX, sizeY)

  The `SDL2.CreateShape(windowLong, Xpos, Ypos, sizeX, sizeY);` function creates a shape within an SDL2 window.

  ### Parameters

    `windowLong`: The SDL2 window where the shape will be created.
    `Xpos`: The X-coordinate position of the shape.
    `Ypos`: The Y-coordinate position of the shape.
    `sizeX`: The width of the shape.
    `sizeY`: The height of the shape.

  ### Example

    ```easy14
      // Create a shape within an SDL2 window at position (200, 200) with size (100, 50)
      SDL2.CreateShape(windowLong, 200, 200, 100, 50);
      ```

## SDL2.ClearWindow(windowLong, Color)

  The `SDL2.ClearWindow(windowLong, Color);` function clears the content of an SDL2 window with a specified color.

  ### Parameters

    `windowLong`: The SDL2 window to clear.
    `Color`: The color to clear the window with.

  ### Example

    ```easy14
      // Clear an SDL2 window with a color (e.g., red)
      SDL2.ClearWindow(windowLong, Color.Red);
      ```

That's the documentation for the `SDL2` class functions in Easy14. These functions allow you to create windows, shapes, and clear window content in your Easy14 programs.
