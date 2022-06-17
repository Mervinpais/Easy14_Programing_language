using System;
using System.Collections.Generic;
using System.IO;
public class ExceptionSender
{
    public void SendException(int typeOfException, string[] textArray = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(GetMessage(typeOfException));
        if (textArray != null)
            Console.WriteLine(textArray);
        Console.WriteLine("Please Fix this error to continue :(");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey();
        Environment.Exit(-1);
    }

    private static string GetMessage(int typeOfException)
    {
        string Message = null;
        string typeOfException_str = typeOfException.ToString();
        switch (typeOfException)
        {
            //! NOTE: These codes below are not error codes that meantions a part in memory where the error happened, these erorr codes are for like example;
            //! - Uh oh, file Math_function1().cs is not found, ERROR CODE: 0x2431 (Not real error code)
            //* Also The Error codes are in Hexa-Decimal

            case 0x000000:
                Message = "EXCEPTION " + typeOfException_str + "; No-Error Exception";
                break;
            case 0x000001:
                Message = "EXCEPTION " + typeOfException_str + "; No-Error Exception";
                break;
            case 0x000002:
                Message = "EXCEPTION " + typeOfException_str + "; Unknown Exception - No Data Was Given";
                break;
            case 0x0000A0:
                Message = "EXCEPTION " + typeOfException_str + "; Application Error - Unknown Application Error";
                break;
            case 0x0000A1:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Not enough computer memory!";
                break;
            case 0x0000A2:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Missing Files...";
                break;
            case 0x0000A3:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Missing System Dll's!";
                break;
            case 0x0000A4:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Not Responding..";
                break;
            case 0x0000A5:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Application Crash";
                break;
            //Program Errors (User code)

            case 0x0000B1:
                Message = "EXCEPTION " + typeOfException_str + "; String inside an Integer/Decimal Function!";
                break;
            case 0x0000B2:
                Message = "EXCEPTION " + typeOfException_str + "; Bool inside an Integer/Decimal Function!";
                break;
            case 0x0000B3:
                Message = "EXCEPTION " + typeOfException_str + "; Integer/Decimal inside an String Function!";
                break;
            case 0x0000B4:
                Message = "EXCEPTION " + typeOfException_str + "; Bool inside a String Function!";
                break;
            case 0x0000B5:
                Message = "EXCEPTION " + typeOfException_str + "; Integer/Decimal inside an Bool Function!";
                break;
            case 0x0000B6:
                Message = "EXCEPTION " + typeOfException_str + "; String inside a Bool Function!";
                break;
            case 0x0000B7:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while adding 2 numbers";
                break;
            case 0x0000B8:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while subtracting 2 numbers";
                break;
            case 0x0000B9:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while multiplying 2 numbers";
                break;
            case 0x000B10:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while dividing 2 numbers";
                break;
            case 0x000B11:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while dividing with 0, You can't divide by Zero!";
                break;
            case 0x000BC1:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while Converting a variable to a string";
                break;
            case 0x000BC2:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while Converting a variable to a integer/Decimal";
                break;
            case 0x000BC3:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured while Converting a variable to a boolean";
                break;
            case 0x000B12:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured; You entered less than the required Digits needed!";
                break;
            case 0x000B13:
                Message = "EXCEPTION " + typeOfException_str + "; An Error Occured; You entered more than the required Digits needed!";
                break;
            case 0xF00001:
                Message = "FILE IO EXCEPTION " + typeOfException_str + "; File Not Found";
                break;
            case 0xF00002:
                Message = "FILE IO EXCEPTION " + typeOfException_str + "; Folder Not Found";
                break;
            case 0xF00003:
                Message = "FILE IO EXCEPTION " + typeOfException_str + "; File is UnReadable!";
                break;
            case 0xF00004:
                Message = "FILE IO EXCEPTION " + typeOfException_str + "; Folder is UnReadable!";
                break;

            //Code Errors (App Errors)
            case 0x0000C0:
                Message = "EXCEPTION " + typeOfException_str + "; Application Code Error - Unknown Code Error";
                break;
            case 0x0000C1:
                Message = "EXCEPTION " + typeOfException_str + "; Application Code Error - Failed to run code";
                break;
            case 0x0000C2:
                Message = "EXCEPTION " + typeOfException_str + "; Application Code Error - Missing Code";
                break;
            //File Errors
            case 0x0000F0:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Unknown File Error";
                break;
            case 0x0000F1:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Can't Access Code File";
                break;
            case 0x0000F2:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - File Not Found";
                break;
            case 0x0000F3:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Method Not Found";
                break;
            case 0x0000F4:
                Message = "EXCEPTION " + typeOfException_str + "; Failed to interperate - Variable Not Found";
                break;
            //Funni/Not Real Errors (or you could say a 0% Chance Error since you can't get it ever)
            case 0x000404:
                Message = "EXCEPTION " + typeOfException_str + "; 404 - Failed to Get ERROR of ERROR";
                break;
            case 0x00FACE:
                Message = "EXCEPTION " + typeOfException_str + "; O_O Did you want Facey?";
                break;
            case 0x00ABCD:
                Message = "EXCEPTION " + typeOfException_str + "; Are you reciting the alaphabet O_O?";
                break;
        }
        return Message;
    }
}