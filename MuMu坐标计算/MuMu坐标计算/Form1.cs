using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MuMu坐标计算.Properties;

namespace MuMu坐标计算
{
    public partial class Form1 : Form
    {
        //防止四个文本框对应的坐标计算冲突（重入保护）
        bool _isProcessingCoordinateChange = false;
        //控制下拉框刷新时的选项变动，防止重复调用
        bool flagFlushingFilename = true;
        //控制分辨率下拉框刷新时的选项变动，防止重复调用
        bool flagFlushingResolution = true;
        //控制分辨率类型下拉框刷新时的选项变动，防止重复调用
        bool flagFlushingResolutionType = true;
        //记录是否全选
        private bool isUpdatingCheckState = false;
        //记录上方文件要读取/修改的按键（已迁移到_keyboardHandler.BindKey1）
        String _mumuJson= "";
        //记录最后修改文件的时间
        DateTime lastWriteTime;
        //保留的小数位数
        String FDP = "F16";
        // 保存上次选中文件的路径
        string lastSelectedFilePath = "";
        //重载文件时的语句
        string reloadingTip = "该操作会导致对当前文件的编辑无法还原，是否继续？";
        MuMuKeymapSyncService _syncService;
        bool _isAutoSync = false;
        int _syncGuard = 0;
        volatile string _pendingSyncPath = null;
        //用于监听键盘按键
        readonly KeyboardBindingHandler _keyboardHandler = new KeyboardBindingHandler();
        //预设按键类型的数据源
        //预设包名分类的数据源
        Dictionary<string, string> PackageNameTypes{ get; } = new Dictionary<string, string>{
            {"官服","com.RoamingStar.BlueArchive-" },
            { "B服","com.RoamingStar.BlueArchive.bilibili-"},
            { "日服","com.YostarJP.BlueArchive-"},
            { "国际服","com.nexon.bluearchive-"},
            { "其他","other"},
            { "宇宙服","萌新666sssaaa"}
        };
        //预设分辨率类型分类的数据源
        Dictionary<string, string> resolutionTypes { get; } = new Dictionary<string, string>{
            {"平板","1" },
            { "手机","2"},
            { "超宽屏","3"},
            { "自定义","4"}
        };
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);
        readonly BackupManager _backupManager = new BackupManager();
        readonly ConfigManager _config = new ConfigManager();
        readonly FileMonitor _fileMonitor = new FileMonitor();
        readonly System.Windows.Forms.ToolTip _featureToolTip = new System.Windows.Forms.ToolTip();
        void OnFormClosing(object s, FormClosingEventArgs e)
        {
            if (_needsIndexBackup && !string.IsNullOrEmpty(_pendingBackupPath))
            {
                _needsIndexBackup = false;
                try { _indexBackup.BackupIndex(_pendingBackupPath); } catch { }
            }
            _syncService?.Stop();
            _indexBackupTimer?.Stop();
            _featureToolTip.Dispose();
        }
        IndexFileBackupManager _indexBackup;
        string _pendingCheckSchemePath;
        bool _needsIndexBackup;
        string _pendingBackupPath;
        System.Windows.Forms.Timer _indexBackupTimer;
        bool _suppressIndexCheck;
        System.Collections.Generic.HashSet<string> _indexDamageNotified;
        StatusNotifier _status;
        //分辨率管理器
        readonly ResolutionManager _resolutionManager = new ResolutionManager();
        public Form1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Font = new Font("Microsoft YaHei", 9F);
            this.FormClosing += OnFormClosing;
            EnsureSearchControls();
            InitializeHiddenMenu();
        }
        //初始化，窗口加载时读取配置文件
        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
            this.FXtextBox.Text = _config.FX;
            this.FYtextBox.Text = _config.FY;
            _keyboardHandler.FindKey = _config.FindKey;
            _keyboardHandler.ResetKey = _config.ResetKey;
            FindKeytextBox.Text = _keyboardHandler.FindKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            ResetKeytextBox.Text = _keyboardHandler.ResetKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            InitializeKeysComboBox(searchKeysCombo);
            //初始化包名选择框
            packageNamecomboBox.DisplayMember = "Key";
            packageNamecomboBox.ValueMember = "Value";
            packageNamecomboBox.DataSource = PackageNameTypes.ToList();
            //初始化第二个包名选择框
            packageNamecomboBox2.DisplayMember = "Key";
            packageNamecomboBox2.ValueMember = "Value";
            packageNamecomboBox2.DataSource = PackageNameTypes.ToList();
            //初始化分辨率类型选择框
            resolutionTypecomboBox.DisplayMember = "Key";
            resolutionTypecomboBox.ValueMember = "Value";
            resolutionTypecomboBox.DataSource = resolutionTypes.ToList();
            //初始化第二个分辨率类型选择框（用于实现异形分辨率转换模块）
            resolutionTypecomboBox2.DisplayMember = "Key";
            resolutionTypecomboBox2.ValueMember = "Value";
            resolutionTypecomboBox2.DataSource = resolutionTypes.ToList();
            //初始化分辨率选择框
            InitializeResolutioncomboBox(resolutioncomboBox);
            //初始化第二个分辨率选择框（用于实现异形分辨率转换模块）默认初始值为1920x1080
            if (resolutionTypecomboBox2.Items.Count > 0)
                resolutionTypecomboBox2.SelectedIndex = 0;
            if (resolutioncomboBox2.Items.Count > 1)
                resolutioncomboBox2.SelectedIndex = 1;
            //尝试获取mumu目录
            if (string.IsNullOrWhiteSpace(_config.JsonFolderPath)) {
                await TryGetJsonFileFolder();
            }
            InitializeFileNamecomboBox(searchFileCombo,false);

            //初始化后加载
            if (searchFileCombo.SelectedValue != null)
            {
                updateJsonUrltextBox(searchFileCombo.SelectedValue.ToString());
                string filePath = @JsonUrltextBox.Text;
                await LoadJsonFileAsync(filePath);
            }
            //初始化按键类型选项框
            keyTypelistcomboBox.DisplayMember = "Key";
            keyTypelistcomboBox.ValueMember = "Value";
            keyTypelistcomboBox.DataSource = ComboBoxInitializer.PredefinedKeyTypes.ToList();
            keyTypelistcomboBox.SelectedIndexChanged += (s, ev) =>
            {
                var adbForm = Application.OpenForms["AdbTouchForm"] as AdbTouchForm;
                if (adbForm != null)
                    adbForm.SyncKeyTypeFromMainForm();
            };
            //初始化按键启用
            SetUndobtnAndRedobtnState();

            _fileMonitor.FileChanged += OnExternalFileChanged;
            _syncService = new MuMuKeymapSyncService();
            _syncService.KeymapChanged += OnMuMuKeymapDetected;
            if (!string.IsNullOrEmpty(_config.JsonFolderPath))
            {
                _syncService.Start(_config.JsonFolderPath);
                _syncService.Enable();
            }
            tabPage1.Controls.Add(Tip1label);
            tabPage1.Controls.Add(Tip2label);
            _status = new StatusNotifier(Tip1label, Tip2label);
            _indexBackup = new IndexFileBackupManager(AppDomain.CurrentDomain.BaseDirectory, PackageNameTypes);

            // 添加子功能入口按钮
            AddFeatureButtons();

            _featureToolTip.SetToolTip(ReadPPButton, "修改/读取范围详见说明书");
            _featureToolTip.SetToolTip(RewriteAndSaveButton, "修改/读取范围详见说明书");

            // 搜索下拉框事件订阅
            searchKeysCombo.DropDown += (sdr, edr) =>
            {
                if (searchKeysCombo.SelectedValue != null)
                    lastSelectedFilePath = searchKeysCombo.SelectedValue.ToString();
                searchKeysCombo.DataSource = null;
                InitializeKeysComboBox(searchKeysCombo);
            };
            searchKeysCombo.FilterRequested += (sdr, text) =>
            {
                InitializeKeysComboBox(searchKeysCombo, text, true);
            };
            searchFileCombo.DropDown += (sdr, edr) =>
            {
                InitializeFileNamecomboBox(searchFileCombo, false);
            };
            searchFileCombo.DropDownClosed += async (sdr, edr) =>
            {
                try
                {
                    if (searchFileCombo.SelectedValue != null)
                    {
                        if (!_isAutoSync) autoSyncCheckBox.Checked = false;
                        if (searchFileCombo.SelectedValue.ToString() == JsonUrltextBox.Text)
                        {
                            return;
                        }
                        var oldPath = JsonUrltextBox.Text;
                        var needsBackup = _needsIndexBackup;
                        _needsIndexBackup = false;
                        if (Undobutton.Enabled || Redobutton.Enabled)
                        {
                            DialogResult result = MessageBox.Show(reloadingTip, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                            {
                                InitializeFileNamecomboBox(searchFileCombo, true);
                                updateJsonUrltextBox(searchFileCombo.SelectedValue.ToString());
                                return;
                            }
                        }
                        updateJsonUrltextBox(searchFileCombo.SelectedValue.ToString());
                        string filePath = JsonUrltextBox.Text;
                        await LoadJsonFileAsync(filePath);
                        TryUpdateJsonFolderPathFromFile(filePath);
                        if (needsBackup && !string.IsNullOrEmpty(oldPath))
                        {
                            var backupPath = oldPath;
                            _ = Task.Run(() => { try { _indexBackup.BackupIndex(backupPath); } catch { } });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"发生错误：{ex.Message}");
                }
            };
            searchFileCombo.FilterRequested += (sdr, text) =>
            {
                if (string.IsNullOrWhiteSpace(text))
                    InitializeFileNamecomboBox(searchFileCombo, false);
                else
                    InitializeFileNamecomboBox(searchFileCombo, true, text);
            };            InitializeKeyboardlistener();
            // 延迟初始化备份检查
            _indexDamageNotified = new System.Collections.Generic.HashSet<string>();
            _indexBackup.CleanupOldBackups(10);

            _indexCheckTimer.Interval = 3000;
            _indexBackupTimer = new System.Windows.Forms.Timer();
            _indexBackupTimer.Interval = 60000;
            _indexBackupTimer.Tick += _indexBackupTimer_Tick;
            _indexBackupTimer.Start();
            this.FormClosed += (s, args) =>
            {
                HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdSave);
                HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdRecall);
            };
            // 初始化索引损坏标记（备份功能已移至子界面）

            adbBtn.Click += (s, args) => OpenAdbTouchForm();

            autoSyncCheckBox.CheckedChanged += (s, _) =>
            {
                if (autoSyncCheckBox.Checked)
                {
                    _syncService?.Enable();
                    TryAutoSyncFromEmulatorKeymap();
                }
                else
                    _syncService?.Disable();
            };

            TryAutoSyncFromEmulatorKeymap();
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "Form1_Load");
                MessageBox.Show("初始化失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool EnsureCaptureReady()
        {
            InitializeFileNamecomboBox(searchFileCombo, true);
            if (_mumuJson == "" || JsonUrltextBox.Text == "")
            { MessageBox.Show("请先加载一个Json文件！"); return false; }
            return true;
        }

        private void ExecuteCaptureKeyCreation(KeyEventArgs key)
        {
            string[] keyValues = ComboBoxInitializer.PredefinedKeyTypes.Values.ToArray();
            string selectedType = keyTypelistcomboBox.SelectedValue?.ToString() ?? "";
            if (selectedType != keyValues[0] && selectedType != keyValues[1])
            { MessageBox.Show("未知错误！请检查按键类型选择。"); return; }

            string keyType = selectedType == keyValues[0] ? "点击按键" : "宏指牌按键";
            string mkey = MuMuJsonEditor.CreateKey(selectedType, key, JSXtextBox.Text, JSYtextBox.Text, MuMuJsonEditor.GetScanCode(key.KeyCode).ToString());
            _mumuJson = MuMuJsonEditor.WriteKeys(mkey, _mumuJson);
            if (WriteToJsonAndBackup() && Application.OpenForms["QuickTouchForm"] == null) { _status.ShowKeyCreated(keyType, key.KeyCode.ToString()); }
        }

        public bool CreateKeyFromQuickTouch(KeyEventArgs key, string coordX, string coordY)
        {
            if (InvokeRequired)
                return (bool)Invoke(new Func<KeyEventArgs, string, string, bool>(CreateKeyFromQuickTouch), key, coordX, coordY);

            string[] keyValues = ComboBoxInitializer.PredefinedKeyTypes.Values.ToArray();
            string selectedType = keyTypelistcomboBox.SelectedValue?.ToString() ?? "";
            if (selectedType != keyValues[0] && selectedType != keyValues[1])
                return false;

            KXtextBox.Text = coordX;
            KYtextBox.Text = coordY;

            string mkey = MuMuJsonEditor.CreateKey(selectedType, key, JSXtextBox.Text, JSYtextBox.Text, MuMuJsonEditor.GetScanCode(key.KeyCode).ToString());
            _mumuJson = MuMuJsonEditor.WriteKeys(mkey, _mumuJson);
            return WriteToJsonAndBackup();
        }

        private void InitializeKeyboardlistener()
        {
            _keyboardHandler.KeyCapturedOnce += (sender, key) =>
            {
                if (Application.OpenForms["QuickTouchForm"] != null) return;
                if (!EnsureCaptureReady()) return;
                ExecuteCaptureKeyCreation(key);
                _status.ShowListeningStopped();
            };

            _keyboardHandler.KeyCapturedContinuously += (sender, key) =>
            {
                if (Application.OpenForms["QuickTouchForm"] != null) return;
                if (!EnsureCaptureReady()) return;
                if (MuMuJsonEditor.FindKey(_mumuJson, key) == -1)
                    ExecuteCaptureKeyCreation(key);
                else
                    _status.ShowKeyExists(key.KeyCode.ToString());
            };
        }
        //创建隐藏菜单用于绑定快捷键
        private void InitializeHiddenMenu()
        {
            // 创建隐藏的MenuStrip
            MenuStrip hiddenMenuStrip = new MenuStrip();
            hiddenMenuStrip.Visible = false;
            this.Controls.Add(hiddenMenuStrip);

            // 创建撤销菜单项（Ctrl+Z）
            ToolStripMenuItem undoToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem.Text = "撤销(&U)";
            undoToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.Z);
            undoToolStripMenuItem.Click += Undobutton_Click;

            // 创建重做菜单项（Ctrl+Y）
            ToolStripMenuItem redoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem.Text = "重做(&R)";
            redoToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.Y);
            redoToolStripMenuItem.Click += Redobutton_Click;

            // 将菜单项添加到MenuStrip
            hiddenMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(), // 分隔线（可选）
                undoToolStripMenuItem,
                redoToolStripMenuItem
            });
        }
        //高复用频率代码整理区域
        //检测空输入
        private bool CheckEmptyText() {
            if (FXtextBox.Text == "" ||
                    FYtextBox.Text == "" ||
                    KXtextBox.Text == "" ||
                    KYtextBox.Text == "" ||
                    JSXtextBox.Text == "" ||
                    JSYtextBox.Text == "") { 
                return true; 
            };
            return false;
        }
        //自动读工具人按键点位
        //基础校验
        private bool WriteKeysCheck()
        {
            try
            {
                if (_mumuJson == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return false;
                }
                // 获取程序所在目录的"data"子文件夹路径
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string dataFolder = Path.Combine(baseDir, "data");
                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    MessageBox.Show("程序目录下无“data”文件夹，请检查您的配置文件！");
                    return false;
                }
                //检查文件夹下是否有json文件
                if (Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly).Length == 0)
                {
                    MessageBox.Show("“data”文件夹中无json文件，请检查您的配置文件！");
                    return false;
                }
                //检查是否删除json文件后又还原但是未重新加载列表（真有人能无聊到触发这个bug吗？？？）
                if (searchKeysCombo.SelectedValue == null || searchKeysCombo.SelectedItem == null || searchKeysCombo.SelectedItem.ToString() == "数据目录不存在"|| searchKeysCombo.SelectedItem.ToString()== "未找到符合条件的文件！")
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
        //写入修改后的mumu文件并备份
        private bool WriteToJsonAndBackup()
        {
            try
            {
                string validatedPath = GetValidatedJsonPath(JsonUrltextBox.Text);
                if (validatedPath == null)
                {
                    statusText.Text = "无效的文件路径";
                    return false;
                }
                string filePath = validatedPath;
                if (_mumuJson == "" || filePath == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return false;
                }

                string tmpPath = filePath + ".tmp";
                File.WriteAllText(tmpPath, _mumuJson, Utf8NoBom);
                File.Replace(tmpPath, filePath, null);
                lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                BackupAfterJsonWriting();

                _needsIndexBackup = true;
                _pendingBackupPath = filePath;

                _pendingCheckSchemePath = filePath;
                _indexCheckTimer.Stop();
                _indexCheckTimer.Start();

                statusText.Text = "保存成功";
                LogService.Info("Form1", string.Format("WriteToJsonAndBackup: 写入成功, JSON长度={0}, 路径={1}", _mumuJson.Length, filePath));
                return true;
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "WriteToJsonAndBackup");
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        //纯粹写入mumu文件
        private bool WriteToJson()
        {
            try
            {
                string validatedPath = GetValidatedJsonPath(JsonUrltextBox.Text);
                if (validatedPath == null)
                {
                    statusText.Text = "无效的文件路径";
                    return false;
                }
                string filePath = validatedPath;
                if (_mumuJson == "" || filePath == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return false;
                }

                string tmpPath = filePath + ".tmp";
                File.WriteAllText(tmpPath, _mumuJson, Utf8NoBom);
                File.Replace(tmpPath, filePath, null);
                lastWriteTime = File.GetLastWriteTimeUtc(filePath);

                _needsIndexBackup = true;
                _pendingBackupPath = filePath;

                _pendingCheckSchemePath = filePath;
                _indexCheckTimer.Stop();
                _indexCheckTimer.Start();

                statusText.Text = "方案已保存。";
                LogService.Info("Form1", string.Format("WriteToJson: 写入成功, JSON长度={0}, 路径={1}", _mumuJson.Length, filePath));
                return true;
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "WriteToJson");
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        private void SetUndobtnAndRedobtnState()
        {
            _backupManager.UpdateButtonStates(Undobutton, Redobutton);
        }
        private bool BackupAfterJsonReading()
        {
            try
            {
                _backupManager.RecordInitial(_mumuJson);
                SetUndobtnAndRedobtnState();
                LogService.Info("Form1", string.Format("BackupAfterJsonReading: 记录初始快照, JSON长度={0}, CanUndo={1}, CanRedo={2}",
                    _mumuJson.Length, Undobutton.Enabled, Redobutton.Enabled));
                return true;
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "BackupAfterJsonReading");
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        private bool BackupAfterJsonWriting()
        {
            try
            {
                _backupManager.RecordChange(_mumuJson);
                SetUndobtnAndRedobtnState();
                LogService.Info("Form1", string.Format("BackupAfterJsonWriting: 记录变更快照, JSON长度={0}, CanUndo={1}, CanRedo={2}",
                    _mumuJson.Length, Undobutton.Enabled, Redobutton.Enabled));
                return true;
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "BackupAfterJsonWriting");
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        //区域结束

        //读取/保存按钮整理区域
        //保存默认分辨率（使用ResolutionManager消除4份重复代码）
        private void FSave_Click(object sender, EventArgs e)
        {
            try
            {
                _config.FX = FXtextBox.Text;
                _config.FY = FYtextBox.Text;
                flagFlushingResolution = false;
                flagFlushingResolutionType = false;

                var (typeCode, resDict) = _resolutionManager.ClassifyResolution(
                    FXtextBox.Text, FYtextBox.Text,
                    _config.Resolution4String);
                if (typeCode == null || resDict == null)
                { flagFlushingResolution = true; flagFlushingResolutionType = true; return; }

                resolutionTypecomboBox.SelectedValue = typeCode;
                deleteUDResolutionbutton.Visible = (typeCode == ResolutionManager.TypeCustom);

                InitializeResolutioncomboBox(resolutioncomboBox);

                if (typeCode == ResolutionManager.TypeCustom && resDict.ContainsKey("*" + (FXtextBox.Text + "x" + FYtextBox.Text)))
                {
                    _config.Resolution4String = MuMuJsonEditor.ResolutionToString(resDict);
                }
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
                statusText.Text = "保存成功：" + FXtextBox.Text + "x" + FYtextBox.Text;
            }
            catch (Exception ex)
            {
                resolutioncomboBox.DataSource = null;
                resolutioncomboBox.Items.Add($"加载失败: {ex.Message}");
                resolutioncomboBox.SelectedIndex = 0;
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
                statusText.Text = "保存失败";
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //读取默认分辨率
        private void FLoad_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!_config.Reload())
                {
                    statusText.Text = "读取配置失败";
                    return;
                }
                FXtextBox.Text = _config.FX;
                FYtextBox.Text = _config.FY;
                flagFlushingResolution = false;
                flagFlushingResolutionType = false;
                InitializeResolutioncomboBox(resolutioncomboBox);
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
                statusText.Text = "读取成功：" + _config.FX + "x" + _config.FY;
            }
            catch (Exception ex)
            {
                statusText.Text = "读取失败";
                MessageBox.Show($"读取配置时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //保存默认快捷键
        private void SaveKeybutton_Click(object sender, EventArgs e)
        {
            _config.FindKey = _keyboardHandler.FindKey;
            _config.ResetKey = _keyboardHandler.ResetKey;
            statusText.Text = "快捷键已保存";
        }
        //读取默认快捷键
        private void LoadKeybutton_Click(object sender, EventArgs e)
        {
            _keyboardHandler.FindKey = _config.FindKey;
            _keyboardHandler.ResetKey = _config.ResetKey;
            FindKeytextBox.Text = _keyboardHandler.FindKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            ResetKeytextBox.Text = _keyboardHandler.ResetKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            //读取完快捷键后重置一下坐标Timer，加载新快捷键
            if (CcheckBox.Checked)
            {
                CcheckBox.Checked = false;
                CcheckBox.Checked = true;
            }
            statusText.Text = "快捷键已读取";
        }
        //区域结束

        //获取修改按键（使用KeyboardBindingHandler简化）
        private void ButtontextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ButtontextBox.Text = "";
            e.Handled = true;
            ButtontextBox.Text = KeyboardBindingHandler.KeyEventArgsToDisplayText(_keyboardHandler.BindKey1);
            statusText.Text = "已绑定按键：" + ButtontextBox.Text;
        }
        private void ButtontextBox_KeyDown(object sender, KeyEventArgs e)
        {
            ButtontextBox.Text = "";
            _keyboardHandler.ProcessBindKey1KeyDown(e);
            e.Handled = true;
            ButtontextBox.Text = KeyboardBindingHandler.KeyEventArgsToDisplayText(e);
            statusText.Text = "已绑定按键：" + ButtontextBox.Text;
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
        //获取保存鼠标坐标快捷键
        private void FindKeytextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (FindKeytextBox.ReadOnly) { return; }
            FindKeytextBox.Text = "";
            bool conflict;
            _keyboardHandler.ProcessFindKeyDown(e, out conflict);
            if (conflict) { MessageBox.Show("快捷键冲突！"); return; }
            e.Handled = true;
            FindKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(e.KeyCode);
            if (CcheckBox.Checked) { CcheckBox.Checked = false; CcheckBox.Checked = true; }
        }
        private void FindKeytextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            FindKeytextBox.Text = "";
            e.Handled = true;
            FindKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(_keyboardHandler.FindKey);
        }
        //获取回溯鼠标坐标快捷键
        private void ResetKeytextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (FindKeytextBox.ReadOnly) { return; }
            ResetKeytextBox.Text = "";
            bool conflict;
            _keyboardHandler.ProcessResetKeyDown(e, out conflict);
            if (conflict) { MessageBox.Show("快捷键冲突！"); return; }
            e.Handled = true;
            ResetKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(e.KeyCode);
            if (CcheckBox.Checked) { CcheckBox.Checked = false; CcheckBox.Checked = true; }
        }
        private void ResetKeytextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ResetKeytextBox.Text = "";
            e.Handled = true;
            ResetKeytextBox.Text = KeyboardBindingHandler.KeyToDisplayText(_keyboardHandler.ResetKey);
        }
        //区域结束

        //打开下拉框时刷新数据
        private void searchKeysCombo_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (searchKeysCombo.SelectedValue != null)
                {
                    lastSelectedFilePath = searchKeysCombo.SelectedValue.ToString();
                }
                searchKeysCombo.DataSource = null;
                InitializeKeysComboBox(searchKeysCombo);
                searchKeysCombo.Visible = true;
                searchKeysCombo.BringToFront();
                searchKeysCombo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //关闭下拉框时隐藏搜索框
        private void searchKeysCombo_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //实现搜索功能
        private void searchKeysCombo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = searchKeysCombo.CurrentSearchText;
                InitializeKeysComboBox(searchKeysCombo, searchText, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //初始化存储的基础键位路径
        private void InitializeKeysComboBox(MuMu坐标计算.SearchableComboBox keysListComboBox, string searchText = null, bool flagBack = true)
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
                string restoreKey = flagBack ? lastSelectedFilePath : (savedValue ?? "");
                ComboBoxInitializer.RestoreSelection(keysListComboBox, items, restoreKey);
            }
            catch (Exception ex)
            {
                keysListComboBox.DataSource = null;
                keysListComboBox.Items.Add($"加载失败: {ex.Message}");
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //按包名分类初始化拖入文件对应文件夹中Json文件路径
        private void InitializeFileNamecomboBox(SearchableComboBox searchFileCombo, bool flagback, string searchText = null)
        {
            try
            {
                flagFlushingFilename = false;
                string dataFolder = _config.JsonFolderPath;
                var jsonFiles = ComboBoxInitializer.TryGetJsonFiles(dataFolder, searchFileCombo);
                if (jsonFiles == null) { flagFlushingFilename = true; return; }

                string[] PackageNamesValues = PackageNameTypes.Values.ToArray();
                if (!string.IsNullOrEmpty(JsonUrltextBox.Text) && flagback)
                {
                    if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[0]) != -1)
                        packageNamecomboBox.SelectedValue = PackageNamesValues[0];
                    else if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[1]) != -1)
                        packageNamecomboBox.SelectedValue = PackageNamesValues[1];
                    else if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[2]) != -1)
                        packageNamecomboBox.SelectedValue = PackageNamesValues[2];
                    else if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[3]) != -1)
                        packageNamecomboBox.SelectedValue = PackageNamesValues[3];
                    else
                        packageNamecomboBox.SelectedValue = PackageNamesValues[4];
                }
                string PackageName = packageNamecomboBox.SelectedValue?.ToString() ?? PackageNamesValues[4];

                var items = new List<KeyValuePair<string, string>>();
                if (PackageName == PackageNamesValues[0] || PackageName == PackageNamesValues[1]
                    || PackageName == PackageNamesValues[2] || PackageName == PackageNamesValues[3])
                {
                    foreach (var file in jsonFiles)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        if (fileName.IndexOf(PackageName) != -1)
                        {
                            fileName = fileName.Replace(PackageName, "");
                            if (string.IsNullOrEmpty(searchText) || fileName.IndexOf(searchText) != -1)
                                items.Add(new KeyValuePair<string, string>(file, fileName));
                        }
                    }
                }
                else if (PackageName == PackageNamesValues[4])
                {
                    foreach (var file in jsonFiles)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        if (fileName.IndexOf(PackageNamesValues[0]) == -1 && fileName.IndexOf(PackageNamesValues[1]) == -1
                            && fileName.IndexOf(PackageNamesValues[2]) == -1 && fileName.IndexOf(PackageNamesValues[3]) == -1)
                        {
                            if (string.IsNullOrEmpty(searchText) || fileName.IndexOf(searchText) != -1)
                                items.Add(new KeyValuePair<string, string>(file, fileName));
                        }
                    }
                }
                else if (PackageName == PackageNamesValues[5])
                {
                    ComboBoxInitializer.ShowEmptyMessage(searchFileCombo, "绿玩哪有宇宙服，你清醒一点。");
                    flagFlushingFilename = true;
                    return;
                }
                else
                {
                    ComboBoxInitializer.ShowEmptyMessage(searchFileCombo, "为啥你能看到这条提示，你找到了我未曾想到的bug！");
                    flagFlushingFilename = true;
                    return;
                }
                if (items.Count == 0)
                {
                    ComboBoxInitializer.ShowEmptyMessage(searchFileCombo,
                        string.IsNullOrEmpty(searchText) ? "该分类下没有对应的Json文件！" : "未找到符合条件的文件！");
                    flagFlushingFilename = true;
                    return;
                }

                ComboBoxInitializer.BindFileItems(searchFileCombo, items);
                ComboBoxInitializer.RestoreSelection(searchFileCombo, items, JsonUrltextBox.Text);
                flagFlushingFilename = true;
            }
            catch (Exception ex)
            {
                searchFileCombo.DataSource = null;
                searchFileCombo.Items.Add($"加载失败: {ex.Message}");
                flagFlushingFilename = true;
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeResolutioncomboBox(ComboBox resolutioncomboBox, string forceTypeCode = null)
        {
            try
            {
                flagFlushingResolution = false;
                flagFlushingResolutionType = false;

                var (typeCode, resDict) = _resolutionManager.ClassifyResolution(
                    FXtextBox.Text, FYtextBox.Text,
                    _config.Resolution4String);
                if (typeCode == null || resDict == null)
                { flagFlushingResolution = true; flagFlushingResolutionType = true; return; }

                if (forceTypeCode != null)
                {
                    typeCode = forceTypeCode;
                    resDict = _resolutionManager.GetResolutionDictByType(forceTypeCode, _config.Resolution4String);
                }

                resolutionTypecomboBox.SelectedValue = typeCode;
                deleteUDResolutionbutton.Visible = (typeCode == ResolutionManager.TypeCustom);

                string key = FXtextBox.Text + "x" + FYtextBox.Text;
                resolutioncomboBox.DisplayMember = "Key";
                resolutioncomboBox.ValueMember = "Value";
                resolutioncomboBox.DataSource = resDict.ToList();
                var selectedItem = ResolutionManager.FindItem(resDict, key);
                if (selectedItem.Key != null)
                    resolutioncomboBox.SelectedItem = selectedItem;
                else if (!string.IsNullOrEmpty(key) && resDict.ContainsKey("*" + key))
                {
                    var item = ResolutionManager.FindItem(resDict, "*" + key);
                    if (item.Key != null) resolutioncomboBox.SelectedItem = item;
                }

                if (forceTypeCode != null)
                {
                    if (resolutioncomboBox.SelectedItem == null && resolutioncomboBox.Items.Count > 0)
                        resolutioncomboBox.SelectedIndex = 0;
                    UpdateResolutionFields();
                }

                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
            }
            catch (Exception ex)
            {
                resolutioncomboBox.DataSource = null;
                resolutioncomboBox.Items.Add($"加载失败: {ex.Message}");
                resolutioncomboBox.SelectedIndex = 0;
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckTextBox_KeyPress(object sender, KeyPressEventArgs e)//输入框输入事件绑定
        {
            // 允许输入数字、小数点、Backspace和退出键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                if (e.KeyChar == (char)8 || e.KeyChar == (char)27) // 允许Backspace和退出键
                {
                    // 不做任何操作
                }
                else
                {
                    e.Handled = true; // 不允许输入其他字符
                }
            }
            // 检查是否已经输入了小数点，如果已经输入，则禁止再次输入小数点
            TextBox textBox = sender as TextBox;
            if (textBox != null && e.KeyChar == '.' && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
        //CheckBox部分
        //输入框锁定
        private void FcheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            FXtextBox.ReadOnly = FcheckBox.Checked;
            FYtextBox.ReadOnly = FcheckBox.Checked;
        }   

        private void KcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            KXtextBox.ReadOnly = KcheckBox.Checked;
            KYtextBox.ReadOnly = KcheckBox.Checked;
        }

        private void JScheckBox_CheckedChanged(object sender, EventArgs e)
        {
            JSXtextBox.ReadOnly = JScheckBox.Checked;
            JSYtextBox.ReadOnly = JScheckBox.Checked;
        }
        //输入框锁定结束
        //窗口置顶
        private void TOPcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TOPcheckBox.Checked == true && this.TopMost == false)
            {
                this.TopMost = true;
            }
            else
            if (TOPcheckBox.Checked == false && this.TopMost == true)
            {
                this.TopMost = false;
            }
        }
        //开启/关闭绑定按键的编辑
        private void EcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            FindKeytextBox.ReadOnly = !EcheckBox.Checked;
            ResetKeytextBox.ReadOnly = !EcheckBox.Checked;
        }
        //开启/关闭自动读取坐标
        //区域结束

        //计算部分，文本框被修改时触发计算，检测flag防止冲突，分辨率为从0开始计算，因此计算用分辨率为真实分辨率-1
        //例：1280x720的模拟器分辨率计算坐标需要分别用1279，719做除数，得到的Json坐标才是更精准的
        //经测试，大部分坐标均可完美还原，但仍有部分坐标存在0.1~0.5不等的误差，但无伤大雅。

        //修改窗口分辨率时视锁定情况进行计算
        private void FtextBox_TextChanged(object sender, EventArgs e)
        {
            if (_isProcessingCoordinateChange) return;
            _isProcessingCoordinateChange = true;
            try{
                    //变量初始化
                    if (!SafeParseHelper.TryGetDouble(FXtextBox, out double FX) ||
                        !SafeParseHelper.TryGetDouble(FYtextBox, out double FY))
                    { return; }
                    FX -= 1;
                    FY -= 1;
                    if (FX <= 0 || FY <= 0) return;
                    if (!SafeParseHelper.TryGetDouble(KXtextBox, out double KX) ||
                        !SafeParseHelper.TryGetDouble(KYtextBox, out double KY) ||
                        !SafeParseHelper.TryGetDouble(JSXtextBox, out double JSX) ||
                        !SafeParseHelper.TryGetDouble(JSYtextBox, out double JSY))
                    { return; }
                    if (KcheckBox.Checked && JScheckBox.Checked)
                    {//双坐标均锁定
                        MessageBox.Show(Form1.ActiveForm, "请至少解锁一类需要得到结果的坐标再修改分辨率！");
                    }
                    else
                    if (KcheckBox.Checked)
                    { //开发者模式坐标锁定,计算Json文件坐标
                        JSXtextBox.Text = (KX / FX).ToString(FDP, System.Globalization.CultureInfo.InvariantCulture);
                        JSYtextBox.Text = (KY / FY).ToString(FDP, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    if (JScheckBox.Checked)
                    { //Json文件坐标锁定,计算开发者模式坐标
                        KXtextBox.Text = (JSX * FX).ToString();
                        KYtextBox.Text = (JSY * FY).ToString();
                    }
                    else
                    {
                        //均不锁定则默认计算Json文件坐标
                        JSXtextBox.Text = (KX / FX).ToString(FDP, System.Globalization.CultureInfo.InvariantCulture);
                        JSYtextBox.Text = (KY / FY).ToString(FDP, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    InitializeResolutioncomboBox(resolutioncomboBox);
                
            }
            catch(ArithmeticException ex)
            {
                MessageBox.Show("发生异常："+ex.Message+"请确保您输入的内容为数字。");
            }
            finally
            {
                _isProcessingCoordinateChange = false;
            }
        }

        private void KtextBox_TextChanged(object sender, EventArgs e)//开发者坐标文本更改事件绑定
        {
            if (_isProcessingCoordinateChange) return;
            _isProcessingCoordinateChange = true;
            try
            {
                    //检查空文本框
                    if (CheckEmptyText()) { return; };
                    //变量初始化
                    if (!SafeParseHelper.TryGetDouble(FXtextBox, out double FX) ||
                        !SafeParseHelper.TryGetDouble(FYtextBox, out double FY))
                    { return; }
                    FX -= 1;
                    FY -= 1;
                    if (FX <= 0 || FY <= 0) return;
                    if (!SafeParseHelper.TryGetDouble(KXtextBox, out double KX) ||
                        !SafeParseHelper.TryGetDouble(KYtextBox, out double KY))
                    { return; }
                    //初始化完毕
                    //计算Json文件坐标
                    JSXtextBox.Text = (KX / FX).ToString(FDP, System.Globalization.CultureInfo.InvariantCulture);
                    JSYtextBox.Text = (KY / FY).ToString(FDP, System.Globalization.CultureInfo.InvariantCulture);
                
            }
            catch (ArithmeticException ex)
            {
                MessageBox.Show("发生异常：" + ex.Message + "请确保您输入的内容为数字。");
            }
            finally
            {
                _isProcessingCoordinateChange = false;
            }
        }

        private void JStextBox_TextChanged(object sender, EventArgs e)//Json文件坐标文本更改事件绑定
        {
            if (_isProcessingCoordinateChange) return;
            _isProcessingCoordinateChange = true;
            try
            {
                    //检查空文本框
                    if (CheckEmptyText()) { return; };
                    //变量初始化
                    if (!SafeParseHelper.TryGetDouble(FXtextBox, out double FX) ||
                        !SafeParseHelper.TryGetDouble(FYtextBox, out double FY))
                    { return; }
                    FX -= 1;
                    FY -= 1;
                    if (FX <= 0 || FY <= 0) return;
                    if (!SafeParseHelper.TryGetDouble(JSXtextBox, out double JSX) ||
                        !SafeParseHelper.TryGetDouble(JSYtextBox, out double JSY))
                    { return; }
                    //初始化完毕
                    //计算开发者模式坐标
                    KXtextBox.Text = (JSX * FX).ToString();
                    KYtextBox.Text = (JSY * FY).ToString();
            }
            catch (ArithmeticException ex)
            {
                MessageBox.Show("发生异常：" + ex.Message + "请确保您输入的内容为数字。");
            }
            finally
            {
                _isProcessingCoordinateChange = false;
            }
        }
        //计算部分结束

        //Timer控件实时获取当前鼠标坐标，与保存坐标一致时修改背景色
        private void Ctimer_Tick(object sender, EventArgs e)
        {
            Point mousePosition = Control.MousePosition;
            NCXtextBox.Text = mousePosition.X.ToString();
            NCYtextBox.Text = mousePosition.Y.ToString();
            if (NCXtextBox.Text == SCXtextBox.Text) {
                NCXtextBox.BackColor = Color.Green;
            }
            else
            {
                NCXtextBox.BackColor = Color.White;
            }
            if (NCYtextBox.Text == SCYtextBox.Text)
            {
                NCYtextBox.BackColor = Color.Green;
            }
            else {
                NCYtextBox.BackColor = Color.White;
            }
        }
        //开启坐标捕获，同时注册/取消全局快捷键。
        private void CcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CcheckBox.Checked == true && Ctimer.Enabled == false)
            {
                Ctimer.Enabled = true;
            }
            else
            if (CcheckBox.Checked == false && Ctimer.Enabled == true) {
                Ctimer.Enabled = false;
            }

            if (CcheckBox.Checked)
            {
                _keyboardHandler.RefreshHotKeyRegistration(Handle, true);
            }
            else {
                HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdSave);
                HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdRecall);
            }
        }
        //编写快捷键的功能部分
        protected override void WndProc(ref Message m)
        {
            const int Mu_HOTKEY = 0x0312;

            switch (m.Msg)
            {
                case Mu_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        //保存坐标
                        case KeyboardBindingHandler.HotKeyIdSave:
                            if (CcheckBox.Checked)
                            {
                                SCXtextBox.Text = NCXtextBox.Text;
                                SCYtextBox.Text = NCYtextBox.Text;
                                statusText.Text = "坐标已保存";
                            }
                            break;
                        //回溯鼠标位置
                        case KeyboardBindingHandler.HotKeyIdRecall:
                            if (CcheckBox.Checked)
                            {
                                if (int.TryParse(SCXtextBox.Text, out int scx) &&
                                    int.TryParse(SCYtextBox.Text, out int scy))
                                {
                                    try { MouseSimulator.MoveMouseTo(scx, scy); }
                                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine("鼠标回溯失败：" + ex.Message); }
                                }
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        //测试/检查按钮，详细功能看跳出提示
        private void CheckButton_Click(object sender, EventArgs e)
        {
            try {
                MessageBox.Show("当前绑定按键为：" + _keyboardHandler.BindKey1.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture) + Environment.NewLine + "当前绑定按键值为：" + _keyboardHandler.BindKey1.KeyValue.ToString()+Environment.NewLine);
                if (_mumuJson == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return;
                }
                if (MuMuJsonEditor.FindKey(_mumuJson, _keyboardHandler.BindKey1) == -1)
                {
                    statusText.Text = "按键不存在";
                    MessageBox.Show("当前Json文件中未找到按键" + ButtontextBox.Text);
                }
                else {
                    statusText.Text = "按键已存在";
                    if (MuMuJsonEditor.CheckType(_mumuJson, _keyboardHandler.BindKey1))
                    {
                        if (MuMuJsonEditor.FindType(_mumuJson, _keyboardHandler.BindKey1) == MuMuJsonEditor.typeClick)
                        {
                            MessageBox.Show("按键" + ButtontextBox.Text + "在文件中且是单击按键，可以直接修改。");
                        }
                        else if(MuMuJsonEditor.FindType(_mumuJson, _keyboardHandler.BindKey1) == MuMuJsonEditor.typeMacro)
                        {
                            MessageBox.Show("按键" + ButtontextBox.Text + "在文件中且是宏按键，仅支持对固定格式的宏指牌修改，其余格式的坐标请自行进入Json文件修改。");
                        }
                        else if (MuMuJsonEditor.FindType(_mumuJson, _keyboardHandler.BindKey1) == MuMuJsonEditor.typeBunchClick)
                        {
                            MessageBox.Show("按键" + ButtontextBox.Text + "在文件中且是连击按键，坐标可直接修改。");
                        }
                    }
                    else {
                        string type = MuMuJsonEditor.FindType(_mumuJson, _keyboardHandler.BindKey1);
                        string typeName = string.IsNullOrEmpty(type) ? "未知类型" : type;
                        MessageBox.Show("按键" + ButtontextBox.Text + "在文件中，类型为 " + typeName + "，小助手不支持修改此类按键的坐标。\n请自行打开Json文件手动修改！");
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine($"[CheckButton_Click] 操作失败: {ex.Message}");
                MessageBox.Show("当前未绑定按键，请检查您的设置！");
            }
        }
        //获取Json文件路径，不会真有人用吧，我觉得拖过来更方便啊。
        private async void OpenJson_Click(object sender, EventArgs e)
        {
            try
            {
                _indexCheckTimer.Stop();
                if (!string.IsNullOrEmpty(_config.JsonFolderPath)) { JsonopenFileDialog.InitialDirectory = _config.JsonFolderPath; }
                if (JsonopenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!_isAutoSync) autoSyncCheckBox.Checked = false;
                    if (Undobutton.Enabled || Redobutton.Enabled)
                    {
                        DialogResult result = MessageBox.Show(reloadingTip, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No)
                        {
                            return;//用户选择了取消
                        }
                    }
                    updateJsonUrltextBox(JsonopenFileDialog.FileName);
                    string filePath = @JsonUrltextBox.Text;
                    await LoadJsonFileAsync(filePath);
                    TryUpdateJsonFolderPathFromFile(filePath);
                    LogService.Info("Form1", "通过OpenJson选择文件: " + filePath);
                    searchFileCombo.Text = "";
                    InitializeFileNamecomboBox(searchFileCombo,true);
                }
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "OpenJson_Click");
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //检查拖入窗口的文件，非json文件不接受
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                        break;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None; // 不是文件拖放，不接受
            }
        }
        //读取拖入窗口的json文件，只取第一个
        private async void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                _indexCheckTimer.Stop();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (!_isAutoSync) autoSyncCheckBox.Checked = false;
                updateJsonUrltextBox(files[0]);
                string filePath = @JsonUrltextBox.Text;
                await LoadJsonFileAsync(filePath);
                TryUpdateJsonFolderPathFromFile(filePath);
                LogService.Info("Form1", "拖放加载Json文件: " + filePath);
                searchFileCombo.Text = "";
                InitializeFileNamecomboBox(searchFileCombo,true);
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "Form1_DragDrop");
                MessageBox.Show($"发生错误：{ex.Message}");
            }
            
        }
        //独立创建指定类型按键并写入的代码，便于复用
        private void createAndwriteSetKey() { if (keyTypelistcomboBox.SelectedValue == null) return;
            string[] keyValues = ComboBoxInitializer.PredefinedKeyTypes.Values.ToArray();
            if (keyTypelistcomboBox.SelectedValue.ToString() == keyValues[0])
            {
                //创建点击按键
                string key = MuMuJsonEditor.CreateKey(keyValues[0], _keyboardHandler.BindKey1, JSXtextBox.Text, JSYtextBox.Text, _keyboardHandler.BindKey1ScanCode);
                _mumuJson = MuMuJsonEditor.WriteKeys(key, _mumuJson);
                if (WriteToJsonAndBackup()) { MessageBox.Show($"点击按键{ButtontextBox.Text}生成并写入成功！如出现问题请转人工。"); }
            }
            else if (keyTypelistcomboBox.SelectedValue.ToString() == keyValues[1])
            {
                //创建宏按键
                string key = MuMuJsonEditor.CreateKey(keyValues[1], _keyboardHandler.BindKey1, JSXtextBox.Text, JSYtextBox.Text, _keyboardHandler.BindKey1ScanCode);
                _mumuJson = MuMuJsonEditor.WriteKeys(key, _mumuJson);
                if (WriteToJsonAndBackup()) { MessageBox.Show($"宏指牌按键{ButtontextBox.Text}生成并写入成功！如出现问题请转人工。"); }
            }
            else
            {
                MessageBox.Show("未知错误！我也想知道你是怎么触发这条提示的？？？");
                return;
            }
        }
        //修改并保存按键
        private void RewriteAndSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                //修改前无论如何，重置下拉框
                InitializeFileNamecomboBox(searchFileCombo, true);
                string filePath = @JsonUrltextBox.Text;
                if (_mumuJson == "" || filePath == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return;
                }
                if (_keyboardHandler.BindKey1 == null)
                {
                    MessageBox.Show("请先绑定一个按键！");
                    return;
                }
                if (replaceKeycheckBox.Checked) {
                    DialogResult result = MessageBox.Show($"是否按预设强制替换对应按键？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    if (MuMuJsonEditor.FindKey(_mumuJson, _keyboardHandler.BindKey1) != -1){
                        //文件中存在按键
                        //先删除
                        _mumuJson = MuMuJsonEditor.DeleteKey(_keyboardHandler.BindKey1.KeyValue.ToString(), _mumuJson);
                    }
                    //后写入
                    createAndwriteSetKey();
                    return;
                }
                if (MuMuJsonEditor.FindKey(_mumuJson, _keyboardHandler.BindKey1) == -1)
                {
                    DialogResult result = MessageBox.Show($"当前Json文件中未找到按键{ButtontextBox.Text},是否新建指定类型的按键？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        return;//用户选择了取消
                    }
                    createAndwriteSetKey();
                }
                else
                {
                    if (MuMuJsonEditor.CheckType(_mumuJson, _keyboardHandler.BindKey1))
                    {
                        try
                        {
                            string modified = MuMuJsonEditor.ReKey(_mumuJson, _keyboardHandler.BindKey1, JSXtextBox.Text, JSYtextBox.Text);
                            if (modified == null)
                            {
                                MessageBox.Show("此宏按键不含 curve_rel:mouse 格式的坐标指令，无法通过小助手修改。\n请自行打开Json文件手动修改坐标！");
                                return;
                            }
                            _mumuJson = modified;
                            if (WriteToJsonAndBackup()) { MessageBox.Show($"按键{ButtontextBox.Text}修改成功并保存，如出现bug请自行打开Json文件手动修改坐标！"); }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("按键修改失败！" + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("按键" + ButtontextBox.Text + "在文件中但不是单击按键或指定的宏指牌按键，为防止bug，请自行打开Json文件手动修改！");
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "RewriteAndSaveButton_Click");
                MessageBox.Show($"发生错误：{ex.Message}");
                return;
            }
            


        }
        //文件被外部修改时的回调（替代 CheckFileChangetimer 轮询）
        private async void OnExternalFileChanged()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (IsDisposed || Disposing) return;
                    OnExternalFileChanged();
                }));
                return;
            }
            try
            {
                if (_isAutoSync) return;
                _suppressIndexCheck = true;
                string filePath = @JsonUrltextBox.Text;
                if (!File.Exists(filePath)) return;
                if (File.GetLastWriteTimeUtc(filePath) == lastWriteTime) return;
                DialogResult result = MessageBox.Show("检测到Json文件被其他程序修改，是否重新加载？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    await LoadJsonFileAsync(filePath);
                }
                else
                {
                    lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                }
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "OnExternalFileChanged");
                MessageBox.Show(string.Format("发生错误：{0}", ex.Message));
            }
        }
        //点击跳转到作者B站主页(<ゝω·)~☆kira
        //读取指定按键的坐标
        private void ReadPPButton_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("当前绑定按键为：" + _keyboardHandler.BindKey1.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture) + Environment.NewLine + "当前绑定按键值为：" + _keyboardHandler.BindKey1.KeyValue.ToString() + Environment.NewLine);
                if (_mumuJson == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return;
                }
                if (MuMuJsonEditor.FindKey(_mumuJson, _keyboardHandler.BindKey1) == -1)
                {
                    MessageBox.Show("当前Json文件中未找到按键" + ButtontextBox.Text);
                }
                else
                {
                    string[] key = MuMuJsonEditor.ReadKeyPP(_mumuJson, _keyboardHandler.BindKey1);
                    if (key == null) { MessageBox.Show("查找坐标失败，请检查您指定的按键中是否有坐标存在！"); return; }
                    JSXtextBox.Text = key[0];
                    JSYtextBox.Text = key[1];
                }
            }
            catch
            {
                MessageBox.Show("当前未绑定按键，请检查您的设置！");
            }
        }
        //将基础键位导入其他键位中
        private void WriteKeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                //修改前无论如何，重置下拉框
                InitializeFileNamecomboBox(searchFileCombo, true);
                if (!WriteKeysCheck()) { return; }
                string[] text = MuMuJsonEditor.FindKeyValues(searchKeysCombo.SelectedValue.ToString());
                if (!MuMuJsonEditor.AreAllKeysMissing(text, _mumuJson)){
                    //按键重复
                    DialogResult result = MessageBox.Show("检测到待写入Json文件存在重复按键，是否继续写入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes){}
                    else
                    {
                        string[] keyText = MuMuJsonEditor.FindKeyTexts(searchKeysCombo.SelectedValue.ToString());
                        string[] repeatKeyText = MuMuJsonEditor.FindAllRepeatKeyTexts(keyText, _mumuJson);
                        string messageKey = string.Join(",", repeatKeyText);
                MessageBox.Show("存在重复按键:" + messageKey + "\n请修改待写入的按键文件后再操作！");
                        return;
                    }
                }
                string keys = MuMuJsonEditor.ReadKeys(searchKeysCombo.SelectedValue.ToString());
                _mumuJson = MuMuJsonEditor.WriteKeys(keys, _mumuJson);
                if (WriteToJsonAndBackup()) { MessageBox.Show("基础键位注入成功！如出现问题请转人工。"); }
            }
            catch(Exception ex) {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //按键去重
        private void DeleteRepeatKeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) { return; }
                string[] text = MuMuJsonEditor.FindKeyValues(searchKeysCombo.SelectedValue.ToString());
                if (MuMuJsonEditor.AreAllKeysMissing(text, _mumuJson))
                {
                    //键位无重复
                    MessageBox.Show("无重复键位，可执行基础键位注入。");
                    return;
                }
                DialogResult result = MessageBox.Show("去重功能存在风险，使用前请确保重复的按键中不存在你要保留的按键。", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { }
                else { return; }
                while (!MuMuJsonEditor.AreAllKeysMissing(text, _mumuJson))
                {
                    //存在重复键位，执行去重
                    string[] keyValue = MuMuJsonEditor.FindKeyValues(searchKeysCombo.SelectedValue.ToString());
                    string[] repeatKeyValues = MuMuJsonEditor.FindAllRepeatKeyValues(keyValue, _mumuJson);
                    _mumuJson = MuMuJsonEditor.DeleteKeys(repeatKeyValues, _mumuJson);
                }
                if (MuMuJsonEditor.AreAllKeysMissing(text, _mumuJson))
                {
                    //键位无重复
                    if (WriteToJsonAndBackup()) { MessageBox.Show("已清除所有重复键位，可执行基础键位注入。"); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //删除对应区域的按键
        private void DeleteRangeRDkeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) { return; }
                DialogResult result = MessageBox.Show("右下区域清空功能存在风险，且当前功能仅支持16：9分辨率的键位文件。\n使用前请确认键位适配分辨率且右下角选牌区不存在要保留的按键！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { }
                else { return; }
                double[] rangeLT = { 0.661, 0.798 };
                double[] rangeRD = { 1.0, 1.0 };
                var results = new List<(double, double, string, string)>();
                results = MuMuJsonEditor.FindRangeKeyValues(rangeLT, rangeRD, _mumuJson);
                if (results.Count == 0) { MessageBox.Show($"右下选牌区域中不存在按键，无需清空。");return; }
                string messageKeyTexts = "";
                foreach (var (x, y, text, vk) in results)
                {
                    _mumuJson = MuMuJsonEditor.DeleteKey(vk, _mumuJson);
                    messageKeyTexts += text+",";
                }
                if (WriteToJsonAndBackup()) { MessageBox.Show($"已清空：{messageKeyTexts}键，如出现问题请转人工！"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
            
        }
        //清除指定区域所有键位
        private void DeleteRangeLTRDkeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) { return; }
                DialogResult result = MessageBox.Show("区域清空功能存在风险，使用前请确认选择区域不存在要保留的按键！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { }
                else { return; }
                //变量初始化
                if (!SafeParseHelper.TryGetDouble(FXtextBox, out double FX) ||
                    !SafeParseHelper.TryGetDouble(FYtextBox, out double FY))
                { return; }
                FX -= 1;
                FY -= 1;
                if (FX <= 0 || FY <= 0) return;
                if (!SafeParseHelper.TryGetDouble(RangeLTXtextBox, out double LTX) ||
                    !SafeParseHelper.TryGetDouble(RangeLTYtextBox, out double LTY) ||
                    !SafeParseHelper.TryGetDouble(RangeRDXtextBox, out double RDX) ||
                    !SafeParseHelper.TryGetDouble(RangeRDYtextBox, out double RDY))
                { return; }
                double[] rangeLT = { (LTX / FX),(LTY / FY) };
                double[] rangeRD = { (RDX / FX),(RDY / FY) };
                var results = new List<(double, double, string, string)>();
                results = MuMuJsonEditor.FindRangeKeyValues(rangeLT, rangeRD, _mumuJson);
                if (results.Count == 0) { MessageBox.Show($"选中区域不存在按键，无需清空。"); return; }
                var keyTexts = new System.Collections.Generic.List<string>();
                foreach (var (x, y, text, vk) in results)
                {
                    _mumuJson = MuMuJsonEditor.DeleteKey(vk, _mumuJson);
                    keyTexts.Add(text);
                }
                if (WriteToJsonAndBackup()) { MessageBox.Show($"已清空：{string.Join(",", keyTexts)}键，如出现问题请转人工！"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}\n请检查您的输入内容！");
            }
        }
        //读取下拉框中文件的按键坐标
        private async void ReadPP2Button_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取程序所在目录的"data"子文件夹路径
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string dataFolder = Path.Combine(baseDir, "data");
                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    MessageBox.Show("程序目录下无“data”文件夹，请检查您的配置文件！");
                    return;
                }
                //检查文件夹下是否有json文件
                if (Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly).Length == 0)
                {
                    MessageBox.Show("“data”文件夹中无json文件，请检查您的配置文件！");
                    return;
                }
                //检查是否删除json文件后又还原但是未重新加载列表（真有人能无聊到触发这个bug吗？？？）
                if (searchKeysCombo.SelectedValue == null 
                    || searchKeysCombo.SelectedItem?.ToString() == "数据目录不存在"
                    || searchKeysCombo.SelectedItem?.ToString() == "未找到符合条件的文件！")
                {
                    MessageBox.Show("请重新选择你的基础键位！");
                    return;
                }
                string myJson = await Task.Run(() => File.ReadAllText(searchKeysCombo.SelectedValue.ToString()));
                MessageBox.Show("当前绑定按键为：" + _keyboardHandler.BindKey2.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture) + Environment.NewLine + "当前绑定按键值为：" + _keyboardHandler.BindKey2.KeyValue.ToString() + Environment.NewLine);
                if (MuMuJsonEditor.FindKey(myJson, _keyboardHandler.BindKey2) == -1)
                {
                    MessageBox.Show("当前Json文件中未找到按键" + Button2textBox.Text);
                }
                else
                {
                    string[] key = MuMuJsonEditor.ReadKeyPP(myJson, _keyboardHandler.BindKey2);
                    if (key == null) { MessageBox.Show("查找坐标失败，请检查您指定的按键中是否有坐标存在！"); return; }
                    JSXtextBox.Text = key[0];
                    JSYtextBox.Text = key[1];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ReadPP2Button_Click] 操作失败: {ex.Message}");
                MessageBox.Show("当前未绑定按键，请检查您的设置！");
            }
        }

        private void WriteKeyButton_Click(object sender, EventArgs e)
        {
            try
            {
                //修改前无论如何，重置下拉框
                InitializeFileNamecomboBox(searchFileCombo, true);
                if (!WriteKeysCheck()) { return; }
                if (_keyboardHandler.BindKey2 == null) { MessageBox.Show("当前未绑定按键，请检查您的设置！");return; }
                string[] text = { _keyboardHandler.BindKey2.KeyValue.ToString() };
                if (!MuMuJsonEditor.AreAllKeysMissing(text, _mumuJson)){
                    //按键重复
                    DialogResult result = MessageBox.Show("检测到待写入Json文件存在重复按键，是否继续写入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes){}
                    else
                    {
                        MessageBox.Show("存在重复按键:" + _keyboardHandler.BindKey2.KeyData + "\n请修改待写入的按键文件后再操作！");
                        return;
                    }
                }
                string key = MuMuJsonEditor.ReadKey(searchKeysCombo.SelectedValue.ToString(),_keyboardHandler.BindKey2);
                _mumuJson = MuMuJsonEditor.WriteKeys(key, _mumuJson);
                if (WriteToJsonAndBackup()) { MessageBox.Show("单键位注入成功！如出现问题请转人工。"); }
            }
            catch(Exception ex) {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        private void Undobutton_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeFileNamecomboBox(searchFileCombo, false);
                LogService.Info("Form1", string.Format("Undobutton_Click: CanUndo={0}, _mumuJson长度={1}", Undobutton.Enabled, _mumuJson.Length));
                var restored = _backupManager.Undo();
                if (restored == null)
                {
                    LogService.Warn("Form1", "Undobutton_Click: Undo()返回null");
                    return;
                }
                _mumuJson = restored;
                var writeResult = WriteToJson();
                SetUndobtnAndRedobtnState();
                LogService.Info("Form1", string.Format("执行撤销操作: 写入结果={0}, 恢复后JSON长度={1}, CanUndo={2}, CanRedo={3}",
                    writeResult, restored.Length, Undobutton.Enabled, Redobutton.Enabled));
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "Undobutton_Click");
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        private void Redobutton_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeFileNamecomboBox(searchFileCombo, false);
                LogService.Info("Form1", string.Format("Redobutton_Click: CanRedo={0}, _mumuJson长度={1}", Redobutton.Enabled, _mumuJson.Length));
                var restored = _backupManager.Redo();
                if (restored == null)
                {
                    LogService.Warn("Form1", "Redobutton_Click: Redo()返回null");
                    return;
                }
                _mumuJson = restored;
                var writeResult = WriteToJson();
                SetUndobtnAndRedobtnState();
                LogService.Info("Form1", string.Format("执行重做操作: 写入结果={0}, 恢复后JSON长度={1}, CanUndo={2}, CanRedo={3}",
                    writeResult, restored.Length, Undobutton.Enabled, Redobutton.Enabled));
            }
            catch (Exception ex)
            {
                LogService.Error("Form1", ex, "Redobutton_Click");
                MessageBox.Show(string.Format("发生错误：{0}", ex.Message));
            }
        }
        //打开拖入json文件文件夹
        private void OpenJsonFolderbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(_config.JsonFolderPath))
                {
                    using (Process.Start(_config.JsonFolderPath)) { }
                }
                else
                {
                    MessageBox.Show("文件夹不存在！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无效路径！发生错误：{ex.Message}");
            }
        }
        //打开预设键位文件夹
        //更新文件地址
        private void updateJsonUrltextBox(string jsonFilePath) {
            try
            {
                JsonUrltextBox.Text = jsonFilePath;
                JsonUrltextBox.TextAlign = HorizontalAlignment.Right;
                JsonUrltextBox.SelectionStart = JsonUrltextBox.TextLength;
                JsonUrltextBox.ScrollToCaret();
                if (File.Exists(jsonFilePath)) {
                    lastWriteTime = File.GetLastWriteTimeUtc(jsonFilePath);
                    _fileMonitor.Watch(jsonFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }




        private void Ktimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!CcheckBox.Checked) { Ktimer.Stop(); return; }
                var pos = MouseSimulator.GetCursorPos();
                mXtextBox.Text = pos.X.ToString();
                mYtextBox.Text = pos.Y.ToString();
            }
            catch (Exception ex)
            {
                Ktimer.Stop();
                Debug.WriteLine(ex.Message);
            }
        }


        private void _indexCheckTimer_Tick(object sender, EventArgs e)
        {
            _indexCheckTimer.Stop();
            if (_suppressIndexCheck) { _suppressIndexCheck = false; return; }
            if (!string.IsNullOrEmpty(_pendingCheckSchemePath))
            {
                _indexBackup.CheckAndNotifyDamage(_pendingCheckSchemePath, JsonUrltextBox.Text, ref _indexDamageNotified, statusText, ref _suppressIndexCheck);
            }
        }

        private void _indexBackupTimer_Tick(object sender, EventArgs e)
        {
            if (!_needsIndexBackup || string.IsNullOrEmpty(_pendingBackupPath)) return;
            var path = _pendingBackupPath;
            _needsIndexBackup = false;
            Task.Run(() =>
            {
                try { _indexBackup.BackupIndex(path); } catch { }
            });
        }


        private void btnGetScreenResolution_Click(object sender, EventArgs e)
        {
            try { var screen = Screen.PrimaryScreen; SXtextBox.Text = screen.Bounds.Width.ToString(); SYtextBox.Text = screen.Bounds.Height.ToString(); }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void ktckReadbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_mumuJson)) { MessageBox.Show("请先加载一个Json文件！"); return; }
                var keys = MuMuJsonEditor.ReadAllKeys(_mumuJson);
                ktckPListcheckedListBox.Items.Clear();
                ktckPListcheckedListBox.Items.Add("全选");
                foreach (var key in keys) ktckPListcheckedListBox.Items.Add(key);
            }
            catch (Exception ex) { MessageBox.Show($"发生错误：{ex.Message}"); }
        }

        private void ktckOPWritebutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_mumuJson)) { MessageBox.Show("请先加载一个Json文件！"); return; }
                if (string.IsNullOrWhiteSpace(ktckKXtextBox.Text) || string.IsNullOrWhiteSpace(ktckKYtextBox.Text)) return;
                string key = MuMuJsonEditor.CreateKey("Click", new KeyEventArgs(Keys.None), ktckKXtextBox.Text, ktckKYtextBox.Text, MuMuJsonEditor.GetKeyText(ktckPListcheckedListBox));
                _mumuJson = MuMuJsonEditor.WriteKeys(key, _mumuJson);
                WriteToJsonAndBackup();
            }
            catch (Exception ex) { MessageBox.Show($"发生错误：{ex.Message}"); }
        }

        private void ktckAPWritebutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_mumuJson)) { MessageBox.Show("请先加载一个Json文件！"); return; }
                var selectedItems = ktckPListcheckedListBox.CheckedItems;
                if (selectedItems.Count == 0 || (selectedItems.Count == 1 && selectedItems[0].ToString() == "全选")) return;
                foreach (var item in selectedItems)
                {
                    string key = MuMuJsonEditor.CreateKey("Click", new KeyEventArgs(Keys.None), ktckKXtextBox.Text, ktckKYtextBox.Text, MuMuJsonEditor.GetKeyText(item.ToString()));
                    _mumuJson = MuMuJsonEditor.WriteKeys(key, _mumuJson);
                }
                WriteToJsonAndBackup();
            }
            catch (Exception ex) { MessageBox.Show($"发生错误：{ex.Message}"); }
        }

        private async void TryGetJsonFileFolderbutton_Click(object sender, EventArgs e)
        {
            try { await TryGetJsonFileFolder(); }
            catch (Exception ex) { MessageBox.Show($"发生错误：{ex.Message}"); }
        }

        private void resolutioncomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (flagFlushingResolution) { UpdateResolutionFields(); }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void deleteUDResolutionbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string key = FXtextBox.Text + "x" + FYtextBox.Text;
                var resDict = MuMuJsonEditor.StringToResolution(_config.Resolution4String);
                if (resDict.ContainsKey("*" + key)) resDict.Remove("*" + key);
                _config.Resolution4String = MuMuJsonEditor.ResolutionToString(resDict);
                InitializeResolutioncomboBox(resolutioncomboBox);
                deleteUDResolutionbutton.Visible = false;
            }
            catch (Exception ex) { MessageBox.Show($"发生错误：{ex.Message}"); }
        }
        private async Task LoadJsonFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) { LogService.Warn("Form1", "文件不存在: " + filePath); MessageBox.Show("文件不存在！"); return; }
            _mumuJson = await Task.Run(() => File.ReadAllText(filePath));
            lastWriteTime = File.GetLastWriteTimeUtc(filePath);
            _fileMonitor.Watch(filePath);
            BackupAfterJsonReading();
            statusText.Text = "加载成功";
        }

        private static string GetValidatedJsonPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (!File.Exists(path)) return null;
            return path;
        }

        private void OpenAdbTouchForm()
        {
            LogService.Info("Form1", "打开ADB触摸采集窗口");
            ShowChildForm(new AdbTouchForm(
                _config.AdbPort,
                _config.AdbPath,
                _config.AdbPortsHistory
            ));
        }

#pragma warning disable CS1998
        private async Task TryGetJsonFileFolder()
        {
            string keymapPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @"AppData\Roaming\Netease\MuMuPlayer\data\keymapConfig");
            if (Directory.Exists(keymapPath))
            {
                _config.JsonFolderPath = keymapPath;
                statusText.Text = "已自动获取MuMu键位配置目录";
                return;
            }
            string altPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @"AppData\Roaming\Netease\MuMuPlayer\data\keymapConfig");
            if (Directory.Exists(altPath))
            {
                _config.JsonFolderPath = altPath;
                statusText.Text = "已自动获取MuMu键位配置目录";
            }
        }
#pragma warning restore CS1998

        private void TryUpdateJsonFolderPathFromFile(string filePath)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                {
                    if (!string.Equals(_config.JsonFolderPath, dir, StringComparison.OrdinalIgnoreCase))
                    {
                        LogService.Info("Form1", "键位配置目录已更新: " + dir);
                        _config.JsonFolderPath = dir;
                        _syncService.Start(dir);
                        _syncService.Enable();
                    }
                }
            }
            catch { }
        }

        private void UpdateResolutionFields()
        {
            if (resolutioncomboBox.SelectedValue == null) return;
            string[] Pvalue = resolutioncomboBox.SelectedValue.ToString().Split(',');
            if (Pvalue.Length >= 2)
            {
                if (int.TryParse(Pvalue[0], out int w) && int.TryParse(Pvalue[1], out int h))
                {
                    FXtextBox.Text = w.ToString();
                    FYtextBox.Text = h.ToString();
                }
            }
        }

        public void SetResolution(int w, int h)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int, int>(SetResolution), w, h);
                return;
            }
            FXtextBox.Text = w.ToString();
            FYtextBox.Text = h.ToString();
        }

        public void SetJSXY(string x, string y)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(SetJSXY), x, y);
                return;
            }
            JSXtextBox.Text = x;
            JSYtextBox.Text = y;
        }

        public void SaveAdbConfig(string port, string path)
        {
            _config.AdbPort = port;
            _config.AdbPath = path;
        }

        public void SaveAdbPortsHistory(string history)
        {
            _config.AdbPortsHistory = history;
        }

        private void AddFeatureButtons()
        {

            _featureToolTip.SetToolTip(featureBtnMouse, "实时追踪鼠标坐标，支持 Ctrl+X 复制，并自动写入当前坐标。");
            featureBtnMouse.Click += (s, e) => ShowChildForm(new MouseTrackForm(_keyboardHandler, _config) { StatusCallback = msg => statusText.Text = msg });
            _featureToolTip.SetToolTip(featureBtnRange, "自定义矩形区域内一键清空所有键位。");
            featureBtnRange.Click += (s, e) => ShowChildForm(new RangeClearForm(
                () => _mumuJson,
                () => double.TryParse(FXtextBox.Text, out double xr) ? xr : 0,
                () => double.TryParse(FYtextBox.Text, out double yr) ? yr : 0,
                m => { _mumuJson = m; return WriteToJsonAndBackup(); }
            ));
            _featureToolTip.SetToolTip(featureBtnTouch, "F11全屏化模拟器后，一键批量采集屏幕上所有按钮的位置和键值。");
            featureBtnTouch.Click += (s, e) => ShowChildForm(new QuickTouchForm(
                _keyboardHandler,
                () => _mumuJson,
                () => double.TryParse(FXtextBox.Text, out double xq) ? xq : 0,
                () => double.TryParse(FYtextBox.Text, out double yq) ? yq : 0,
                () => {
                    InitializeFileNamecomboBox(searchFileCombo, true);
                    if (_mumuJson == "" || JsonUrltextBox.Text == "")
                    { MessageBox.Show("请先加载一个Json文件！"); return false; }
                    return true;
                },
                (key, x, y) => CreateKeyFromQuickTouch(key, x, y)
            ));
            _featureToolTip.SetToolTip(featureBtnWide, "从异形分辨率方案文件中读取预置位点，并转换为当前分辨率坐标。");
            featureBtnWide.Click += (s, e) => ShowChildForm(new WideScreenForm(
                () => _mumuJson, m => { _mumuJson = m; return WriteToJsonAndBackup(); },
                () => double.TryParse(FXtextBox.Text, out double x) ? x : 0,
                () => double.TryParse(FYtextBox.Text, out double y) ? y : 0,
                _resolutionManager, _config, PackageNameTypes, resolutionTypes, pkg => _config.JsonFolderPath
            ));
            _featureToolTip.SetToolTip(featureBtnBackup, "管理按键方案索引文件的备份，支持手动备份、还原和清理旧备份。");
            featureBtnBackup.Click += (s, e) => ShowChildForm(new IndexBackupForm(
                _indexBackup, PackageNameTypes, () => JsonUrltextBox.Text,
                () => packageNamecomboBox.SelectedValue?.ToString() ?? "",
                t => statusText.Text = t,
                (t, c2) => { statusText.Text = t; statusText.ForeColor = c2; },
                () => { statusText.Text = "就绪"; statusText.ForeColor = System.Drawing.SystemColors.ControlText; }
            ));

            _featureToolTip.SetToolTip(featureBtnLog, "查看和管理程序运行日志，方便反馈bug时提供详情。");
            featureBtnLog.Click += (s, e) => ShowChildForm(new LogViewerForm());

            SetupKeyPresetFeature();
        }
        async void OnMuMuKeymapDetected(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (Interlocked.CompareExchange(ref _syncGuard, 1, 0) != 0)
            {
                _pendingSyncPath = filePath;
                Debug.WriteLine("[AutoSync] 忙碌中, 积压待处理: " + filePath);
                return;
            }

            while (true)
            {
                string currentPath = _pendingSyncPath ?? filePath;
                _pendingSyncPath = null;

                try
                {
                    currentPath = currentPath.Replace('/', '\\');
                    if (currentPath == JsonUrltextBox.Text) goto next;

                    if (Undobutton.Enabled || Redobutton.Enabled)
                    {
                        DialogResult r = MessageBox.Show(reloadingTip, "警告",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (r == DialogResult.No) goto next;
                    }

                    _isAutoSync = true;
                    updateJsonUrltextBox(currentPath);

                    string matchedPkg = MatchPackageFromPath(currentPath);
                    if (packageNamecomboBox.SelectedValue?.ToString() != matchedPkg)
                        packageNamecomboBox.SelectedValue = matchedPkg;

                    InitializeFileNamecomboBox(searchFileCombo, false);
                    if (searchFileCombo.SelectedValue?.ToString() != currentPath)
                        goto next;

                    await LoadJsonFileAsync(currentPath);
                    statusText.Text = "已自动同步：" + Path.GetFileNameWithoutExtension(currentPath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[AutoSync] 同步失败: " + ex.Message);
                }
                finally
                {
                    _isAutoSync = false;
                }

            next:
                if (_pendingSyncPath == null)
                {
                    Interlocked.Exchange(ref _syncGuard, 0);
                    break;
                }
                Debug.WriteLine("[AutoSync] 处理积压的同步: " + _pendingSyncPath);
            }
        }

        private void TryAutoSyncFromEmulatorKeymap()
        {
            if (!autoSyncCheckBox.Checked || _syncService == null) return;

            string bestPath = null;
            DateTime bestTime = DateTime.MinValue;

            foreach (var pkg in PackageNameTypes)
            {
                if (pkg.Value == "other" || pkg.Value == "萌新666sssaaa") continue;
                string pkgStripped = pkg.Value.TrimEnd('-');
                string indexPath = Path.Combine(_config.JsonFolderPath, pkgStripped + ".json");
                if (!File.Exists(indexPath)) continue;
                string currentPath = MuMuKeymapSyncService.ReadCurrentPath(indexPath);
                if (currentPath == null || !File.Exists(currentPath))
                    continue;

                DateTime writeTime = File.GetLastWriteTime(indexPath);
                if (writeTime > bestTime)
                {
                    bestTime = writeTime;
                    bestPath = currentPath;
                }
            }

            if (bestPath != null && bestPath != JsonUrltextBox.Text)
                OnMuMuKeymapDetected(bestPath);
        }

        private string MatchPackageFromPath(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            return PackageNameTypes.Values
                .Where(v => v != "other" && v != "萌新666sssaaa")
                .OrderByDescending(v => v.Length)
                .FirstOrDefault(v => fileName.StartsWith(v.TrimEnd('-')))
                ?? "other";
        }
    }
}
