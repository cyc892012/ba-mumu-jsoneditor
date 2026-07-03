using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace MuMu坐标计算
{
    partial class WideScreenForm : Form
    {
        private readonly Func<string> _getCurrentJson;
        private readonly Func<string, bool> _writeJsonCallback;
        private readonly Func<double> _getResolutionX;
        private readonly Func<double> _getResolutionY;
        private readonly ResolutionManager _resolutionManager;
        private readonly ConfigManager _config;
        private readonly Dictionary<string, string> _packageNameTypes;
        private readonly Dictionary<string, string> _resolutionTypes;
        private readonly Func<string, string> _getDataFolder;

        private PBClass.ClickKeyInfo[] _clickKeyinfo;
        private bool _isUpdatingCheckState;

        public WideScreenForm(
            Func<string> getCurrentJson,
            Func<string, bool> writeJsonCallback,
            Func<double> getResolutionX,
            Func<double> getResolutionY,
            ResolutionManager resolutionManager,
            ConfigManager config,
            Dictionary<string, string> packageNameTypes,
            Dictionary<string, string> resolutionTypes,
            Func<string, string> getDataFolder)
        {
            _getCurrentJson = getCurrentJson;
            _writeJsonCallback = writeJsonCallback;
            _getResolutionX = getResolutionX;
            _getResolutionY = getResolutionY;
            _resolutionManager = resolutionManager;
            _config = config;
            _packageNameTypes = packageNameTypes;
            _resolutionTypes = resolutionTypes;
            _getDataFolder = getDataFolder;

            InitializeComponent();

            _topCheckBox.CheckedChanged += TopCheckBox_CheckedChanged;
            _packageNamecomboBox.SelectedIndexChanged += PackageNamecomboBox_SelectedIndexChanged;
            _resolutionTypecomboBox.SelectedIndexChanged += ResolutionTypecomboBox_SelectedIndexChanged;
            _ktckReadbutton.Click += KtckReadbutton_Click;
            _ktckPListcheckedListBox.ItemCheck += KtckPListcheckedListBox_ItemCheck;
            _ktckPListcheckedListBox.SelectedIndexChanged += KtckPListcheckedListBox_SelectedIndexChanged;
            _ktckOPWritebutton.Click += KtckOPWritebutton_Click;
            _ktckAPWritebutton.Click += KtckAPWritebutton_Click;

            _fileNameSearchCombo.DropDown += (sdr, edr) =>
            {
                string searchText = _fileNameSearchCombo.CurrentSearchText;
                if (string.IsNullOrWhiteSpace(searchText))
                    RefreshFileNameComboBox(_fileNameSearchCombo);
                else
                    RefreshFileNameComboBox(_fileNameSearchCombo, searchText);
            };
            _fileNameSearchCombo.FilterRequested += (sdr, text) =>
            {
                RefreshFileNameComboBox(_fileNameSearchCombo, text);
            };

            InitData();
        }

        private void InitData()
        {
            _packageNamecomboBox.DataSource = new List<KeyValuePair<string, string>>(_packageNameTypes);
            _packageNamecomboBox.DisplayMember = "Key";
            _packageNamecomboBox.ValueMember = "Value";

            _resolutionTypecomboBox.DataSource = new List<KeyValuePair<string, string>>(_resolutionTypes);
            _resolutionTypecomboBox.DisplayMember = "Key";
            _resolutionTypecomboBox.ValueMember = "Value";
            if (_resolutionTypecomboBox.Items.Count > 0)
            {
                _resolutionTypecomboBox.SelectedIndex = 0;
                RefreshResolutionComboBox();
            }

            RefreshFileNameComboBox(_fileNameSearchCombo);
        }

        private void RefreshResolutionComboBox()
        {
            string typeCode = _resolutionTypecomboBox.SelectedValue?.ToString() ?? "";
            if (string.IsNullOrEmpty(typeCode)) return;
            var dict = _resolutionManager.GetResolutionDictByType(typeCode, _config.Resolution4String);
            if (dict == null) return;

            var items = new List<KeyValuePair<string, string>>();
            foreach (var kv in dict)
                items.Add(new KeyValuePair<string, string>(kv.Key, kv.Value));

            _resolutioncomboBox.DataSource = null;
            _resolutioncomboBox.DisplayMember = "Key";
            _resolutioncomboBox.ValueMember = "Value";
            _resolutioncomboBox.DataSource = items;
            if (_resolutioncomboBox.Items.Count > 0) _resolutioncomboBox.SelectedIndex = Math.Min(1, _resolutioncomboBox.Items.Count - 1);
        }

        private void RefreshFileNameComboBox(SearchableComboBox cb, string searchText = null)
        {
            string dataFolder = _getDataFolder(null);
            if (string.IsNullOrEmpty(dataFolder)) return;

            var jsonFiles = ComboBoxInitializer.TryGetJsonFiles(dataFolder, cb);
            if (jsonFiles == null) return;

            string[] PackageNamesValues = new string[_packageNameTypes.Count];
            int i = 0;
            foreach (var kv in _packageNameTypes)
                PackageNamesValues[i++] = kv.Value;

            string PackageName = _packageNamecomboBox.SelectedValue == null ? "" : _packageNamecomboBox.SelectedValue.ToString();
            var items = new List<KeyValuePair<string, string>>();

            int count = PackageNamesValues.Length;
            if (count >= 4 && (PackageName == PackageNamesValues[0] || PackageName == PackageNamesValues[1]
                || PackageName == PackageNamesValues[2] || PackageName == PackageNamesValues[3]))
            {
                foreach (var file in jsonFiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    if (fileName.IndexOf(PackageName) != -1)
                    {
                        fileName = fileName.Replace(PackageName, "");
                        if (string.IsNullOrEmpty(searchText) || fileName.IndexOf(searchText) != -1)
                            items.Add(new KeyValuePair<string, string>(file, fileName));
                    }
                }
            }
            else if (count >= 5 && PackageName == PackageNamesValues[4])
            {
                foreach (var file in jsonFiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    if (fileName.IndexOf(PackageNamesValues[0]) == -1
                        && fileName.IndexOf(PackageNamesValues[1]) == -1
                        && fileName.IndexOf(PackageNamesValues[2]) == -1
                        && fileName.IndexOf(PackageNamesValues[3]) == -1)
                    {
                        if (string.IsNullOrEmpty(searchText) || fileName.IndexOf(searchText) != -1)
                            items.Add(new KeyValuePair<string, string>(file, fileName));
                    }
                }
            }
            else if (count >= 6 && PackageName == PackageNamesValues[5])
            {
                ComboBoxInitializer.ShowEmptyMessage(cb, "绿玩哪有宇宙服，你清醒一点。");
                return;
            }

            if (items.Count == 0)
            {
                ComboBoxInitializer.ShowEmptyMessage(cb, "未找到符合条件的文件！");
                return;
            }

            cb.DataSource = null;
            cb.DisplayMember = "Value";
            cb.ValueMember = "Key";
            cb.DataSource = items;
            if (items.Count > 0) cb.SelectedIndex = 0;
        }

        private void ResolutionTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshResolutionComboBox();
        }

        private void PackageNamecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshFileNameComboBox(_fileNameSearchCombo);
        }

        private void KtckReadbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fileNameSearchCombo.SelectedValue == null || _resolutioncomboBox.SelectedValue == null)
                {
                    MessageBox.Show("请先选择有效的键位文件和分辨率。");
                    return;
                }
                string filePath = _fileNameSearchCombo.SelectedValue?.ToString() ?? "";
                string kJson = File.ReadAllText(filePath, Encoding.UTF8);
                string[] Pvalue = (_resolutioncomboBox.SelectedValue?.ToString() ?? "").Split(',');
                if (Pvalue.Length < 2 || !int.TryParse(Pvalue[0], out int KX) || !int.TryParse(Pvalue[1], out int KY))
                {
                    MessageBox.Show("分辨率数据格式无效，请重新选择分辨率。");
                    return;
                }
                _clickKeyinfo = MuMuJsonEditor.GetClickKeys(kJson, KX, KY);
                _ktckPListcheckedListBox.Items.Clear();
                _ktckPListcheckedListBox.Items.Add("全选");
                foreach (var item in _clickKeyinfo)
                    _ktckPListcheckedListBox.Items.Add(item.KeyText);
                _ktckKXtextBox.Text = "0";
                _ktckKYtextBox.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private void KtckPListcheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_isUpdatingCheckState) return;
            _isUpdatingCheckState = true;
            try
            {
                if (e.Index == 0)
                {
                    bool isChecked = (e.NewValue == CheckState.Checked);
                    for (int i = 1; i < _ktckPListcheckedListBox.Items.Count; i++)
                        _ktckPListcheckedListBox.SetItemChecked(i, isChecked);
                    _ktckKXtextBox.Text = "0";
                    _ktckKYtextBox.Text = "0";
                }
                else
                {
                    bool allChecked = true;
                    for (int i = 1; i < _ktckPListcheckedListBox.Items.Count; i++)
                    {
                        if (i != e.Index && !_ktckPListcheckedListBox.GetItemChecked(i))
                        {
                            allChecked = false;
                            break;
                        }
                    }
                    if (e.NewValue == CheckState.Checked && allChecked)
                        _ktckPListcheckedListBox.SetItemChecked(0, true);
                    else
                        _ktckPListcheckedListBox.SetItemChecked(0, false);
                }
            }
            finally
            {
                _isUpdatingCheckState = false;
            }
        }

        private void KtckPListcheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = _ktckPListcheckedListBox.SelectedIndex;
                if (selectedIndex <= 0)
                {
                    _ktckKXtextBox.Text = "0";
                    _ktckKYtextBox.Text = "0";
                    _ktckCKXtextBox.Text = "0";
                    _ktckCKYtextBox.Text = "0";
                    return;
                }
                int dataIndex = selectedIndex - 1;
                if (_clickKeyinfo != null && dataIndex >= 0 && dataIndex < _clickKeyinfo.Length)
                {
                    var selectedKey = _clickKeyinfo[dataIndex];
                    _ktckKXtextBox.Text = selectedKey.RelX.ToString("F6", CultureInfo.InvariantCulture);
                    _ktckKYtextBox.Text = selectedKey.RelY.ToString("F6", CultureInfo.InvariantCulture);
                    if (_resolutioncomboBox.SelectedValue == null) return;
                    string[] Pvalue = (_resolutioncomboBox.SelectedValue?.ToString() ?? "").Split(',');
                    if (Pvalue.Length < 2 || !int.TryParse(Pvalue[0], out int SX) ||
                        !int.TryParse(Pvalue[1], out int SY))
                        return;
                    int FX = (int)_getResolutionX();
                    int FY = (int)_getResolutionY();
                    double mX = selectedKey.RelX;
                    double mY = selectedKey.RelY;
                    double[] result = MuMuJsonEditor.CalculateCoordinatesKToCK(SX, SY, FX, FY, mX, mY);
                    if (result[2] < 0)
                        _lblWideScreenCoord.Text = "对应宽屏坐标（部分超出边界）：";
                    else
                        _lblWideScreenCoord.Text = "对应宽屏坐标：";
                    _ktckCKXtextBox.Text = result[0].ToString();
                    _ktckCKYtextBox.Text = result[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private string CreateKey(string filePath, string KeyText, double x, double y)
        {
            try
            {
                string keyJson = MuMuJsonEditor.ReadKey(filePath, KeyText);
                if (string.IsNullOrEmpty(keyJson)) return "";

                if (_resolutioncomboBox.SelectedValue == null) return "";
                string[] Pvalue = (_resolutioncomboBox.SelectedValue?.ToString() ?? "").Split(',');
                if (Pvalue.Length < 2 || !int.TryParse(Pvalue[0], out int KX) ||
                    !int.TryParse(Pvalue[1], out int KY))
                    return "";
                int FX = (int)_getResolutionX();
                int FY = (int)_getResolutionY();
                double[] result = MuMuJsonEditor.CalculateCoordinatesKToCK(KX, KY, FX, FY, x, y);
                if (result[2] < 0)
                {
                    MessageBox.Show("警告：目标分辨率小于源分辨率，部分坐标会超出目标边界！生成的坐标可能不准确。",
                        "超出边界", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                double X = FX > 1 ? result[0] / (FX - 1) : 0.0;
                double Y = FY > 1 ? result[1] / (FY - 1) : 0.0;

                var keyObj = JObject.Parse(keyJson);
                string type = keyObj["type"]?.Value<string>();

                if (type == MuMuJsonEditor.typeClick || type == MuMuJsonEditor.typeBunchClick)
                {
                    SetRelToken(keyObj["icon"]?["rel_position"], X, Y);
                    SetRelToken(keyObj["rel_work_position"], X, Y);
                }
                else if (type == MuMuJsonEditor.typeMacro)
                {
                    var pressActions = keyObj["press_actions"] as JArray;
                    if (pressActions == null) return "";
                    bool replaced = false;
                    for (int i = 0; i < pressActions.Count; i++)
                    {
                        var action = pressActions[i]?.Value<string>();
                        if (action != null && action.StartsWith("curve_rel:mouse;("))
                        {
                             pressActions[i] = "curve_rel:mouse;(" + X.ToString(CultureInfo.InvariantCulture) + "," + Y.ToString(CultureInfo.InvariantCulture) + ")";
                            replaced = true;
                        }
                    }
                    if (!replaced)
                    {
                        MessageBox.Show("此按键不含可修改的坐标格式，无法进行异形分辨率移植！");
                        return "";
                    }
                }
                else
                {
                    MessageBox.Show("此按键不含可修改的坐标格式，无法进行异形分辨率移植！");
                    return "";
                }

                return keyObj.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
                return "";
            }
        }

        private static void SetRelToken(JToken posObj, double x, double y)
        {
            if (posObj == null) return;
            posObj["rel_x"] = x;
            posObj["rel_y"] = y;
        }

        private void KtckOPWritebutton_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = _ktckPListcheckedListBox.SelectedIndex;
                if (selectedIndex <= 0)
                {
                    MessageBox.Show("发生错误：没有选中有效项或选中了\"全选\"");
                    return;
                }
                int dataIndex = selectedIndex - 1;
                if (dataIndex >= 0 && dataIndex < (_clickKeyinfo != null ? _clickKeyinfo.Length : 0))
                {
                    if (_fileNameSearchCombo.SelectedValue == null)
                    {
                        MessageBox.Show("请先选择有效的键位文件。");
                        return;
                    }
                    string filePath = _fileNameSearchCombo.SelectedValue?.ToString() ?? "";
                    string key = CreateKey(filePath, _clickKeyinfo[dataIndex].KeyText,
                        _clickKeyinfo[dataIndex].RelX, _clickKeyinfo[dataIndex].RelY);
                    if (string.IsNullOrEmpty(key)) return;
                    string currentJson = _getCurrentJson?.Invoke(); if (currentJson == null) return;
                    string modified = MuMuJsonEditor.WriteKeys(key, currentJson);
                    if (_writeJsonCallback?.Invoke(modified) == true)
                        MessageBox.Show("键位移植成功！如出现问题请转人工或撤销操作！");
                }
            }
            catch (Exception ex) { MessageBox.Show("发生错误：" + ex.Message); }
        }

        private void KtckAPWritebutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fileNameSearchCombo.SelectedValue == null || _resolutioncomboBox.SelectedValue == null)
                {
                    MessageBox.Show("请先选择有效的键位文件和分辨率。");
                    return;
                }
                string filePath = _fileNameSearchCombo.SelectedValue?.ToString() ?? "";
                string currentJson = _getCurrentJson?.Invoke(); if (currentJson == null) return;
                string modified = currentJson;
                foreach (int index in _ktckPListcheckedListBox.CheckedIndices)
                {
                    if (index <= 0) continue;
                    int dataIndex = index - 1;
                    if (dataIndex >= 0 && dataIndex < (_clickKeyinfo != null ? _clickKeyinfo.Length : 0))
                    {
                        string key = CreateKey(filePath, _clickKeyinfo[dataIndex].KeyText,
                            _clickKeyinfo[dataIndex].RelX, _clickKeyinfo[dataIndex].RelY);
                        if (string.IsNullOrEmpty(key)) continue;
                        modified = MuMuJsonEditor.WriteKeys(key, modified);
                    }
                }
                if (_writeJsonCallback?.Invoke(modified) == true)
                    MessageBox.Show("选择键位移植成功！如出现问题请转人工或撤销操作！键位冲突请手动更改！");
            }
            catch (Exception ex) { MessageBox.Show("发生错误：" + ex.Message); }
        }

        private void TopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = _topCheckBox.Checked;
        }
    }
}
