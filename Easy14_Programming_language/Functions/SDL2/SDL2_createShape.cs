using SDL2;
using System;

namespace Easy14_Programming_Language
{
    public static class SDL2_createShape
    {
        public static void Interperate(long window, int x, int y, int w, int h)
        {
            IntPtr window_intPtr = (IntPtr)window;
            try
            {
                IntPtr renderer = SDL.SDL_GetRenderer(window_intPtr);
                SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
                // Draws a point at (20, 20) using the currently set color.
                SDL.SDL_RenderDrawPoint(renderer, 150, 150);
                // Specify the coordinates for our rectangle we will be drawing.
                var rect = new SDL.SDL_Rect
                {
                    x = x,
                    y = y,
                    w = w,
                    h = h
                };

                SDL.SDL_SetRenderDrawColor(renderer, 135, 206, 235, 255);

                // Set the color to red before drawing our shape
                SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);

                // Draw a filled in rectangle.
                SDL.SDL_RenderFillRect(renderer, ref rect);

                SDL.SDL_RenderPresent(renderer);
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ConsolePrint.Interperate("Unspecified Error; No Renderer found!");
                Console.ResetColor();
            }
        }
    }
}
