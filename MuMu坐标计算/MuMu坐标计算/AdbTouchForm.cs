using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace MuMu坐标计算
{
    public partial class AdbTouchForm : Form
    {
        private AdbClient _adbClient;
        private TouchCollector _touchCollector;
        private string _configAdbPath;
        private bool _shownResolutionMsg;

        private TouchCoordinate _lastCapturedCoord;
        private bool _hasLastCoord;
        private KeyboardBindingHandler _keyboardHandler;
        private bool _isGenerateHandling;
        private bool _isGenerateSelectedMode;
        private bool _isSyncing;
        private bool _isDetecting;
        private List<MuMuPortItem> _cachedInstances;

        private void InitListViewColumns()
        {
            lvTouchCoords.Columns.Add("序号", 40);
            lvTouchCoords.Columns.Add("相对X", 85);
            lvTouchCoords.Columns.Add("相对Y", 85);
            lvTouchCoords.Columns.Add("点击数", 55);
            lvTouchCoords.Columns.Add("屏幕X", 65);
            lvTouchCoords.Columns.Add("屏幕Y", 65);
        }

        public AdbTouchForm(string adbPort, string adbPath, string portsHistory)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;
            Font = new Font("Microsoft YaHei", 9F);
            cmbAdbPort.DropDownWidth = 250;
            cmbAdbPort.SelectedIndexChanged += CmbAdbPort_SelectedIndexChanged;
            txtPortInput.TextChanged += TxtPortInput_TextChanged;
            InitListViewColumns();
            _topCheckBox.CheckedChanged += TopCheckBox_CheckedChanged;
            LoadPortHistory(portsHistory, adbPort);
            lblAdbStatus.Text = "未连接";

            if (!string.IsNullOrEmpty(adbPath))
            {
                txtAdbPath.Text = adbPath;
                _configAdbPath = adbPath;
            }

            SetupKeyboardEvents();
        }

        private void SetupKeyboardEvents()
        {
            _keyboardHandler = new KeyboardBindingHandler();
            _keyboardHandler.KeyCapturedOnce += OnKeyCapturedGenerateOnce;
            _keyboardHandler.KeyCapturedContinuously += OnKeyCapturedGenerateContinuously;

            _generateOnceCheckBox.CheckedChanged += GenerateOnceCheckBox_CheckedChanged;
            _generateMultipleCheckBox.CheckedChanged += GenerateMultipleCheckBox_CheckedChanged;

            _keyTypeComboBox.DisplayMember = "Key";
            _keyTypeComboBox.ValueMember = "Value";
            _keyTypeComboBox.DataSource = ComboBoxInitializer.PredefinedKeyTypes.ToList();

            SyncKeyTypeFromMainForm();

            _keyTypeComboBox.SelectedIndexChanged += KeyTypeComboBox_SelectedIndexChanged;

            _generateSelectedCheckBox.CheckedChanged += GenerateSelectedCheckBox_CheckedChanged;

            lvTouchCoords.SelectedIndexChanged += LvTouchCoords_SelectedIndexChanged;
        }

        private void GenerateOnceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_isGenerateHandling) return;
            _isGenerateHandling = true;
            try
            {
                if (_generateOnceCheckBox.Checked)
                {
                    _generateMultipleCheckBox.Checked = false;
                    _generateSelectedCheckBox.Checked = false;
                    _isGenerateSelectedMode = false;
                    SyncKeyTypeFromMainForm();
                    _keyboardHandler.SetListenOnceMode(true);
                    _lblGenerateTip.Text = "提示：已开启键盘监听（单次）";
                    if (!_hasLastCoord)
                        _lblGenerateTip.Text = "提示：已开启键盘监听（单次），请先捕获坐标";
                }
                else
                {
                    _keyboardHandler.StopAllListening();
                    _lblGenerateTip.Text = "提示：已关闭生成功能";
                }
            }
            finally { _isGenerateHandling = false; }
        }

        private void GenerateMultipleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_isGenerateHandling) return;
            _isGenerateHandling = true;
            try
            {
                if (_generateMultipleCheckBox.Checked)
                {
                    _generateOnceCheckBox.Checked = false;
                    _generateSelectedCheckBox.Checked = false;
                    _isGenerateSelectedMode = false;
                    SyncKeyTypeFromMainForm();
                    _keyboardHandler.SetListenContinuouslyMode(true);
                    _lblGenerateTip.Text = "提示：已开启键盘监听（连续）";
                    if (!_hasLastCoord)
                        _lblGenerateTip.Text = "提示：已开启键盘监听（连续），请先捕获坐标";
                }
                else
                {
                    _keyboardHandler.StopAllListening();
                    _lblGenerateTip.Text = "提示：已关闭生成功能";
                }
            }
            finally { _isGenerateHandling = false; }
        }

        private void GenerateSelectedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_isGenerateHandling) return;
            _isGenerateHandling = true;
            try
            {
                if (_generateSelectedCheckBox.Checked)
                {
                    _generateOnceCheckBox.Checked = false;
                    _generateMultipleCheckBox.Checked = false;
                    _keyboardHandler.SetListenContinuouslyMode(true);
                    _isGenerateSelectedMode = true;
                    SyncKeyTypeFromMainForm();
                    UpdateGenerateSelectedTip();
                }
                else
                {
                    _keyboardHandler.StopAllListening();
                    _isGenerateSelectedMode = false;
                    _lblGenerateTip.Text = "提示：已关闭生成功能";
                }
            }
            finally { _isGenerateHandling = false; }
        }

        private void LvTouchCoords_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isGenerateSelectedMode)
                UpdateGenerateSelectedTip();
        }

        private void UpdateGenerateSelectedTip()
        {
            if (lvTouchCoords.SelectedItems.Count > 0)
                _lblGenerateTip.Text = "提示：已开启键盘监听（生成选中）";
            else
                _lblGenerateTip.Text = "提示：已开启键盘监听（生成选中），请选中坐标行后按键";
        }

        private void KeyTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm != null)
                mainForm.SetCurrentKeyType(_keyTypeComboBox.SelectedValue?.ToString() ?? "");
        }

        public void SyncKeyTypeFromMainForm()
        {
            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm == null) return;
            string keyType = mainForm.GetCurrentKeyType();
            if (!string.IsNullOrEmpty(keyType))
                _keyTypeComboBox.SelectedValue = keyType;
        }

        private void OnKeyCapturedGenerateOnce(object sender, KeyEventArgs key)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnKeyCapturedGenerateOnce(sender, key)));
                return;
            }
            ExecuteGenerateOnce(key);
        }

        private void OnKeyCapturedGenerateContinuously(object sender, KeyEventArgs key)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnKeyCapturedGenerateContinuously(sender, key)));
                return;
            }
            if (_isGenerateSelectedMode)
                ExecuteGenerateSelected(key);
            else
                ExecuteGenerateContinuously(key);
        }

        private void ExecuteGenerateOnce(KeyEventArgs key)
        {
            if (!_hasLastCoord)
            {
                _lblGenerateTip.Text = "提示：请先捕获坐标！";
                return;
            }
            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm == null) return;

            string myJson = mainForm.GetMuMuJson();
            if (string.IsNullOrEmpty(myJson))
            {
                _lblGenerateTip.Text = "提示：主窗口未加载Json文件！";
                _generateOnceCheckBox.Checked = false;
                return;
            }

            string keyType = _keyTypeComboBox.SelectedValue?.ToString() ?? "Click";
            string selectedType = keyType == MuMuJsonEditor.typeClick ? MuMuJsonEditor.typeClick : MuMuJsonEditor.typeMacro;
            string typeChinese = selectedType == "Click" ? "单击" : "宏指牌";

            if (IsExactDuplicate(myJson, key, selectedType, _lastCapturedCoord.RelX, _lastCapturedCoord.RelY))
            {
                _lblGenerateTip.Text = "已存在：" + typeChinese + "按键" + key.KeyCode + "（坐标、按键、类型完全一致）";
                _generateOnceCheckBox.Checked = false;
                return;
            }

            bool hasNameConflictOnce = MuMuJsonEditor.FindKey(myJson, key) != -1;

            string scanCode = MuMuJsonEditor.GetScanCode(key.KeyCode).ToString();
            string mkey = MuMuJsonEditor.CreateKey(selectedType, key,
                _lastCapturedCoord.RelX.ToString("F16", CultureInfo.InvariantCulture),
                _lastCapturedCoord.RelY.ToString("F16", CultureInfo.InvariantCulture),
                scanCode);
            mainForm.SetMuMuJson(MuMuJsonEditor.WriteKeys(mkey, myJson));
            mainForm.SaveJsonAndBackup();
            _generateOnceCheckBox.Checked = false;

            if (hasNameConflictOnce)
                _lblGenerateTip.Text = "已生成：" + typeChinese + "按键" + key.KeyCode + "（同名按键已存在其他模板，模拟器中会冲突请自行修改）";
            else
                _lblGenerateTip.Text = "已生成：" + typeChinese + "按键" + key.KeyCode.ToString();
        }

        private void ExecuteGenerateContinuously(KeyEventArgs key)
        {
            if (!_hasLastCoord)
            {
                _lblGenerateTip.Text = "提示：请先捕获坐标！";
                return;
            }
            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm == null) return;

            string myJson = mainForm.GetMuMuJson();
            if (string.IsNullOrEmpty(myJson))
            {
                _lblGenerateTip.Text = "提示：主窗口未加载Json文件！";
                return;
            }

            string keyType = _keyTypeComboBox.SelectedValue?.ToString() ?? "Click";
            string selectedType = keyType == MuMuJsonEditor.typeClick ? MuMuJsonEditor.typeClick : MuMuJsonEditor.typeMacro;
            string typeChinese = selectedType == "Click" ? "单击" : "宏指牌";

            if (IsExactDuplicate(myJson, key, selectedType, _lastCapturedCoord.RelX, _lastCapturedCoord.RelY))
            {
                _lblGenerateTip.Text = "已存在：" + typeChinese + "按键" + key.KeyCode.ToString() + "（坐标、按键、类型完全一致）";
                return;
            }

            if (MuMuJsonEditor.FindKey(myJson, key) != -1)
            {
                _lblGenerateTip.Text = "已生成：" + typeChinese + "按键" + key.KeyCode.ToString() + "（同名按键已存在其他模板，模拟器中会冲突请自行修改）";
            }

            string scanCode = MuMuJsonEditor.GetScanCode(key.KeyCode).ToString();
            string mkey = MuMuJsonEditor.CreateKey(selectedType, key,
                _lastCapturedCoord.RelX.ToString("F16", CultureInfo.InvariantCulture),
                _lastCapturedCoord.RelY.ToString("F16", CultureInfo.InvariantCulture),
                scanCode);
            mainForm.SetMuMuJson(MuMuJsonEditor.WriteKeys(mkey, myJson));
            mainForm.SaveJsonAndBackup();
            _lblGenerateTip.Text = "已生成：" + typeChinese + "按键" + key.KeyCode.ToString();
        }

        private void ExecuteGenerateSelected(KeyEventArgs key)
        {
            if (lvTouchCoords.SelectedItems.Count == 0)
            {
                _lblGenerateTip.Text = "请先在坐标列表中选中一行坐标！";
                return;
            }
            var info = lvTouchCoords.SelectedItems[0].Tag as TouchCollector.TouchCoordInfo;
            if (info == null) return;
            double relX = info.Coord.RelX;
            double relY = info.Coord.RelY;

            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm == null) return;

            string myJson = mainForm.GetMuMuJson();
            if (string.IsNullOrEmpty(myJson))
            {
                _lblGenerateTip.Text = "提示：主窗口未加载Json文件！";
                return;
            }

            string keyType = _keyTypeComboBox.SelectedValue?.ToString() ?? "Click";
            string selectedType = keyType == "Click" ? "Click" : "Macro";
            string typeChinese = selectedType == "Click" ? "单击" : "宏指牌";

            if (IsExactDuplicate(myJson, key, selectedType, relX, relY))
            {
                _lblGenerateTip.Text = "已存在：" + typeChinese + "按键" + key.KeyCode + "（坐标、按键、类型完全一致）";
                return;
            }

            bool hasNameConflict = MuMuJsonEditor.FindKey(myJson, key) != -1;

            string scanCode = MuMuJsonEditor.GetScanCode(key.KeyCode).ToString();
            string mkey = MuMuJsonEditor.CreateKey(selectedType, key,
                relX.ToString("F16", CultureInfo.InvariantCulture),
                relY.ToString("F16", CultureInfo.InvariantCulture), scanCode);
            mainForm.SetMuMuJson(MuMuJsonEditor.WriteKeys(mkey, myJson));
            mainForm.SaveJsonAndBackup();

            if (hasNameConflict)
                _lblGenerateTip.Text = "已生成：" + typeChinese + "按键" + key.KeyCode + "（同名按键已存在其他模板，模拟器中会冲突请自行修改）";
            else
                _lblGenerateTip.Text = "已生成：" + typeChinese + "按键" + key.KeyCode;
        }

        private bool IsExactDuplicate(string myJson, KeyEventArgs key, string keyType, double relX, double relY)
        {
            try
            {
                int idx = MuMuJsonEditor.FindKey(myJson, key);
                if (idx == -1) return false;

                var json = JObject.Parse(myJson);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null || idx >= keymaps.Count) return false;

                var keyObj = keymaps[idx] as JObject;
                if (keyObj == null) return false;

                string existingType = keyObj["type"]?.Value<string>();
                if (existingType != keyType) return false;

                var relPos = keyObj["icon"]?["rel_position"];
                if (relPos == null) return false;
                double ex = relPos["rel_x"]?.Value<double>() ?? -1;
                double ey = relPos["rel_y"]?.Value<double>() ?? -1;

                return Math.Abs(ex - relX) < 0.00000000000001 && Math.Abs(ey - relY) < 0.00000000000001;
            }
            catch
            {
                return false;
            }
        }

        private void LoadPortHistory(string portsHistory, string defaultPort)
        {
            cmbAdbPort.Items.Clear();

            var ports = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(portsHistory))
            {
                string[] split = portsHistory.Split(new[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (string p in split)
                {
                    string trimmed = p.Trim();
                    if (!string.IsNullOrEmpty(trimmed) && !ports.Contains(trimmed))
                        ports.Add(trimmed);
                }
            }

            if (ports.Count == 0)
                ports.Add(defaultPort);

            foreach (string p in ports)
                cmbAdbPort.Items.Add(new MuMuPortItem { Port = p });
            if (cmbAdbPort.Items.Count > 0) cmbAdbPort.SelectedIndex = 0;
        }

        private void SaveCurrentPortToHistory()
        {
            string currentPort = txtPortInput.Text?.Trim();
            if (string.IsNullOrEmpty(currentPort) || !Regex.IsMatch(currentPort, @"^\d+$")) return;

            var ports = new System.Collections.Generic.List<string>();
            foreach (object obj in cmbAdbPort.Items)
            {
                if (obj is MuMuPortItem item)
                    ports.Add(item.Port);
            }

            if (!ports.Contains(currentPort))
                ports.Insert(0, currentPort);

            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm != null)
                mainForm.SaveAdbPortsHistory(string.Join(",", ports.ToArray()));
        }

        private string LookupInstanceName(string port)
        {
            if (_cachedInstances != null)
            {
                foreach (var inst in _cachedInstances)
                {
                    if (inst.Port == port) return inst.InstanceName;
                }
            }

            string adbPath = GetEffectiveAdbPath();
            if (!string.IsNullOrEmpty(adbPath))
            {
                string installBase = FindMuMuInstallDir(adbPath);
                if (!string.IsNullOrEmpty(installBase))
                {
                    var instances = QueryMuMu12Instances(installBase);
                    if (instances != null)
                    {
                        _cachedInstances = instances;
                        foreach (var inst in instances)
                        {
                            if (inst.Port == port) return inst.InstanceName;
                        }
                    }
                }
            }
            return null;
        }

        private void FillInstanceNameForPort(string port)
        {
            string instanceName = LookupInstanceName(port);

            foreach (var obj in cmbAdbPort.Items)
            {
                if (obj is MuMuPortItem item && item.Port == port)
                {
                    if (!string.IsNullOrEmpty(instanceName) && string.IsNullOrEmpty(item.InstanceName))
                    {
                        int index = cmbAdbPort.Items.IndexOf(item);
                        cmbAdbPort.Items.RemoveAt(index);
                        var updatedItem = new MuMuPortItem { Port = port, InstanceName = instanceName };
                        cmbAdbPort.Items.Insert(index, updatedItem);
                        _isSyncing = true;
                        try
                        {
                            cmbAdbPort.SelectedItem = updatedItem;
                            txtPortInput.Text = port;
                        }
                        finally { _isSyncing = false; }
                        return;
                    }

                    _isSyncing = true;
                    try
                    {
                        cmbAdbPort.SelectedItem = item;
                        txtPortInput.Text = port;
                    }
                    finally { _isSyncing = false; }
                    return;
                }
            }

            var newItem = new MuMuPortItem { Port = port, InstanceName = instanceName ?? "" };
            _isSyncing = true;
            try
            {
                cmbAdbPort.Items.Insert(0, newItem);
                cmbAdbPort.SelectedIndex = 0;
                txtPortInput.Text = port;
            }
            finally { _isSyncing = false; }
        }

        private void CmbAdbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isSyncing) return;
            _isSyncing = true;
            try
            {
                if (cmbAdbPort.SelectedItem is MuMuPortItem item)
                    txtPortInput.Text = item.Port;
            }
            finally { _isSyncing = false; }
        }

        private void TxtPortInput_TextChanged(object sender, EventArgs e)
        {
            if (_isSyncing) return;
            string input = txtPortInput.Text.Trim();
            if (string.IsNullOrEmpty(input) || !Regex.IsMatch(input, @"^\d+$"))
                return;

            _isSyncing = true;
            try
            {
                foreach (var obj in cmbAdbPort.Items)
                {
                    if (obj is MuMuPortItem item && item.Port == input)
                    {
                        cmbAdbPort.SelectedItem = item;
                        return;
                    }
                }
                cmbAdbPort.SelectedIndex = -1;
            }
            finally { _isSyncing = false; }
        }

        private void btnAutoDetectPort_Click(object sender, EventArgs e)
        {
            if (_isDetecting) return;
            _isDetecting = true;
            try
            {
                btnAutoDetectPort.Enabled = false;
                lblAdbStatus.ForeColor = Color.Black;

                List<MuMuPortItem> detectedPorts = AutoDetectPorts();
                if (detectedPorts.Count > 0)
                {
                    cmbAdbPort.Items.Clear();
                    foreach (var item in detectedPorts)
                        cmbAdbPort.Items.Add(item);
                    cmbAdbPort.SelectedIndex = 0;
                    lblAdbStatus.Text = "已检测到 " + detectedPorts.Count + " 个端口: " + string.Join(", ", detectedPorts.Select(p => p.Port));
                }
                else
                {
                    lblAdbStatus.ForeColor = Color.Red;
                }
                btnAutoDetectPort.Enabled = true;
            }
            finally
            {
                _isDetecting = false;
            }
        }

        private List<MuMuPortItem> AutoDetectPorts()
        {
            string adbPath = GetEffectiveAdbPath();
            if (string.IsNullOrEmpty(adbPath))
            {
                lblAdbStatus.Text = "未找到 adb.exe，请手动指定路径或先启动 MuMu 模拟器。";
                return new List<MuMuPortItem>();
            }

            string installBase = FindMuMuInstallDir(adbPath);
            bool hasMuMu12 = !string.IsNullOrEmpty(installBase)
                && FindMuMuManagerPath(installBase) != null;
            bool hasMuMu6 = !string.IsNullOrEmpty(installBase)
                && System.IO.Directory.Exists(System.IO.Path.Combine(installBase, @"emulator\nemu"));

            var portMap = new Dictionary<string, MuMuPortItem>();
            var preExisting = new HashSet<string>();

            Application.DoEvents();
            lblAdbStatus.Text = "正在检测 MuMu 模拟器端口...";

            if (hasMuMu12)
            {
                var instances = QueryMuMu12Instances(installBase);
                if (instances != null && instances.Count > 0)
                {
                    foreach (var inst in instances)
                    {
                        if (!portMap.ContainsKey(inst.Port))
                            portMap[inst.Port] = inst;
                    }
                    _cachedInstances = new List<MuMuPortItem>(instances);
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        string port = QueryMuMu12Port(installBase, i);
                        if (string.IsNullOrEmpty(port)) continue;
                        if (!portMap.ContainsKey(port))
                            portMap[port] = new MuMuPortItem { Port = port };
                    }
                }
            }

            AdbClient scanClient = new AdbClient(adbPath, "127.0.0.1:0");
            try
            {
                string devicesOutput = scanClient.Execute("devices", 5000);
                foreach (Match m in Regex.Matches(devicesOutput, @"127\.0\.0\.1:(\d+)\s+device"))
                {
                    string p = m.Groups[1].Value;
                    preExisting.Add(p);
                    if (!portMap.ContainsKey(p))
                        portMap[p] = new MuMuPortItem { Port = p };
                }
            }
            catch (Exception) { LogService.Warn("AdbTouchForm", "ADB devices扫描失败"); }

            if (hasMuMu6)
            {
                foreach (string port in new[] { "7555", "21503", "21513", "21523", "21533" })
                {
                    if (portMap.ContainsKey(port)) continue;
                    Application.DoEvents();
                    lblAdbStatus.Text = "尝试端口 " + port + "...";
                    if (TryAdbConnect(scanClient, port, out bool unauth))
                        portMap[port] = new MuMuPortItem { Port = port };
                    else if (unauth)
                        MessageBox.Show("请在模拟器中点击「允许 USB 调试」。",
                            "ADB 未授权", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (!hasMuMu12 && !hasMuMu6)
            {
                for (int i = 0; i < 20; i++)
                {
                    string port = (16384 + i * 32).ToString();
                    if (portMap.ContainsKey(port)) continue;
                    Application.DoEvents();
                    lblAdbStatus.Text = "尝试端口 " + port + "...";
                    if (TryAdbConnect(scanClient, port, out bool unauth))
                    {
                        portMap[port] = new MuMuPortItem { Port = port };
                        break;
                    }
                    else if (unauth)
                        MessageBox.Show("请在模拟器中点击「允许 USB 调试」。",
                            "ADB 未授权", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            foreach (var kv in portMap)
            {
                if (!preExisting.Contains(kv.Key))
                {
                    try { scanClient.Execute("disconnect 127.0.0.1:" + kv.Key, 3000); }
                    catch (Exception) { LogService.Warn("AdbTouchForm", "断开ADB连接失败: " + kv.Key); }
                }
            }

            scanClient.Dispose();

            if (portMap.Count == 0)
                lblAdbStatus.Text = "无法连接到模拟器，请检查 USB 调试授权。";

            return new List<MuMuPortItem>(portMap.Values);
        }

        private static string FindAdbNearExe(string exeDir)
        {
            string dir = exeDir;
            while (!string.IsNullOrEmpty(dir))
            {
                string candidate = System.IO.Path.Combine(dir, "adb.exe");
                if (System.IO.File.Exists(candidate))
                    return candidate;

                var p = System.IO.Directory.GetParent(dir);
                if (p == null) break;
                dir = p.FullName;
            }
            return null;
        }

        private static string FindMuMuInstallDir(string startPath)
        {
            if (string.IsNullOrEmpty(startPath)) return null;
            string dir = System.IO.Path.GetDirectoryName(startPath);
            while (!string.IsNullOrEmpty(dir))
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(dir, @"shell\adb.exe")))
                    return dir;
                if (System.IO.File.Exists(System.IO.Path.Combine(dir, @"nx_main\adb.exe")))
                    return dir;
                if (System.IO.Directory.Exists(System.IO.Path.Combine(dir, @"emulator\nemu")))
                    return dir;
                string parent = System.IO.Path.GetDirectoryName(dir);
                if (parent == dir) break;
                dir = parent;
            }
            return null;
        }

        private static string FindMuMuManagerPath(string installBase)
        {
            string nxPath = System.IO.Path.Combine(installBase, @"nx_main\MuMuManager.exe");
            if (System.IO.File.Exists(nxPath)) return nxPath;
            string shellPath = System.IO.Path.Combine(installBase, @"shell\MuMuManager.exe");
            if (System.IO.File.Exists(shellPath)) return shellPath;
            return null;
        }

        private static string FindAdbInInstallDir(string installDir)
        {
            string path = System.IO.Path.Combine(installDir, @"shell\adb.exe");
            if (System.IO.File.Exists(path)) return path;
            path = System.IO.Path.Combine(installDir, @"emulator\nemu\vmonitor\bin\adb_server.exe");
            if (System.IO.File.Exists(path)) return path;
            path = System.IO.Path.Combine(installDir, @"emulator\nemu\vmonitor\bin\adb.exe");
            if (System.IO.File.Exists(path)) return path;
            return null;
        }

        private static List<MuMuPortItem> QueryMuMu12Instances(string installBase)
        {
            string mumuMgr = FindMuMuManagerPath(installBase);
            if (mumuMgr == null) return null;

            try
            {
                string json = ExecuteMuMuManager(mumuMgr, "info -v all");
                if (string.IsNullOrEmpty(json)) return null;

                var result = new List<MuMuPortItem>();

                try
                {
                    var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Newtonsoft.Json.Linq.JObject>>(json);
                    if (dict != null)
                    {
                        foreach (var kv in dict)
                        {
                            int port = kv.Value["adb_port"]?.Value<int>() ?? 0;
                            if (port == 0) continue;
                            string name = kv.Value["name"]?.Value<string>() ?? "";
                            result.Add(new MuMuPortItem { Port = port.ToString(), InstanceName = name });
                        }
                        if (result.Count > 0) return result;
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    try
                    {
                        var array = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(json);
                        if (array != null)
                        {
                            foreach (var obj in array)
                            {
                                int port = obj["adb_port"]?.Value<int>() ?? 0;
                                if (port == 0) continue;
                                string name = obj["name"]?.Value<string>() ?? "";
                                result.Add(new MuMuPortItem { Port = port.ToString(), InstanceName = name });
                            }
                            if (result.Count > 0) return result;
                        }
                    }
                    catch (Newtonsoft.Json.JsonException)
                    {
                        foreach (Match m in Regex.Matches(json, @"\{(?:[^{}]|(?<o>\{)|(?<-o>\}))+(?(o)(?!))\}", RegexOptions.Singleline))
                        {
                            try
                            {
                                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(m.Value);
                                if (obj != null)
                                {
                                    int port = obj["adb_port"]?.Value<int>() ?? 0;
                                    if (port == 0) continue;
                                    string name = obj["name"]?.Value<string>() ?? "";
                                    result.Add(new MuMuPortItem { Port = port.ToString(), InstanceName = name });
                                }
                            }
                            catch (Newtonsoft.Json.JsonException) { }
                        }
                        if (result.Count > 0) return result;
                    }
                }
            }
            catch (Exception ex) { LogService.Warn("AdbTouchForm", "查询MuMu12实例失败: " + ex.Message); }
            return null;
        }

        private static string ExecuteMuMuManager(string exePath, string arguments)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    StandardErrorEncoding = System.Text.Encoding.UTF8,
                    CreateNoWindow = true
                };
                using (var proc = new Process { StartInfo = psi })
                {
                    if (!proc.Start()) return null;
                    var readOutput = System.Threading.Tasks.Task.Run(() => proc.StandardOutput.ReadToEnd());
                    var readError = System.Threading.Tasks.Task.Run(() => proc.StandardError.ReadToEnd());
                    if (!System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { readOutput, readError }, 5000))
                    {
                        try { proc.Kill(); } catch (Exception) { }
                        return null;
                    }
                    if (!proc.WaitForExit(3000)) { try { proc.Kill(); } catch (Exception) { } }
                    string result = readOutput.Result;
                    if (string.IsNullOrEmpty(result))
                        result = readError.Result;
                    return result;
                }
            }
            catch (Exception) { return null; }
        }

        private static string QueryMuMu12Port(string installBase, int index)
        {
            string mumuMgr = FindMuMuManagerPath(installBase);
            if (mumuMgr == null) return null;
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = mumuMgr,
                    Arguments = "adb -v " + index,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    StandardErrorEncoding = System.Text.Encoding.UTF8,
                    CreateNoWindow = true
                };
                using (var proc = new Process { StartInfo = psi })
                {
                    if (!proc.Start()) return null;
                    var readOutput = System.Threading.Tasks.Task.Run(() => proc.StandardOutput.ReadToEnd());
                    var readError = System.Threading.Tasks.Task.Run(() => proc.StandardError.ReadToEnd());
                    if (!System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { readOutput, readError }, 5000))
                    {
                        try { proc.Kill(); } catch (Exception) { }
                        return null;
                    }
                    if (!proc.WaitForExit(3000)) { try { proc.Kill(); } catch (Exception) { } }
                    string combined = readOutput.Result + "\n" + readError.Result;
                    var match = Regex.Match(combined, @"\b(\d{4,5})\b");
                    if (match.Success) return match.Groups[1].Value;
                }
            }
            catch (Exception) { LogService.Warn("AdbTouchForm", "查询MuMu12端口失败"); }
            return null;
        }

        private static bool TryAdbConnect(AdbClient client, string port, out bool unauthorized)
        {
            unauthorized = false;
            try
            {
                string result = client.Execute("connect 127.0.0.1:" + port, 1500);
                if (result.Contains("connected to") || result.Contains("already connected"))
                    return true;
                if (result.Contains("unauthorized"))
                    unauthorized = true;
            }
            catch (TimeoutException) { }
            catch (Exception) { LogService.Warn("AdbTouchForm", "尝试ADB连接失败，端口: " + port); }
            return false;
        }

        private void btnBrowseAdb_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "选择 adb.exe";
            dialog.Filter = "adb 程序|adb*.exe|所有文件|*.exe";
            dialog.InitialDirectory = !string.IsNullOrEmpty(txtAdbPath.Text)
                ? System.IO.Path.GetDirectoryName(txtAdbPath.Text)
                : Application.StartupPath;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtAdbPath.Text = dialog.FileName;
                _configAdbPath = dialog.FileName;
            }
        }

        private void btnAdbConnect_Click(object sender, EventArgs e)
        {
            LogService.Info("AdbTouchForm", "尝试ADB连接");
            string port = txtPortInput.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(port) || !Regex.IsMatch(port, @"^\d+$"))
            {
                MessageBox.Show("请输入有效的端口号。");
                return;
            }

            string deviceSerial = "127.0.0.1:" + port;
            string adbPath = GetEffectiveAdbPath();

            if (string.IsNullOrEmpty(adbPath))
            {
                MessageBox.Show("未找到 adb.exe，请确认 MuMu 模拟器已安装，或手动指定 adb.exe 路径。");
                return;
            }

            if (_adbClient == null)
                _adbClient = new AdbClient(adbPath, deviceSerial);
            else
                _adbClient.SetDevice(adbPath, deviceSerial);

            if (_touchCollector != null)
            {
                _touchCollector.Dispose();
                _touchCollector = null;
            }
            _touchCollector = new TouchCollector(_adbClient);
            _touchCollector.CoordinateCaptured += OnCoordinateCaptured;
            _touchCollector.StatusChanged += OnStatusChanged;
            _touchCollector.ErrorOccurred += OnErrorOccurred;
            _touchCollector.ResolutionDetected += OnResolutionDetected;

            btnAutoDetectPort.Enabled = false;
            btnAdbConnect.Enabled = false;
            btnAdbStop.Enabled = true;
            cmbAdbPort.Enabled = false;
            txtPortInput.Enabled = false;

            bool started = _touchCollector.Start();

            if (!started)
            {
                LogService.Warn("AdbTouchForm", "ADB连接启动失败，端口: " + port);
                _touchCollector.Dispose();
                _touchCollector = null;
                btnAutoDetectPort.Enabled = true;
                btnAdbConnect.Enabled = true;
                btnAdbStop.Enabled = false;
                cmbAdbPort.Enabled = true;
                txtPortInput.Enabled = true;
            }
            else
            {
                SaveCurrentPortToHistory();
                FillInstanceNameForPort(port);
            }
        }

        private void btnAdbStop_Click(object sender, EventArgs e)
        {
            LogService.Info("AdbTouchForm", "停止ADB采集");
            if (_touchCollector != null)
            {
                _touchCollector.Dispose();
                _touchCollector = null;
            }
            if (_adbClient != null)
            {
                _adbClient.Dispose();
                _adbClient = null;
            }
            _shownResolutionMsg = false;
            _hasLastCoord = false;
            btnAutoDetectPort.Enabled = true;
            btnAdbConnect.Enabled = true;
            btnAdbStop.Enabled = false;
            cmbAdbPort.Enabled = true;
            txtPortInput.Enabled = true;
        }

        private void OnCoordinateCaptured(TouchCoordinate coord)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new Action<TouchCoordinate>(OnCoordinateCaptured), coord);
                return;
            }
            _lastCapturedCoord = coord;
            _hasLastCoord = true;
            UpdateListView();
            UpdateGenerateTipStatus();
        }

        private void UpdateGenerateTipStatus()
        {
            if (!_hasLastCoord && (_generateOnceCheckBox.Checked || _generateMultipleCheckBox.Checked))
            {
                _lblGenerateTip.Text = "提示：请先捕获坐标！";
            }
            else if (_hasLastCoord && _generateOnceCheckBox.Checked)
            {
                _lblGenerateTip.Text = "提示：已开启键盘监听（单次）";
            }
            else if (_hasLastCoord && _generateMultipleCheckBox.Checked)
            {
                _lblGenerateTip.Text = "提示：已开启键盘监听（连续）";
            }
        }

        private void OnStatusChanged(string status)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnStatusChanged), status);
                return;
            }
            lblAdbStatus.Text = status;
            lblAdbStatus.ForeColor = Color.Black;
        }

        private void OnErrorOccurred(string error)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnErrorOccurred), error);
                return;
            }
            LogService.Error("AdbTouchForm", "ADB错误: " + error);
            lblAdbStatus.Text = error;
            lblAdbStatus.ForeColor = Color.Red;
            btnAutoDetectPort.Enabled = true;
            btnAdbConnect.Enabled = true;
            btnAdbStop.Enabled = false;
            cmbAdbPort.Enabled = true;
            txtPortInput.Enabled = true;
            MessageBox.Show(error, "ADB 错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void OnResolutionDetected(int w, int h)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new Action<int, int>(OnResolutionDetected), w, h);
                return;
            }
            lblResolution.Text = string.Format("{0} x {1}", w, h);
            lblResolution.ForeColor = System.Drawing.Color.Black;
            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm != null)
            {
                mainForm.SetResolution(w, h);
            }
            if (!_shownResolutionMsg)
            {
                _shownResolutionMsg = true;
                MessageBox.Show(string.Format(
                    "采集已连接！\n\n" +
                    "模拟器分辨率: {0} x {1}\n已同步至主界面。\n\n" +
                    "• 通过 MuMu 鼠标集成传感器采集坐标\n" +
                    "  (65535级分辨率，远高于屏幕)\n" +
                    "• 精度可达小数点后1位\n" +
                    "• 后台静默采集，不影响游戏操作",
                    w, h), "连接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateListView()
        {
            if (_touchCollector == null) return;
            lvTouchCoords.BeginUpdate();
            lvTouchCoords.Items.Clear();
            lvTouchCoords.ForeColor = Color.Black;
            lvTouchCoords.BackColor = Color.White;
            var coords = _touchCollector.GetUniqueCoords();
            int index = 1;
            foreach (var info in coords)
            {
                var item = new ListViewItem(index.ToString());
                item.ForeColor = Color.Black;
                item.BackColor = Color.White;
                var sub1 = new ListViewItem.ListViewSubItem(item, info.Coord.RelX.ToString("F6"), Color.Black, Color.White, item.Font);
                var sub2 = new ListViewItem.ListViewSubItem(item, info.Coord.RelY.ToString("F6"), Color.Black, Color.White, item.Font);
                var sub3 = new ListViewItem.ListViewSubItem(item, info.HitCount.ToString(), Color.Black, Color.White, item.Font);
                var sub4 = new ListViewItem.ListViewSubItem(item, info.Coord.X.ToString("F1"), Color.Black, Color.White, item.Font);
                var sub5 = new ListViewItem.ListViewSubItem(item, info.Coord.Y.ToString("F1"), Color.Black, Color.White, item.Font);
                item.SubItems.Add(sub1);
                item.SubItems.Add(sub2);
                item.SubItems.Add(sub3);
                item.SubItems.Add(sub4);
                item.SubItems.Add(sub5);
                item.UseItemStyleForSubItems = false;
                item.Tag = info;
                lvTouchCoords.Items.Add(item);
                index++;
            }
            lvTouchCoords.EndUpdate();
        }

        private void lvTouchCoords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = lvTouchCoords.GetItemAt(e.X, e.Y);
            if (item == null || item.Tag == null) return;
            var info = (TouchCollector.TouchCoordInfo)item.Tag;
            ApplyToMainForm(info);
        }

        private void btnAdbApply_Click(object sender, EventArgs e)
        {
            if (lvTouchCoords.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先选择一个坐标项。");
                return;
            }
            var item = lvTouchCoords.SelectedItems[0];
            if (!(item.Tag is TouchCollector.TouchCoordInfo info)) return;
            ApplyToMainForm(info);
        }

        private void ApplyToMainForm(TouchCollector.TouchCoordInfo info)
        {
            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm != null)
            {
                mainForm.SetJSXY(info.Coord.RelX.ToString("F16", CultureInfo.InvariantCulture),
                                 info.Coord.RelY.ToString("F16", CultureInfo.InvariantCulture));
            }
            else
            {
                MessageBox.Show("坐标: rel_x=" + info.Coord.RelX.ToString("F16", CultureInfo.InvariantCulture)
                    + ", rel_y=" + info.Coord.RelY.ToString("F16", CultureInfo.InvariantCulture));
            }
        }

        private void btnAdbClear_Click(object sender, EventArgs e)
        {
            if (_touchCollector != null) _touchCollector.ClearCoords();
            lvTouchCoords.Items.Clear();
            _hasLastCoord = false;
            UpdateGenerateTipStatus();
        }

        private void btnResetAdbConfig_Click(object sender, EventArgs e)
        {
            txtAdbPath.Clear();
            _configAdbPath = null;
            cmbAdbPort.Items.Clear();
            txtPortInput.Text = "16384";
            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm != null)
            {
                mainForm.SaveAdbConfig("16384", "");
                mainForm.SaveAdbPortsHistory("16384");
                MessageBox.Show("已清空。\nAdbPort=16384\nAdbPath=\"\"\nAdbPortsHistory=16384\n关闭此对话框后请重启窗口验证。");
            }
            lblAdbStatus.Text = "已清空连接设置";
            lblAdbStatus.ForeColor = Color.Black;
        }

        private string GetEffectiveAdbPath()
        {
            if (!string.IsNullOrEmpty(txtAdbPath.Text)
                && System.IO.File.Exists(txtAdbPath.Text))
                return txtAdbPath.Text;

            string resolved = ResolveAdbPath();
            if (!string.IsNullOrEmpty(resolved))
                txtAdbPath.Text = resolved;
            return resolved;
        }

        private string ResolveAdbPath()
        {
            if (!string.IsNullOrEmpty(_configAdbPath) && System.IO.File.Exists(_configAdbPath))
                return _configAdbPath;

            string procAdb = FindAdbViaProcess("MuMuNxService");
            if (!string.IsNullOrEmpty(procAdb)) return procAdb;
            procAdb = FindAdbViaProcess("crashpad_handler");
            if (!string.IsNullOrEmpty(procAdb)) return procAdb;

            string localAdb = System.IO.Path.Combine(Application.StartupPath, "adb.exe");
            if (System.IO.File.Exists(localAdb)) return localAdb;

            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {
                if (!drive.IsReady) continue;
                string mumuRoot = System.IO.Path.Combine(drive.RootDirectory.FullName, "mumu");
                if (!System.IO.Directory.Exists(mumuRoot)) continue;
                foreach (string candidate in System.IO.Directory.GetDirectories(mumuRoot, "MuMuPlayer*"))
                {
                    string p = System.IO.Path.Combine(candidate, @"nx_main\adb.exe");
                    if (System.IO.File.Exists(p)) return p;
                    string deviceShell = System.IO.Path.Combine(candidate, "nx_device");
                    if (!System.IO.Directory.Exists(deviceShell)) continue;
                    foreach (string verDir in System.IO.Directory.GetDirectories(deviceShell))
                    {
                        p = System.IO.Path.Combine(verDir, @"shell\adb.exe");
                        if (System.IO.File.Exists(p)) return p;
                    }
                }
            }

            var programFilesDirs = new List<string>();
            string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            if (!string.IsNullOrEmpty(pf)) programFilesDirs.Add(System.IO.Path.Combine(pf, "Netease"));
            string pfx86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            if (!string.IsNullOrEmpty(pfx86)) programFilesDirs.Add(System.IO.Path.Combine(pfx86, "Netease"));
            foreach (string baseDir in programFilesDirs)
            {
                if (!System.IO.Directory.Exists(baseDir)) continue;
                foreach (string muMuDir in System.IO.Directory.GetDirectories(baseDir, "MuMuPlayer*"))
                {
                    foreach (string sub in new[] { @"shell\adb.exe", @"emulator\nemu\vmonitor\bin\adb_server.exe", @"emulator\nemu\vmonitor\bin\adb.exe" })
                    {
                        string p = System.IO.Path.Combine(muMuDir, sub);
                        if (System.IO.File.Exists(p)) return p;
                    }
                }
            }

            return null;
        }

        private static string FindAdbViaProcess(string processName)
        {
            Process[] procs = null;
            try { procs = Process.GetProcessesByName(processName); } catch { return null; }
            if (procs == null || procs.Length == 0) return null;
            try
            {
                string exePath = procs[0].MainModule.FileName;
                foreach (var p in procs) { try { p.Dispose(); } catch (Exception) { } }
                string dir = System.IO.Path.GetDirectoryName(exePath);
                string adb = System.IO.Path.Combine(dir, "adb.exe");
                if (System.IO.File.Exists(adb)) return adb;
                adb = System.IO.Path.Combine(dir, @"..\adb.exe");
                adb = System.IO.Path.GetFullPath(adb);
                if (System.IO.File.Exists(adb)) return adb;
            }
            catch
            {
                foreach (var p in procs) { try { p.Dispose(); } catch (Exception) { } }
            }
            return null;
        }

        private void TopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = _topCheckBox.Checked;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_touchCollector != null)
            {
                _touchCollector.CoordinateCaptured -= OnCoordinateCaptured;
                _touchCollector.StatusChanged -= OnStatusChanged;
                _touchCollector.ErrorOccurred -= OnErrorOccurred;
                _touchCollector.ResolutionDetected -= OnResolutionDetected;
                _touchCollector.Dispose();
                _touchCollector = null;
            }

            if (_adbClient != null)
            {
                _adbClient.Dispose();
                _adbClient = null;
            }

            if (_keyboardHandler != null)
            {
                _keyboardHandler.KeyCapturedOnce -= OnKeyCapturedGenerateOnce;
                _keyboardHandler.KeyCapturedContinuously -= OnKeyCapturedGenerateContinuously;
                _keyboardHandler.StopAllListening();
                _keyboardHandler.Dispose();
            }

            var mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm != null)
            {
                mainForm.SaveAdbConfig(txtPortInput.Text, txtAdbPath.Text);
            }

            base.OnFormClosed(e);
        }
    }

    internal class MuMuPortItem
    {
        public string Port { get; set; }
        public string InstanceName { get; set; }

        public string DisplayText =>
            string.IsNullOrEmpty(InstanceName) ? Port : InstanceName + " (" + Port + ")";

        public override string ToString() => DisplayText;
    }
}
