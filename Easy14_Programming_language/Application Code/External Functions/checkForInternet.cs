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
            try
            {
                PingReply reply = p.Send(host, 2500);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch { }
            return result;
        }
    }
}
