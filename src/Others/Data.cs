namespace SeleniumExpTestProject.src.Others
{
    internal static class Data
    {
        internal const string webAppUrl = "https://pbarbeiro-secrets.herokuapp.com/";
        private static string testCaseUsername = string.Empty;
        private static string testCasePassword = string.Empty;
        internal const string registerButtonText = "Register";

        #region Set
        internal static void SetTestCaseUsername(string username)
        {
            testCaseUsername = username;
        }

        internal static void SetTestCasePassword(string password)
        {
            testCaseUsername = password;
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
