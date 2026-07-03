namespace MuMu坐标计算
{
    partial class AdbTouchForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox cmbAdbPort;
        private System.Windows.Forms.Button btnAutoDetectPort;
        private System.Windows.Forms.Button btnResetAdbConfig;
        private System.Windows.Forms.TextBox txtAdbPath;
        private System.Windows.Forms.Button btnBrowseAdb;
        private System.Windows.Forms.Button btnAdbConnect;
        private System.Windows.Forms.Button btnAdbStop;
        private System.Windows.Forms.Label lblAdbStatus;
        private System.Windows.Forms.ListView lvTouchCoords;
        private System.Windows.Forms.Button btnAdbApply;
        private System.Windows.Forms.Button btnAdbClear;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.CheckBox _topCheckBox;
        private System.Windows.Forms.CheckBox _generateOnceCheckBox;
        private System.Windows.Forms.CheckBox _generateMultipleCheckBox;
        private System.Windows.Forms.CheckBox _generateSelectedCheckBox;
        private System.Windows.Forms.ComboBox _keyTypeComboBox;
        private System.Windows.Forms.Label _lblGenerateTip;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbAdbPort = new System.Windows.Forms.ComboBox();
            this.btnAutoDetectPort = new System.Windows.Forms.Button();
            this.txtAdbPath = new System.Windows.Forms.TextBox();
            this.btnBrowseAdb = new System.Windows.Forms.Button();
            this.btnAdbConnect = new System.Windows.Forms.Button();
            this.btnAdbStop = new System.Windows.Forms.Button();
            this.lblAdbStatus = new System.Windows.Forms.Label();
            this.lvTouchCoords = new System.Windows.Forms.ListView();
            this.btnAdbApply = new System.Windows.Forms.Button();
            this.btnAdbClear = new System.Windows.Forms.Button();
            this.lblResolution = new System.Windows.Forms.Label();
            this.lblAdbPath = new System.Windows.Forms.Label();
            this.lblInstance = new System.Windows.Forms.Label();
            this.lblPortInput = new System.Windows.Forms.Label();
            this.txtPortInput = new System.Windows.Forms.TextBox();
            this.btnResetAdbConfig = new System.Windows.Forms.Button();
            this._generateOnceCheckBox = new System.Windows.Forms.CheckBox();
            this._generateMultipleCheckBox = new System.Windows.Forms.CheckBox();
            this._lblGenerateTip = new System.Windows.Forms.Label();
            this._topCheckBox = new System.Windows.Forms.CheckBox();
            this._generateSelectedCheckBox = new System.Windows.Forms.CheckBox();
            this._keyTypeComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbAdbPort
            // 
            this.cmbAdbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAdbPort.Location = new System.Drawing.Point(52, 64);
            this.cmbAdbPort.Name = "cmbAdbPort";
            this.cmbAdbPort.Size = new System.Drawing.Size(170, 20);
            this.cmbAdbPort.TabIndex = 4;
            // 
            // btnAutoDetectPort
            // 
            this.btnAutoDetectPort.Location = new System.Drawing.Point(12, 93);
            this.btnAutoDetectPort.Name = "btnAutoDetectPort";
            this.btnAutoDetectPort.Size = new System.Drawing.Size(75, 25);
            this.btnAutoDetectPort.TabIndex = 7;
            this.btnAutoDetectPort.Text = "自动获取";
            this.btnAutoDetectPort.Click += new System.EventHandler(this.btnAutoDetectPort_Click);
            // 
            // txtAdbPath
            // 
            this.txtAdbPath.Location = new System.Drawing.Point(90, 34);
            this.txtAdbPath.Name = "txtAdbPath";
            this.txtAdbPath.Size = new System.Drawing.Size(270, 21);
            this.txtAdbPath.TabIndex = 1;
            // 
            // btnBrowseAdb
            // 
            this.btnBrowseAdb.Location = new System.Drawing.Point(376, 31);
            this.btnBrowseAdb.Name = "btnBrowseAdb";
            this.btnBrowseAdb.Size = new System.Drawing.Size(60, 25);
            this.btnBrowseAdb.TabIndex = 2;
            this.btnBrowseAdb.Text = "浏览";
            this.btnBrowseAdb.Click += new System.EventHandler(this.btnBrowseAdb_Click);
            // 
            // btnAdbConnect
            // 
            this.btnAdbConnect.Location = new System.Drawing.Point(139, 93);
            this.btnAdbConnect.Name = "btnAdbConnect";
            this.btnAdbConnect.Size = new System.Drawing.Size(85, 28);
            this.btnAdbConnect.TabIndex = 9;
            this.btnAdbConnect.Text = "连接并采集";
            this.btnAdbConnect.Click += new System.EventHandler(this.btnAdbConnect_Click);
            // 
            // btnAdbStop
            // 
            this.btnAdbStop.Enabled = false;
            this.btnAdbStop.Location = new System.Drawing.Point(241, 93);
            this.btnAdbStop.Name = "btnAdbStop";
            this.btnAdbStop.Size = new System.Drawing.Size(90, 28);
            this.btnAdbStop.TabIndex = 10;
            this.btnAdbStop.Text = "停止采集";
            this.btnAdbStop.Click += new System.EventHandler(this.btnAdbStop_Click);
            // 
            // lblAdbStatus
            // 
            this.lblAdbStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblAdbStatus.Location = new System.Drawing.Point(12, 128);
            this.lblAdbStatus.Name = "lblAdbStatus";
            this.lblAdbStatus.Size = new System.Drawing.Size(426, 20);
            this.lblAdbStatus.TabIndex = 9;
            this.lblAdbStatus.Text = "未连接";
            // 
            // lvTouchCoords
            // 
            this.lvTouchCoords.BackColor = System.Drawing.Color.White;
            this.lvTouchCoords.ForeColor = System.Drawing.Color.Black;
            this.lvTouchCoords.FullRowSelect = true;
            this.lvTouchCoords.GridLines = true;
            this.lvTouchCoords.HideSelection = false;
            this.lvTouchCoords.Location = new System.Drawing.Point(12, 152);
            this.lvTouchCoords.Name = "lvTouchCoords";
            this.lvTouchCoords.Size = new System.Drawing.Size(426, 190);
            this.lvTouchCoords.TabIndex = 11;
            this.lvTouchCoords.UseCompatibleStateImageBehavior = false;
            this.lvTouchCoords.View = System.Windows.Forms.View.Details;
            this.lvTouchCoords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTouchCoords_MouseDoubleClick);
            // 
            // btnAdbApply
            // 
            this.btnAdbApply.Location = new System.Drawing.Point(12, 350);
            this.btnAdbApply.Name = "btnAdbApply";
            this.btnAdbApply.Size = new System.Drawing.Size(120, 28);
            this.btnAdbApply.TabIndex = 12;
            this.btnAdbApply.Text = "应用到主窗口";
            this.btnAdbApply.Click += new System.EventHandler(this.btnAdbApply_Click);
            // 
            // btnAdbClear
            // 
            this.btnAdbClear.Location = new System.Drawing.Point(140, 350);
            this.btnAdbClear.Name = "btnAdbClear";
            this.btnAdbClear.Size = new System.Drawing.Size(80, 28);
            this.btnAdbClear.TabIndex = 13;
            this.btnAdbClear.Text = "清空列表";
            this.btnAdbClear.Click += new System.EventHandler(this.btnAdbClear_Click);
            // 
            // lblResolution
            // 
            this.lblResolution.ForeColor = System.Drawing.Color.Gray;
            this.lblResolution.Location = new System.Drawing.Point(300, 128);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(138, 20);
            this.lblResolution.TabIndex = 10;
            this.lblResolution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAdbPath
            // 
            this.lblAdbPath.AutoSize = true;
            this.lblAdbPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAdbPath.Location = new System.Drawing.Point(12, 35);
            this.lblAdbPath.Name = "lblAdbPath";
            this.lblAdbPath.Size = new System.Drawing.Size(64, 17);
            this.lblAdbPath.TabIndex = 0;
            this.lblAdbPath.Text = "ADB 路径:";
            // 
            // lblInstance
            // 
            this.lblInstance.AutoSize = true;
            this.lblInstance.Location = new System.Drawing.Point(12, 67);
            this.lblInstance.Name = "lblInstance";
            this.lblInstance.Size = new System.Drawing.Size(35, 12);
            this.lblInstance.TabIndex = 3;
            this.lblInstance.Text = "实例:";
            // 
            // lblPortInput
            // 
            this.lblPortInput.AutoSize = true;
            this.lblPortInput.Location = new System.Drawing.Point(226, 67);
            this.lblPortInput.Name = "lblPortInput";
            this.lblPortInput.Size = new System.Drawing.Size(35, 12);
            this.lblPortInput.TabIndex = 5;
            this.lblPortInput.Text = "端口:";
            // 
            // txtPortInput
            // 
            this.txtPortInput.Location = new System.Drawing.Point(266, 64);
            this.txtPortInput.Name = "txtPortInput";
            this.txtPortInput.Size = new System.Drawing.Size(55, 21);
            this.txtPortInput.TabIndex = 6;
            // 
            // btnResetAdbConfig
            // 
            this.btnResetAdbConfig.Location = new System.Drawing.Point(102, 93);
            this.btnResetAdbConfig.Name = "btnResetAdbConfig";
            this.btnResetAdbConfig.Size = new System.Drawing.Size(22, 25);
            this.btnResetAdbConfig.TabIndex = 8;
            this.btnResetAdbConfig.Text = "x";
            this.btnResetAdbConfig.Click += new System.EventHandler(this.btnResetAdbConfig_Click);
            // 
            // _generateOnceCheckBox
            // 
            this._generateOnceCheckBox.AutoSize = true;
            this._generateOnceCheckBox.Location = new System.Drawing.Point(12, 388);
            this._generateOnceCheckBox.Name = "_generateOnceCheckBox";
            this._generateOnceCheckBox.Size = new System.Drawing.Size(72, 16);
            this._generateOnceCheckBox.TabIndex = 14;
            this._generateOnceCheckBox.Text = "生成一次";
            this._generateOnceCheckBox.UseVisualStyleBackColor = true;
            // 
            // _generateMultipleCheckBox
            // 
            this._generateMultipleCheckBox.AutoSize = true;
            this._generateMultipleCheckBox.Location = new System.Drawing.Point(113, 388);
            this._generateMultipleCheckBox.Name = "_generateMultipleCheckBox";
            this._generateMultipleCheckBox.Size = new System.Drawing.Size(72, 16);
            this._generateMultipleCheckBox.TabIndex = 15;
            this._generateMultipleCheckBox.Text = "生成多次";
            this._generateMultipleCheckBox.UseVisualStyleBackColor = true;
            // 
            // _lblGenerateTip
            // 
            this._lblGenerateTip.AutoSize = true;
            this._lblGenerateTip.ForeColor = System.Drawing.Color.Red;
            this._lblGenerateTip.Location = new System.Drawing.Point(12, 412);
            this._lblGenerateTip.Name = "_lblGenerateTip";
            this._lblGenerateTip.Size = new System.Drawing.Size(125, 12);
            this._lblGenerateTip.TabIndex = 16;
            this._lblGenerateTip.Text = "提示：已关闭生成功能";
            // 
            // _topCheckBox
            // 
            this._topCheckBox.AutoSize = true;
            this._topCheckBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._topCheckBox.Location = new System.Drawing.Point(11, 9);
            this._topCheckBox.Name = "_topCheckBox";
            this._topCheckBox.Size = new System.Drawing.Size(75, 21);
            this._topCheckBox.TabIndex = 0;
            this._topCheckBox.Text = "窗口置顶";
            this._topCheckBox.UseVisualStyleBackColor = true;
            // 
            // _generateSelectedCheckBox
            // 
            this._generateSelectedCheckBox.AutoSize = true;
            this._generateSelectedCheckBox.Location = new System.Drawing.Point(209, 388);
            this._generateSelectedCheckBox.Name = "_generateSelectedCheckBox";
            this._generateSelectedCheckBox.Size = new System.Drawing.Size(72, 16);
            this._generateSelectedCheckBox.TabIndex = 17;
            this._generateSelectedCheckBox.Text = "生成选中";
            this._generateSelectedCheckBox.UseVisualStyleBackColor = true;
            // 
            // _keyTypeComboBox
            // 
            this._keyTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._keyTypeComboBox.FormattingEnabled = true;
            this._keyTypeComboBox.Location = new System.Drawing.Point(305, 386);
            this._keyTypeComboBox.Name = "_keyTypeComboBox";
            this._keyTypeComboBox.Size = new System.Drawing.Size(90, 20);
            this._keyTypeComboBox.TabIndex = 18;
            // 
            // AdbTouchForm
            // 
            this.ClientSize = new System.Drawing.Size(449, 431);
            this.Controls.Add(this._topCheckBox);
            this.Controls.Add(this.lblAdbPath);
            this.Controls.Add(this.txtAdbPath);
            this.Controls.Add(this.btnBrowseAdb);
            this.Controls.Add(this.lblInstance);
            this.Controls.Add(this.cmbAdbPort);
            this.Controls.Add(this.lblPortInput);
            this.Controls.Add(this.txtPortInput);
            this.Controls.Add(this.btnAutoDetectPort);
            this.Controls.Add(this.btnResetAdbConfig);
            this.Controls.Add(this.btnAdbConnect);
            this.Controls.Add(this.btnAdbStop);
            this.Controls.Add(this.lblAdbStatus);
            this.Controls.Add(this.lblResolution);
            this.Controls.Add(this.lvTouchCoords);
            this.Controls.Add(this.btnAdbApply);
            this.Controls.Add(this.btnAdbClear);
            this.Controls.Add(this._generateOnceCheckBox);
            this.Controls.Add(this._generateMultipleCheckBox);
            this.Controls.Add(this._generateSelectedCheckBox);
            this.Controls.Add(this._keyTypeComboBox);
            this.Controls.Add(this._lblGenerateTip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::MuMu坐标计算.Properties.Resources.AppIcon;
            this.MaximizeBox = false;
            this.Name = "AdbTouchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ADB 触控采集";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblAdbPath;
        private System.Windows.Forms.Label lblInstance;
        private System.Windows.Forms.Label lblPortInput;
        private System.Windows.Forms.TextBox txtPortInput;
    }
}
