using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Forms;

namespace GlobalKeyboardHook.Sample
{
    public partial class MainWindow : Window
    {
        KeyboardListener kl = new(Array.Empty<Keys>());
        public MainWindow() => InitializeComponent();

        bool KeyPresshooked;
        bool KeyDownhooked;
        bool KeyUphooked;

        private void hook_btn_Click(object sender, RoutedEventArgs e)
        {
            logs.Items.Clear();
            kl.Hook();
            KeyPressCB_CheckChanged(this, e);
            KeyDownCB_CheckChanged(this, e);
            KeyUpCB_CheckChanged(this, e);
            kl.HookedKeysChanged += Kl_HookedKeysChanged;
            Kl_HookedKeysChanged(this, new(kl.GetHookedKeys(), null, kl.GetHookedKeys(), DateTime.Now));

            BlockKey.Checked += BlockKey_CheckChanged;
            BlockKey.Unchecked += BlockKey_CheckChanged;

            logs.Items.Add($"KeyboardListener Hooked! (Keys: {string.Join(" | ", kl.GetHookedKeys())})");

            hook_btn.IsEnabled = false;
            unhook_btn.IsEnabled = true;
            tbaddnew.IsEnabled = true;
            HookedKeys_SelectionChanged(this, null);
            BlockKey.IsEnabled = true;
            SetHookAllKeys.IsEnabled = true;
            KeyPressCB.IsEnabled = true;
            KeyDownCB.IsEnabled = true;
            KeyUpCB.IsEnabled = true;
        }
        private void unhook_btn_Click(object sender, RoutedEventArgs e)
        {
            logs.Items.Clear();
            kl.UnHook();
            kl.KeyPress -= Kl_KeyPress;
            kl.KeyDown -= Kl_KeyDown;
            kl.KeyUp -= Kl_KeyUp;
            kl.HookedKeysChanged -= Kl_HookedKeysChanged;
            HookedKeys.Items.Clear();

            BlockKey.Checked -= BlockKey_CheckChanged;
            BlockKey.Unchecked -= BlockKey_CheckChanged;

            logs.Items.Add("KeyboardListener Unhooked!");

            hook_btn.IsEnabled = true;
            unhook_btn.IsEnabled = false;
            tbaddnew.IsEnabled = false;
            HookedKeys_SelectionChanged(this, null);
            BlockKey.IsEnabled = false;
            SetHookAllKeys.IsEnabled = false;
            KeyPressCB.IsEnabled = false;
            KeyDownCB.IsEnabled = false;
            KeyUpCB.IsEnabled = false;
        }

        private void AddHooked(object sender, System.Windows.Input.KeyEventArgs e) { kl.AddHookedKey((Keys)System.Windows.Input.KeyInterop.VirtualKeyFromKey(e.Key)); HookedKeys.Focus(); HookedKeys.SelectedIndex = HookedKeys.Items.Count - 1; e.Handled = true; }
        private void RemoveHooked(object sender, RoutedEventArgs e) { kl.RemoveHookedKeyAt(HookedKeys.SelectedIndex); e.Handled = true; }
        private void SetHookAllKeys_CheckChanged(object sender, RoutedEventArgs e) => kl.HookAllKeys = SetHookAllKeys.IsChecked.HasValue ? SetHookAllKeys.IsChecked.Value : false;
        private void BlockKey_CheckChanged(object sender, RoutedEventArgs e) => kl.BlockKeys = BlockKey.IsChecked.HasValue ? BlockKey.IsChecked.Value : false;

        private void HookedKeys_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { if (HookedKeys.SelectedIndex == -1) btnremove.IsEnabled = false; else btnremove.IsEnabled = true; }
        private void KeyPressCB_CheckChanged(object sender, RoutedEventArgs e) { if (KeyPressCB.IsChecked.HasValue ? KeyPressCB.IsChecked.Value : false && !KeyPresshooked) kl.KeyPress += Kl_KeyPress; else kl.KeyPress -= Kl_KeyPress; }
        private void KeyDownCB_CheckChanged(object sender, RoutedEventArgs e) { if (KeyDownCB.IsChecked.HasValue ? KeyDownCB.IsChecked.Value : false && !KeyDownhooked) kl.KeyDown += Kl_KeyDown; else kl.KeyDown -= Kl_KeyDown; }
        private void KeyUpCB_CheckChanged(object sender, RoutedEventArgs e) { if (KeyUpCB.IsChecked.HasValue ? KeyUpCB.IsChecked.Value : false && !KeyUphooked) kl.KeyUp += Kl_KeyUp; else kl.KeyUp -= Kl_KeyUp; }

        private void Kl_KeyPress(object? sender, Events.KeyEventArgs e) => logs.Items.Insert(0, $"Action:    Press\t\t Key:    {e.Key}\t\t TimeStamp:    {e.TimeStamp}");
        private void Kl_KeyDown(object? sender, Events.KeyEventArgs e) => logs.Items.Insert(0, $"Action:    Down\t\t Key:    {e.Key}\t\t TimeStamp:    {e.TimeStamp}");
        private void Kl_KeyUp(object? sender, Events.KeyEventArgs e) => logs.Items.Insert(0, $"Action:    Up\t\t Key:    {e.Key}\t\t TimeStamp:    {e.TimeStamp}");
        private void Kl_HookedKeysChanged(object? sender, Events.HookedKeysChangedArgs e)
        {
            
            HookedKeys.Items.Clear();
            foreach (Keys key in e.AllKeys)
                HookedKeys.Items.Add(key.ToString());
        }

        private void tbaddnew_GotFocus(object sender, RoutedEventArgs e)
        {
            tbaddnew.Text = "Press a Key...";
            tbaddnew.Background = Brushes.LightGray;
        }

        private void tbaddnew_LostFocus(object sender, RoutedEventArgs e)
        {
            tbaddnew.Text = "Press here to add new key";
            tbaddnew.Background = Brushes.White;
        }

    }
}
