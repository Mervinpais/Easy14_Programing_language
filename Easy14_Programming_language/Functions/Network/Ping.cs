using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Net.NetworkInformation;

namespace Easy14_Programming_Language
{
    public static class NetworkPing
    {
        public static IPStatus Interperate(string address)
        {
            try
            {
                Ping ping = new Ping();
                return ping.Send(address).Status;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return IPStatus.Unknown; }
        }
    }
}