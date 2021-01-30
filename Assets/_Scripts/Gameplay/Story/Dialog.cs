namespace Mcpgnz.DesktopFramework
{
    using System;
    using UnityEngine;

    public static class Dialog
    {
        public static string Character(Identifier character, string message, IconEx avatar, params string[] options)
        {
            var name = character.ToString();
            if (avatar == null)
            {
                avatar = Config.FindAvatar(character);
            }

            return Message(name, message, avatar, options);
        }

        public static string Message(string title, string message, IconEx icon, params string[] options)
        {
            string response = null;

            try
            {
                Debug.Log(message);
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