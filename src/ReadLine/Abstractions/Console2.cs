using Godot;
using System;
using System.Threading.Tasks;

namespace Internal.ReadLine.Abstractions
{
	internal class Console2 : IConsole
	{
		public Control Terminal { get; set; }

		public int CursorLeft => (int)Terminal.Call("get_cursor_x") + 1;

		public int CursorTop => (int)Terminal.Call("get_cursor_y") + 1;

		public int BufferWidth => (int)Terminal.Get("cols");

		public int BufferHeight => (int)Terminal.Get("rows");

		public bool PasswordMode { get; set; }

		public Console2(Control terminal) => Terminal = terminal;

		public void SetBufferSize(int width, int height)
		{
			Console.SetBufferSize(width, height);
			Terminal.Set("cols", width);
			Terminal.Set("rows", height);
		}

		public void SetCursorPosition(int left, int top)
		{
			if (!PasswordMode)
			{
				Console.SetCursorPosition(left, top);
				Terminal.Call("write", $"\u001b[{top};{left}H");
			}
		}

		public void Write(string value)
		{
			if (PasswordMode)
				value = new String(default(char), value.Length);

			Console.Write(value);
			Terminal.Call("write", value);
		}

		public void WriteLine(string value)
		{
			Console.WriteLine(value);
			Terminal.Call("write", value + "\r\n");
		}
	}
}
