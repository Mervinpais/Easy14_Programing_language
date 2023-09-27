using System;
using System.Net.NetworkInformation;

namespace Easy14_Programming_Language
{
    public static class checkForInternet
    {
        public static bool IsConnectedToInternet()
        {
            string host = "www.google.com";
            Ping p = new Ping();
            try
            {
                if (p.Send(host, 3000).Status == IPStatus.Success) 
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured while checking internet...\n\n {e.InnerException}\n");
                return false;
            }
        }
    }
}
