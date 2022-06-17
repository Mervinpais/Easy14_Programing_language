using System;
using System.Threading.Tasks;
using SDL2;

namespace Easy14_Programming_Language
{
    class SDL2_makeWindow
    {
        IntPtr window;
        IntPtr renderer;
        bool running = true;

        /// <summary>
        /// Setup all of the SDL resources we'll need to display a window.
        /// </summary>
        void Setup(int sizeX = 200, int sizeY = 200, int posX = SDL.SDL_WINDOWPOS_UNDEFINED, int posY = SDL.SDL_WINDOWPOS_UNDEFINED, string title = "myWindow")
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
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

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
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Checks to see if there are any events to be processed.
        /// </summary>
        void PollEvents()
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Renders to the window.
        /// </summary>
        void Render(byte red = 30, byte green = 30, byte blue = 30, byte alpha = 255)
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

        /// <summary>
        /// Clean up the resources that were created.
        /// </summary>
        void CleanUp()
        {
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        public long interperate(int sizeX = 200, int sizeY = 200, int posX = SDL.SDL_WINDOWPOS_UNDEFINED, int posY = SDL.SDL_WINDOWPOS_UNDEFINED, string title = "myWindow", byte red = 30, byte green = 30, byte blue = 30, byte alpha = 255)
        {
            Setup(sizeX, sizeY, posX, posY, title);
            while (running)
            {
                PollEvents();
                Render(red, green, blue, alpha);
                //System.Threading.Thread.Sleep(1);
            }

            CleanUp();
            long window_long = Convert.ToInt64(Convert.ToString(window), 16);
            return window_long;
        }
    }
}
