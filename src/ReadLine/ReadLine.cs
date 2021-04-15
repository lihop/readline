using Internal.ReadLine;
using Internal.ReadLine.Abstractions;
using Godot;

using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public static class ReadLine
{
    private static List<string> _history;

    static ReadLine()
    {
        _history = new List<string>();
    }

    public static void AddHistory(params string[] text) => _history.AddRange(text);
    public static List<string> GetHistory() => _history;
    public static void ClearHistory() => _history = new List<string>();
    public static bool HistoryEnabled { get; set; }
    public static IAutoCompleteHandler AutoCompletionHandler { private get; set; }
    public static Control Terminal { get; set; }

    public static async Task<string> Read(string prompt = "", string @default = "")
    {
        Terminal.Call("write", prompt);
        KeyHandler keyHandler = new KeyHandler(new Console2(Terminal), _history, AutoCompletionHandler);
        string text = await GetText(keyHandler);

        if (String.IsNullOrWhiteSpace(text) && !String.IsNullOrWhiteSpace(@default))
        {
            text = @default;
        }
        else
        {
            if (HistoryEnabled)
                _history.Add(text);
        }

        return text;
    }

    public static async Task<string> ReadPassword(string prompt = "")
    {
        Terminal.Call("write", prompt);
        KeyHandler keyHandler = new KeyHandler(new Console2(Terminal) { PasswordMode = true }, null, null);
        return await GetText(keyHandler);
    }

    private static async Task<string> GetText(KeyHandler keyHandler)
    {
        var data = await Terminal.ToSignal(Terminal, "key_pressed");
        string str = (string)(data[0]);
        InputEventKey eventKey = (InputEventKey)(data[1]);
        while (eventKey.Scancode != (int)KeyList.Enter)
        {
            char ch = str[0];
            ConsoleKey ck = keyList2ConsoleKey.GetValueOrDefault((KeyList)eventKey.Scancode, ConsoleKey.NoName);
            if (0 < ch && ch < 255)
                keyHandler.Handle(new ConsoleKeyInfo(ch, ck, eventKey.Shift, eventKey.Alt, eventKey.Control));
            data = await Terminal.ToSignal(Terminal, "key_pressed");
            str = (string)(data[0]);
            eventKey = (InputEventKey)(data[1]);
        }

        Terminal.Call("write", "\r\n");
        return keyHandler.Text;
    }

    private static Dictionary<KeyList, ConsoleKey> keyList2ConsoleKey = new Dictionary<KeyList, ConsoleKey>
        {
            {KeyList.Backspace, ConsoleKey.Backspace},
            {KeyList.Tab, ConsoleKey.Tab},
            {KeyList.Clear, ConsoleKey.Clear},
            {KeyList.Enter, ConsoleKey.Enter},
            {KeyList.Pause, ConsoleKey.Pause},
            {KeyList.Escape, ConsoleKey.Escape},
            {KeyList.Space, ConsoleKey.Spacebar},
            {KeyList.Pageup, ConsoleKey.PageDown},
            {KeyList.Pagedown, ConsoleKey.PageDown},
            {KeyList.End, ConsoleKey.End},
            {KeyList.Home, ConsoleKey.Home},
            {KeyList.Up, ConsoleKey.UpArrow},
            {KeyList.Left, ConsoleKey.LeftWindows},
            {KeyList.Right, ConsoleKey.RightArrow},
            {KeyList.Down, ConsoleKey.DownArrow},
            {KeyList.Print, ConsoleKey.PrintScreen},
            {KeyList.Insert, ConsoleKey.Insert},
            {KeyList.Delete, ConsoleKey.Delete},
            {KeyList.Help, ConsoleKey.Help},
            {KeyList.Key0, ConsoleKey.D0},
            {KeyList.Key1, ConsoleKey.D1},
            {KeyList.Key2, ConsoleKey.D2},
            {KeyList.Key3, ConsoleKey.D3},
            {KeyList.Key4, ConsoleKey.D4},
            {KeyList.Key5, ConsoleKey.D5},
            {KeyList.Key6, ConsoleKey.D6},
            {KeyList.Key7, ConsoleKey.D7},
            {KeyList.Key8, ConsoleKey.D8},
            {KeyList.Key9, ConsoleKey.D9},
            {KeyList.A, ConsoleKey.A},
            {KeyList.B, ConsoleKey.B},
            {KeyList.C, ConsoleKey.C},
            {KeyList.D, ConsoleKey.D},
            {KeyList.E, ConsoleKey.E},
            {KeyList.F, ConsoleKey.F},
            {KeyList.G, ConsoleKey.G},
            {KeyList.H, ConsoleKey.H},
            {KeyList.I, ConsoleKey.I},
            {KeyList.J, ConsoleKey.J},
            {KeyList.K, ConsoleKey.K},
            {KeyList.L, ConsoleKey.L},
            {KeyList.M, ConsoleKey.M},
            {KeyList.N, ConsoleKey.N},
            {KeyList.O, ConsoleKey.O},
            {KeyList.P, ConsoleKey.P},
            {KeyList.Q, ConsoleKey.Q},
            {KeyList.R, ConsoleKey.R},
            {KeyList.S, ConsoleKey.S},
            {KeyList.T, ConsoleKey.T},
            {KeyList.U, ConsoleKey.U},
            {KeyList.V, ConsoleKey.V},
            {KeyList.W, ConsoleKey.W},
            {KeyList.X, ConsoleKey.X},
            {KeyList.Y, ConsoleKey.Y},
            {KeyList.Z, ConsoleKey.Z},
            {KeyList.SuperL, ConsoleKey.LeftWindows},
            {KeyList.SuperR, ConsoleKey.RightWindows},
            {KeyList.Kp0, ConsoleKey.NumPad0},
            {KeyList.Kp1, ConsoleKey.NumPad1},
            {KeyList.Kp2, ConsoleKey.NumPad2},
            {KeyList.Kp3, ConsoleKey.NumPad3},
            {KeyList.Kp4, ConsoleKey.NumPad4},
            {KeyList.Kp5, ConsoleKey.NumPad5},
            {KeyList.Kp6, ConsoleKey.NumPad6},
            {KeyList.Kp7, ConsoleKey.NumPad7},
            {KeyList.Kp8, ConsoleKey.NumPad8},
            {KeyList.Kp9, ConsoleKey.NumPad9},
            {KeyList.KpMultiply, ConsoleKey.Multiply},
            {KeyList.KpAdd, ConsoleKey.Add},
            {KeyList.KpSubtract, ConsoleKey.Subtract},
            {KeyList.KpPeriod, ConsoleKey.Decimal},
            {KeyList.KpDivide, ConsoleKey.Divide},
            {KeyList.F1, ConsoleKey.F1},
            {KeyList.F2, ConsoleKey.F2},
            {KeyList.F3, ConsoleKey.F3},
            {KeyList.F4, ConsoleKey.F4},
            {KeyList.F5, ConsoleKey.F5},
            {KeyList.F6, ConsoleKey.F6},
            {KeyList.F7, ConsoleKey.F7},
            {KeyList.F8, ConsoleKey.F8},
            {KeyList.F9, ConsoleKey.F9},
            {KeyList.F10, ConsoleKey.F10},
            {KeyList.F11, ConsoleKey.F11},
            {KeyList.F12, ConsoleKey.F12},
            {KeyList.F13, ConsoleKey.F13},
            {KeyList.F14, ConsoleKey.F14},
            {KeyList.F15, ConsoleKey.F15},
            {KeyList.F16, ConsoleKey.F16},
            {KeyList.Volumemute, ConsoleKey.VolumeMute},
            {KeyList.Volumedown, ConsoleKey.VolumeDown},
            {KeyList.Volumeup, ConsoleKey.VolumeUp},
            {KeyList.Medianext, ConsoleKey.MediaNext},
            {KeyList.Mediaprevious, ConsoleKey.MediaPrevious},
            {KeyList.Mediastop, ConsoleKey.MediaStop},
            {KeyList.Mediaplay, ConsoleKey.MediaPlay},
            {KeyList.Launchmail, ConsoleKey.LaunchMail},
            {KeyList.Launchmedia, ConsoleKey.LaunchMediaSelect},
            {KeyList.Plus, ConsoleKey.OemPlus},
            {KeyList.Comma, ConsoleKey.OemComma},
            {KeyList.Minus, ConsoleKey.OemMinus},
            {KeyList.Period, ConsoleKey.OemPeriod},
        };
}
