namespace WebAPI.Resources
{
    public static class MessagesManager
    {
        public static string CannotBeEmpty(string arg1)
        {
            return ArgumentizeString(Messages.CannotBeEmpty, arg1);
        }

        public static string UsernameOrEmailIsAlreadyTaken(string arg1)
        {
            return ArgumentizeString(Messages.UsernameOrEmailAlreadyTaken, arg1);
        }

        public static string UserNotFound
        {
            get => Messages.UserNotFound;
        }

        public static string CannotBeEmptyOrWhitespaceOnly
        {
            get => Messages.CannotBeEmptyOrWhitespaceOnly;
        }

        public static string UsernameOrPasswordIsIncorrect
        {
            get => Messages.UsernameOrPasswordIsIncorrect;
        }

        public static string ApplicationNotFound
        {
            get => Messages.ApplicationNotFound;
        }

        public static string InvalidEmail
        {
            get => Messages.InvalidEmail;
        }

        private static string ArgumentizeString(string input, params string[] args)
        {
            int i = 0;
            foreach (var arg in args)
            {
                input = input.Replace($"%{++i}", arg);
            }

            return input;
        }
    }
}
