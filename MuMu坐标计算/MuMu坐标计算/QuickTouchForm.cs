using System;
using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class QuickTouchForm : Form
    {
        private readonly Timer _ktimer;
        private readonly KeyboardBindingHandler _keyboardHandler;
        private readonly Func<string> _getCurrentJson;
        private readonly Func<double> _getResolutionX;
        private readonly Func<double> _getResolutionY;
        private readonly Func<bool> _ensureReady;
        private readonly Func<KeyEventArgs, string, string, bool> _createKeyCallback;

        public QuickTouchForm(
            KeyboardBindingHandler keyboardHandler,
            Func<string> getCurrentJson,
            Func<double> getResolutionX,
            Func<double> getResolutionY,
            Func<bool> ensureReady,
            Func<KeyEventArgs, string, string, bool> createKeyCallback)
        {
            _keyboardHandler = keyboardHandler;
            _getCurrentJson = getCurrentJson;
            _getResolutionX = getResolutionX;
            _getResolutionY = getResolutionY;
            _ensureReady = ensureReady;
            _createKeyCallback = createKeyCallback;

            _ktimer = new Timer { Interval = 100 };

            InitializeComponent();

            _sXtextBox.KeyPress += CheckTextBox_KeyPress;
            _sYtextBox.KeyPress += CheckTextBox_KeyPress;
            _btnGetScreenResolution.Click += BtnGetScreenResolution_Click;
            _topCheckBox.CheckedChanged += TopCheckBox_CheckedChanged;
            _createKeyOncecheckBox.CheckedChanged += CreateKeyOncecheckBox_CheckedChanged;
            _createKeyscheckBox.CheckedChanged += CreateKeyscheckBox_CheckedChanged;
            _ktimer.Tick += Ktimer_Tick;

            Load += (s, e) =>
            {
                Size screenSize = Screen.FromControl(this).Bounds.Size;
                _sXtextBox.Text = screenSize.Width.ToString();
                _sYtextBox.Text = screenSize.Height.ToString();
            };

            FormClosed += (s, e) =>
            {
                _ktimer.Enabled = false;
                _keyboardHandler.KeyCapturedOnce -= OnKeyCapturedOnce;
                _keyboardHandler.KeyCapturedContinuously -= OnKeyCapturedContinuously;
            };
            Disposed += (s, e) =>
            {
                _ktimer?.Dispose();
            };

            SetupKeyboardEvents();
        }

        private void SetupKeyboardEvents()
        {
            _keyboardHandler.KeyCapturedOnce += OnKeyCapturedOnce;
            _keyboardHandler.KeyCapturedContinuously += OnKeyCapturedContinuously;
        }

        private void OnKeyCapturedOnce(object sender, KeyEventArgs key)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (IsDisposed) return;
                    ExecuteKeyCreation(key);
                }));
                return;
            }
            ExecuteKeyCreation(key);
        }

        private void OnKeyCapturedContinuously(object sender, KeyEventArgs key)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (IsDisposed) return;
                    ExecuteContinuousKeyCreation(key);
                }));
                return;
            }
            ExecuteContinuousKeyCreation(key);
        }

        private void ExecuteKeyCreation(KeyEventArgs key)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            if (!_ensureReady()) return;

            if (!double.TryParse(_nXtextBox.Text, out double absX) ||
                !double.TryParse(_nYtextBox.Text, out double absY))
                return;

            _createKeyOncecheckBox.Checked = false;
            if (_createKeyCallback(key, absX.ToString(System.Globalization.CultureInfo.InvariantCulture), absY.ToString(System.Globalization.CultureInfo.InvariantCulture)))
            {
                _tip1label.Text = "已生成：" + key.KeyCode + "；提示：已关闭键盘监听";
                _tip2label.Text = "";
            }
            else
            {
                _tip1label.Text = "提示：已关闭键盘监听（写入失败）";
            }
        }

        private void ExecuteContinuousKeyCreation(KeyEventArgs key)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            if (!_ensureReady()) return;
            string currentJson = _getCurrentJson();
            if (MuMuJsonEditor.FindKey(currentJson, key) == -1)
            {
                if (!double.TryParse(_nXtextBox.Text, out double absX) ||
                    !double.TryParse(_nYtextBox.Text, out double absY))
                    return;

                if (_createKeyCallback(key, absX.ToString(System.Globalization.CultureInfo.InvariantCulture), absY.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    _tip2label.Text = "已生成：" + key.KeyCode;
                }
            }
            else
            {
                _tip2label.Text = "已存在：" + key.KeyCode;
            }
        }

        private void CheckTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
                e.Handled = true;
        }

        private void BtnGetScreenResolution_Click(object sender, EventArgs e)
        {
            Size screenSize = Screen.FromControl(this).Bounds.Size;
            _sXtextBox.Text = screenSize.Width.ToString();
            _sYtextBox.Text = screenSize.Height.ToString();
        }

        private void TopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = _topCheckBox.Checked;
        }

        private void CreateKeyOncecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool isChecked = _createKeyOncecheckBox.Checked;
                if (isChecked)
                {
                    _createKeyscheckBox.Checked = false;
                    _keyboardHandler.SetListenOnceMode(true);
                    _tip1label.Text = "提示：已开启键盘监听（单次）";
                }
                if (isChecked && _ktimer.Enabled == false)
                    _ktimer.Enabled = true;
                else if (!isChecked && _ktimer.Enabled == true && !_createKeyscheckBox.Checked)
                    _ktimer.Enabled = false;
                if (!isChecked && !_createKeyscheckBox.Checked)
                {
                    _keyboardHandler.StopAllListening();
                    _tip1label.Text = "提示：已关闭键盘监听";
                }
            }
            catch (Exception ex) { MessageBox.Show("发生错误：" + ex.Message); }
        }

        private void CreateKeyscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool isChecked = _createKeyscheckBox.Checked;
                if (isChecked)
                {
                    _createKeyOncecheckBox.Checked = false;
                    _keyboardHandler.SetListenContinuouslyMode(true);
                    _tip1label.Text = "提示：已开启键盘监听（连续）";
                }
                if (isChecked && _ktimer.Enabled == false)
                    _ktimer.Enabled = true;
                else if (!isChecked && _ktimer.Enabled == true && !_createKeyOncecheckBox.Checked)
                    _ktimer.Enabled = false;
                if (!isChecked && !_createKeyOncecheckBox.Checked)
                {
                    _keyboardHandler.StopAllListening();
                    _tip1label.Text = "提示：已关闭键盘监听";
                }
            }
            catch (Exception ex) { MessageBox.Show("发生错误：" + ex.Message); }
        }

        private void Ktimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Point mousePosition = Control.MousePosition;
                _mXtextBox.Text = mousePosition.X.ToString();
                _mYtextBox.Text = mousePosition.Y.ToString();

                if (!double.TryParse(_sXtextBox.Text, out double SX) ||
                    !double.TryParse(_sYtextBox.Text, out double SY) ||
                    !double.TryParse(_mXtextBox.Text, out double mX) ||
                    !double.TryParse(_mYtextBox.Text, out double mY))
                    return;

                double FX = _getResolutionX();
                double FY = _getResolutionY();
                if (FX <= 0 || FY <= 0) return;

                double[] result = MuMuJsonEditor.CalculateCoordinatesMouseToSimulator((int)SX, (int)SY, (int)FX, (int)FY, mX, mY);
                _nXtextBox.Text = result[0].ToString();
                _nYtextBox.Text = result[1].ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("坐标计算失败：" + ex.Message);
                _createKeyOncecheckBox.Checked = false;
                _createKeyscheckBox.Checked = false;
                _ktimer.Enabled = false;
            }
        }
    }
}
