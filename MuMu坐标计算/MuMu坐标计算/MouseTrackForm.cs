using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class MouseTrackForm : Form
    {
        private readonly Timer _ctimer;
        private readonly KeyboardBindingHandler _keyboardHandler;
        private readonly ConfigManager _config;
        public Action<string> StatusCallback { get; set; }

        public MouseTrackForm(KeyboardBindingHandler keyboardHandler, ConfigManager config)
        {
            _keyboardHandler = keyboardHandler;
            _config = config;

            _ctimer = new Timer { Interval = 50 };

            InitializeComponent();

            _topCheckBox.CheckedChanged += TopCheckBox_CheckedChanged;
            _cCheckBox.CheckedChanged += CCheckBox_CheckedChanged;
            _eCheckBox.CheckedChanged += ECheckBox_CheckedChanged;
            _ncXtextBox.KeyPress += CheckTextBox_KeyPress;
            _ncYtextBox.KeyPress += CheckTextBox_KeyPress;
            _scXtextBox.KeyPress += CheckTextBox_KeyPress;
            _scYtextBox.KeyPress += CheckTextBox_KeyPress;
            _findKeytextBox.KeyDown += FindKeytextBox_KeyDown;
            _findKeytextBox.KeyPress += FindKeytextBox_KeyPress;
            _resetKeytextBox.KeyDown += ResetKeytextBox_KeyDown;
            _resetKeytextBox.KeyPress += ResetKeytextBox_KeyPress;
            _saveKeybutton.Click += SaveKeybutton_Click;
            _loadKeybutton.Click += LoadKeybutton_Click;
            _ctimer.Tick += Ctimer_Tick;

            FormClosed += (s, e) =>
            {
                try { HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdSave); } catch { }
                try { HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdRecall); } catch { }
                _ctimer.Enabled = false;
            };
            Disposed += (s, e) =>
            {
                _ctimer.Enabled = false;
                _ctimer.Dispose();
            };

            LoadConfig();
        }

        private void LoadConfig()
        {
            _keyboardHandler.FindKey = _config.FindKey;
            _keyboardHandler.ResetKey = _config.ResetKey;
            _findKeytextBox.Text = _keyboardHandler.FindKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            _resetKeytextBox.Text = _keyboardHandler.ResetKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            try { _keyboardHandler.RefreshHotKeyRegistration(Handle, true); } catch { }
        }

        private void CheckTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
                e.Handled = true;
        }

        private void Ctimer_Tick(object sender, EventArgs e)
        {
            Point mousePosition = Control.MousePosition;
            _ncXtextBox.Text = mousePosition.X.ToString();
            _ncYtextBox.Text = mousePosition.Y.ToString();
            if (int.TryParse(_ncXtextBox.Text, out int ncXVal) && int.TryParse(_scXtextBox.Text, out int scXVal))
                _ncXtextBox.BackColor = ncXVal == scXVal ? Color.Green : Color.White;
            else
                _ncXtextBox.BackColor = Color.White;
            if (int.TryParse(_ncYtextBox.Text, out int ncYVal) && int.TryParse(_scYtextBox.Text, out int scYVal))
                _ncYtextBox.BackColor = ncYVal == scYVal ? Color.Green : Color.White;
            else
                _ncYtextBox.BackColor = Color.White;
        }

        private void CCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _ctimer.Enabled = _cCheckBox.Checked;

            if (_cCheckBox.Checked)
            {
                _keyboardHandler.RefreshHotKeyRegistration(Handle, true);
            }
            else
            {
                HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdSave);
                HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdRecall);
            }
        }

        private void ECheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _findKeytextBox.ReadOnly = !_eCheckBox.Checked;
            _resetKeytextBox.ReadOnly = !_eCheckBox.Checked;
        }

        private void FindKeytextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (_findKeytextBox.ReadOnly) return;
            _findKeytextBox.Text = "";
            bool conflict;
            _keyboardHandler.ProcessFindKeyDown(e, out conflict);
            if (conflict) { MessageBox.Show("快捷键冲突！"); return; }
            e.Handled = true;
            _findKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(e.KeyCode);
            if (_cCheckBox.Checked) { _cCheckBox.Checked = false; _cCheckBox.Checked = true; }
        }

        private void FindKeytextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _findKeytextBox.Text = "";
            e.Handled = true;
            _findKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(_keyboardHandler.FindKey);
        }

        private void ResetKeytextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (_resetKeytextBox.ReadOnly) return;
            _resetKeytextBox.Text = "";
            bool conflict;
            _keyboardHandler.ProcessResetKeyDown(e, out conflict);
            if (conflict) { MessageBox.Show("快捷键冲突！"); return; }
            e.Handled = true;
            _resetKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(e.KeyCode);
            if (_cCheckBox.Checked) { _cCheckBox.Checked = false; _cCheckBox.Checked = true; }
        }

        private void ResetKeytextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _resetKeytextBox.Text = "";
            e.Handled = true;
            _resetKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(_keyboardHandler.ResetKey);
        }

        private void SaveKeybutton_Click(object sender, EventArgs e)
        {
            _config.FindKey = _keyboardHandler.FindKey;
            _config.ResetKey = _keyboardHandler.ResetKey;
        }

        private void LoadKeybutton_Click(object sender, EventArgs e)
        {
            _keyboardHandler.FindKey = _config.FindKey;
            _keyboardHandler.ResetKey = _config.ResetKey;
            _findKeytextBox.Text = _keyboardHandler.FindKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            _resetKeytextBox.Text = _keyboardHandler.ResetKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            if (_cCheckBox.Checked)
            {
                try { _keyboardHandler.RefreshHotKeyRegistration(Handle, true); } catch { }
            }
        }

        private void TopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = _topCheckBox.Checked;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            if (m.Msg == WM_HOTKEY)
            {
                switch (m.WParam.ToInt32())
                {
                    case KeyboardBindingHandler.HotKeyIdSave:
                        if (_cCheckBox.Checked)
                        {
                            _scXtextBox.Text = _ncXtextBox.Text;
                            _scYtextBox.Text = _ncYtextBox.Text;
                            StatusCallback?.Invoke("坐标已保存");
                        }
                        break;
                    case KeyboardBindingHandler.HotKeyIdRecall:
                        if (_cCheckBox.Checked)
                        {
                            if (int.TryParse(_scXtextBox.Text, out int scx) &&
                                int.TryParse(_scYtextBox.Text, out int scy))
                            {
                                try { MouseSimulator.MoveMouseTo(scx, scy); }
                                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("鼠标回溯失败：" + ex.Message); }
                            }
                        }
                        break;
                }
            }
            base.WndProc(ref m);
        }
    }
}
