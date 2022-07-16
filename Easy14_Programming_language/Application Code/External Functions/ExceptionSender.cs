using System;
public static class ExceptionSender
{
    public static void SendException(string typeOfException, string[] textArray = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        string message = GetMessage(typeOfException);
        Console.WriteLine("EXCEPTION " + message);
        if (textArray != null)
            Console.WriteLine(string.Join(Environment.NewLine, textArray));
        Console.ResetColor();
        Console.ReadKey();
        //Environment.Exit(-1);
    }

    private static string GetMessage(string typeOfException)
    {
        string Message = null;
        string typeOfException_str = typeOfException.ToString();
        switch (typeOfException_str)
        {
            case "0x000000":
                Message = $"{typeOfException_str}; No-Error Exception";
                break;
            case "0x000001":
                Message = $"{typeOfException_str}; No-Error Exception";
                break;
            case "0x000002":
                Message = $"{typeOfException_str}; Unknown Exception - No Data Was Given";
                break;
            case "0x0000A0":
                Message = $"{typeOfException_str}; Application Error - Unknown Application Error";
                break;
            case "0x0000A1":
                Message = $"{typeOfException_str}; Failed to interperate - Not enough computer memory!";
                break;
            case "0x0000A2":
                Message = $"{typeOfException_str}; Failed to interperate - Missing Files...";
                break;
            case "0x0000A3":
                Message = $"{typeOfException_str}; Failed to interperate - Missing System Dll's!";
                break;
            case "0x0000A4":
                Message = $"{typeOfException_str}; Failed to interperate - Not Responding..";
                break;
            case "0x0000A5":
                Message = $"{typeOfException_str}; Failed to interperate - Application Crash";
                break;
            //Program Errors (User code)

            case "0x0000B1":
                Message = $"{typeOfException_str}; String inside an Integer/Decimal Function!";
                break;
            case "0x0000B2":
                Message = $"{typeOfException_str}; Bool inside an Integer/Decimal Function!";
                break;
            case "0x0000B3":
                Message = $"{typeOfException_str}; Integer/Decimal inside an String Function!";
                break;
            case "0x0000B4":
                Message = $"{typeOfException_str}; Bool inside a String Function!";
                break;
            case "0x0000B5":
                Message = $"{typeOfException_str}; Integer/Decimal inside an Bool Function!";
                break;
            case "0x0000B6":
                Message = $"{typeOfException_str}; String inside a Bool Function!";
                break;
            case "0x0000B7":
                Message = $"{typeOfException_str}; An Error Occured while adding 2 numbers";
                break;
            case "0x0000B8":
                Message = $"{typeOfException_str}; An Error Occured while subtracting 2 numbers";
                break;
            case "0x0000B9":
                Message = $"{typeOfException_str}; An Error Occured while multiplying 2 numbers";
                break;
            case "0x000B10":
                Message = $"{typeOfException_str}; An Error Occured while dividing 2 numbers";
                break;
            case "0x000B11":
                Message = $"{typeOfException_str}; An Error Occured while dividing with 0, You can't divide by Zero!";
                break;
            case "0x000BC1":
                Message = $"{typeOfException_str}; An Error Occured while Converting a variable to a string";
                break;
            case "0x000BC2":
                Message = $"{typeOfException_str}; An Error Occured while Converting a variable to a integer/Decimal";
                break;
            case "0x000BC3":
                Message = $"{typeOfException_str}; An Error Occured while Converting a variable to a boolean";
                break;
            case "0x000B12":
                Message = $"{typeOfException_str}; An Error Occured; You entered less than the required Digits needed!";
                break;
            case "0x000B13":
                Message = $"{typeOfException_str}; An Error Occured; You entered more than the required Digits needed!";
                break;
            case "0xF00001":
                Message = $"{typeOfException_str}; File Not Found";
                break;
            case "0xF00002":
                Message = $"{typeOfException_str}; Folder Not Found";
                break;
            case "0xF00003":
                Message = $"{typeOfException_str}; File is UnReadable!";
                break;
            case "0xF00004":
                Message = $"{typeOfException_str}; Folder is UnReadable!";
                break;
            case "0xF00005":
                Message = $"{typeOfException_str}; File is UnAccessable!";
                break;
            case "0xF00006":
                Message = $"{typeOfException_str}; Folder is UnAccessable!";
                break;
            case "0xFC0001":
                Message = $"{typeOfException_str}; Method is not Found!";
                break;
            case "0xFC0002":
                Message = $"{typeOfException_str}; Variable is not Found!";
                break;

            //Code Errors (App Errors)
            case "0x0000C0":
                Message = $"{typeOfException_str}; Application Code Error - Unknown Code Error";
                break;
            case "0x0000C1":
                Message = $"{typeOfException_str}; Application Code Error - Failed to run code";
                break;
            case "0x0000C2":
                Message = $"{typeOfException_str}; Application Code Error - Missing Code";
                break;
            //Funni/Not Real Errors (or you could say a 0% Chance Error since you can't get it ever)
            case "0x000404":
                Message = $"{typeOfException_str}; 404 - Failed to Get ERROR of ERROR";
                break;
            case "0x00FACE":
                Message = $"{typeOfException_str}; O_O Did you want Facey?";
                break;
            case "0x00ABCD":
                Message = $"{typeOfException_str}; Are you reciting the alaphabet O_O?";
                break;
            default:
                Message = $"{typeOfException_str}; An Unknown Error Occured";
                break;
        }
        return Message;
    }
}