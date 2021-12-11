# GlobalKeyboardHook
An easy to use libary to listen to all global keyboard inputs on windows for C#.
It supports specifc key listening or general listening of all Keyboard keys.
It alsow allows to block the captured key so no other application or even the OS can recieve the pressed key.

‎
## Information
- .NET (Core) 6.0
- C# 10.0
- Compatable with: WinForms & WPF

‎
## How To Use
### Create new KeyboardListener
```csharp
using System; // Required for Arrays
using System.Windows.Forms; // Required for Keys
using GlobalKeyboardHook; // Required for KeyBoardListener

// Creates a new KeyboardListener wich only listens to given keyboard inputs
KeyboardListener keyboardlistener = new KeyboardListener(new[] { Keys.A, Keys.X, Keys.LWin })

// Creates a new KeyboardListener wich listens to all keyboard inputs
KeyboardListener keyboardlistener = new KeyboardListener()
```
### Events of KeyboardListener
```csharp
// Fires when KeyboardListener registers a new key press
keyboardlistener.KeyPress += keyboardlistener_KeyPress
void keyboardlistener_KeyPress(object? sender, GlobalKeyboardHook.Events.KeyEventArgs e)
{
  Console.WriteLine($"Action: Press     Key: {e.Key}    TimeStamp: {e.TimeStamp}");
}

// Fires as long as the KeyboardListener registers a key is down
keyboardlistener.KeyDown += keyboardlistener_KeyDown
void keyboardlistener_KeyDown(object? sender, GlobalKeyboardHook.Events.KeyEventArgs e)
{
  Console.WriteLine($"Action: Down     Key: {e.Key}    TimeStamp: {e.TimeStamp}");
}

// Fires when KeyboardListener registers a key gets released
keyboardlistener.KeyUp += keyboardlistener_KeyUp
void keyboardlistener_KeyUp(object? sender, GlobalKeyboardHook.Events.KeyEventArgs e)
{
  Console.WriteLine($"Action: Up     Key: {e.Key}    TimeStamp: {e.TimeStamp}");
}

// Fires when KeyboardListener HookedKeys-List changes
keyboardlistener.HookedKeysChanged += keyboardlistener_HookedKeysChanged
void keyboardlistener_HookedKeysChanged(object? sender, GlobalKeyboardHook.Events.HookedKeysChangedArgs e)
{
  foreach (Keys key in e.AddedKeys)
  {
    Console.WriteLine($"Action: New key added to HookedKeys-List     Key: {e.Key}    TimeStamp: {e.TimeStamp}");
  }
  foreach (Keys key in e.RemovedKeys)
  {
    Console.WriteLine($"Action: Key removed from HookedKeys-List     Key: {e.Key}    TimeStamp: {e.TimeStamp}");
  }
  
  Console.WriteLine($"Action: HookedKeys-List changed    All Keys: {string.Join("; ", e.AllKeys)}");
}
```
### Options of KeyboardListener
```csharp
// Enable/Disable if the KeyboardListener should listen to all keyboard inputs
keyboardlistener.HookAllKeys = true

// Enable/Disable if the KeyboardListener should block the keyboard input for other applications and the os
keyboardlistener.BlockKeys = true
```
### Key functions of KeyboardListener
```csharp
// Gets a full list of all keys the KeyboardListener listens
Keys[] allKeys = keyboardlistener.GetHookedKeys()

// Adds a key to the list of all keys the KeyboardListener listens
keyboardlistener.AddHookedKey(Keys.F12);

// Removes a key from the list of all keys the KeyboardListener listens
keyboardlistener.RemoveHookedKey(Keys.F12);
// Removes a key from the list of all keys the KeyboardListener listens
keyboardlistener.RemoveHookedKeyAt(1);
```

‎
## Isues
If you find any issues with this libary, please create a new [Issue](https://github.com/IcySnex/GlobalKeyboardHook/issues/new) on this GitHub or join my [Discord-Server](https://discord.gg/JFUGnaCQRm).
Feel free to create a new [Pull Request](https://github.com/IcySnex/GlobalKeyboardHook/compare) if you would to improve something.

‎
## License
This project is licensed under the [Apache License 2.0](https://github.com/IcySnex/GlobalKeyboardHook/blob/main/.LICENSE).  You are allowed to use code for your own projects. You are allowed to modify this bot however you want. You are allowed to publish your modified project, just dont forget to give enough credits.
