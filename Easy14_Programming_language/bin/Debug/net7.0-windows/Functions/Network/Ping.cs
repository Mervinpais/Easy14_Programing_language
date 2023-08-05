using System;
using System.IO;
using System.Net.NetworkInformation;

namespace Easy14_Programming_Language
{
    public static class NetworkPing
    {
        public static void Interperate(string code_part, string[] textArray)
        {
            string textToPrint;
            code_part = code_part.TrimStart();

            if (code_part.StartsWith("Ping("))
            {
                bool foundUsing = false;
                string[] someLINEs = textArray;
                if (textArray == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the DeleteFile function.");
                    Console.ResetColor();
                    return;
                }
                foreach (string x in someLINEs)
                {
                    if (x.StartsWith("using"))
                    {
                        if (x == "using Network;")
                        {
                            foundUsing = true;
                            break;
                        }
                    }
                    if (x == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Network' wasnt referenced to use 'Ping' without its reference  (Use Network.Ping(\"*Website*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith("Network.Ping(")) { }

            if (code_part.StartsWith("Network.Ping("))
            {
                code_part = code_part.Substring("Network.Ping(".Length);
            }
            else if (code_part.StartsWith("Ping("))
            {
                code_part = code_part.Substring("Ping(".Length);
            }

            code_part = code_part.TrimEnd().TrimStart();
            code_part = code_part.Substring(0, code_part.Length - 2);
            if (code_part.EndsWith(")"))
            {
                code_part = code_part.Substring(0, code_part.Length - 1);
            }

            textToPrint = code_part;
            if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                var contentInFile = textToPrint.Replace('"'.ToString(), "");
                try
                {
                    try
                    {
                        Ping ping = new Ping();
                        PingReply reply = ping.Send(contentInFile, 1500);
                        if (reply != null)
                        {
                            Console.WriteLine("Status;\n   " + reply.Status);
                            Console.WriteLine("Time;\n   " + reply.RoundtripTime);
                            Console.WriteLine("Address;\n   " + reply.Address);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR; Couldn't Ping Address due to an error");
                        Console.WriteLine("Extra Info is below;\n\n" + e);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Read File at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            else if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
            {
                if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                {
                    string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                    foreach (string file in files)
                    {
                        if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "") == textToPrint)
                        {
                            try
                            {
                                Ping ping = new Ping();
                                PingReply reply = ping.Send(file, 1500);
                                if (reply != null)
                                {
                                    Console.WriteLine("Status;\n" + reply.Status + "\n Time;\n" + reply.RoundtripTime + "\n Address;\n" + reply.Address);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ERROR; Couldn't Ping Address due to an error");
                                Console.WriteLine("Extra Info is below;\n\n" + e);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}