namespace Mcpgnz.DesktopFramework
{
    using System;

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
            string response = null;

            try
            {
                var iconPath = icon == null ? null : icon.AbsolutePath;
                var msg = new MessageBox(title, message, options, iconPath)
                {
                    OnButtonClick = r =>
                    {
                        response = r;
                    }
                };
                msg.ShowDialog();
            }
            catch (Exception e)
            {
            }

            return response;
        }
    }
}