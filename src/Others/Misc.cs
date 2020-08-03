using System;
using System.Threading;

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

        public static void Beep()//Sends command to emit a beep sound
        {
            Console.Beep(Data.beepFrequency, Data.beepDuration);
        }

        #region Extras

        internal static void StartProgramMsg()//Contains startup message to be displayed to user
        {
            Console.WriteLine($"*** - Initializing program to test {Data.webAppUrl} - ***");
            Space();
            Sleep(3);

            Console.WriteLine($"A beep sound with frequency {Data.beepFrequency}(hertz) will be played whenever your attention is required.");
            Space();
            Sleep(3);

            Console.WriteLine("Press any key to test sound. --It is highly recommended to stop/pause any current playing sound/music --");

            Space();
            ListenForKeyPress();
            Beep();
            Space();

            Space();
            Console.WriteLine("A beep should have played by now. Press any button to startup the test..");
            Space();
            ListenForKeyPress();
            Space();
        }

        internal static void ErrorFoundQuit(string cause)//Used to quit the app when an unhandled error occurs
        {
            Beep();
            Console.WriteLine($"An error occured while {cause}");
            Space();
            Sleep(3);

            Console.WriteLine("Press any key to acknowledge error & quit.");
            Space();
            Console.ReadKey();
            Environment.Exit(0);
        }

        internal static void Space()//Inserts an empty line in CLI
        {
            Console.WriteLine();
        }

        internal static void ListenForKeyPress()//waits for any key to be pressed
        {
            Console.ReadKey();
        }

        internal static void Sleep(double secs)//Forces Thread to sleep for specified seconds
        {
            Thread.Sleep(secs*1000);
        }
        #endregion
    }
}