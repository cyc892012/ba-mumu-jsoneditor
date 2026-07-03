using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class IndexBackupForm : Form
    {
        private readonly IndexFileBackupManager _indexBackup;
        private readonly Dictionary<string, string> _packageNameTypes;
        private readonly Func<string> _getCurrentFilePath;
        private readonly Func<string> _getSelectedPackage;
        private readonly Action<string> _setStatusText;
        private readonly Action<string, Color> _setStatusWarning;
        private readonly Action _resetStatus;

        private bool _isPopulatingComboBox;
        private bool _isUpdatingSelectAll;
        private List<IndexFileBackupManager.OrphanSchemeInfo> _orphanSchemes;

        private const int MaxBackupCount = 10;

        public IndexBackupForm(
            IndexFileBackupManager indexBackup,
            Dictionary<string, string> packageNameTypes,
            Func<string> getCurrentFilePath,
            Func<string> getSelectedPackage,
            Action<string> setStatusText,
            Action<string, Color> setStatusWarning,
            Action resetStatus)
        {
            _indexBackup = indexBackup;
            _packageNameTypes = packageNameTypes;
            _getCurrentFilePath = getCurrentFilePath;
            _getSelectedPackage = getSelectedPackage;
            _setStatusText = setStatusText;
            _setStatusWarning = setStatusWarning;
            _resetStatus = resetStatus;

            InitializeComponent();

            _topCheckBox.CheckedChanged += TopCheckBox_CheckedChanged;
            _indexFileComboBox.SelectedIndexChanged += IndexFileComboBox_SelectedIndexChanged;
            _refreshButton.Click += RefreshButton_Click;
            _manualBackupButton.Click += ManualBackupButton_Click;
            _restoreButton.Click += RestoreButton_Click;
            _cleanupButton.Click += CleanupButton_Click;
            _scanOrphansButton.Click += ScanOrphansButton_Click;
            _addToIndexButton.Click += AddToIndexButton_Click;
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            _selectAllCheckBox.CheckedChanged += SelectAllCheckBox_CheckedChanged;
            _backupListView.DoubleClick += BackupListView_DoubleClick;
            _backupListView.SelectedIndexChanged += BackupListView_SelectedIndexChanged;
            _orphanListBox.ItemCheck += OrphanListBox_ItemCheck;

            _backupListView.Columns.Add("备份时间", 140);
            _backupListView.Columns.Add("方案数", 55, HorizontalAlignment.Center);
            _backupListView.Columns.Add("方案预览", 440);

            Load += (s, e) => RefreshBackupList();
        }

        private void IndexFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isPopulatingComboBox) return;
            HideOrphanSection();
            RefreshBackupList();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            HideOrphanSection();
            RefreshBackupList();
        }

        private void HideOrphanSection()
        {
            _orphanListBox.Visible = false;
            _addToIndexButton.Enabled = false;
            _searchTextBox.Visible = false;
            _selectAllCheckBox.Visible = false;
            _orphanSchemes = null;
        }

        private void ShowOrphanSection()
        {
            _orphanListBox.Visible = true;
            _addToIndexButton.Enabled = true;
            _searchTextBox.Visible = true;
            _searchTextBox.Text = "";
            _selectAllCheckBox.Visible = true;
            _isUpdatingSelectAll = true;
            _selectAllCheckBox.Checked = true;
            _isUpdatingSelectAll = false;
        }

        private void ScanOrphansButton_Click(object sender, EventArgs e)
        {
            try
            {
                string indexFileName = _indexFileComboBox.SelectedValue == null ? null : _indexFileComboBox.SelectedValue.ToString();
                if (string.IsNullOrEmpty(indexFileName))
                {
                    MessageBox.Show("请先在下拉框选择一个索引文件！");
                    return;
                }

                _orphanSchemes = _indexBackup.ScanOrphanSchemes(indexFileName);

                _orphanListBox.Items.Clear();
                if (_orphanSchemes.Count == 0)
                {
                    HideOrphanSection();
                    MessageBox.Show("没有发现缺失的方案，索引文件是完整的。", "扫描结果",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PopulateOrphanListBox(_orphanSchemes);
                ShowOrphanSection();
                UpdateOrphanLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("扫描失败：" + ex.Message);
            }
        }

        private void PopulateOrphanListBox(List<IndexFileBackupManager.OrphanSchemeInfo> schemes)
        {
            _orphanListBox.BeginUpdate();
            _orphanListBox.Items.Clear();
            foreach (var s in schemes)
            {
                _orphanListBox.Items.Add(s.SchemeName, true);
            }
            _orphanListBox.EndUpdate();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterOrphanList();
        }

        private void FilterOrphanList()
        {
            if (_orphanSchemes == null || _orphanSchemes.Count == 0) return;

            string filter = _searchTextBox.Text.Trim();
            List<IndexFileBackupManager.OrphanSchemeInfo> filtered;

            if (string.IsNullOrEmpty(filter))
            {
                filtered = _orphanSchemes;
            }
            else
            {
                var keywords = filter.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                filtered = _orphanSchemes.Where(s =>
                    keywords.Any(k => s.SchemeName.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            _orphanListBox.BeginUpdate();
            _orphanListBox.Items.Clear();
            foreach (var s in filtered)
            {
                _orphanListBox.Items.Add(s.SchemeName, true);
            }
            _orphanListBox.EndUpdate();

            _isUpdatingSelectAll = true;
            _selectAllCheckBox.Checked = filtered.Count > 0 && GetCheckedCount(filtered) == filtered.Count;
            _isUpdatingSelectAll = false;

            UpdateOrphanLabel();
        }

        private int GetCheckedCount(List<IndexFileBackupManager.OrphanSchemeInfo> schemes)
        {
            if (_orphanSchemes == null) return 0;
            var visibleSet = new HashSet<string>(schemes.Select(s => s.SchemeName), StringComparer.OrdinalIgnoreCase);
            int count = 0;
            for (int i = 0; i < _orphanListBox.Items.Count; i++)
            {
                string itemText = _orphanListBox.Items[i].ToString();
                if (visibleSet.Contains(itemText) && _orphanListBox.GetItemChecked(i))
                    count++;
            }
            return count;
        }

        private void SelectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingSelectAll) return;

            bool check = _selectAllCheckBox.Checked;
            _orphanListBox.BeginUpdate();
            for (int i = 0; i < _orphanListBox.Items.Count; i++)
            {
                _orphanListBox.SetItemChecked(i, check);
            }
            _orphanListBox.EndUpdate();
            UpdateOrphanLabel();
        }

        private void AddToIndexButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_orphanSchemes == null || _orphanSchemes.Count == 0) return;

                string indexFileName = _indexFileComboBox.SelectedValue == null ? null : _indexFileComboBox.SelectedValue.ToString();
                if (string.IsNullOrEmpty(indexFileName)) return;

                var selected = new List<IndexFileBackupManager.OrphanSchemeInfo>();
                var visibleItems = new HashSet<string>(
                    _orphanListBox.Items.Cast<object>().Select(x => x.ToString()),
                    StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < _orphanListBox.Items.Count; i++)
                {
                    if (_orphanListBox.GetItemChecked(i))
                    {
                        string name = _orphanListBox.Items[i].ToString();
                        var scheme = _orphanSchemes.Find(s =>
                            string.Equals(s.SchemeName, name, StringComparison.OrdinalIgnoreCase));
                        if (scheme != null)
                            selected.Add(scheme);
                    }
                }

                if (selected.Count == 0)
                {
                    MessageBox.Show("请至少勾选一个方案！");
                    return;
                }

                if (MessageBox.Show(
                    "确认将以下 " + selected.Count + " 个方案写入索引文件？\n\n" +
                    string.Join("\n", selected.ConvertAll(s => "  " + s.SchemeName)) +
                    "\n\n写入后需要重启 MuMu 模拟器才能看到。",
                    "确认补全", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                int added = _indexBackup.AddSchemesToIndex(indexFileName, selected);
                if (added > 0)
                {
                    _orphanSchemes.RemoveAll(s => selected.Contains(s));
                    PopulateOrphanListBox(_orphanSchemes);

                    if (_orphanSchemes.Count == 0)
                    {
                        HideOrphanSection();
                    }
                    else
                    {
                        _isUpdatingSelectAll = true;
                        _selectAllCheckBox.Checked = true;
                        _isUpdatingSelectAll = false;
                        UpdateOrphanLabel();
                    }

                    MessageBox.Show("已成功将 " + added + " 个方案写入索引文件！\n请重启 MuMu 模拟器以使更改生效。",
                        "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("写入失败，索引文件可能被锁定。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private void OrphanListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                UpdateOrphanLabel();
                UpdateSelectAllState();
            }));
        }

        private void UpdateSelectAllState()
        {
            if (_isUpdatingSelectAll) return;
            int total = _orphanListBox.Items.Count;
            int checkedCount = 0;
            for (int i = 0; i < total; i++)
            {
                if (_orphanListBox.GetItemChecked(i)) checkedCount++;
            }
            _isUpdatingSelectAll = true;
            _selectAllCheckBox.Checked = total > 0 && checkedCount == total;
            _isUpdatingSelectAll = false;
        }

        private void UpdateOrphanLabel()
        {
            int total = _orphanListBox.Items.Count;
            int checkedCount = 0;
            for (int i = 0; i < total; i++)
            {
                if (_orphanListBox.GetItemChecked(i)) checkedCount++;
            }
            _orphanLabel.Text = string.Format("缺失的方案（{0}/{1} 已勾选）", checkedCount, total);
        }

        private void BackupListView_DoubleClick(object sender, EventArgs e)
        {
            if (_backupListView.SelectedItems.Count == 0) return;
            var selected = _backupListView.SelectedItems[0];
            string timestamp = selected.SubItems[0].Text;
            string indexFileName = _indexFileComboBox.SelectedValue == null ? null : _indexFileComboBox.SelectedValue.ToString();
            if (string.IsNullOrEmpty(indexFileName)) return;
            var schemes = _indexBackup.GetSchemesInBackup(indexFileName, timestamp);
            MessageBox.Show(string.Join("\n", schemes), "备份详情 - " + timestamp,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BackupListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            _restoreButton.Enabled = (_backupListView.SelectedItems.Count > 0);
        }

        private void ManualBackupButton_Click(object sender, EventArgs e)
        {
            try
            {
                string indexFileName = _indexFileComboBox.SelectedValue == null ? null : _indexFileComboBox.SelectedValue.ToString();
                if (string.IsNullOrEmpty(indexFileName))
                {
                    MessageBox.Show("请先在下拉框选择一个索引文件！");
                    return;
                }

                if (_indexBackup.BackupIndexDirect(indexFileName))
                    MessageBox.Show("索引备份成功！");
                else
                    MessageBox.Show("备份失败（索引不存在或内容未变化）。");
                RefreshBackupList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_backupListView.SelectedItems.Count == 0) return;
                string timestamp = _backupListView.SelectedItems[0].SubItems[0].Text;
                string indexFileName = _indexFileComboBox.SelectedValue == null ? null : _indexFileComboBox.SelectedValue.ToString();
                if (string.IsNullOrEmpty(indexFileName)) return;
                if (MessageBox.Show(
                    "确认还原索引文件 " + indexFileName + " 到备份时间 " + timestamp + "？",
                    "确认还原", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                if (_indexBackup.RestoreBackup(indexFileName, timestamp))
                {
                    _resetStatus();
                    MessageBox.Show("还原成功！请重启 MuMu 模拟器以使更改生效。",
                        "还原成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshBackupList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private void CleanupButton_Click(object sender, EventArgs e)
        {
            try
            {
                _indexBackup.CleanupOldBackups(MaxBackupCount);
                RefreshBackupList();
                MessageBox.Show("已清理旧备份，每个索引最多保留最近10个备份。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private void RefreshBackupList()
        {
            PopulateIndexFileComboBox();
            string selectedIndex = _indexFileComboBox.SelectedValue == null ? null : _indexFileComboBox.SelectedValue.ToString();
            _scanOrphansButton.Enabled = !string.IsNullOrEmpty(selectedIndex);
            if (string.IsNullOrEmpty(selectedIndex))
            {
                _backupListView.Items.Clear();
                _backupTipLabel.Text = "暂无备份记录。每次保存按键方案时会自动备份对应索引文件。";
                _backupTipLabel.Visible = true;
                _manualBackupButton.Enabled = false;
                return;
            }
            _manualBackupButton.Enabled = true;
            var backups = _indexBackup.GetBackups(selectedIndex);
            if (backups.Count == 0)
            {
                _backupListView.Items.Clear();
                _backupTipLabel.Text = "暂无备份记录。每次保存按键方案时会自动备份对应索引文件。";
                _backupTipLabel.Visible = true;
                _restoreButton.Enabled = false;
                return;
            }
            _backupTipLabel.Visible = false;
            _backupListView.BeginUpdate();
            _backupListView.Items.Clear();
            foreach (var b in backups)
            {
                string preview = b.Schemes.Count > 3
                    ? string.Join(", ", b.Schemes.GetRange(0, 3)) + " ..."
                    : string.Join(", ", b.Schemes);
                var item = new ListViewItem(b.Timestamp);
                item.SubItems.Add(b.SchemeCount.ToString());
                item.SubItems.Add(preview);
                _backupListView.Items.Add(item);
            }
            _backupListView.EndUpdate();
            _restoreButton.Enabled = false;
        }

        private void PopulateIndexFileComboBox()
        {
            _isPopulatingComboBox = true;
            var available = _indexBackup.GetAvailableIndexFiles();
            var items = new List<KeyValuePair<string, string>>();
            foreach (var file in available)
            {
                string baseName = System.IO.Path.GetFileNameWithoutExtension(file);
                string displayName = file;
                foreach (var kv in _packageNameTypes)
                {
                    string prefix = kv.Value.TrimEnd('-');
                    if (baseName == prefix)
                    {
                        displayName = kv.Key + " (" + baseName + ")";
                        break;
                    }
                }
                items.Add(new KeyValuePair<string, string>(displayName, file));
            }
            string previousValue = _indexFileComboBox.SelectedValue == null ? null : _indexFileComboBox.SelectedValue.ToString();
            _indexFileComboBox.DataSource = null;
            _indexFileComboBox.DisplayMember = "Key";
            _indexFileComboBox.ValueMember = "Value";
            _indexFileComboBox.DataSource = items;
            if (!string.IsNullOrEmpty(previousValue) && items.FindIndex(x => x.Value == previousValue) >= 0)
                _indexFileComboBox.SelectedValue = previousValue;
            else if (items.Count > 0)
            {
                string selectedPkg = _getSelectedPackage?.Invoke();
                string matchedIndex = null;
                if (!string.IsNullOrEmpty(selectedPkg))
                {
                    string target = selectedPkg.TrimEnd('-') + ".json";
                    matchedIndex = items.Find(x =>
                        string.Equals(x.Value, target, StringComparison.OrdinalIgnoreCase)).Value;
                }
                if (matchedIndex == null)
                {
                    string currentPath = _getCurrentFilePath();
                    if (!string.IsNullOrEmpty(currentPath))
                    {
                        matchedIndex = _indexBackup.ResolveIndexFilePath(currentPath);
                    }
                }
                if (matchedIndex != null && items.FindIndex(x => x.Value == matchedIndex) >= 0)
                    _indexFileComboBox.SelectedValue = matchedIndex;
            }
            _isPopulatingComboBox = false;
        }

        private void TopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = _topCheckBox.Checked;
        }
    }
}
