using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.Linq;
using System.Management;

public class ExceptionSender
{
    public void SendException(int typeOfException)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(GetMessage(typeOfException));
        Console.WriteLine("Please Fix this error to continue :(");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey();
        Environment.Exit(-1);
    }
    
    private static string GetMessage(int typeOfException) 
    {
        string Message = null;
        switch (typeOfException)
        {
            //! NOTE: These codes below are not error codes that meantions a part in memory where the error happened, these erorr codes are for like example;
            //! - Uh oh, file Math_().cs is not found, ERROR CODE: 0x2431 (Not real error code)
            //! So just be aware about this
            //* Also The Error codes are in Hexa-Decimal
            case 0x000000:
                Message = "EXCEPTION " + typeOfException + "; No-Error Exception";
                break;
            case 0x000001:
                Message = "EXCEPTION " + typeOfException + "; No-Error Exception";
                break;
            case 0x000002:
                Message = "EXCEPTION " + typeOfException + "; Basic Exception - No Data Was Given";
                break;
            case 0x000003:
                Message = "EXCEPTION " + typeOfException + "; Failed to interperate - Not enough computer memory!";
                break;
            case 0x000004:
                Message = "EXCEPTION " + typeOfException + "; Failed to interperate - Missing Files...";
                break;
            case 0x000005:
                Message = "EXCEPTION " + typeOfException + "; Failed to interperate - Missing System Dll's!";
                break;
            case 0x000006:
                Message = "EXCEPTION " + typeOfException + "; Failed to interperate - Not Responding..";
                break;
            case 0x0000F0:
                Message = "EXCEPTION " + typeOfException + "; Failed to interperate - Can't Access Code File";
                break;
            case 0x000404:
                Message = "EXCEPTION " + typeOfException + "; 404 - Failed to Get ERROR of ERROR";
                break;
            case 0x00FACE:
                Message = "EXCEPTION " + typeOfException + "; O_O Did you want Facey?";
                break;
            case 0x00ABCD:
                Message = "EXCEPTION " + typeOfException + "; Are you reciting the alaphabet O_O?";
                break;
        }
        return Message;
    }
}