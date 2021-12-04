using static GlobalKeyboardHook.KeyboardListener;

namespace GlobalKeyboardHook.Events
{
    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; }
        public DateTime TimeStamp { get; }
        public HookStruct Struct { get; }

        public KeyEventArgs(Keys Key, DateTime TimeStamp, HookStruct Struct)
        {
            this.Key = Key;
            this.TimeStamp = TimeStamp;
            this.Struct = Struct;
        }
    }
}
