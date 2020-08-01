namespace SeleniumExpTestProject.src.Others
{
    internal static class Data
    {
        internal const string webAppUrl = "https://pbarbeiro-secrets.herokuapp.com/";
        private static string testCaseUsername = string.Empty;
        private static string testCasePassword = string.Empty;
        internal const int threadSleepTime = 400;
        internal const string loginRoute = "login";
        internal const string loginText = "Login";
        internal const string registerButtonText = "Register";
        internal const string excMsgUsingDriverBlock = "Exception found insde 'using' block:";
        internal const string excMsgMain = "Exception found:";
        internal const string excMsgIsLoggedIn = "Exception found while checking logged in state";
        internal const string excMsgIsTextPresent = "Exception found while checking if text is present in element:";

        #region Set
        internal static void SetTestCaseUsername(string username)
        {
            testCaseUsername = username;
        }

        internal static void SetTestCasePassword(string password)
        {
            testCasePassword = password;
        }
        #endregion

        #region Get
        internal static string GetTestCaseUsername()
        {
            return testCaseUsername;
        }

        internal static string GetTestCasePassword()
        {
            return testCasePassword;
        }
        #endregion
    }
}
