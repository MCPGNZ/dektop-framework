namespace Mcpgnz.DesktopFramework
{
    public static class Dialog
    {
        public static string Clipper(string message, params string[] options)
        {
            return Message("Clipper", message, null, options);
        }

        public static string Message(string title, string message, IconEx icon, params string[] options)
        {
            var iconPath = icon == null ? null : icon._Path;

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