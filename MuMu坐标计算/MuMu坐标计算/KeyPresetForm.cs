using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    internal partial class KeyPresetForm : Form
    {
        private readonly KeyboardBindingHandler _keyboardHandler;
        private readonly Func<string> _getCurrentJson;
        private readonly Action<string> _setCurrentJson;
        private readonly Func<bool> _saveJsonAndBackup;
        private readonly Action<bool> _refreshFileComboBox;
        private string _lastSelectedFilePath = "";

        public KeyPresetForm(
            KeyboardBindingHandler keyboardHandler,
            Func<string> getCurrentJson,
            Action<string> setCurrentJson,
            Func<bool> saveJsonAndBackup,
            Action<bool> refreshFileComboBox)
        {
            _keyboardHandler = keyboardHandler;
            _getCurrentJson = getCurrentJson;
            _setCurrentJson = setCurrentJson;
            _saveJsonAndBackup = saveJsonAndBackup;
            _refreshFileComboBox = refreshFileComboBox;

            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;
            Font = new Font("Microsoft YaHei", 9F);
            InitializeUI();
        }

        private void InitializeUI()
        {
            _topCheckBox.CheckedChanged += (s, e) => this.TopMost = _topCheckBox.Checked;
            WriteKeysButton.Click += WriteKeysButton_Click;
            WriteKeyButton.Click += WriteKeyButton_Click;
            DeleteRepeatKeysButton.Click += DeleteRepeatKeysButton_Click;
            DeleteRangeRDkeysButton.Click += DeleteRangeRDkeysButton_Click;
            importKeymapbutton.Click += importKeymapbutton_Click;
            openPresetJsonFolderbutton.Click += openPresetJsonFolderbutton_Click;
            deleteDataJsonbutton.Click += deleteDataJsonbutton_Click;
            ReadPP2Button.Click += ReadPP2Button_Click;
            Button2textBox.KeyDown += Button2textBox_KeyDown;
            Button2textBox.KeyPress += Button2textBox_KeyPress;

            InitializeKeysComboBox(searchKeysCombo);
            searchKeysCombo.DropDown += (sdr, edr) =>
            {
                if (searchKeysCombo.SelectedValue != null)
                    _lastSelectedFilePath = searchKeysCombo.SelectedValue?.ToString() ?? "";
                searchKeysCombo.DataSource = null;
                InitializeKeysComboBox(searchKeysCombo);
            };
            searchKeysCombo.FilterRequested += (sdr, text) =>
            {
                InitializeKeysComboBox(searchKeysCombo, text, true);
            };
        }

        private void InitializeKeysComboBox(SearchableComboBox keysListComboBox, string searchText = null, bool flagBack = true)
        {
            try
            {
                string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                var jsonFiles = ComboBoxInitializer.TryGetJsonFiles(dataFolder, keysListComboBox);
                if (jsonFiles == null) return;

                var items = new List<KeyValuePair<string, string>>();
                foreach (var file in jsonFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (string.IsNullOrEmpty(searchText) || fileName.IndexOf(searchText) != -1)
                        items.Add(new KeyValuePair<string, string>(file, fileName));
                }
                if (items.Count == 0 && !string.IsNullOrEmpty(searchText))
                {
                    ComboBoxInitializer.ShowEmptyMessage(keysListComboBox, "未找到符合条件的文件！");
                    return;
                }

                string savedValue = keysListComboBox.SelectedValue?.ToString();
                ComboBoxInitializer.BindFileItems(keysListComboBox, items);
                string restoreKey = flagBack ? _lastSelectedFilePath : (savedValue ?? "");
                ComboBoxInitializer.RestoreSelection(keysListComboBox, items, restoreKey);
            }
            catch (Exception ex)
            {
                keysListComboBox.DataSource = null;
                keysListComboBox.Items.Add($"加载失败: {ex.Message}");
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool WriteKeysCheck()
        {
            try
            {
                if (string.IsNullOrEmpty(_getCurrentJson()))
                {
                    MessageBox.Show("请先在主窗口加载一个Json文件！");
                    return false;
                }
                string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                if (!Directory.Exists(dataFolder))
                {
                    MessageBox.Show("程序目录下无\"data\"文件夹，请检查您的配置文件！");
                    return false;
                }
                if (Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly).Length == 0)
                {
                    MessageBox.Show("\"data\"文件夹中无json文件，请检查您的配置文件！");
                    return false;
                }
                if (searchKeysCombo.SelectedItem == null || searchKeysCombo.SelectedItem.ToString() == "数据目录不存在" || searchKeysCombo.SelectedItem.ToString() == "未找到符合条件的文件！")
                {
                    MessageBox.Show("文件不存在，请重新选择你的基础键位！");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }

        private void WriteKeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                _refreshFileComboBox(true);
                if (!WriteKeysCheck()) return;
                string myJson = _getCurrentJson();
                string[] text = MuMuJsonEditor.FindKeyValues(searchKeysCombo.SelectedValue?.ToString() ?? "");
                if (!MuMuJsonEditor.AreAllKeysMissing(text, myJson))
                {
                    DialogResult result = MessageBox.Show("检测到待写入Json文件存在重复按键，是否继续写入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        string[] keyText = MuMuJsonEditor.FindKeyTexts(searchKeysCombo.SelectedValue?.ToString() ?? "");
                        string[] repeatKeyText = MuMuJsonEditor.FindAllRepeatKeyTexts(keyText, myJson);
                        string messageKey = string.Join(",", repeatKeyText);
                        MessageBox.Show("存在重复按键:" + messageKey + "\n请修改待写入的按键文件后再操作！");
                        return;
                    }
                }
                string keys = MuMuJsonEditor.ReadKeys(searchKeysCombo.SelectedValue?.ToString() ?? "");
                _setCurrentJson(MuMuJsonEditor.WriteKeys(keys, myJson));
                if (_saveJsonAndBackup()) { MessageBox.Show("基础键位注入成功！如出现问题请转人工。"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void WriteKeyButton_Click(object sender, EventArgs e)
        {
            try
            {
                _refreshFileComboBox(true);
                if (!WriteKeysCheck()) return;
                if (_keyboardHandler.BindKey2 == null) { MessageBox.Show("当前未绑定按键，请检查您的设置！"); return; }
                string myJson = _getCurrentJson();
                string[] text = { _keyboardHandler.BindKey2.KeyValue.ToString() };
                if (!MuMuJsonEditor.AreAllKeysMissing(text, myJson))
                {
                    DialogResult result = MessageBox.Show("检测到待写入Json文件存在重复按键，是否继续写入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        MessageBox.Show("存在重复按键:" + _keyboardHandler.BindKey2.KeyData + "\n请修改待写入的按键文件后再操作！");
                        return;
                    }
                }
                string key = MuMuJsonEditor.ReadKey(searchKeysCombo.SelectedValue?.ToString() ?? "", _keyboardHandler.BindKey2);
                _setCurrentJson(MuMuJsonEditor.WriteKeys(key, myJson));
                if (_saveJsonAndBackup()) { MessageBox.Show("单键位注入成功！如出现问题请转人工。"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void DeleteRepeatKeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) return;
                string myJson = _getCurrentJson();
                string[] text = MuMuJsonEditor.FindKeyValues(searchKeysCombo.SelectedValue?.ToString() ?? "");
                if (MuMuJsonEditor.AreAllKeysMissing(text, myJson))
                {
                    MessageBox.Show("无重复键位，可执行基础键位注入。");
                    return;
                }
                DialogResult result = MessageBox.Show("去重功能存在风险，使用前请确保重复的按键中不存在你要保留的按键。", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
                while (!MuMuJsonEditor.AreAllKeysMissing(text, myJson))
                {
                    string[] keyValue = MuMuJsonEditor.FindKeyValues(searchKeysCombo.SelectedValue?.ToString() ?? "");
                    string[] repeatKeyValues = MuMuJsonEditor.FindAllRepeatKeyValues(keyValue, myJson);
                    myJson = MuMuJsonEditor.DeleteKeys(repeatKeyValues, myJson);
                }
                _setCurrentJson(myJson);
                if (MuMuJsonEditor.AreAllKeysMissing(text, myJson))
                {
                    if (_saveJsonAndBackup()) { MessageBox.Show("已清除所有重复键位，可执行基础键位注入。"); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void DeleteRangeRDkeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) return;
                DialogResult result = MessageBox.Show("右下区域清空功能存在风险，且当前功能仅支持16：9分辨率的键位文件。\n使用前请确认键位适配分辨率且右下角选牌区不存在要保留的按键！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
                double[] rangeLT = { 0.661, 0.798 };
                double[] rangeRD = { 1.0, 1.0 };
                string myJson = _getCurrentJson();
                var results = MuMuJsonEditor.FindRangeKeyValues(rangeLT, rangeRD, myJson);
                if (results.Count == 0) { MessageBox.Show("右下选牌区域中不存在按键，无需清空。"); return; }
                string messageKeyTexts = "";
                foreach (var (x, y, text, vk) in results)
                {
                    myJson = MuMuJsonEditor.DeleteKey(vk, myJson);
                    messageKeyTexts += text + ",";
                }
                _setCurrentJson(myJson);
                if (_saveJsonAndBackup()) { MessageBox.Show($"已清空：{messageKeyTexts}键，如出现问题请转人工！"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void importKeymapbutton_Click(object sender, EventArgs e)
        {
            try
            {
                LogService.Info("KeyPresetForm", "导入键位映射");
                string dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                if (!Directory.Exists(dataFolderPath))
                    Directory.CreateDirectory(dataFolderPath);

                var mainForm = Application.OpenForms["Form1"] as Form1;
                if (mainForm == null) { MessageBox.Show("主窗口未打开！"); return; }

                string mainJsonPath = mainForm.GetJsonFilePath();
                if (string.IsNullOrEmpty(mainJsonPath)) { MessageBox.Show("主窗口未加载文件！"); return; }

                string fileName = Path.GetFileName(mainJsonPath);
                if (string.IsNullOrEmpty(fileName) || fileName == ".json")
                {
                    MessageBox.Show("请先在主窗口选择一个有效的文件！");
                    return;
                }

                fileName = mainForm.StripPackageName(fileName);

                string filePath = Path.Combine(dataFolderPath, fileName);
                string myJson = _getCurrentJson();
                if (string.IsNullOrEmpty(myJson))
                {
                    MessageBox.Show("请先在主窗口加载一个Json文件！");
                    return;
                }
                if (File.Exists(filePath))
                {
                    DialogResult result2 = MessageBox.Show("检测到data文件夹中已有同名文件，是否覆盖？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result2 == DialogResult.Yes)
                    {
                        File.WriteAllText(filePath, myJson, Encoding.UTF8);
                        MessageBox.Show("覆写成功！请点开下拉框选择你需要的文件。");
                    }
                }
                else
                {
                    File.WriteAllText(filePath, myJson, Encoding.UTF8);
                    MessageBox.Show("导入成功！请点开下拉框选择你需要的文件。");
                }
            }
            catch (Exception ex)
            {
                LogService.Error("KeyPresetForm", ex, "importKeymapbutton_Click");
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void openPresetJsonFolderbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string dataFolderPath = Path.Combine(Application.StartupPath, "data");
                if (!Directory.Exists(dataFolderPath))
                    Directory.CreateDirectory(dataFolderPath);

                using (Process.Start(new ProcessStartInfo
                {
                    FileName = dataFolderPath,
                    UseShellExecute = true,
                    Verb = "open"
                })) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteDataJsonbutton_Click(object sender, EventArgs e)
        {
            try
            {
                LogService.Info("KeyPresetForm", "删除选中的基础键位文件");
                string selectedPath = searchKeysCombo.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                {
                    MessageBox.Show("请先选择一个有效的基础键位文件！");
                    return;
                }
                string fileName = Path.GetFileName(selectedPath);
                DialogResult result = MessageBox.Show($"确认删除文件 \"{fileName}\"？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    File.Delete(selectedPath);
                    MessageBox.Show($"已删除文件 \"{fileName}\"！");
                    InitializeKeysComboBox(searchKeysCombo);
                }
            }
            catch (Exception ex)
            {
                LogService.Error("KeyPresetForm", ex, "deleteDataJsonbutton_Click");
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void ReadPP2Button_Click(object sender, EventArgs e)
        {
            try
            {
                string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                if (!Directory.Exists(dataFolder))
                {
                    MessageBox.Show("程序目录下无\"data\"文件夹，请检查您的配置文件！");
                    return;
                }
                if (Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly).Length == 0)
                {
                    MessageBox.Show("\"data\"文件夹中无json文件，请检查您的配置文件！");
                    return;
                }
                if (searchKeysCombo.SelectedItem == null || searchKeysCombo.SelectedItem.ToString() == "数据目录不存在")
                {
                    MessageBox.Show("请重新选择你的基础键位！");
                    return;
                }
                string selectedValue = searchKeysCombo.SelectedValue?.ToString() ?? "";
                if (string.IsNullOrEmpty(selectedValue)) { MessageBox.Show("未选中有效的JSON文件路径！"); return; }
                string myJson = File.ReadAllText(selectedValue, Encoding.UTF8);
                MessageBox.Show("当前绑定按键为：" + _keyboardHandler.BindKey2.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture) + Environment.NewLine + "当前绑定按键值为：" + _keyboardHandler.BindKey2.KeyValue.ToString() + Environment.NewLine);
                if (MuMuJsonEditor.FindKey(myJson, _keyboardHandler.BindKey2) == -1)
                {
                    MessageBox.Show("当前Json文件中未找到按键" + Button2textBox.Text);
                }
                else
                {
                    string[] key = MuMuJsonEditor.ReadKeyPP(myJson, _keyboardHandler.BindKey2);
                    if (key == null) { MessageBox.Show("查找坐标失败，请检查您指定的按键中是否有坐标存在！"); return; }
                    var mainForm = Application.OpenForms["Form1"] as Form1;
                    if (mainForm != null)
                    {
                        mainForm.SetJSXY(key[0], key[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ReadPP2Button_Click] 操作失败: {ex.Message}");
                MessageBox.Show("当前未绑定按键，请检查您的设置！");
            }
        }

        private void Button2textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Button2textBox.Text = "";
            e.Handled = true;
            Button2textBox.Text = KeyboardBindingHandler.KeyEventArgsToDisplayText(_keyboardHandler.BindKey2);
        }

        private void Button2textBox_KeyDown(object sender, KeyEventArgs e)
        {
            Button2textBox.Text = "";
            _keyboardHandler.ProcessBindKey2KeyDown(e);
            e.Handled = true;
            Button2textBox.Text = KeyboardBindingHandler.KeyEventArgsToDisplayText(e);
        }
    }
}
