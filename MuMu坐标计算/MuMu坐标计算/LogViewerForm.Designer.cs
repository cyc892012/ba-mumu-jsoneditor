using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class LogViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListView _lvLogs;
        private Button _btnOpenFolder;
        private Button _btnViewLog;
        private Button _btnCopyPath;
        private Button _btnDeleteLog;
        private Label _lblInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            _lvLogs = new ListView();
            _lvLogs.Location = new Point(12, 12);
            _lvLogs.Size = new Size(600, 280);
            _lvLogs.View = View.Details;
            _lvLogs.FullRowSelect = true;
            _lvLogs.GridLines = true;
            _lvLogs.Columns.Add("文件名", 200);
            _lvLogs.Columns.Add("大小", 80);
            _lvLogs.Columns.Add("创建时间", 300);

            _btnOpenFolder = new Button();
            _btnOpenFolder.Location = new Point(12, 302);
            _btnOpenFolder.Size = new Size(140, 30);
            _btnOpenFolder.Text = "打开日志文件夹";
            _btnOpenFolder.UseVisualStyleBackColor = true;

            _btnViewLog = new Button();
            _btnViewLog.Location = new Point(162, 302);
            _btnViewLog.Size = new Size(140, 30);
            _btnViewLog.Text = "查看选中日志";
            _btnViewLog.UseVisualStyleBackColor = true;
            _btnViewLog.Enabled = false;

            _btnCopyPath = new Button();
            _btnCopyPath.Location = new Point(312, 302);
            _btnCopyPath.Size = new Size(140, 30);
            _btnCopyPath.Text = "复制日志路径";
            _btnCopyPath.UseVisualStyleBackColor = true;
            _btnCopyPath.Enabled = false;

            _btnDeleteLog = new Button();
            _btnDeleteLog.Location = new Point(462, 302);
            _btnDeleteLog.Size = new Size(150, 30);
            _btnDeleteLog.Text = "删除选中日志";
            _btnDeleteLog.UseVisualStyleBackColor = true;
            _btnDeleteLog.Enabled = false;

            _lblInfo = new Label();
            _lblInfo.Location = new Point(12, 340);
            _lblInfo.Size = new Size(600, 30);
            _lblInfo.TextAlign = ContentAlignment.MiddleLeft;
            _lblInfo.ForeColor = Color.Gray;

            this.Text = "日志管理";
            this.Size = new Size(640, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Microsoft YaHei", 9F);
            this.AutoScaleMode = AutoScaleMode.None;

            this.Controls.Add(_lvLogs);
            this.Controls.Add(_btnOpenFolder);
            this.Controls.Add(_btnViewLog);
            this.Controls.Add(_btnCopyPath);
            this.Controls.Add(_btnDeleteLog);
            this.Controls.Add(_lblInfo);

            this.ResumeLayout(false);
        }
    }
}
