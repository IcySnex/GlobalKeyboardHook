using System.Runtime.InteropServices;

namespace GlobalKeyboardHook
{
	public class KeyboardListener
	{
		#region DLL
		[DllImport("user32.dll")]
		static extern IntPtr SetWindowsHookEx(int id, HookAction action, IntPtr instance, uint thread);
		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr instance);
		[DllImport("user32.dll")]
		static extern int CallNextHookEx(IntPtr id, int code, int wParam, ref HookStruct lParam);
		[DllImport("kernel32.dll")]
		static extern IntPtr LoadLibrary(string libary);
		#endregion
		#region Declarations
		public delegate int HookAction(int code, int wParam, ref HookStruct lParam);
		public struct HookStruct { public int keyCode; public int scanCode; public int flags; public int time; public int dwExtraInfo; }

		IntPtr HookId = IntPtr.Zero;
        static HookAction HookActionDelegate;
		bool ispr;
		#endregion
		#region General
		/// <summary>
		/// Fires when KeyboardListener registers a new key press
		/// </summary>
		public EventHandler<Events.KeyEventArgs>? KeyPress;
		/// <summary>
		/// Fires as long as the KeyboardListener registers a key is down
		/// </summary>
		public EventHandler<Events.KeyEventArgs>? KeyDown;
		/// <summary>
		/// Fires when KeyboardListener registers a key gets released
		/// </summary>
		public EventHandler<Events.KeyEventArgs>? KeyUp;
		/// <summary>
		/// Fires when KeyboardListener HookedKeys-List changes
		/// </summary>
		public EventHandler<Events.HookedKeysChangedArgs>? HookedKeysChanged;

		/// <summary>
		/// Creates a new KeyboardListener wich listens to all keyboard inputs
		/// </summary>
		public KeyboardListener()
		{
			HookAllKeys = true;
			HookActionDelegate = hookProc;
			Hook();
		}
		/// <summary>
		/// Creates a new KeyboardListener wich only listens to given keyboard inputs
		/// </summary>
		/// <param name="Keys">Key array wich the KeyboardListeners listens</param>
		public KeyboardListener(Keys[] Keys)
		{
			HookedKeys = Keys.ToList();
			HookedKeysChanged?.Invoke(this, new(Keys, null, Keys, DateTime.Now));
			HookActionDelegate = hookProc;
			Hook();
		}
		~KeyboardListener() => UnHook();

		/// <summary>
		/// Hook the current instance to the WindowsAPI
		/// </summary>
		public void Hook() => HookId = SetWindowsHookEx(13, HookActionDelegate, LoadLibrary("User32"), 0);
		/// <summary>
		/// Unhook the current instance from the WindowsAPI
		/// </summary>
		public void UnHook() => UnhookWindowsHookEx(HookId);

		int hookProc(int code, int wParam, ref HookStruct lParam)
		{
			if (code >= 0)
			{
				Keys key = (Keys)lParam.keyCode;
				if (HookedKeys.Contains(key) || HookAllKeys)
				{
					if ((wParam == 0x100 || wParam == 0x104))
					{
						if (!ispr && KeyPress != null) KeyPress.Invoke(this, new(key, DateTime.Now, lParam));
						if (KeyDown != null) KeyDown.Invoke(this, new(key, DateTime.Now, lParam));
						ispr = true;
					}
					else if ((wParam == 0x101 || wParam == 0x105))
					{
						if (KeyUp != null) KeyUp.Invoke(this, new(key, DateTime.Now, lParam));
						ispr = false;
					}
					if (BlockKeys) return 1;
				}
			}
			return CallNextHookEx(HookId, code, wParam, ref lParam);
		}
		#endregion
		#region HookedKeys
		List<Keys> HookedKeys { get; } = new List<Keys>();
		/// <summary>
		/// Enable/Disable if the KeyboardListener should listen to all keyboard inputs
		/// </summary>
		public bool HookAllKeys { get; set; }
		/// <summary>
		/// Enable/Disable if the KeyboardListener should block the keyboard input for other applications and the os
		/// </summary>
		public bool BlockKeys { get; set; } = true;

		/// <summary>
		/// Gets a full list of all keys the KeyboardListener listens
		/// </summary>
		/// <returns></returns>
		public Keys[] GetHookedKeys() => HookedKeys.ToArray();
		/// <summary>
		/// Adds a key to the list of all keys the KeyboardListener listens
		/// </summary>
		/// <param name="Key">Key wich should get added</param>
		public void AddHookedKey(Keys Key) { HookedKeys.Add(Key); if (HookedKeysChanged != null) HookedKeysChanged.Invoke(this, new(Key, null, GetHookedKeys(), DateTime.Now)); }
		/// <summary>
		/// Removes a key from the list of all keys the KeyboardListener listens
		/// </summary>
		/// <param name="Key">Key wich should be deleted</param>
		public void RemoveHookedKey(Keys Key) { HookedKeys.Remove(Key); if (HookedKeysChanged != null) HookedKeysChanged.Invoke(this, new(null, Key, GetHookedKeys(), DateTime.Now)); }
		/// <summary>
		/// Removes a key from the list of all keys the KeyboardListener listens
		/// </summary>
		/// <param name="Index">Index at wich position the key should be deleted</param>
		public void RemoveHookedKeyAt(int Index) { RemoveHookedKey(HookedKeys.ElementAt(Index)); }
		#endregion
	}
}