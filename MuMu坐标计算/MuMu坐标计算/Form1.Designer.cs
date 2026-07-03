using System;

namespace MuMu坐标计算
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdSave); } catch { }
                try { HotKey.UnregisterHotKey(Handle, KeyboardBindingHandler.HotKeyIdRecall); } catch { }
                // 卸载键盘绑定处理器
                _keyboardHandler?.Dispose();
                if (_fileMonitor != null) _fileMonitor.FileChanged -= OnExternalFileChanged;
                _fileMonitor?.Dispose();
                if (_syncService != null) _syncService.KeymapChanged -= OnMuMuKeymapDetected;
                _syncService?.Dispose();
                _featureToolTip?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.KXtextBox = new System.Windows.Forms.TextBox();
            this.KXlabel = new System.Windows.Forms.Label();
            this.KY1label = new System.Windows.Forms.Label();
            this.KYtextBox = new System.Windows.Forms.TextBox();
            this.FYlabel = new System.Windows.Forms.Label();
            this.FYtextBox = new System.Windows.Forms.TextBox();
            this.FXtextBox = new System.Windows.Forms.TextBox();
            this.JSYlabel = new System.Windows.Forms.Label();
            this.JSYtextBox = new System.Windows.Forms.TextBox();
            this.JSXlabel = new System.Windows.Forms.Label();
            this.JSXtextBox = new System.Windows.Forms.TextBox();
            this.FcheckBox = new System.Windows.Forms.CheckBox();
            this.JScheckBox = new System.Windows.Forms.CheckBox();
            this.KcheckBox = new System.Windows.Forms.CheckBox();
            this.FSave = new System.Windows.Forms.Button();
            this.FLoad = new System.Windows.Forms.Button();
            this.TOPcheckBox = new System.Windows.Forms.CheckBox();
            this.NCXtextBox = new System.Windows.Forms.TextBox();
            this.lblMouseCurrentX = new System.Windows.Forms.Label();
            this.lblMouseCurrentY = new System.Windows.Forms.Label();
            this.NCYtextBox = new System.Windows.Forms.TextBox();
            this.lblMouseSavedX = new System.Windows.Forms.Label();
            this.SCXtextBox = new System.Windows.Forms.TextBox();
            this.lblMouseSavedY = new System.Windows.Forms.Label();
            this.SCYtextBox = new System.Windows.Forms.TextBox();
            this.CcheckBox = new System.Windows.Forms.CheckBox();
            this.Ctimer = new System.Windows.Forms.Timer(this.components);
            this.JsonopenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.JsonUrltextBox = new System.Windows.Forms.TextBox();
            this.lblBindKey = new System.Windows.Forms.Label();
            this.ButtontextBox = new System.Windows.Forms.TextBox();
            this.CheckButton = new System.Windows.Forms.Button();
            this.OpenJson = new System.Windows.Forms.Button();
            this.RewriteAndSaveButton = new System.Windows.Forms.Button();
            this.EcheckBox = new System.Windows.Forms.CheckBox();
            this.lblCtrlPrefix1 = new System.Windows.Forms.Label();
            this.lblCtrlPrefix2 = new System.Windows.Forms.Label();
            this.FindKeytextBox = new System.Windows.Forms.TextBox();
            this.ResetKeytextBox = new System.Windows.Forms.TextBox();
            this.SaveKeybutton = new System.Windows.Forms.Button();
            this.LoadKeybutton = new System.Windows.Forms.Button();
            this.lblSaveMouseCoord = new System.Windows.Forms.Label();
            this.lblMoveMouseToSaved = new System.Windows.Forms.Label();
            this.ReadPPButton = new System.Windows.Forms.Button();
            this.FunctiontabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblFeatureCaption = new System.Windows.Forms.Label();
            this.btnFeaturePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.featureBtnMouse = new System.Windows.Forms.Button();
            this.featureBtnRange = new System.Windows.Forms.Button();
            this.featureBtnTouch = new System.Windows.Forms.Button();
            this.featureBtnWide = new System.Windows.Forms.Button();
            this.featureBtnBackup = new System.Windows.Forms.Button();
            this.featureBtnKeyPreset = new System.Windows.Forms.Button();
            this.featureBtnLog = new System.Windows.Forms.Button();
            this.DeleteRangeLTRDkeysButton = new System.Windows.Forms.Button();
            this.lblRangeClearTip = new System.Windows.Forms.Label();
            this.RangeRDXtextBox = new System.Windows.Forms.TextBox();
            this.RangeRDYtextBox = new System.Windows.Forms.TextBox();
            this.lblRangeLTX = new System.Windows.Forms.Label();
            this.RangeLTXtextBox = new System.Windows.Forms.TextBox();
            this.RangeLTYtextBox = new System.Windows.Forms.TextBox();
            this.lblRangeLTY = new System.Windows.Forms.Label();
            this.lblRangeRDX = new System.Windows.Forms.Label();
            this.lblRangeRDY = new System.Windows.Forms.Label();
            this.Tip2label = new System.Windows.Forms.Label();
            this.Tip1label = new System.Windows.Forms.Label();
            this.lblAdminTip = new System.Windows.Forms.Label();
            this.nXtextBox = new System.Windows.Forms.TextBox();
            this.lblColon3 = new System.Windows.Forms.Label();
            this.nYtextBox = new System.Windows.Forms.TextBox();
            this.lblInternalCoord = new System.Windows.Forms.Label();
            this.mXtextBox = new System.Windows.Forms.TextBox();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.mYtextBox = new System.Windows.Forms.TextBox();
            this.lblCurrentMouseCoord = new System.Windows.Forms.Label();
            this.SXtextBox = new System.Windows.Forms.TextBox();
            this.btnGetScreenResolution = new System.Windows.Forms.Button();
            this.lblScreenResX = new System.Windows.Forms.Label();
            this.SYtextBox = new System.Windows.Forms.TextBox();
            this.lblDesktopRes = new System.Windows.Forms.Label();
            this.lblFullScreenTip = new System.Windows.Forms.Label();
            this.ktckAPWritebutton = new System.Windows.Forms.Button();
            this.ktckOPWritebutton = new System.Windows.Forms.Button();
            this.ktckReadbutton = new System.Windows.Forms.Button();
            this.ktckPListcheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.fileNamecomboBox2 = new System.Windows.Forms.ComboBox();
            this.packageNamecomboBox2 = new System.Windows.Forms.ComboBox();
            this.resolutioncomboBox2 = new System.Windows.Forms.ComboBox();
            this.resolutionTypecomboBox2 = new System.Windows.Forms.ComboBox();
            this.ktckCKXtextBox = new System.Windows.Forms.TextBox();
            this.lblColon4 = new System.Windows.Forms.Label();
            this.ktckCKYtextBox = new System.Windows.Forms.TextBox();
            this.lblWideScreenCoord = new System.Windows.Forms.Label();
            this.ktckKXtextBox = new System.Windows.Forms.TextBox();
            this.lblColon2 = new System.Windows.Forms.Label();
            this.ktckKYtextBox = new System.Windows.Forms.TextBox();
            this.lblSourceRes = new System.Windows.Forms.Label();
            this.lblWideScreenTip = new System.Windows.Forms.Label();
            this.fileNameSearchtextBox2 = new System.Windows.Forms.TextBox();
            this.lblSelectedCoord = new System.Windows.Forms.Label();
            this.keyTypelistcomboBox = new System.Windows.Forms.ComboBox();
            this.Undobutton = new System.Windows.Forms.Button();
            this.Redobutton = new System.Windows.Forms.Button();
            this.OpenJsonFolderbutton = new System.Windows.Forms.Button();
            this.replaceKeycheckBox = new System.Windows.Forms.CheckBox();
            this.packageNamecomboBox = new System.Windows.Forms.ComboBox();
            this.TryGetJsonFileFolderbutton = new System.Windows.Forms.Button();
            this.resolutionTypecomboBox = new System.Windows.Forms.ComboBox();
            this.resolutioncomboBox = new System.Windows.Forms.ComboBox();
            this.deleteUDResolutionbutton = new System.Windows.Forms.Button();
            this.Ktimer = new System.Windows.Forms.Timer(this.components);
            this._indexCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.gbFileAndResolution = new System.Windows.Forms.GroupBox();
            this.searchFileCombo = new MuMu坐标计算.SearchableComboBox();
            this.autoSyncCheckBox = new System.Windows.Forms.CheckBox();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblResScheme = new System.Windows.Forms.Label();
            this.lblResolution = new System.Windows.Forms.Label();
            this.gbKeyEdit = new System.Windows.Forms.GroupBox();
            this.lblKeyType = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.adbBtn = new System.Windows.Forms.ToolStripButton();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusAuthor = new System.Windows.Forms.ToolStripStatusLabel();
            this.FunctiontabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.btnFeaturePanel.SuspendLayout();
            this.gbFileAndResolution.SuspendLayout();
            this.gbKeyEdit.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // KXtextBox
            // 
            this.KXtextBox.Location = new System.Drawing.Point(119, 51);
            this.KXtextBox.Name = "KXtextBox";
            this.KXtextBox.Size = new System.Drawing.Size(100, 21);
            this.KXtextBox.TabIndex = 0;
            this.KXtextBox.Text = "0";
            this.KXtextBox.TextChanged += new System.EventHandler(this.KtextBox_TextChanged);
            this.KXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // KXlabel
            // 
            this.KXlabel.AutoSize = true;
            this.KXlabel.Location = new System.Drawing.Point(6, 54);
            this.KXlabel.Name = "KXlabel";
            this.KXlabel.Size = new System.Drawing.Size(107, 12);
            this.KXlabel.TabIndex = 1;
            this.KXlabel.Text = "开发者模式坐标X：";
            // 
            // KY1label
            // 
            this.KY1label.AutoSize = true;
            this.KY1label.Location = new System.Drawing.Point(6, 87);
            this.KY1label.Name = "KY1label";
            this.KY1label.Size = new System.Drawing.Size(107, 12);
            this.KY1label.TabIndex = 3;
            this.KY1label.Text = "开发者模式坐标Y：";
            // 
            // KYtextBox
            // 
            this.KYtextBox.Location = new System.Drawing.Point(119, 84);
            this.KYtextBox.Name = "KYtextBox";
            this.KYtextBox.Size = new System.Drawing.Size(100, 21);
            this.KYtextBox.TabIndex = 2;
            this.KYtextBox.Text = "0";
            this.KYtextBox.TextChanged += new System.EventHandler(this.KtextBox_TextChanged);
            this.KYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // FYlabel
            // 
            this.FYlabel.AutoSize = true;
            this.FYlabel.Location = new System.Drawing.Point(318, 77);
            this.FYlabel.Name = "FYlabel";
            this.FYlabel.Size = new System.Drawing.Size(11, 12);
            this.FYlabel.TabIndex = 7;
            this.FYlabel.Text = "x";
            // 
            // FYtextBox
            // 
            this.FYtextBox.Location = new System.Drawing.Point(332, 74);
            this.FYtextBox.Name = "FYtextBox";
            this.FYtextBox.Size = new System.Drawing.Size(42, 21);
            this.FYtextBox.TabIndex = 6;
            this.FYtextBox.Text = "720";
            this.FYtextBox.TextChanged += new System.EventHandler(this.FtextBox_TextChanged);
            this.FYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // FXtextBox
            // 
            this.FXtextBox.Location = new System.Drawing.Point(272, 74);
            this.FXtextBox.Name = "FXtextBox";
            this.FXtextBox.Size = new System.Drawing.Size(42, 21);
            this.FXtextBox.TabIndex = 4;
            this.FXtextBox.Text = "1280";
            this.FXtextBox.TextChanged += new System.EventHandler(this.FtextBox_TextChanged);
            this.FXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // JSYlabel
            // 
            this.JSYlabel.AutoSize = true;
            this.JSYlabel.Location = new System.Drawing.Point(278, 84);
            this.JSYlabel.Name = "JSYlabel";
            this.JSYlabel.Size = new System.Drawing.Size(95, 12);
            this.JSYlabel.TabIndex = 11;
            this.JSYlabel.Text = "Json文件坐标Y：";
            // 
            // JSYtextBox
            // 
            this.JSYtextBox.Location = new System.Drawing.Point(391, 81);
            this.JSYtextBox.Name = "JSYtextBox";
            this.JSYtextBox.Size = new System.Drawing.Size(151, 21);
            this.JSYtextBox.TabIndex = 10;
            this.JSYtextBox.Text = "0";
            this.JSYtextBox.TextChanged += new System.EventHandler(this.JStextBox_TextChanged);
            this.JSYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // JSXlabel
            // 
            this.JSXlabel.AutoSize = true;
            this.JSXlabel.Location = new System.Drawing.Point(278, 51);
            this.JSXlabel.Name = "JSXlabel";
            this.JSXlabel.Size = new System.Drawing.Size(95, 12);
            this.JSXlabel.TabIndex = 9;
            this.JSXlabel.Text = "Json文件坐标X：";
            // 
            // JSXtextBox
            // 
            this.JSXtextBox.Location = new System.Drawing.Point(391, 48);
            this.JSXtextBox.Name = "JSXtextBox";
            this.JSXtextBox.Size = new System.Drawing.Size(151, 21);
            this.JSXtextBox.TabIndex = 8;
            this.JSXtextBox.Text = "0";
            this.JSXtextBox.TextChanged += new System.EventHandler(this.JStextBox_TextChanged);
            this.JSXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // FcheckBox
            // 
            this.FcheckBox.AutoSize = true;
            this.FcheckBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FcheckBox.Location = new System.Drawing.Point(521, 74);
            this.FcheckBox.Name = "FcheckBox";
            this.FcheckBox.Size = new System.Drawing.Size(51, 21);
            this.FcheckBox.TabIndex = 12;
            this.FcheckBox.Text = "锁定";
            this.FcheckBox.UseVisualStyleBackColor = true;
            this.FcheckBox.CheckStateChanged += new System.EventHandler(this.FcheckBox_CheckStateChanged);
            // 
            // JScheckBox
            // 
            this.JScheckBox.AutoSize = true;
            this.JScheckBox.Location = new System.Drawing.Point(280, 25);
            this.JScheckBox.Name = "JScheckBox";
            this.JScheckBox.Size = new System.Drawing.Size(48, 16);
            this.JScheckBox.TabIndex = 13;
            this.JScheckBox.Text = "锁定";
            this.JScheckBox.UseVisualStyleBackColor = true;
            this.JScheckBox.CheckedChanged += new System.EventHandler(this.JScheckBox_CheckedChanged);
            // 
            // KcheckBox
            // 
            this.KcheckBox.AutoSize = true;
            this.KcheckBox.Location = new System.Drawing.Point(8, 25);
            this.KcheckBox.Name = "KcheckBox";
            this.KcheckBox.Size = new System.Drawing.Size(48, 16);
            this.KcheckBox.TabIndex = 14;
            this.KcheckBox.Text = "锁定";
            this.KcheckBox.UseVisualStyleBackColor = true;
            this.KcheckBox.CheckedChanged += new System.EventHandler(this.KcheckBox_CheckedChanged);
            // 
            // FSave
            // 
            this.FSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FSave.Location = new System.Drawing.Point(376, 72);
            this.FSave.Name = "FSave";
            this.FSave.Size = new System.Drawing.Size(40, 25);
            this.FSave.TabIndex = 15;
            this.FSave.Text = "保存";
            this.FSave.UseVisualStyleBackColor = true;
            this.FSave.Click += new System.EventHandler(this.FSave_Click);
            // 
            // FLoad
            // 
            this.FLoad.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FLoad.Location = new System.Drawing.Point(423, 72);
            this.FLoad.Name = "FLoad";
            this.FLoad.Size = new System.Drawing.Size(40, 25);
            this.FLoad.TabIndex = 16;
            this.FLoad.Text = "读取";
            this.FLoad.UseVisualStyleBackColor = true;
            this.FLoad.Click += new System.EventHandler(this.FLoad_Click_1);
            // 
            // TOPcheckBox
            // 
            this.TOPcheckBox.AutoSize = true;
            this.TOPcheckBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TOPcheckBox.Location = new System.Drawing.Point(583, 74);
            this.TOPcheckBox.Name = "TOPcheckBox";
            this.TOPcheckBox.Size = new System.Drawing.Size(75, 21);
            this.TOPcheckBox.TabIndex = 18;
            this.TOPcheckBox.Text = "窗口置顶";
            this.TOPcheckBox.UseVisualStyleBackColor = true;
            this.TOPcheckBox.CheckedChanged += new System.EventHandler(this.TOPcheckBox_CheckedChanged);
            // 
            // NCXtextBox
            // 
            this.NCXtextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.NCXtextBox.Location = new System.Drawing.Point(108, 57);
            this.NCXtextBox.Name = "NCXtextBox";
            this.NCXtextBox.Size = new System.Drawing.Size(42, 21);
            this.NCXtextBox.TabIndex = 19;
            this.NCXtextBox.Text = "0";
            this.NCXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // lblMouseCurrentX
            // 
            this.lblMouseCurrentX.AutoSize = true;
            this.lblMouseCurrentX.Location = new System.Drawing.Point(7, 60);
            this.lblMouseCurrentX.Name = "lblMouseCurrentX";
            this.lblMouseCurrentX.Size = new System.Drawing.Size(95, 12);
            this.lblMouseCurrentX.TabIndex = 20;
            this.lblMouseCurrentX.Text = "当前鼠标坐标X：";
            // 
            // lblMouseCurrentY
            // 
            this.lblMouseCurrentY.AutoSize = true;
            this.lblMouseCurrentY.Location = new System.Drawing.Point(7, 91);
            this.lblMouseCurrentY.Name = "lblMouseCurrentY";
            this.lblMouseCurrentY.Size = new System.Drawing.Size(95, 12);
            this.lblMouseCurrentY.TabIndex = 22;
            this.lblMouseCurrentY.Text = "当前鼠标坐标Y：";
            // 
            // NCYtextBox
            // 
            this.NCYtextBox.Location = new System.Drawing.Point(108, 88);
            this.NCYtextBox.Name = "NCYtextBox";
            this.NCYtextBox.Size = new System.Drawing.Size(42, 21);
            this.NCYtextBox.TabIndex = 21;
            this.NCYtextBox.Text = "0";
            this.NCYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // lblMouseSavedX
            // 
            this.lblMouseSavedX.AutoSize = true;
            this.lblMouseSavedX.Location = new System.Drawing.Point(166, 60);
            this.lblMouseSavedX.Name = "lblMouseSavedX";
            this.lblMouseSavedX.Size = new System.Drawing.Size(71, 12);
            this.lblMouseSavedX.TabIndex = 24;
            this.lblMouseSavedX.Text = "保存坐标X：";
            // 
            // SCXtextBox
            // 
            this.SCXtextBox.Location = new System.Drawing.Point(240, 57);
            this.SCXtextBox.Name = "SCXtextBox";
            this.SCXtextBox.Size = new System.Drawing.Size(42, 21);
            this.SCXtextBox.TabIndex = 23;
            this.SCXtextBox.Text = "0";
            this.SCXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // lblMouseSavedY
            // 
            this.lblMouseSavedY.AutoSize = true;
            this.lblMouseSavedY.Location = new System.Drawing.Point(166, 91);
            this.lblMouseSavedY.Name = "lblMouseSavedY";
            this.lblMouseSavedY.Size = new System.Drawing.Size(71, 12);
            this.lblMouseSavedY.TabIndex = 26;
            this.lblMouseSavedY.Text = "保存坐标Y：";
            // 
            // SCYtextBox
            // 
            this.SCYtextBox.Location = new System.Drawing.Point(240, 88);
            this.SCYtextBox.Name = "SCYtextBox";
            this.SCYtextBox.Size = new System.Drawing.Size(42, 21);
            this.SCYtextBox.TabIndex = 25;
            this.SCYtextBox.Text = "0";
            this.SCYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // CcheckBox
            // 
            this.CcheckBox.AutoSize = true;
            this.CcheckBox.Location = new System.Drawing.Point(9, 37);
            this.CcheckBox.Name = "CcheckBox";
            this.CcheckBox.Size = new System.Drawing.Size(120, 16);
            this.CcheckBox.TabIndex = 27;
            this.CcheckBox.Text = "启用鼠标坐标捕捉";
            this.CcheckBox.UseVisualStyleBackColor = true;
            this.CcheckBox.CheckedChanged += new System.EventHandler(this.CcheckBox_CheckedChanged);
            // 
            // Ctimer
            // 
            this.Ctimer.Tick += new System.EventHandler(this.Ctimer_Tick);
            // 
            // JsonopenFileDialog
            // 
            this.JsonopenFileDialog.Filter = "JSON (*.json)|*.json";
            this.JsonopenFileDialog.Title = "打开要修改的按键Json文件";
            // 
            // JsonUrltextBox
            // 
            this.JsonUrltextBox.Location = new System.Drawing.Point(47, 46);
            this.JsonUrltextBox.Name = "JsonUrltextBox";
            this.JsonUrltextBox.ReadOnly = true;
            this.JsonUrltextBox.Size = new System.Drawing.Size(518, 21);
            this.JsonUrltextBox.TabIndex = 29;
            this.JsonUrltextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblBindKey
            // 
            this.lblBindKey.AutoSize = true;
            this.lblBindKey.Location = new System.Drawing.Point(6, 21);
            this.lblBindKey.Name = "lblBindKey";
            this.lblBindKey.Size = new System.Drawing.Size(59, 12);
            this.lblBindKey.TabIndex = 31;
            this.lblBindKey.Text = "绑定按键:";
            // 
            // ButtontextBox
            // 
            this.ButtontextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ButtontextBox.Location = new System.Drawing.Point(65, 18);
            this.ButtontextBox.Name = "ButtontextBox";
            this.ButtontextBox.Size = new System.Drawing.Size(36, 21);
            this.ButtontextBox.TabIndex = 32;
            this.ButtontextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ButtontextBox_KeyDown);
            this.ButtontextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtontextBox_KeyPress);
            // 
            // CheckButton
            // 
            this.CheckButton.Location = new System.Drawing.Point(105, 17);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(42, 23);
            this.CheckButton.TabIndex = 33;
            this.CheckButton.Text = "检查";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // OpenJson
            // 
            this.OpenJson.Location = new System.Drawing.Point(569, 46);
            this.OpenJson.Name = "OpenJson";
            this.OpenJson.Size = new System.Drawing.Size(42, 23);
            this.OpenJson.TabIndex = 38;
            this.OpenJson.Text = "浏览";
            this.OpenJson.UseVisualStyleBackColor = true;
            this.OpenJson.Click += new System.EventHandler(this.OpenJson_Click);
            // 
            // RewriteAndSaveButton
            // 
            this.RewriteAndSaveButton.Location = new System.Drawing.Point(447, 17);
            this.RewriteAndSaveButton.Name = "RewriteAndSaveButton";
            this.RewriteAndSaveButton.Size = new System.Drawing.Size(80, 23);
            this.RewriteAndSaveButton.TabIndex = 39;
            this.RewriteAndSaveButton.Text = "修改并保存";
            this.RewriteAndSaveButton.UseVisualStyleBackColor = true;
            this.RewriteAndSaveButton.Click += new System.EventHandler(this.RewriteAndSaveButton_Click);
            // 
            // EcheckBox
            // 
            this.EcheckBox.AutoSize = true;
            this.EcheckBox.Location = new System.Drawing.Point(310, 37);
            this.EcheckBox.Name = "EcheckBox";
            this.EcheckBox.Size = new System.Drawing.Size(84, 16);
            this.EcheckBox.TabIndex = 40;
            this.EcheckBox.Text = "编辑快捷键";
            this.EcheckBox.UseVisualStyleBackColor = true;
            this.EcheckBox.CheckedChanged += new System.EventHandler(this.EcheckBox_CheckedChanged);
            // 
            // lblCtrlPrefix1
            // 
            this.lblCtrlPrefix1.AutoSize = true;
            this.lblCtrlPrefix1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.lblCtrlPrefix1.Location = new System.Drawing.Point(309, 58);
            this.lblCtrlPrefix1.Name = "lblCtrlPrefix1";
            this.lblCtrlPrefix1.Size = new System.Drawing.Size(47, 14);
            this.lblCtrlPrefix1.TabIndex = 41;
            this.lblCtrlPrefix1.Text = "Ctrl+";
            // 
            // lblCtrlPrefix2
            // 
            this.lblCtrlPrefix2.AutoSize = true;
            this.lblCtrlPrefix2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.lblCtrlPrefix2.Location = new System.Drawing.Point(309, 95);
            this.lblCtrlPrefix2.Name = "lblCtrlPrefix2";
            this.lblCtrlPrefix2.Size = new System.Drawing.Size(47, 14);
            this.lblCtrlPrefix2.TabIndex = 42;
            this.lblCtrlPrefix2.Text = "Ctrl+";
            // 
            // FindKeytextBox
            // 
            this.FindKeytextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.FindKeytextBox.Location = new System.Drawing.Point(362, 57);
            this.FindKeytextBox.Name = "FindKeytextBox";
            this.FindKeytextBox.ReadOnly = true;
            this.FindKeytextBox.Size = new System.Drawing.Size(41, 21);
            this.FindKeytextBox.TabIndex = 43;
            this.FindKeytextBox.Text = "D";
            this.FindKeytextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindKeytextBox_KeyDown);
            this.FindKeytextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FindKeytextBox_KeyPress);
            // 
            // ResetKeytextBox
            // 
            this.ResetKeytextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ResetKeytextBox.Location = new System.Drawing.Point(362, 92);
            this.ResetKeytextBox.Name = "ResetKeytextBox";
            this.ResetKeytextBox.ReadOnly = true;
            this.ResetKeytextBox.Size = new System.Drawing.Size(41, 21);
            this.ResetKeytextBox.TabIndex = 44;
            this.ResetKeytextBox.Text = "F";
            this.ResetKeytextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ResetKeytextBox_KeyDown);
            this.ResetKeytextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ResetKeytextBox_KeyPress);
            // 
            // SaveKeybutton
            // 
            this.SaveKeybutton.Location = new System.Drawing.Point(411, 33);
            this.SaveKeybutton.Name = "SaveKeybutton";
            this.SaveKeybutton.Size = new System.Drawing.Size(62, 20);
            this.SaveKeybutton.TabIndex = 45;
            this.SaveKeybutton.Text = "保存设置";
            this.SaveKeybutton.UseVisualStyleBackColor = true;
            this.SaveKeybutton.Click += new System.EventHandler(this.SaveKeybutton_Click);
            // 
            // LoadKeybutton
            // 
            this.LoadKeybutton.Location = new System.Drawing.Point(483, 33);
            this.LoadKeybutton.Name = "LoadKeybutton";
            this.LoadKeybutton.Size = new System.Drawing.Size(62, 20);
            this.LoadKeybutton.TabIndex = 46;
            this.LoadKeybutton.Text = "读取设置";
            this.LoadKeybutton.UseVisualStyleBackColor = true;
            this.LoadKeybutton.Click += new System.EventHandler(this.LoadKeybutton_Click);
            // 
            // lblSaveMouseCoord
            // 
            this.lblSaveMouseCoord.AutoSize = true;
            this.lblSaveMouseCoord.Location = new System.Drawing.Point(409, 60);
            this.lblSaveMouseCoord.Name = "lblSaveMouseCoord";
            this.lblSaveMouseCoord.Size = new System.Drawing.Size(101, 12);
            this.lblSaveMouseCoord.TabIndex = 47;
            this.lblSaveMouseCoord.Text = "保存当前鼠标坐标";
            // 
            // lblMoveMouseToSaved
            // 
            this.lblMoveMouseToSaved.AutoSize = true;
            this.lblMoveMouseToSaved.Location = new System.Drawing.Point(409, 97);
            this.lblMoveMouseToSaved.Name = "lblMoveMouseToSaved";
            this.lblMoveMouseToSaved.Size = new System.Drawing.Size(113, 12);
            this.lblMoveMouseToSaved.TabIndex = 48;
            this.lblMoveMouseToSaved.Text = "移动鼠标至保存位置";
            // 
            // ReadPPButton
            // 
            this.ReadPPButton.Location = new System.Drawing.Point(151, 17);
            this.ReadPPButton.Name = "ReadPPButton";
            this.ReadPPButton.Size = new System.Drawing.Size(72, 23);
            this.ReadPPButton.TabIndex = 50;
            this.ReadPPButton.Text = "读取坐标";
            this.ReadPPButton.UseVisualStyleBackColor = true;
            this.ReadPPButton.Click += new System.EventHandler(this.ReadPPButton_Click);
            // 
            // FunctiontabControl
            // 
            this.FunctiontabControl.Controls.Add(this.tabPage1);
            this.FunctiontabControl.Controls.Add(this.tabPage2);
            this.FunctiontabControl.Location = new System.Drawing.Point(8, 179);
            this.FunctiontabControl.Name = "FunctiontabControl";
            this.FunctiontabControl.SelectedIndex = 0;
            this.FunctiontabControl.Size = new System.Drawing.Size(664, 315);
            this.FunctiontabControl.TabIndex = 53;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.KXlabel);
            this.tabPage1.Controls.Add(this.KXtextBox);
            this.tabPage1.Controls.Add(this.KYtextBox);
            this.tabPage1.Controls.Add(this.KY1label);
            this.tabPage1.Controls.Add(this.JSXtextBox);
            this.tabPage1.Controls.Add(this.JSXlabel);
            this.tabPage1.Controls.Add(this.JSYtextBox);
            this.tabPage1.Controls.Add(this.JSYlabel);
            this.tabPage1.Controls.Add(this.JScheckBox);
            this.tabPage1.Controls.Add(this.KcheckBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(656, 289);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "坐标计算";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblFeatureCaption);
            this.tabPage2.Controls.Add(this.btnFeaturePanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(656, 289);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "更多功能";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblFeatureCaption
            // 
            this.lblFeatureCaption.AutoSize = true;
            this.lblFeatureCaption.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.lblFeatureCaption.Location = new System.Drawing.Point(10, 14);
            this.lblFeatureCaption.Name = "lblFeatureCaption";
            this.lblFeatureCaption.Size = new System.Drawing.Size(205, 19);
            this.lblFeatureCaption.TabIndex = 0;
            this.lblFeatureCaption.Text = "点击按钮打开对应功能子窗口：";
            // 
            // btnFeaturePanel
            // 
            this.btnFeaturePanel.Controls.Add(this.featureBtnMouse);
            this.btnFeaturePanel.Controls.Add(this.featureBtnRange);
            this.btnFeaturePanel.Controls.Add(this.featureBtnTouch);
            this.btnFeaturePanel.Controls.Add(this.featureBtnWide);
            this.btnFeaturePanel.Controls.Add(this.featureBtnBackup);
            this.btnFeaturePanel.Controls.Add(this.featureBtnKeyPreset);
            this.btnFeaturePanel.Controls.Add(this.featureBtnLog);
            this.btnFeaturePanel.Location = new System.Drawing.Point(6, 44);
            this.btnFeaturePanel.Name = "btnFeaturePanel";
            this.btnFeaturePanel.Size = new System.Drawing.Size(643, 218);
            this.btnFeaturePanel.TabIndex = 0;
            // 
            // featureBtnMouse
            // 
            this.featureBtnMouse.Location = new System.Drawing.Point(3, 3);
            this.featureBtnMouse.Name = "featureBtnMouse";
            this.featureBtnMouse.Size = new System.Drawing.Size(160, 30);
            this.featureBtnMouse.TabIndex = 0;
            this.featureBtnMouse.Text = "鼠标回溯";
            this.featureBtnMouse.UseVisualStyleBackColor = true;
            // 
            // featureBtnRange
            // 
            this.featureBtnRange.Location = new System.Drawing.Point(169, 3);
            this.featureBtnRange.Name = "featureBtnRange";
            this.featureBtnRange.Size = new System.Drawing.Size(160, 30);
            this.featureBtnRange.TabIndex = 1;
            this.featureBtnRange.Text = "区域键位清空";
            this.featureBtnRange.UseVisualStyleBackColor = true;
            // 
            // featureBtnTouch
            // 
            this.featureBtnTouch.Location = new System.Drawing.Point(335, 3);
            this.featureBtnTouch.Name = "featureBtnTouch";
            this.featureBtnTouch.Size = new System.Drawing.Size(160, 30);
            this.featureBtnTouch.TabIndex = 2;
            this.featureBtnTouch.Text = "全屏快速摸点";
            this.featureBtnTouch.UseVisualStyleBackColor = true;
            // 
            // featureBtnWide
            // 
            this.featureBtnWide.Location = new System.Drawing.Point(3, 39);
            this.featureBtnWide.Name = "featureBtnWide";
            this.featureBtnWide.Size = new System.Drawing.Size(160, 30);
            this.featureBtnWide.TabIndex = 3;
            this.featureBtnWide.Text = "超宽屏坐标转换";
            this.featureBtnWide.UseVisualStyleBackColor = true;
            // 
            // featureBtnBackup
            // 
            this.featureBtnBackup.Location = new System.Drawing.Point(169, 39);
            this.featureBtnBackup.Name = "featureBtnBackup";
            this.featureBtnBackup.Size = new System.Drawing.Size(160, 30);
            this.featureBtnBackup.TabIndex = 4;
            this.featureBtnBackup.Text = "索引备份";
            this.featureBtnBackup.UseVisualStyleBackColor = true;
            // 
            // featureBtnKeyPreset
            // 
            this.featureBtnKeyPreset.Location = new System.Drawing.Point(335, 39);
            this.featureBtnKeyPreset.Name = "featureBtnKeyPreset";
            this.featureBtnKeyPreset.Size = new System.Drawing.Size(160, 30);
            this.featureBtnKeyPreset.TabIndex = 5;
            this.featureBtnKeyPreset.Text = "基础键位预设";
            this.featureBtnKeyPreset.UseVisualStyleBackColor = true;
            // 
            // featureBtnLog
            // 
            this.featureBtnLog.Location = new System.Drawing.Point(3, 75);
            this.featureBtnLog.Name = "featureBtnLog";
            this.featureBtnLog.Size = new System.Drawing.Size(160, 30);
            this.featureBtnLog.TabIndex = 6;
            this.featureBtnLog.Text = "日志管理";
            this.featureBtnLog.UseVisualStyleBackColor = true;
            // 
            // DeleteRangeLTRDkeysButton
            // 
            this.DeleteRangeLTRDkeysButton.Location = new System.Drawing.Point(438, 63);
            this.DeleteRangeLTRDkeysButton.Name = "DeleteRangeLTRDkeysButton";
            this.DeleteRangeLTRDkeysButton.Size = new System.Drawing.Size(63, 23);
            this.DeleteRangeLTRDkeysButton.TabIndex = 56;
            this.DeleteRangeLTRDkeysButton.Text = "区域清空";
            this.DeleteRangeLTRDkeysButton.UseVisualStyleBackColor = true;
            this.DeleteRangeLTRDkeysButton.Click += new System.EventHandler(this.DeleteRangeLTRDkeysButton_Click);
            // 
            // lblRangeClearTip
            // 
            this.lblRangeClearTip.AutoSize = true;
            this.lblRangeClearTip.Location = new System.Drawing.Point(10, 11);
            this.lblRangeClearTip.Name = "lblRangeClearTip";
            this.lblRangeClearTip.Size = new System.Drawing.Size(521, 36);
            this.lblRangeClearTip.TabIndex = 22;
            this.lblRangeClearTip.Text = "这个模块是对 右下清空 按钮的功能补充。原按钮只对通用的16:9比例的右下角进行键位清除。\r\n\r\n该模块录入指定区域左上角X,Y坐标，右下角X,Y坐标后，按 区域" +
    "清空 按钮清空该区域所有键位。";
            // 
            // RangeRDXtextBox
            // 
            this.RangeRDXtextBox.Location = new System.Drawing.Point(322, 65);
            this.RangeRDXtextBox.Name = "RangeRDXtextBox";
            this.RangeRDXtextBox.Size = new System.Drawing.Size(100, 21);
            this.RangeRDXtextBox.TabIndex = 20;
            this.RangeRDXtextBox.Text = "0";
            this.RangeRDXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // RangeRDYtextBox
            // 
            this.RangeRDYtextBox.Location = new System.Drawing.Point(322, 98);
            this.RangeRDYtextBox.Name = "RangeRDYtextBox";
            this.RangeRDYtextBox.Size = new System.Drawing.Size(100, 21);
            this.RangeRDYtextBox.TabIndex = 21;
            this.RangeRDYtextBox.Text = "0";
            this.RangeRDYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // lblRangeLTX
            // 
            this.lblRangeLTX.AutoSize = true;
            this.lblRangeLTX.Location = new System.Drawing.Point(10, 68);
            this.lblRangeLTX.Name = "lblRangeLTX";
            this.lblRangeLTX.Size = new System.Drawing.Size(95, 12);
            this.lblRangeLTX.TabIndex = 13;
            this.lblRangeLTX.Text = "区域左上坐标X：";
            // 
            // RangeLTXtextBox
            // 
            this.RangeLTXtextBox.Location = new System.Drawing.Point(107, 65);
            this.RangeLTXtextBox.Name = "RangeLTXtextBox";
            this.RangeLTXtextBox.Size = new System.Drawing.Size(100, 21);
            this.RangeLTXtextBox.TabIndex = 12;
            this.RangeLTXtextBox.Text = "0";
            this.RangeLTXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // RangeLTYtextBox
            // 
            this.RangeLTYtextBox.Location = new System.Drawing.Point(107, 98);
            this.RangeLTYtextBox.Name = "RangeLTYtextBox";
            this.RangeLTYtextBox.Size = new System.Drawing.Size(100, 21);
            this.RangeLTYtextBox.TabIndex = 14;
            this.RangeLTYtextBox.Text = "0";
            this.RangeLTYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // lblRangeLTY
            // 
            this.lblRangeLTY.AutoSize = true;
            this.lblRangeLTY.Location = new System.Drawing.Point(10, 101);
            this.lblRangeLTY.Name = "lblRangeLTY";
            this.lblRangeLTY.Size = new System.Drawing.Size(95, 12);
            this.lblRangeLTY.TabIndex = 15;
            this.lblRangeLTY.Text = "区域左上坐标Y：";
            // 
            // lblRangeRDX
            // 
            this.lblRangeRDX.AutoSize = true;
            this.lblRangeRDX.Location = new System.Drawing.Point(221, 68);
            this.lblRangeRDX.Name = "lblRangeRDX";
            this.lblRangeRDX.Size = new System.Drawing.Size(95, 12);
            this.lblRangeRDX.TabIndex = 17;
            this.lblRangeRDX.Text = "区域右下坐标X：";
            // 
            // lblRangeRDY
            // 
            this.lblRangeRDY.AutoSize = true;
            this.lblRangeRDY.Location = new System.Drawing.Point(221, 101);
            this.lblRangeRDY.Name = "lblRangeRDY";
            this.lblRangeRDY.Size = new System.Drawing.Size(95, 12);
            this.lblRangeRDY.TabIndex = 19;
            this.lblRangeRDY.Text = "区域右下坐标Y：";
            // 
            // Tip2label
            // 
            this.Tip2label.AutoSize = true;
            this.Tip2label.ForeColor = System.Drawing.Color.Red;
            this.Tip2label.Location = new System.Drawing.Point(257, 93);
            this.Tip2label.Name = "Tip2label";
            this.Tip2label.Size = new System.Drawing.Size(167, 12);
            this.Tip2label.TabIndex = 36;
            this.Tip2label.Text = "                           ";
            // 
            // Tip1label
            // 
            this.Tip1label.AutoSize = true;
            this.Tip1label.ForeColor = System.Drawing.Color.Red;
            this.Tip1label.Location = new System.Drawing.Point(257, 75);
            this.Tip1label.Name = "Tip1label";
            this.Tip1label.Size = new System.Drawing.Size(125, 12);
            this.Tip1label.TabIndex = 35;
            // 
            // lblAdminTip
            // 
            this.lblAdminTip.AutoSize = true;
            this.lblAdminTip.ForeColor = System.Drawing.Color.Red;
            this.lblAdminTip.Location = new System.Drawing.Point(3, 24);
            this.lblAdminTip.Name = "lblAdminTip";
            this.lblAdminTip.Size = new System.Drawing.Size(287, 12);
            this.lblAdminTip.TabIndex = 34;
            this.lblAdminTip.Text = "提示2：如无法生成按键请以管理员模式启动小助手。";
            // 
            // nXtextBox
            // 
            this.nXtextBox.Location = new System.Drawing.Point(90, 129);
            this.nXtextBox.Name = "nXtextBox";
            this.nXtextBox.Size = new System.Drawing.Size(42, 21);
            this.nXtextBox.TabIndex = 30;
            this.nXtextBox.Text = "0";
            // 
            // lblColon3
            // 
            this.lblColon3.AutoSize = true;
            this.lblColon3.Location = new System.Drawing.Point(136, 133);
            this.lblColon3.Name = "lblColon3";
            this.lblColon3.Size = new System.Drawing.Size(17, 12);
            this.lblColon3.TabIndex = 33;
            this.lblColon3.Text = "：";
            // 
            // nYtextBox
            // 
            this.nYtextBox.Location = new System.Drawing.Point(154, 129);
            this.nYtextBox.Name = "nYtextBox";
            this.nYtextBox.Size = new System.Drawing.Size(42, 21);
            this.nYtextBox.TabIndex = 32;
            this.nYtextBox.Text = "0";
            // 
            // lblInternalCoord
            // 
            this.lblInternalCoord.AutoSize = true;
            this.lblInternalCoord.Location = new System.Drawing.Point(3, 132);
            this.lblInternalCoord.Name = "lblInternalCoord";
            this.lblInternalCoord.Size = new System.Drawing.Size(89, 12);
            this.lblInternalCoord.TabIndex = 31;
            this.lblInternalCoord.Text = "对应内部坐标：";
            // 
            // mXtextBox
            // 
            this.mXtextBox.Location = new System.Drawing.Point(89, 97);
            this.mXtextBox.Name = "mXtextBox";
            this.mXtextBox.Size = new System.Drawing.Size(42, 21);
            this.mXtextBox.TabIndex = 25;
            this.mXtextBox.Text = "0";
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Location = new System.Drawing.Point(135, 101);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(17, 12);
            this.lblColon1.TabIndex = 28;
            this.lblColon1.Text = "：";
            // 
            // mYtextBox
            // 
            this.mYtextBox.Location = new System.Drawing.Point(153, 97);
            this.mYtextBox.Name = "mYtextBox";
            this.mYtextBox.Size = new System.Drawing.Size(42, 21);
            this.mYtextBox.TabIndex = 27;
            this.mYtextBox.Text = "0";
            // 
            // lblCurrentMouseCoord
            // 
            this.lblCurrentMouseCoord.AutoSize = true;
            this.lblCurrentMouseCoord.Location = new System.Drawing.Point(3, 100);
            this.lblCurrentMouseCoord.Name = "lblCurrentMouseCoord";
            this.lblCurrentMouseCoord.Size = new System.Drawing.Size(89, 12);
            this.lblCurrentMouseCoord.TabIndex = 26;
            this.lblCurrentMouseCoord.Text = "当前鼠标坐标：";
            // 
            // SXtextBox
            // 
            this.SXtextBox.Location = new System.Drawing.Point(78, 43);
            this.SXtextBox.Name = "SXtextBox";
            this.SXtextBox.Size = new System.Drawing.Size(42, 21);
            this.SXtextBox.TabIndex = 17;
            this.SXtextBox.Text = "0";
            this.SXtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // btnGetScreenResolution
            // 
            this.btnGetScreenResolution.Location = new System.Drawing.Point(188, 43);
            this.btnGetScreenResolution.Name = "btnGetScreenResolution";
            this.btnGetScreenResolution.Size = new System.Drawing.Size(65, 21);
            this.btnGetScreenResolution.TabIndex = 21;
            this.btnGetScreenResolution.Text = "自动获取";
            this.btnGetScreenResolution.UseVisualStyleBackColor = true;
            this.btnGetScreenResolution.Click += new System.EventHandler(this.btnGetScreenResolution_Click);
            // 
            // lblScreenResX
            // 
            this.lblScreenResX.AutoSize = true;
            this.lblScreenResX.Location = new System.Drawing.Point(124, 47);
            this.lblScreenResX.Name = "lblScreenResX";
            this.lblScreenResX.Size = new System.Drawing.Size(11, 12);
            this.lblScreenResX.TabIndex = 20;
            this.lblScreenResX.Text = "X";
            // 
            // SYtextBox
            // 
            this.SYtextBox.Location = new System.Drawing.Point(139, 43);
            this.SYtextBox.Name = "SYtextBox";
            this.SYtextBox.Size = new System.Drawing.Size(42, 21);
            this.SYtextBox.TabIndex = 19;
            this.SYtextBox.Text = "0";
            this.SYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // lblDesktopRes
            // 
            this.lblDesktopRes.AutoSize = true;
            this.lblDesktopRes.Location = new System.Drawing.Point(3, 46);
            this.lblDesktopRes.Name = "lblDesktopRes";
            this.lblDesktopRes.Size = new System.Drawing.Size(77, 12);
            this.lblDesktopRes.TabIndex = 18;
            this.lblDesktopRes.Text = "桌面分辨率：";
            // 
            // lblFullScreenTip
            // 
            this.lblFullScreenTip.AutoSize = true;
            this.lblFullScreenTip.ForeColor = System.Drawing.Color.Red;
            this.lblFullScreenTip.Location = new System.Drawing.Point(3, 9);
            this.lblFullScreenTip.Name = "lblFullScreenTip";
            this.lblFullScreenTip.Size = new System.Drawing.Size(407, 12);
            this.lblFullScreenTip.TabIndex = 0;
            this.lblFullScreenTip.Text = "提示：按F11将MuMu模拟器全屏化后可使用该功能，建议将小助手窗口置顶。";
            // 
            // ktckAPWritebutton
            // 
            this.ktckAPWritebutton.Location = new System.Drawing.Point(438, 94);
            this.ktckAPWritebutton.Name = "ktckAPWritebutton";
            this.ktckAPWritebutton.Size = new System.Drawing.Size(75, 23);
            this.ktckAPWritebutton.TabIndex = 87;
            this.ktckAPWritebutton.Text = "批量写入";
            this.ktckAPWritebutton.UseVisualStyleBackColor = true;
            this.ktckAPWritebutton.Click += new System.EventHandler(this.ktckAPWritebutton_Click);
            // 
            // ktckOPWritebutton
            // 
            this.ktckOPWritebutton.Location = new System.Drawing.Point(347, 94);
            this.ktckOPWritebutton.Name = "ktckOPWritebutton";
            this.ktckOPWritebutton.Size = new System.Drawing.Size(75, 23);
            this.ktckOPWritebutton.TabIndex = 86;
            this.ktckOPWritebutton.Text = "单点写入";
            this.ktckOPWritebutton.UseVisualStyleBackColor = true;
            this.ktckOPWritebutton.Click += new System.EventHandler(this.ktckOPWritebutton_Click);
            // 
            // ktckReadbutton
            // 
            this.ktckReadbutton.Location = new System.Drawing.Point(347, 12);
            this.ktckReadbutton.Name = "ktckReadbutton";
            this.ktckReadbutton.Size = new System.Drawing.Size(75, 23);
            this.ktckReadbutton.TabIndex = 85;
            this.ktckReadbutton.Text = "读取";
            this.ktckReadbutton.UseVisualStyleBackColor = true;
            this.ktckReadbutton.Click += new System.EventHandler(this.ktckReadbutton_Click);
            // 
            // ktckPListcheckedListBox
            // 
            this.ktckPListcheckedListBox.FormattingEnabled = true;
            this.ktckPListcheckedListBox.Items.AddRange(new object[] {
            "全选"});
            this.ktckPListcheckedListBox.Location = new System.Drawing.Point(9, 66);
            this.ktckPListcheckedListBox.Name = "ktckPListcheckedListBox";
            this.ktckPListcheckedListBox.Size = new System.Drawing.Size(120, 84);
            this.ktckPListcheckedListBox.TabIndex = 84;
            this.ktckPListcheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ktckPListcheckedListBox_ItemCheck);
            this.ktckPListcheckedListBox.SelectedIndexChanged += new System.EventHandler(this.ktckPListcheckedListBox_SelectedIndexChanged);
            // 
            // fileNamecomboBox2
            // 
            this.fileNamecomboBox2.DropDownHeight = 200;
            this.fileNamecomboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileNamecomboBox2.FormattingEnabled = true;
            this.fileNamecomboBox2.IntegralHeight = false;
            this.fileNamecomboBox2.Location = new System.Drawing.Point(83, 14);
            this.fileNamecomboBox2.Name = "fileNamecomboBox2";
            this.fileNamecomboBox2.Size = new System.Drawing.Size(258, 20);
            this.fileNamecomboBox2.TabIndex = 80;
            this.fileNamecomboBox2.DropDown += new System.EventHandler(this.fileNamecomboBox2_DropDown);
            this.fileNamecomboBox2.DropDownClosed += new System.EventHandler(this.fileNamecomboBox2_DropDownClosed);
            // 
            // packageNamecomboBox2
            // 
            this.packageNamecomboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packageNamecomboBox2.FormattingEnabled = true;
            this.packageNamecomboBox2.Location = new System.Drawing.Point(9, 14);
            this.packageNamecomboBox2.Name = "packageNamecomboBox2";
            this.packageNamecomboBox2.Size = new System.Drawing.Size(68, 20);
            this.packageNamecomboBox2.TabIndex = 79;
            // 
            // resolutioncomboBox2
            // 
            this.resolutioncomboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutioncomboBox2.FormattingEnabled = true;
            this.resolutioncomboBox2.Location = new System.Drawing.Point(186, 40);
            this.resolutioncomboBox2.Name = "resolutioncomboBox2";
            this.resolutioncomboBox2.Size = new System.Drawing.Size(155, 20);
            this.resolutioncomboBox2.TabIndex = 78;
            // 
            // resolutionTypecomboBox2
            // 
            this.resolutionTypecomboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionTypecomboBox2.FormattingEnabled = true;
            this.resolutionTypecomboBox2.Location = new System.Drawing.Point(114, 40);
            this.resolutionTypecomboBox2.Name = "resolutionTypecomboBox2";
            this.resolutionTypecomboBox2.Size = new System.Drawing.Size(68, 20);
            this.resolutionTypecomboBox2.TabIndex = 77;
            this.resolutionTypecomboBox2.SelectedIndexChanged += new System.EventHandler(this.resolutionTypecomboBox2_SelectedIndexChanged);
            // 
            // ktckCKXtextBox
            // 
            this.ktckCKXtextBox.Location = new System.Drawing.Point(226, 96);
            this.ktckCKXtextBox.Name = "ktckCKXtextBox";
            this.ktckCKXtextBox.ReadOnly = true;
            this.ktckCKXtextBox.Size = new System.Drawing.Size(42, 21);
            this.ktckCKXtextBox.TabIndex = 38;
            this.ktckCKXtextBox.Text = "0";
            // 
            // lblColon4
            // 
            this.lblColon4.AutoSize = true;
            this.lblColon4.Location = new System.Drawing.Point(272, 100);
            this.lblColon4.Name = "lblColon4";
            this.lblColon4.Size = new System.Drawing.Size(17, 12);
            this.lblColon4.TabIndex = 41;
            this.lblColon4.Text = "：";
            // 
            // ktckCKYtextBox
            // 
            this.ktckCKYtextBox.Location = new System.Drawing.Point(290, 96);
            this.ktckCKYtextBox.Name = "ktckCKYtextBox";
            this.ktckCKYtextBox.ReadOnly = true;
            this.ktckCKYtextBox.Size = new System.Drawing.Size(42, 21);
            this.ktckCKYtextBox.TabIndex = 40;
            this.ktckCKYtextBox.Text = "0";
            // 
            // lblWideScreenCoord
            // 
            this.lblWideScreenCoord.AutoSize = true;
            this.lblWideScreenCoord.Location = new System.Drawing.Point(139, 99);
            this.lblWideScreenCoord.Name = "lblWideScreenCoord";
            this.lblWideScreenCoord.Size = new System.Drawing.Size(89, 12);
            this.lblWideScreenCoord.TabIndex = 39;
            this.lblWideScreenCoord.Text = "对应宽屏坐标：";
            // 
            // ktckKXtextBox
            // 
            this.ktckKXtextBox.Location = new System.Drawing.Point(225, 64);
            this.ktckKXtextBox.Name = "ktckKXtextBox";
            this.ktckKXtextBox.ReadOnly = true;
            this.ktckKXtextBox.Size = new System.Drawing.Size(42, 21);
            this.ktckKXtextBox.TabIndex = 34;
            this.ktckKXtextBox.Text = "0";
            // 
            // lblColon2
            // 
            this.lblColon2.AutoSize = true;
            this.lblColon2.Location = new System.Drawing.Point(271, 68);
            this.lblColon2.Name = "lblColon2";
            this.lblColon2.Size = new System.Drawing.Size(17, 12);
            this.lblColon2.TabIndex = 37;
            this.lblColon2.Text = "：";
            // 
            // ktckKYtextBox
            // 
            this.ktckKYtextBox.Location = new System.Drawing.Point(289, 64);
            this.ktckKYtextBox.Name = "ktckKYtextBox";
            this.ktckKYtextBox.ReadOnly = true;
            this.ktckKYtextBox.Size = new System.Drawing.Size(42, 21);
            this.ktckKYtextBox.TabIndex = 36;
            this.ktckKYtextBox.Text = "0";
            // 
            // lblSourceRes
            // 
            this.lblSourceRes.AutoSize = true;
            this.lblSourceRes.Location = new System.Drawing.Point(7, 45);
            this.lblSourceRes.Name = "lblSourceRes";
            this.lblSourceRes.Size = new System.Drawing.Size(113, 12);
            this.lblSourceRes.TabIndex = 35;
            this.lblSourceRes.Text = "待读取文件分辨率：";
            // 
            // lblWideScreenTip
            // 
            this.lblWideScreenTip.AutoSize = true;
            this.lblWideScreenTip.ForeColor = System.Drawing.Color.Red;
            this.lblWideScreenTip.Location = new System.Drawing.Point(322, 128);
            this.lblWideScreenTip.Name = "lblWideScreenTip";
            this.lblWideScreenTip.Size = new System.Drawing.Size(209, 12);
            this.lblWideScreenTip.TabIndex = 1;
            this.lblWideScreenTip.Text = "提示：测试向功能，出现问题请转人工";
            // 
            // fileNameSearchtextBox2
            // 
            this.fileNameSearchtextBox2.Location = new System.Drawing.Point(83, -7);
            this.fileNameSearchtextBox2.Name = "fileNameSearchtextBox2";
            this.fileNameSearchtextBox2.Size = new System.Drawing.Size(258, 21);
            this.fileNameSearchtextBox2.TabIndex = 81;
            this.fileNameSearchtextBox2.Visible = false;
            this.fileNameSearchtextBox2.TextChanged += new System.EventHandler(this.fileNameSearchtextBox2_TextChanged);
            this.fileNameSearchtextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fileNameSearchtextBox2_KeyDown);
            // 
            // lblSelectedCoord
            // 
            this.lblSelectedCoord.AutoSize = true;
            this.lblSelectedCoord.Location = new System.Drawing.Point(139, 68);
            this.lblSelectedCoord.Name = "lblSelectedCoord";
            this.lblSelectedCoord.Size = new System.Drawing.Size(89, 12);
            this.lblSelectedCoord.TabIndex = 83;
            this.lblSelectedCoord.Text = "当前选择坐标：";
            // 
            // keyTypelistcomboBox
            // 
            this.keyTypelistcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyTypelistcomboBox.FormattingEnabled = true;
            this.keyTypelistcomboBox.Location = new System.Drawing.Point(281, 18);
            this.keyTypelistcomboBox.Name = "keyTypelistcomboBox";
            this.keyTypelistcomboBox.Size = new System.Drawing.Size(85, 20);
            this.keyTypelistcomboBox.TabIndex = 63;
            // 
            // Undobutton
            // 
            this.Undobutton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Undobutton.Location = new System.Drawing.Point(575, 13);
            this.Undobutton.Name = "Undobutton";
            this.Undobutton.Size = new System.Drawing.Size(40, 25);
            this.Undobutton.TabIndex = 64;
            this.Undobutton.Text = "撤销";
            this.Undobutton.UseVisualStyleBackColor = true;
            this.Undobutton.Click += new System.EventHandler(this.Undobutton_Click);
            // 
            // Redobutton
            // 
            this.Redobutton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Redobutton.Location = new System.Drawing.Point(619, 13);
            this.Redobutton.Name = "Redobutton";
            this.Redobutton.Size = new System.Drawing.Size(40, 25);
            this.Redobutton.TabIndex = 65;
            this.Redobutton.Text = "重做";
            this.Redobutton.UseVisualStyleBackColor = true;
            this.Redobutton.Click += new System.EventHandler(this.Redobutton_Click);
            // 
            // OpenJsonFolderbutton
            // 
            this.OpenJsonFolderbutton.Location = new System.Drawing.Point(615, 46);
            this.OpenJsonFolderbutton.Name = "OpenJsonFolderbutton";
            this.OpenJsonFolderbutton.Size = new System.Drawing.Size(44, 23);
            this.OpenJsonFolderbutton.TabIndex = 66;
            this.OpenJsonFolderbutton.Text = "打开";
            this.OpenJsonFolderbutton.UseVisualStyleBackColor = true;
            this.OpenJsonFolderbutton.Click += new System.EventHandler(this.OpenJsonFolderbutton_Click);
            // 
            // replaceKeycheckBox
            // 
            this.replaceKeycheckBox.AutoSize = true;
            this.replaceKeycheckBox.Location = new System.Drawing.Point(370, 18);
            this.replaceKeycheckBox.Name = "replaceKeycheckBox";
            this.replaceKeycheckBox.Size = new System.Drawing.Size(72, 16);
            this.replaceKeycheckBox.TabIndex = 67;
            this.replaceKeycheckBox.Text = "强制替换";
            this.replaceKeycheckBox.UseVisualStyleBackColor = true;
            // 
            // packageNamecomboBox
            // 
            this.packageNamecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packageNamecomboBox.FormattingEnabled = true;
            this.packageNamecomboBox.Location = new System.Drawing.Point(91, 17);
            this.packageNamecomboBox.Name = "packageNamecomboBox";
            this.packageNamecomboBox.Size = new System.Drawing.Size(58, 20);
            this.packageNamecomboBox.TabIndex = 68;
            this.packageNamecomboBox.SelectedIndexChanged += new System.EventHandler(this.packageNamecomboBox_SelectedIndexChanged);
            // 
            // TryGetJsonFileFolderbutton
            // 
            this.TryGetJsonFileFolderbutton.Location = new System.Drawing.Point(10, 16);
            this.TryGetJsonFileFolderbutton.Name = "TryGetJsonFileFolderbutton";
            this.TryGetJsonFileFolderbutton.Size = new System.Drawing.Size(75, 23);
            this.TryGetJsonFileFolderbutton.TabIndex = 74;
            this.TryGetJsonFileFolderbutton.Text = "获取路径";
            this.TryGetJsonFileFolderbutton.UseVisualStyleBackColor = true;
            this.TryGetJsonFileFolderbutton.Click += new System.EventHandler(this.TryGetJsonFileFolderbutton_Click);
            // 
            // resolutionTypecomboBox
            // 
            this.resolutionTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionTypecomboBox.FormattingEnabled = true;
            this.resolutionTypecomboBox.Location = new System.Drawing.Point(47, 74);
            this.resolutionTypecomboBox.Name = "resolutionTypecomboBox";
            this.resolutionTypecomboBox.Size = new System.Drawing.Size(57, 20);
            this.resolutionTypecomboBox.TabIndex = 75;
            this.resolutionTypecomboBox.SelectedIndexChanged += new System.EventHandler(this.resolutionTypecomboBox_SelectedIndexChanged);
            // 
            // resolutioncomboBox
            // 
            this.resolutioncomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutioncomboBox.FormattingEnabled = true;
            this.resolutioncomboBox.Location = new System.Drawing.Point(110, 74);
            this.resolutioncomboBox.Name = "resolutioncomboBox";
            this.resolutioncomboBox.Size = new System.Drawing.Size(93, 20);
            this.resolutioncomboBox.TabIndex = 76;
            this.resolutioncomboBox.SelectedIndexChanged += new System.EventHandler(this.resolutioncomboBox_SelectedIndexChanged);
            // 
            // deleteUDResolutionbutton
            // 
            this.deleteUDResolutionbutton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.deleteUDResolutionbutton.Location = new System.Drawing.Point(469, 72);
            this.deleteUDResolutionbutton.Name = "deleteUDResolutionbutton";
            this.deleteUDResolutionbutton.Size = new System.Drawing.Size(40, 25);
            this.deleteUDResolutionbutton.TabIndex = 77;
            this.deleteUDResolutionbutton.Text = "删除";
            this.deleteUDResolutionbutton.UseVisualStyleBackColor = true;
            this.deleteUDResolutionbutton.Visible = false;
            this.deleteUDResolutionbutton.Click += new System.EventHandler(this.deleteUDResolutionbutton_Click);
            // 
            // Ktimer
            // 
            this.Ktimer.Tick += new System.EventHandler(this.Ktimer_Tick);
            // 
            // _indexCheckTimer
            // 
            this._indexCheckTimer.Tick += new System.EventHandler(this._indexCheckTimer_Tick);
            // 
            // gbFileAndResolution
            // 
            this.gbFileAndResolution.Controls.Add(this.searchFileCombo);
            this.gbFileAndResolution.Controls.Add(this.TryGetJsonFileFolderbutton);
            this.gbFileAndResolution.Controls.Add(this.packageNamecomboBox);
            this.gbFileAndResolution.Controls.Add(this.autoSyncCheckBox);
            this.gbFileAndResolution.Controls.Add(this.Undobutton);
            this.gbFileAndResolution.Controls.Add(this.Redobutton);
            this.gbFileAndResolution.Controls.Add(this.lblFilePath);
            this.gbFileAndResolution.Controls.Add(this.JsonUrltextBox);
            this.gbFileAndResolution.Controls.Add(this.OpenJson);
            this.gbFileAndResolution.Controls.Add(this.OpenJsonFolderbutton);
            this.gbFileAndResolution.Controls.Add(this.lblResScheme);
            this.gbFileAndResolution.Controls.Add(this.resolutionTypecomboBox);
            this.gbFileAndResolution.Controls.Add(this.resolutioncomboBox);
            this.gbFileAndResolution.Controls.Add(this.lblResolution);
            this.gbFileAndResolution.Controls.Add(this.FXtextBox);
            this.gbFileAndResolution.Controls.Add(this.FYlabel);
            this.gbFileAndResolution.Controls.Add(this.FYtextBox);
            this.gbFileAndResolution.Controls.Add(this.FSave);
            this.gbFileAndResolution.Controls.Add(this.FLoad);
            this.gbFileAndResolution.Controls.Add(this.FcheckBox);
            this.gbFileAndResolution.Controls.Add(this.deleteUDResolutionbutton);
            this.gbFileAndResolution.Controls.Add(this.TOPcheckBox);
            this.gbFileAndResolution.Location = new System.Drawing.Point(8, 4);
            this.gbFileAndResolution.Name = "gbFileAndResolution";
            this.gbFileAndResolution.Size = new System.Drawing.Size(664, 111);
            this.gbFileAndResolution.TabIndex = 80;
            this.gbFileAndResolution.TabStop = false;
            this.gbFileAndResolution.Text = "文件与分辨率";
            // 
            // searchFileCombo
            // 
            this.searchFileCombo.DataSource = null;
            this.searchFileCombo.DisplayMember = "";
            this.searchFileCombo.DropDownHeight = 200;
            this.searchFileCombo.DroppedDown = false;
            this.searchFileCombo.Location = new System.Drawing.Point(152, 17);
            this.searchFileCombo.Name = "searchFileCombo";
            this.searchFileCombo.SelectedIndex = -1;
            this.searchFileCombo.SelectedItem = null;
            this.searchFileCombo.Size = new System.Drawing.Size(274, 25);
            this.searchFileCombo.TabIndex = 69;
            this.searchFileCombo.ValueMember = "";
            // 
            // autoSyncCheckBox
            // 
            this.autoSyncCheckBox.Checked = true;
            this.autoSyncCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoSyncCheckBox.Location = new System.Drawing.Point(433, 15);
            this.autoSyncCheckBox.Name = "autoSyncCheckBox";
            this.autoSyncCheckBox.Size = new System.Drawing.Size(142, 27);
            this.autoSyncCheckBox.TabIndex = 71;
            this.autoSyncCheckBox.Text = "自动同步模拟器按键";
            this.autoSyncCheckBox.UseVisualStyleBackColor = true;
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(6, 49);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(35, 12);
            this.lblFilePath.TabIndex = 83;
            this.lblFilePath.Text = "路径:";
            // 
            // lblResScheme
            // 
            this.lblResScheme.AutoSize = true;
            this.lblResScheme.Location = new System.Drawing.Point(6, 77);
            this.lblResScheme.Name = "lblResScheme";
            this.lblResScheme.Size = new System.Drawing.Size(35, 12);
            this.lblResScheme.TabIndex = 84;
            this.lblResScheme.Text = "方案:";
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.Location = new System.Drawing.Point(222, 77);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(47, 12);
            this.lblResolution.TabIndex = 85;
            this.lblResolution.Text = "分辨率:";
            // 
            // gbKeyEdit
            // 
            this.gbKeyEdit.Controls.Add(this.lblBindKey);
            this.gbKeyEdit.Controls.Add(this.ButtontextBox);
            this.gbKeyEdit.Controls.Add(this.CheckButton);
            this.gbKeyEdit.Controls.Add(this.ReadPPButton);
            this.gbKeyEdit.Controls.Add(this.lblKeyType);
            this.gbKeyEdit.Controls.Add(this.keyTypelistcomboBox);
            this.gbKeyEdit.Controls.Add(this.replaceKeycheckBox);
            this.gbKeyEdit.Controls.Add(this.RewriteAndSaveButton);
            this.gbKeyEdit.Location = new System.Drawing.Point(8, 121);
            this.gbKeyEdit.Name = "gbKeyEdit";
            this.gbKeyEdit.Size = new System.Drawing.Size(664, 51);
            this.gbKeyEdit.TabIndex = 81;
            this.gbKeyEdit.TabStop = false;
            this.gbKeyEdit.Text = "按键编辑";
            // 
            // lblKeyType
            // 
            this.lblKeyType.AutoSize = true;
            this.lblKeyType.Location = new System.Drawing.Point(237, 21);
            this.lblKeyType.Name = "lblKeyType";
            this.lblKeyType.Size = new System.Drawing.Size(35, 12);
            this.lblKeyType.TabIndex = 86;
            this.lblKeyType.Text = "类型:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adbBtn,
            this.statusText,
            this.statusAuthor});
            this.statusStrip1.Location = new System.Drawing.Point(0, 496);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(675, 23);
            this.statusStrip1.TabIndex = 82;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // adbBtn
            // 
            this.adbBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.adbBtn.Name = "adbBtn";
            this.adbBtn.Size = new System.Drawing.Size(85, 21);
            this.adbBtn.Text = "ADB触控采集";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(493, 18);
            this.statusText.Spring = true;
            this.statusText.Text = "就绪";
            // 
            // statusAuthor
            // 
            this.statusAuthor.IsLink = true;
            this.statusAuthor.Name = "statusAuthor";
            this.statusAuthor.Size = new System.Drawing.Size(82, 18);
            this.statusAuthor.Text = "By：漆黑人形";
            this.statusAuthor.Click += new System.EventHandler(this.statusAuthor_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 519);
            this.Controls.Add(this.gbFileAndResolution);
            this.Controls.Add(this.gbKeyEdit);
            this.Controls.Add(this.FunctiontabControl);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "MuMu摸点小助手3.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.FunctiontabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.btnFeaturePanel.ResumeLayout(false);
            this.gbFileAndResolution.ResumeLayout(false);
            this.gbFileAndResolution.PerformLayout();
            this.gbKeyEdit.ResumeLayout(false);
            this.gbKeyEdit.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox KXtextBox;
        private System.Windows.Forms.Label KXlabel;
        private System.Windows.Forms.Label KY1label;
        private System.Windows.Forms.TextBox KYtextBox;
        private System.Windows.Forms.Label FYlabel;
        private System.Windows.Forms.TextBox FYtextBox;
        private System.Windows.Forms.TextBox FXtextBox;
        private System.Windows.Forms.Label JSYlabel;
        private System.Windows.Forms.TextBox JSYtextBox;
        private System.Windows.Forms.Label JSXlabel;
        private System.Windows.Forms.TextBox JSXtextBox;
        private System.Windows.Forms.CheckBox FcheckBox;
        private System.Windows.Forms.CheckBox JScheckBox;
        private System.Windows.Forms.CheckBox KcheckBox;
        private System.Windows.Forms.Button FSave;
        private System.Windows.Forms.Button FLoad;
        private System.Windows.Forms.CheckBox TOPcheckBox;
        private System.Windows.Forms.TextBox NCXtextBox;
        private System.Windows.Forms.Label lblMouseCurrentX;
        private System.Windows.Forms.Label lblMouseCurrentY;
        private System.Windows.Forms.TextBox NCYtextBox;
        private System.Windows.Forms.Label lblMouseSavedX;
        private System.Windows.Forms.TextBox SCXtextBox;
        private System.Windows.Forms.Label lblMouseSavedY;
        private System.Windows.Forms.TextBox SCYtextBox;
        private System.Windows.Forms.CheckBox CcheckBox;
        private System.Windows.Forms.Timer Ctimer;
        private System.Windows.Forms.OpenFileDialog JsonopenFileDialog;
        private System.Windows.Forms.TextBox JsonUrltextBox;
        private System.Windows.Forms.Label lblBindKey;
        private System.Windows.Forms.TextBox ButtontextBox;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.Button OpenJson;
        private System.Windows.Forms.Button RewriteAndSaveButton;
        private System.Windows.Forms.CheckBox EcheckBox;
        private System.Windows.Forms.Label lblCtrlPrefix1;
        private System.Windows.Forms.Label lblCtrlPrefix2;
        private System.Windows.Forms.TextBox FindKeytextBox;
        private System.Windows.Forms.TextBox ResetKeytextBox;
        private System.Windows.Forms.Button SaveKeybutton;
        private System.Windows.Forms.Button LoadKeybutton;
        private System.Windows.Forms.Label lblSaveMouseCoord;
        private System.Windows.Forms.Label lblMoveMouseToSaved;
        private System.Windows.Forms.Button ReadPPButton;
        private System.Windows.Forms.TabControl FunctiontabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox RangeRDXtextBox;
        private System.Windows.Forms.TextBox RangeRDYtextBox;
        private System.Windows.Forms.Label lblRangeLTX;
        private System.Windows.Forms.TextBox RangeLTXtextBox;
        private System.Windows.Forms.TextBox RangeLTYtextBox;
        private System.Windows.Forms.Label lblRangeLTY;
        private System.Windows.Forms.Label lblRangeRDX;
        private System.Windows.Forms.Label lblRangeRDY;
        private System.Windows.Forms.Label lblRangeClearTip;
        private System.Windows.Forms.Button DeleteRangeLTRDkeysButton;
        private System.Windows.Forms.ComboBox keyTypelistcomboBox;
        private System.Windows.Forms.Button Undobutton;
        private System.Windows.Forms.Button Redobutton;
        private System.Windows.Forms.Button OpenJsonFolderbutton;
        private System.Windows.Forms.CheckBox replaceKeycheckBox;
        private System.Windows.Forms.ComboBox packageNamecomboBox;
        private MuMu坐标计算.SearchableComboBox searchFileCombo;
        private System.Windows.Forms.CheckBox autoSyncCheckBox;
        private System.Windows.Forms.Button TryGetJsonFileFolderbutton;
        private System.Windows.Forms.ComboBox resolutionTypecomboBox;
        private System.Windows.Forms.ComboBox resolutioncomboBox;
        private System.Windows.Forms.Button deleteUDResolutionbutton;
        private System.Windows.Forms.TextBox SXtextBox;
        private System.Windows.Forms.Button btnGetScreenResolution;
        private System.Windows.Forms.Label lblScreenResX;
        private System.Windows.Forms.TextBox SYtextBox;
        private System.Windows.Forms.Label lblDesktopRes;
        private System.Windows.Forms.TextBox nXtextBox;
        private System.Windows.Forms.Label lblColon3;
        private System.Windows.Forms.TextBox nYtextBox;
        private System.Windows.Forms.Label lblInternalCoord;
        private System.Windows.Forms.TextBox mXtextBox;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.TextBox mYtextBox;
        private System.Windows.Forms.Label lblCurrentMouseCoord;
        private System.Windows.Forms.Timer Ktimer;
        private System.Windows.Forms.Label lblAdminTip;
        private System.Windows.Forms.Label lblFullScreenTip;
        private System.Windows.Forms.Label Tip1label;
        private System.Windows.Forms.Label Tip2label;
        private System.Windows.Forms.Label lblWideScreenTip;
        private System.Windows.Forms.TextBox ktckCKXtextBox;
        private System.Windows.Forms.Label lblColon4;
        private System.Windows.Forms.TextBox ktckCKYtextBox;
        private System.Windows.Forms.Label lblWideScreenCoord;
        private System.Windows.Forms.TextBox ktckKXtextBox;
        private System.Windows.Forms.Label lblColon2;
        private System.Windows.Forms.TextBox ktckKYtextBox;
        private System.Windows.Forms.Label lblSourceRes;
        private System.Windows.Forms.ComboBox fileNamecomboBox2;
        private System.Windows.Forms.ComboBox packageNamecomboBox2;
        private System.Windows.Forms.ComboBox resolutioncomboBox2;
        private System.Windows.Forms.ComboBox resolutionTypecomboBox2;
        private System.Windows.Forms.TextBox fileNameSearchtextBox2;
        private System.Windows.Forms.Label lblSelectedCoord;
        private System.Windows.Forms.CheckedListBox ktckPListcheckedListBox;
        private System.Windows.Forms.Button ktckAPWritebutton;
        private System.Windows.Forms.Button ktckOPWritebutton;
        private System.Windows.Forms.Button ktckReadbutton;
        private System.Windows.Forms.GroupBox gbFileAndResolution;
        private System.Windows.Forms.GroupBox gbKeyEdit;
        private System.Windows.Forms.FlowLayoutPanel btnFeaturePanel;
        private System.Windows.Forms.Button featureBtnMouse;
        private System.Windows.Forms.Button featureBtnRange;
        private System.Windows.Forms.Button featureBtnTouch;
        private System.Windows.Forms.Button featureBtnWide;
        private System.Windows.Forms.Button featureBtnBackup;
        private System.Windows.Forms.Button featureBtnKeyPreset;
        private System.Windows.Forms.Button featureBtnLog;
        private System.Windows.Forms.Label lblFeatureCaption;
        private System.Windows.Forms.ToolStripButton adbBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripStatusLabel statusAuthor;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label lblResScheme;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.Label lblKeyType;
        private System.Windows.Forms.Timer _indexCheckTimer;
    }
}
