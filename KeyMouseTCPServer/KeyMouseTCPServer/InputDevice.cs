using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeyMouseTCPServer
{
    class InputDevice
    {
        int screenWidth;
        int screenHeight;
        String startStr;
        String separatorStr;
        String endStr;
        public InputDevice()
        {
            //Console.WriteLine("startid");
            screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            // 0xE000はunicodeの私用領域。文字が割る振られていないので制御文字として使った。

            // C#で、文字コードを表す文字列から、文字に変換する - Qiita
            // http://qiita.com/rohinomiya/items/ca30f311f881cbb33ca8
            startStr = Convert.ToChar(0xE000).ToString();// "{" = 7B
            separatorStr = Convert.ToChar(0xE001).ToString();// 
            endStr = Convert.ToChar(0xE002).ToString();// "}" = 7D
            //Console.WriteLine(Convert.ToChar(0x7B).ToString());
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };
        // キーボードイベント(keybd_eventの引数と同様のデータ)
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };
        // ハードウェアイベント
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };
        // 各種イベント(SendInputの引数データ)
        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        };
        // キー操作、マウス操作をシミュレート(擬似的に操作する)
        [DllImport("user32.dll")]
        private extern static void SendInput(
            int nInputs, ref INPUT pInputs, int cbsize);
        private const int INPUT_MOUSE = 0;                  // マウスイベント
        private const int INPUT_KEYBOARD = 1;               // キーボードイベント
        private const int INPUT_HARDWARE = 2;               // ハードウェアイベント

        private const int MOUSEEVENTF_MOVE = 0x1;           // マウスを移動する
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;    // 絶対座標指定
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;       // 左　ボタンを押す
        private const int MOUSEEVENTF_LEFTUP = 0x4;         // 左　ボタンを離す
        private const int MOUSEEVENTF_RIGHTDOWN = 0x8;      // 右　ボタンを押す
        private const int MOUSEEVENTF_RIGHTUP = 0x10;       // 右　ボタンを離す
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;    // 中央ボタンを押す
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;      // 中央ボタンを離す
        private const int MOUSEEVENTF_WHEEL = 0x800;        // ホイールを回転する
        private const int WHEEL_DELTA = 120;                // ホイール回転値


        private string _spoolStr = "";
        public void setKey(string data)
        {
            _spoolStr += data;
        }
        public void execute()
        {
            //Console.WriteLine("spool,{0}", _spoolStr.Length);
            if (_spoolStr.Length == 0)
            {
                return;
            }


            int startIndex = _spoolStr.IndexOf(startStr);//"{"
            int endIndex = _spoolStr.IndexOf(endStr);//"}"
            if (startIndex == -1 || endIndex == -1)
            {
                return;
            }

            string result = _spoolStr.Substring(startIndex + 1, endIndex - 1);
            if (_spoolStr.Length > endIndex + 1)
            {
                _spoolStr = _spoolStr.Substring(endIndex + 1);
            }
            else
            {
                _spoolStr = "";
            }

            string[] keys = result.Split(separatorStr.ToCharArray());
            if (keys[0] == "key")
            {
                //Console.WriteLine(keys[1]);
                SendKeys.SendWait(escapeSendableChar(keys[1]));
            }
            else if (keys[0] == "xy")
            {
                if (keys.Length == 3)
                {
                    // http://dobon.net/vb/dotnet/system/cursorposition.html
                    // クライアント座標を画面座標に変換する
                    System.Drawing.Point mp = new System.Drawing.Point();
                    mp.X = (int)(float.Parse(keys[1]) * screenWidth);
                    mp.Y = (int)(float.Parse(keys[2]) * screenHeight);
                    //マウスポインタの位置を設定する
                    System.Windows.Forms.Cursor.Position = mp;
                    //Console.WriteLine(mp.ToString());
                }
            }
            else if (keys[0] == "ml" || keys[0] == "mr")
            {
                // マウス操作実行用のデータ
                const int num = 2;
                INPUT[] inp = new INPUT[num];

                inp[0].type = INPUT_MOUSE;
                inp[1].type = INPUT_MOUSE;
                if (keys[0] == "ml")
                {
                    inp[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
                    inp[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;
                }
                else
                {
                    inp[0].mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
                    inp[1].mi.dwFlags = MOUSEEVENTF_RIGHTUP;
                }
                inp[0].mi.dx = 0;
                inp[0].mi.dy = 0;
                inp[0].mi.mouseData = 0;
                inp[0].mi.dwExtraInfo = 0;
                inp[0].mi.time = 0;

                inp[1].mi.dx = 0;
                inp[1].mi.dy = 0;
                inp[1].mi.mouseData = 0;
                inp[1].mi.dwExtraInfo = 0;
                inp[1].mi.time = 0;

                // マウス操作実行
                SendInput(1, ref inp[0], Marshal.SizeOf(inp[0]));
                SendInput(1, ref inp[1], Marshal.SizeOf(inp[1]));
            }

        }
        private string escapeSendableChar(string data)
        {
            string result = data;
            // http://msdn.microsoft.com/ja-jp/library/system.windows.forms.sendkeys.send(v=vs.110).aspx

            // todo 連続して送信すると、1文字目とは限らないので結局引っかかる。
            if (data.Substring(0, 1) == "(") { result = "{(}"; };
            if (data.Substring(0, 1) == ")") { result = "{)}"; };
            if (data.Substring(0, 1) == "{") { result = "{{}"; };
            if (data.Substring(0, 1) == "}") { result = "{}}"; };
            if (data.Substring(0, 1) == "+") { result = "{+}"; };
            if (data.Substring(0, 1) == "^") { result = "{^}"; };
            if (data.Substring(0, 1) == "%") { result = "{%}"; };
            if (data.Substring(0, 1) == "~") { result = "{~}"; };
            return result;
        }
    }
}
