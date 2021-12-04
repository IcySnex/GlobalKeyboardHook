namespace GlobalKeyboardHook.Events
{
    public class HookedKeysChangedArgs : EventArgs
    {
        public Keys[]? AddedKeys { get; }
        public Keys[]? RemovedKeys { get; }
        public Keys[] AllKeys { get; }
        public DateTime TimeStamp { get; }

        public HookedKeysChangedArgs(Keys? AddedKey, Keys? RemovedKey, Keys[] AllKeys, DateTime TimeStamp)
        {
            AddedKeys = AddedKey.HasValue ? new[] { AddedKey.Value } : null;
            RemovedKeys = RemovedKey.HasValue ? new[] { RemovedKey.Value } : null;
            this.AllKeys = AllKeys;
            this.TimeStamp = TimeStamp;
        }
        public HookedKeysChangedArgs(Keys[]? AddedKeys, Keys[]? RemovedKeys, Keys[] AllKeys, DateTime TimeStamp)
        {
            this.AddedKeys = AddedKeys;
            this.RemovedKeys = RemovedKeys;
            this.AllKeys = AllKeys;
            this.TimeStamp = TimeStamp;
        }
    }
}
