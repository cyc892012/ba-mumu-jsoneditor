using System;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class LogViewerForm : Form
    {
        private readonly string _logDir;
        private static readonly string LogDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public LogViewerForm()
        {
            _logDir = LogDir;
            InitializeComponent();
            _lvLogs.SelectedIndexChanged += (s, e) => UpdateButtonStates();
            _btnOpenFolder.Click += BtnOpenFolder_Click;
            _btnViewLog.Click += BtnViewLog_Click;
            _btnCopyPath.Click += BtnCopyPath_Click;
            _btnDeleteLog.Click += BtnDeleteLog_Click;
            Load += (s, e) => RefreshLogList();
        }

        private void RefreshLogList()
        {
            _lvLogs.Items.Clear();
            try
            {
                if (!System.IO.Directory.Exists(_logDir))
                {
                    _lblInfo.Text = "日志目录不存在: " + _logDir;
                    return;
                }

                var logFiles = new System.Collections.Generic.List<string>(System.IO.Directory.GetFiles(_logDir, "log_*.txt"));
                logFiles.Sort();
                logFiles.Reverse();

                foreach (var file in logFiles)
                {
                    var fi = new System.IO.FileInfo(file);
                    var item = new ListViewItem(fi.Name);
                    item.SubItems.Add(FormatFileSize(fi.Length));
                    item.SubItems.Add(fi.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.Tag = fi.FullName;
                    _lvLogs.Items.Add(item);
                }

                _lblInfo.Text = string.Format("共 {0} 个日志文件 (最多保留10个)    目录: {1}", _lvLogs.Items.Count, _logDir);
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _lblInfo.Text = "加载日志列表失败: " + ex.Message;
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = _lvLogs.SelectedItems.Count > 0;
            _btnViewLog.Enabled = hasSelection;
            _btnCopyPath.Enabled = hasSelection;
            _btnDeleteLog.Enabled = hasSelection;
        }

        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (!System.IO.Directory.Exists(_logDir))
                    System.IO.Directory.CreateDirectory(_logDir);

                using (System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _logDir,
                    UseShellExecute = true,
                    Verb = "open"
                })) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开文件夹失败: " + ex.Message);
            }
        }

        private void BtnViewLog_Click(object sender, EventArgs e)
        {
            if (_lvLogs.SelectedItems.Count == 0) return;
            string filePath = _lvLogs.SelectedItems[0].Tag as string;
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath)) return;

            try
            {
                using (System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = "\"" + filePath + "\"",
                    UseShellExecute = false
                })) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开日志文件失败: " + ex.Message);
            }
        }

        private void BtnCopyPath_Click(object sender, EventArgs e)
        {
            if (_lvLogs.SelectedItems.Count == 0) return;
            string filePath = _lvLogs.SelectedItems[0].Tag as string;
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                Clipboard.SetText(filePath);
                _lblInfo.Text = "已复制: " + filePath;
            }
            catch (Exception ex)
            {
                _lblInfo.Text = "复制失败: " + ex.Message;
            }
        }

        private void BtnDeleteLog_Click(object sender, EventArgs e)
        {
            if (_lvLogs.SelectedItems.Count == 0) return;
            string filePath = _lvLogs.SelectedItems[0].Tag as string;
            string fileName = _lvLogs.SelectedItems[0].Text;

            if (MessageBox.Show("确认删除日志文件 \"" + fileName + "\" ？",
                "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    RefreshLogList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败: " + ex.Message);
                }
            }
        }

        private static string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("F1") + " KB";
            return (bytes / (1024.0 * 1024.0)).ToString("F2") + " MB";
        }
    }
}
