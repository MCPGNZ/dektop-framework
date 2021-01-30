namespace Mcpgnz.DesktopFramework
{
    using System;

    public static class Dialog
    {
        public static string Character(Story.Character character, string message, params string[] options)
        {
            switch (character)
            {
                case Story.Character.Explorer: return Message("Explorer", message, Config.ExplorerAvatar, options);
                case Story.Character.Bin: return Message("Bin", message, Config.ClippyAvatar, options);
                case Story.Character.Windows: return Message("Windows", message, Config.ClippyAvatar, options);
                case Story.Character.Clippy: return Message("Clippt", message, Config.ClippyAvatar, options);

                case Story.Character.Error: return Message("Error", message, Config.ErrorAvatar, options);

                default: throw new ArgumentOutOfRangeException(nameof(character), character, null);
            }
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