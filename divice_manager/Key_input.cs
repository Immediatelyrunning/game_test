using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace game_test.divice_manager
{
    /// <summary>
    /// 检测键盘按键状态的辅助类
    /// </summary>
    public static class Key_input
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        public static bool Is_key_donw(Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }
    }
}
