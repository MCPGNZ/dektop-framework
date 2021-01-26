using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace UnityRawInput
{
    [Flags]
    public enum NativeMouseEvent : uint
    {
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_RBUTTONDOWN = 0x204,
        WM_RBUTTONUP = 0x205,
        WM_MOUSEWHEEL = 0x20A,
        WM_MOUSEHWHEEL = 0x20E,
    }

    public struct MousePosition
    {
        public int x;
        public int y;
    }

    public class RawMouseInput
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEHOOKSTRUCTEX
        {
            public POINT pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
            public uint mouseData;

            public static MOUSEHOOKSTRUCTEX CreateFromPtr(IntPtr ptr)
            {
                return (MOUSEHOOKSTRUCTEX)Marshal.PtrToStructure(ptr, typeof(MOUSEHOOKSTRUCTEX));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public UIntPtr dwExtraInfo;

            public static MSLLHOOKSTRUCT CreateFromPtr(IntPtr ptr)
            {
                return (MSLLHOOKSTRUCT)Marshal.PtrToStructure(ptr, typeof(MSLLHOOKSTRUCT));
            }
        }

        public static event Action<MousePosition> OnMouseMove;
        public static event Action<MousePosition> OnMouseLeftDown;
        public static event Action<MousePosition> OnMouseLeftUp;
        public static event Action<MousePosition> OnMouseRightDown;
        public static event Action<MousePosition> OnMouseRightUp;
        public static event Action<MousePosition, int> OnMouseWheel;

        /// <summary>
        /// Whether the service is running and input messages are being processed.
        /// </summary>
        public static bool IsRunning => hookPtr != IntPtr.Zero;
        /// <summary>
        /// Whether input messages should be handled when the application is not in focus.
        /// </summary>
        public static bool WorkInBackground { get; private set; }
        /// <summary>
        /// Whether handled input messages should not be propagated further.
        /// </summary>
        public static bool InterceptMessages { get; set; }

        private static IntPtr hookPtr = IntPtr.Zero;

        /// <summary>
        /// Initializes the service and starts processing input messages.
        /// </summary>
        /// <param name="workInBackground">Whether input messages should be handled when the application is not in focus.</param>
        /// <returns>Whether the service started successfully.</returns>
        public static bool Start(bool workInBackground)
        {
            if (IsRunning) return false;
            WorkInBackground = workInBackground;
            return SetHook();
        }

        /// <summary>
        /// Terminates the service and stops processing input messages.
        /// </summary>
        public static void Stop()
        {
            RemoveHook();
        }

        private static bool SetHook()
        {
            if (hookPtr == IntPtr.Zero)
            {
                if (WorkInBackground) hookPtr = Win32API.SetWindowsHookEx(HookType.WH_MOUSE_LL, HandleLowLevelHookProc, IntPtr.Zero, 0);
                else hookPtr = Win32API.SetWindowsHookEx(HookType.WH_MOUSE, HandleHookProc, IntPtr.Zero, (int)Win32API.GetCurrentThreadId());
            }

            if (hookPtr == IntPtr.Zero) return false;

            return true;
        }

        private static void RemoveHook()
        {
            if (hookPtr != IntPtr.Zero)
            {
                Win32API.UnhookWindowsHookEx(hookPtr);
                hookPtr = IntPtr.Zero;
            }
        }

        private static bool HandleMouseEvent(NativeMouseEvent eventCode, MousePosition pos, int wheelDelta)
        {
            switch (eventCode)
            {
                case NativeMouseEvent.WM_MOUSEMOVE:
                    OnMouseMove?.Invoke(pos);
                    return true;

                case NativeMouseEvent.WM_LBUTTONDOWN:
                    OnMouseLeftDown?.Invoke(pos);
                    return true;

                case NativeMouseEvent.WM_LBUTTONUP:
                    OnMouseLeftUp?.Invoke(pos);
                    return true;

                case NativeMouseEvent.WM_RBUTTONDOWN:
                    OnMouseRightDown?.Invoke(pos);
                    return true;

                case NativeMouseEvent.WM_RBUTTONUP:
                    OnMouseRightUp?.Invoke(pos);
                    return true;

                case NativeMouseEvent.WM_MOUSEWHEEL:
                    OnMouseWheel?.Invoke(pos, wheelDelta);
                    return true;

                default:
                    return false;
            }
        }

        [AOT.MonoPInvokeCallback(typeof(Win32API.HookProc))]
        private static int HandleHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
                return Win32API.CallNextHookEx(hookPtr, code, wParam, lParam);

            NativeMouseEvent eventCode = unchecked((NativeMouseEvent)wParam);
            MOUSEHOOKSTRUCTEX data = MOUSEHOOKSTRUCTEX.CreateFromPtr(lParam);
            MousePosition pos = new MousePosition { x = data.pt.x, y = data.pt.y };
            int wheelDelta = unchecked((short)((long)data.mouseData >> 16));

            if (HandleMouseEvent(eventCode, pos, wheelDelta) && InterceptMessages)
                return 1;
            else
                return Win32API.CallNextHookEx(hookPtr, 0, wParam, lParam);
        }

        [AOT.MonoPInvokeCallback(typeof(Win32API.HookProc))]
        private static int HandleLowLevelHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
                return Win32API.CallNextHookEx(hookPtr, code, wParam, lParam);

            NativeMouseEvent eventCode = unchecked((NativeMouseEvent)wParam);
            MSLLHOOKSTRUCT data = MSLLHOOKSTRUCT.CreateFromPtr(lParam);
            MousePosition pos = new MousePosition { x = data.pt.x, y = data.pt.y };
            int wheelDelta = unchecked((short)((long)data.mouseData >> 16));

            if (HandleMouseEvent(eventCode, pos, wheelDelta) && InterceptMessages)
                return 1;
            else
                return Win32API.CallNextHookEx(hookPtr, 0, wParam, lParam);
        }
    }
}