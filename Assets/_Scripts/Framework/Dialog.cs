namespace Mcpgnz.DesktopFramework
{
    public static class Dialog
    {
        public static string Error(string message, params string[] options)
        {
            return Message("Clipper", message, Config.ErrorAvatar, options);
        }

        public static string Explorer(string message, params string[] options)
        {
            return Message("Clipper", message, Config.ExplorerAvatar, options);
        }
        public static string Clipper(string message, params string[] options)
        {
            return Message("Clipper", message, Config.ClippyAvatar, options);
        }

        public static string Message(string title, string message, IconEx icon, params string[] options)
        {
            var iconPath = icon == null ? null : icon.AbsolutePath;

            string response = null;
            var msg = new MessageBox(title, message, options, iconPath)
            {
                OnButtonClick = r =>
                {
                    response = r;
                }
            };
            msg.ShowDialog();
            return response;
        }
    }
}