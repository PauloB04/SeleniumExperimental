namespace SeleniumExpTestProject.src.Others
{
    internal static class Data
    {
        internal const string webAppUrl = "https://pbarbeiro-secrets.herokuapp.com/";
        private static string testCaseUsername = string.Empty;
        private static string testCasePassword = string.Empty;
        internal const string registerButtonText = "Register";
        internal const string ExcMsgUsingDriverBlock = "Exception found insde 'using' block:";
        internal const string ExcMsgMain = "Exception found:";

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
