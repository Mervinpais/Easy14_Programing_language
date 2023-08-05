using SDL2;
using System;

namespace Easy14_Programming_Language
{
    public static class SDL2_clearScreen
    {
        public static void Interperate(long window, string color = "darkgrey")
        {
            IntPtr window_intPtr = (IntPtr)window;
            IntPtr renderer = SDL.SDL_GetRenderer(window_intPtr);

            SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);

            //Make a case for the color
            switch (color)
            {
                case "red":
                    SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255); break;
                case "green":
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 255, 0, 255); break;
                case "blue":
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 255, 255); break;
                case "yellow":
                    SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 0, 255); break;
                case "black":
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255); break;
                case "white":
                    SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255); break;
                case "purple":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 0, 128, 255); break;
                case "orange":
                    SDL.SDL_SetRenderDrawColor(renderer, 255, 165, 0, 255); break;
                case "brown":
                    SDL.SDL_SetRenderDrawColor(renderer, 165, 42, 42, 255); break;
                case "grey":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 128, 128, 255); break;
                case "darkgrey":
                    SDL.SDL_SetRenderDrawColor(renderer, 64, 64, 64, 255); break;
                case "lightgrey":
                    SDL.SDL_SetRenderDrawColor(renderer, 192, 192, 192, 255); break;
                case "darkgreen":
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 128, 0, 255); break;
                case "darkblue":
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 128, 255); break;
                case "darkred":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 0, 0, 255); break;
                case "darkyellow":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 128, 0, 255); break;
                case "darkpurple":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 0, 128, 255); break;
                case "darkorange":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 128, 0, 255); break;
                case "darkbrown":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 0, 0, 255); break;
                case "lightgreen":
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 128, 128, 255); break;
                case "lightblue":
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 128, 128, 255); break;
                case "lightred":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 0, 0, 255); break;
                case "lightyellow":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 128, 0, 255); break;
                case "lightpurple":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 0, 128, 255); break;
                case "lightorange":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 128, 0, 255); break;
                case "lightbrown":
                    SDL.SDL_SetRenderDrawColor(renderer, 128, 0, 0, 255); break;
            }

            //Clear the GUI
            SDL.SDL_RenderClear(renderer);

            //Update the GUI
            SDL.SDL_RenderPresent(renderer);
        }
    }
}
