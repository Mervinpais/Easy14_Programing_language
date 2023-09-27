using SDL2;
using System;

namespace Easy14_Programming_Language
{
    public static class SDL2_makeWindow
    {
        static IntPtr window;
        static IntPtr renderer;

        public static void Setup(int sizeX = 200, int sizeY = 200, int posX = SDL.SDL_WINDOWPOS_UNDEFINED, int posY = SDL.SDL_WINDOWPOS_UNDEFINED, string title = "myWindow")
        {
            // Initilizes SDL.
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
            }

            // Create a new window given a title, size, and passes it a flag indicating it should be shown.
            window = SDL.SDL_CreateWindow(
                title,
                posX,
                posY,
                sizeX,
                sizeY,
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
            );

            if (window == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
            }

            // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
            renderer = SDL.SDL_CreateRenderer(
                window,
                -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (renderer == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\nWindow \"{window}\" was created and shown");
            Console.ResetColor();
            Console.WriteLine("");
        }

        static string PollEvents(bool running = true)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
            {
                //Console.WriteLine(e.type);
                if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT && e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                {
                    // ... Handle window close for each window ...
                    // Note, you can also check e.window.windowID to check which
                    // of your windows the event came from.
                    // e.g.:
                    if (SDL.SDL_GetWindowID(window) == e.window.windowID)
                    {
                        // ... close window A ...
                        running = false;
                        return "<QUIT>";
                    }
                }
            }
            return "<?>";
        }

        static void Render(byte red = 30, byte green = 30, byte blue = 30, byte alpha = 255)
        {
            // Sets the color that the screen will be cleared with.
            SDL.SDL_SetRenderDrawColor(renderer, red, green, blue, alpha);

            // Clears the current render surface.
            SDL.SDL_RenderClear(renderer);

            /*
            // Set the color to red before drawing our shape
            SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);

            // Draw a line from top left to bottom right
            DL.SDL_RenderDrawLine(renderer, 0, 0, 640, 480);
            */

            // Switches out the currently presented render surface with the one we just did work on.
            SDL.SDL_RenderPresent(renderer);
        }

        static void CleanUp()
        {
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            //SDL.SDL_Quit();
        }

        public static long Interperate(int sizeX = 200, int sizeY = 200, int posX = SDL.SDL_WINDOWPOS_UNDEFINED, int posY = SDL.SDL_WINDOWPOS_UNDEFINED, string title = "myWindow", byte red = 30, byte green = 30, byte blue = 30, byte alpha = 255)
        {
            if (sizeX < 1)
            {
                sizeX = 200;
            }
            else if (sizeY < 1)
            {
                sizeY = 200;
            }
            else if (posX < 0)
            {
                posX = SDL.SDL_WINDOWPOS_UNDEFINED;
            }
            else if (posY < 0)
            {
                posY = SDL.SDL_WINDOWPOS_UNDEFINED;
            }
            else if (title == null)
            {
                title = "myWindow";
            }
            else if (red < 0 || red > 225)
            {
                red = 30;
            }
            else if (green < 0 || green > 225)
            {
                green = 30;
            }
            else if (blue < 0 || blue > 225)
            {
                blue = 30;
            }
            else if (alpha < 0 || alpha > 100)
            {
                alpha = 255;
            }

            Setup(sizeX, sizeY, posX, posY, title);
            Render(red, green, blue, alpha);
            bool running = true;
            while (running)
            {
                string polledEvents = PollEvents();
                if (polledEvents == "<QUIT>") running = false;
            }

            CleanUp();
            long window_long = Convert.ToInt64(Convert.ToString(window), 16);
            return window_long;
        }
    }
}