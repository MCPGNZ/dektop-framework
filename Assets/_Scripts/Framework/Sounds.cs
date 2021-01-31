namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    class Sounds
    {
        public class Sound
        {
            public Sound(string path)
            {
                this.Path = path;
                try
                {
                    this.Player = new System.Media.SoundPlayer(this.Path);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"unable to play {this.Path}: {e.ToString()}");
                }
            }

            public void Play()
            {
                if (this.Player != null)
                {
                    try
                    {
                        this.Player.Play();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"unable to play {this.Path}: {e.ToString()}");
                    }
                }
            }

            public readonly string Path = null;
            private System.Media.SoundPlayer Player = null;
        }

        public static readonly Sound Alarm01 = new Sound("C:/Windows/Media/Alarm01.wav");
        public static readonly Sound Alarm02 = new Sound("C:/Windows/Media/Alarm02.wav");
        public static readonly Sound Alarm03 = new Sound("C:/Windows/Media/Alarm03.wav");
        public static readonly Sound Alarm04 = new Sound("C:/Windows/Media/Alarm04.wav");
        public static readonly Sound Alarm05 = new Sound("C:/Windows/Media/Alarm05.wav");
        public static readonly Sound Alarm06 = new Sound("C:/Windows/Media/Alarm06.wav");
        public static readonly Sound Alarm07 = new Sound("C:/Windows/Media/Alarm07.wav");
        public static readonly Sound Alarm08 = new Sound("C:/Windows/Media/Alarm08.wav");
        public static readonly Sound Alarm09 = new Sound("C:/Windows/Media/Alarm09.wav");
        public static readonly Sound Alarm10 = new Sound("C:/Windows/Media/Alarm10.wav");
        public static readonly Sound Chimes = new Sound("C:/Windows/Media/chimes.wav");
        public static readonly Sound Chord = new Sound("C:/Windows/Media/chord.wav");
        public static readonly Sound Ding = new Sound("C:/Windows/Media/ding.wav");
        public static readonly Sound Notify = new Sound("C:/Windows/Media/notify.wav");
        public static readonly Sound Recycle = new Sound("C:/Windows/Media/recycle.wav");
        public static readonly Sound Ring01 = new Sound("C:/Windows/Media/Ring01.wav");
        public static readonly Sound Ring02 = new Sound("C:/Windows/Media/Ring02.wav");
        public static readonly Sound Ring03 = new Sound("C:/Windows/Media/Ring03.wav");
        public static readonly Sound Ring04 = new Sound("C:/Windows/Media/Ring04.wav");
        public static readonly Sound Ring05 = new Sound("C:/Windows/Media/Ring05.wav");
        public static readonly Sound Ring06 = new Sound("C:/Windows/Media/Ring06.wav");
        public static readonly Sound Ring07 = new Sound("C:/Windows/Media/Ring07.wav");
        public static readonly Sound Ring08 = new Sound("C:/Windows/Media/Ring08.wav");
        public static readonly Sound Ring09 = new Sound("C:/Windows/Media/Ring09.wav");
        public static readonly Sound Ring10 = new Sound("C:/Windows/Media/Ring10.wav");
        public static readonly Sound Ringout = new Sound("C:/Windows/Media/ringout.wav");
        public static readonly Sound SpeechDisambiguation = new Sound("C:/Windows/Media/Speech Disambiguation.wav");
        public static readonly Sound SpeechMisrecognition = new Sound("C:/Windows/Media/Speech Misrecognition.wav");
        public static readonly Sound SpeechOff = new Sound("C:/Windows/Media/Speech Off.wav");
        public static readonly Sound SpeechOn = new Sound("C:/Windows/Media/Speech On.wav");
        public static readonly Sound SpeechSleep = new Sound("C:/Windows/Media/Speech Sleep.wav");
        public static readonly Sound Tada = new Sound("C:/Windows/Media/tada.wav");
        public static readonly Sound WindowsBackground = new Sound("C:/Windows/Media/Windows Background.wav");
        public static readonly Sound WindowsBalloon = new Sound("C:/Windows/Media/Windows Balloon.wav");
        public static readonly Sound WindowsBatteryCritical = new Sound("C:/Windows/Media/Windows Battery Critical.wav");
        public static readonly Sound WindowsBatteryLow = new Sound("C:/Windows/Media/Windows Battery Low.wav");
        public static readonly Sound WindowsCriticalStop = new Sound("C:/Windows/Media/Windows Critical Stop.wav");
        public static readonly Sound WindowsDefault = new Sound("C:/Windows/Media/Windows Default.wav");
        public static readonly Sound WindowsDing = new Sound("C:/Windows/Media/Windows Ding.wav");
        public static readonly Sound WindowsError = new Sound("C:/Windows/Media/Windows Error.wav");
        public static readonly Sound WindowsExclamation = new Sound("C:/Windows/Media/Windows Exclamation.wav");
        public static readonly Sound WindowsFeedDiscovered = new Sound("C:/Windows/Media/Windows Feed Discovered.wav");
        public static readonly Sound WindowsForeground = new Sound("C:/Windows/Media/Windows Foreground.wav");
        public static readonly Sound WindowsHardwareFail = new Sound("C:/Windows/Media/Windows Hardware Fail.wav");
        public static readonly Sound WindowsHardwareInsert = new Sound("C:/Windows/Media/Windows Hardware Insert.wav");
        public static readonly Sound WindowsHardwareRemove = new Sound("C:/Windows/Media/Windows Hardware Remove.wav");
        public static readonly Sound WindowsInformationBar = new Sound("C:/Windows/Media/Windows Information Bar.wav");
        public static readonly Sound WindowsLogoffSound = new Sound("C:/Windows/Media/Windows Logoff Sound.wav");
        public static readonly Sound WindowsLogon = new Sound("C:/Windows/Media/Windows Logon.wav");
        public static readonly Sound WindowsMenuCommand = new Sound("C:/Windows/Media/Windows Menu Command.wav");
        public static readonly Sound WindowsMessageNudge = new Sound("C:/Windows/Media/Windows Message Nudge.wav");
        public static readonly Sound WindowsMinimize = new Sound("C:/Windows/Media/Windows Minimize.wav");
        public static readonly Sound WindowsNavigationStart = new Sound("C:/Windows/Media/Windows Navigation Start.wav");
        public static readonly Sound WindowsNotify = new Sound("C:/Windows/Media/Windows Notify.wav");
        public static readonly Sound WindowsNotifyCalendar = new Sound("C:/Windows/Media/Windows Notify Calendar.wav");
        public static readonly Sound WindowsNotifyEmail = new Sound("C:/Windows/Media/Windows Notify Email.wav");
        public static readonly Sound WindowsNotifyMessaging = new Sound("C:/Windows/Media/Windows Notify Messaging.wav");
        public static readonly Sound WindowsNotifySystemGeneric = new Sound("C:/Windows/Media/Windows Notify System Generic.wav");
        public static readonly Sound WindowsPopupBlocked = new Sound("C:/Windows/Media/Windows Pop-up Blocked.wav");
        public static readonly Sound WindowsPrintcomplete = new Sound("C:/Windows/Media/Windows Print complete.wav");
        public static readonly Sound WindowsProximityConnection = new Sound("C:/Windows/Media/Windows Proximity Connection.wav");
        public static readonly Sound WindowsProximityNotification = new Sound("C:/Windows/Media/Windows Proximity Notification.wav");
        public static readonly Sound WindowsRecycle = new Sound("C:/Windows/Media/Windows Recycle.wav");
        public static readonly Sound WindowsRestore = new Sound("C:/Windows/Media/Windows Restore.wav");
        public static readonly Sound WindowsRingin = new Sound("C:/Windows/Media/Windows Ringin.wav");
        public static readonly Sound WindowsRingout = new Sound("C:/Windows/Media/Windows Ringout.wav");
        public static readonly Sound WindowsShutdown = new Sound("C:/Windows/Media/Windows Shutdown.wav");
        public static readonly Sound WindowsStartup = new Sound("C:/Windows/Media/Windows Startup.wav");
        public static readonly Sound WindowsUnlock = new Sound("C:/Windows/Media/Windows Unlock.wav");
        public static readonly Sound WindowsUserAccountControl = new Sound("C:/Windows/Media/Windows User Account Control.wav");

        public static void Demo()
        {
            List<Sound> sounds = typeof(Sounds).GetFields().Where((f) => f.IsStatic).Select((f) => (Sound)f.GetValue(null)).ToList();
            UnityRawInput.RawKey[] keys = (UnityRawInput.RawKey[])typeof(UnityRawInput.RawKey).GetEnumValues();

            UnityRawInput.RawKeyInput.OnKeyDown += (UnityRawInput.RawKey key) =>
            {
                foreach (Tuple<Sound, UnityRawInput.RawKey> sound_key in sounds.Zip(keys, Tuple.Create))
                {
                    if (sound_key.Item2 == key)
                    {
                        UnityEngine.Debug.Log($"play {sound_key.Item1.Path}");
                        sound_key.Item1.Play();
                    }
                }
            };
        }

    }
}
