using System;

namespace SeleniumExpTestProject.src.Others
{
    public static class Misc
    {
        public static bool IsStringValid(string stringToCheck)//Checks if string passed is valid
        {
            if (!string.IsNullOrEmpty(stringToCheck) && !string.IsNullOrWhiteSpace(stringToCheck))
                return true;

            return false;
        }

        public static bool IsStringNullOr(string stringToCheck)//Checks if string passed is null, empty or whitespace
        {
            if (string.IsNullOrEmpty(stringToCheck) || string.IsNullOrWhiteSpace(stringToCheck))
                return true;

            return false;
        }

        public static string HandleException(Exception e, string msg)//Handles Exception messages
        {
            Console.WriteLine(msg);
            Console.WriteLine($"Error: {e}");
            Console.WriteLine($"Error message: {e.Message}");
            Console.WriteLine($"Code trace: {e.StackTrace}");

            return msg;//TODO: Rethink return value, find way to return error msg & code trace as well (for logging and/or unit testing)
        }
    }
}
