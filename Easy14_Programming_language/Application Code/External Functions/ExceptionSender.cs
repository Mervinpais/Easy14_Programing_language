using System;
public static class ExceptionSender
{
    public static void SendException(string typeOfException, string[] textArray = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        string message = GetMessage(typeOfException);
        Console.WriteLine("Error: Exception " + message);
        if (textArray != null)
            Console.WriteLine(string.Join(Environment.NewLine, textArray));
        Console.ResetColor();
        Console.ReadKey();
        //Environment.Exit(-1);
    }

    private static string GetMessage(string typeOfException)
    {
        string Message = null;
        const string unknownException = "UNKNOWN EXCEPTION";
        const string noException = "NO EXCEPTION";
        const string ErrorException = "ERROR EXCEPTION"; //what
        switch (typeOfException)
        {
            case noException: Message = $"{typeOfException}; Non-Error Exception"; break;
            case unknownException: Message = $"{typeOfException}; ? Exception"; break;
            case "0x0000A0": Message = $"{typeOfException}; Application Error - Unknown Application Error"; break;
            case "0x0000A1": Message = $"{typeOfException}; Failed - Not enough computer memory!"; break;
            case "0x0000A2": Message = $"{typeOfException}; Failed - Missing Files..."; break;
            case "0x0000A3": Message = $"{typeOfException}; Failed - Missing System Dll's!"; break;
            case "0x0000A4": Message = $"{typeOfException}; Failed - Not Responding.."; break;
            case "0x0000A5": Message = $"{typeOfException}; Failed - Application Crash"; break;

            //Program Errors (User code)

            case "0x0000B1": Message = $"{typeOfException}; String inside an Integer/Decimal Function!"; break;
            case "0x0000B2": Message = $"{typeOfException}; Bool inside an Integer/Decimal Function!"; break;
            case "0x0000B3": Message = $"{typeOfException}; Integer/Decimal inside an String Function!"; break;
            case "0x0000B4": Message = $"{typeOfException}; Bool inside a String Function!"; break;
            case "0x0000B5": Message = $"{typeOfException}; Integer/Decimal inside an Bool Function!"; break;
            case "0x0000B6": Message = $"{typeOfException}; String inside a Bool Function!"; break;
            case "0x0000B7": Message = $"{typeOfException}; An Error Occured while adding 2 numbers"; break;
            case "0x0000B8": Message = $"{typeOfException}; An Error Occured while subtracting 2 numbers"; break;
            case "0x0000B9": Message = $"{typeOfException}; An Error Occured while multiplying 2 numbers"; break;
            case "0x000B10": Message = $"{typeOfException}; An Error Occured while dividing 2 numbers"; break;
            case "0x000B11": Message = $"{typeOfException}; An Error Occured while dividing with 0, You can't divide by Zero!"; break;
            case "0x000BC1": Message = $"{typeOfException}; An Error Occured while Converting a variable to a string"; break;
            case "0x000BC2": Message = $"{typeOfException}; An Error Occured while Converting a variable to a integer/Decimal"; break;
            case "0x000BC3": Message = $"{typeOfException}; An Error Occured while Converting a variable to a boolean"; break;
            case "0x000B12": Message = $"{typeOfException}; An Error Occured; You entered less than the required Digits needed!"; break;
            case "0x000B13": Message = $"{typeOfException}; An Error Occured; You entered more than the required Digits needed!"; break;
            case "0xF00001": Message = $"{typeOfException}; File Not Found"; break;
            case "0xF00002": Message = $"{typeOfException}; Folder Not Found"; break;
            case "0xF00003": Message = $"{typeOfException}; File is UnReadable!"; break;
            case "0xF00004": Message = $"{typeOfException}; Folder is UnReadable!"; break;
            case "0xF00005": Message = $"{typeOfException}; File is UnAccessable!"; break;
            case "0xF00006": Message = $"{typeOfException}; Folder is UnAccessable!"; break;
            case "0xFC0001": Message = $"{typeOfException}; Method is not Found!"; break;
            case "0xFC0002": Message = $"{typeOfException}; Variable is not Found!"; break;

            //Code Errors (App Errors)
            case "0x0000C0": Message = $"{typeOfException}; Application Code Error - Unknown Code Error"; break;
            case "0x0000C1": Message = $"{typeOfException}; Application Code Error - Failed to run code"; break;
            case "0x0000C2": Message = $"{typeOfException}; Application Code Error - Missing Code"; break;
            //Funni/Not Real Errors (or you could say a 0% Chance Error since you can't get it ever)
            case "0x000404": Message = $"{typeOfException}; 404 - Failed to Get ERROR of ERROR"; break;
            default: Message = $"{typeOfException}; An Unknown Error Occured"; break;
        }
        return Message;
    }
}