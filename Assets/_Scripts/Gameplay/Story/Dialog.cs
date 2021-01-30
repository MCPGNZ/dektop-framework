namespace Mcpgnz.DesktopFramework
{
    using System;

    public static class Dialog
    {
        public static string Character(Identifier character, string message, params string[] options)
        {
            var name = character.ToString();
            var avatar = Config.FindAvatar(character);

            return Message(name, message, avatar, options);
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