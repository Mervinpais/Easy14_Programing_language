using System;
using System.Net.NetworkInformation;

namespace Easy14_Programming_Language
{
    public static class checkForInternet
    {
        public static bool IsConnectedToInternet()
        {
            string host = "www.google.com";
            bool result = false;
            Ping p = new Ping();
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    PingReply reply = p.Send(host, 2000);
                    if (reply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                    else { return false; }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occured while checking internet...\n\n {e.InnerException}\n");
                    return false;
                }
            }
            return result;
        }
    }
}
