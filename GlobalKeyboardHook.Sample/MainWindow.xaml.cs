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

        private void hook_btn_Click(object sender, RoutedEventArgs e)
        {
            logs.Items.Clear();
            kl.Hook();
            kl.KeyDown += Kl_KeyDown;
            kl.KeyUp += Kl_KeyUp;
            kl.HookedKeysChanged += Kl_HookedKeysChanged;
            Kl_HookedKeysChanged(this, new(kl.GetHookedKeys(), null, kl.GetHookedKeys(), DateTime.Now));

            BlockKey.Checked += BlockKey_CheckChanged;
            BlockKey.Unchecked += BlockKey_CheckChanged;

            logs.Items.Add($"KeyboardListener Hooked! (Keys: {string.Join(" | ", kl.GetHookedKeys())})");
        }
        private void unhook_btn_Click(object sender, RoutedEventArgs e)
        {
            logs.Items.Clear();
            kl.UnHook();
            kl.KeyDown -= Kl_KeyDown;
            kl.KeyUp -= Kl_KeyUp;
            kl.HookedKeysChanged -= Kl_HookedKeysChanged;
            HookedKeys.Items.Clear();

            BlockKey.Checked -= BlockKey_CheckChanged;
            BlockKey.Unchecked -= BlockKey_CheckChanged;

            logs.Items.Add("KeyboardListener Unhooked!");
        }

        private void AddHooked(object sender, System.Windows.Input.KeyEventArgs e) { kl.AddHookedKey((Keys)System.Windows.Input.KeyInterop.VirtualKeyFromKey(e.Key)); HookedKeys.Focus(); HookedKeys.SelectedIndex = HookedKeys.Items.Count - 1; e.Handled = true; }
        private void RemoveHooked(object sender, RoutedEventArgs e) { kl.RemoveHookedKeyAt(HookedKeys.SelectedIndex); e.Handled = true; }
        private void SetHookAllKeys_CheckChanged(object sender, RoutedEventArgs e) => kl.HookAllKeys = SetHookAllKeys.IsChecked.HasValue ? SetHookAllKeys.IsChecked.Value : false;
        private void BlockKey_CheckChanged(object sender, RoutedEventArgs e) => kl.BlockKeys = BlockKey.IsChecked.HasValue ? BlockKey.IsChecked.Value : false;

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
