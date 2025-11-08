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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.FXlabel = new System.Windows.Forms.Label();
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NCYtextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SCXtextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SCYtextBox = new System.Windows.Forms.TextBox();
            this.CcheckBox = new System.Windows.Forms.CheckBox();
            this.Ctimer = new System.Windows.Forms.Timer(this.components);
            this.JsonopenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.LoadJson = new System.Windows.Forms.Button();
            this.JsonUrltextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ButtontextBox = new System.Windows.Forms.TextBox();
            this.CheckButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.OpenJson = new System.Windows.Forms.Button();
            this.RewriteAndSaveButton = new System.Windows.Forms.Button();
            this.CheckFileChangetimer = new System.Windows.Forms.Timer(this.components);
            this.EcheckBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.FindKeytextBox = new System.Windows.Forms.TextBox();
            this.ResetKeytextBox = new System.Windows.Forms.TextBox();
            this.SaveKeybutton = new System.Windows.Forms.Button();
            this.LoadKeybutton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.QhrxlinkLabel = new System.Windows.Forms.LinkLabel();
            this.ReadPPButton = new System.Windows.Forms.Button();
            this.KeysListcomboBox = new System.Windows.Forms.ComboBox();
            this.WriteKeysButton = new System.Windows.Forms.Button();
            this.FunctiontabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.DeleteRangeLTRDkeysButton = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.RangeRDXtextBox = new System.Windows.Forms.TextBox();
            this.RangeRDYtextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RangeLTXtextBox = new System.Windows.Forms.TextBox();
            this.RangeLTYtextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.DeleteRepeatKeysButton = new System.Windows.Forms.Button();
            this.DeleteRangeRDkeysButton = new System.Windows.Forms.Button();
            this.Button2textBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.ReadPP2Button = new System.Windows.Forms.Button();
            this.WriteKeyButton = new System.Windows.Forms.Button();
            this.autoReadcheckBox = new System.Windows.Forms.CheckBox();
            this.Button3textBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.keyTypelistcomboBox = new System.Windows.Forms.ComboBox();
            this.Undobutton = new System.Windows.Forms.Button();
            this.Redobutton = new System.Windows.Forms.Button();
            this.OpenJsonFolderbutton = new System.Windows.Forms.Button();
            this.FunctiontabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.FYlabel.Location = new System.Drawing.Point(14, 94);
            this.FYlabel.Name = "FYlabel";
            this.FYlabel.Size = new System.Drawing.Size(95, 12);
            this.FYlabel.TabIndex = 7;
            this.FYlabel.Text = "分辨率Y（竖）：";
            // 
            // FYtextBox
            // 
            this.FYtextBox.Location = new System.Drawing.Point(110, 91);
            this.FYtextBox.Name = "FYtextBox";
            this.FYtextBox.Size = new System.Drawing.Size(42, 21);
            this.FYtextBox.TabIndex = 6;
            this.FYtextBox.Text = "720";
            this.FYtextBox.TextChanged += new System.EventHandler(this.FtextBox_TextChanged);
            this.FYtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckTextBox_KeyPress);
            // 
            // FXlabel
            // 
            this.FXlabel.AutoSize = true;
            this.FXlabel.Location = new System.Drawing.Point(14, 51);
            this.FXlabel.Name = "FXlabel";
            this.FXlabel.Size = new System.Drawing.Size(95, 12);
            this.FXlabel.TabIndex = 5;
            this.FXlabel.Text = "分辨率X（横）：";
            // 
            // FXtextBox
            // 
            this.FXtextBox.Location = new System.Drawing.Point(110, 48);
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
            this.FcheckBox.Location = new System.Drawing.Point(16, 32);
            this.FcheckBox.Name = "FcheckBox";
            this.FcheckBox.Size = new System.Drawing.Size(48, 16);
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
            this.FSave.Location = new System.Drawing.Point(163, 40);
            this.FSave.Name = "FSave";
            this.FSave.Size = new System.Drawing.Size(81, 35);
            this.FSave.TabIndex = 15;
            this.FSave.Text = "保存分辨率";
            this.FSave.UseVisualStyleBackColor = true;
            this.FSave.Click += new System.EventHandler(this.FSave_Click);
            // 
            // FLoad
            // 
            this.FLoad.Location = new System.Drawing.Point(163, 83);
            this.FLoad.Name = "FLoad";
            this.FLoad.Size = new System.Drawing.Size(81, 35);
            this.FLoad.TabIndex = 16;
            this.FLoad.Text = "读取分辨率";
            this.FLoad.UseVisualStyleBackColor = true;
            this.FLoad.Click += new System.EventHandler(this.FLoad_Click_1);
            // 
            // TOPcheckBox
            // 
            this.TOPcheckBox.AutoSize = true;
            this.TOPcheckBox.Location = new System.Drawing.Point(16, 12);
            this.TOPcheckBox.Name = "TOPcheckBox";
            this.TOPcheckBox.Size = new System.Drawing.Size(72, 16);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "当前鼠标坐标X：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "当前鼠标坐标Y：";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(166, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "保存坐标X：";
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(166, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "保存坐标Y：";
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
            // LoadJson
            // 
            this.LoadJson.Location = new System.Drawing.Point(446, 12);
            this.LoadJson.Name = "LoadJson";
            this.LoadJson.Size = new System.Drawing.Size(41, 21);
            this.LoadJson.TabIndex = 28;
            this.LoadJson.Text = "加载";
            this.LoadJson.UseVisualStyleBackColor = true;
            this.LoadJson.Click += new System.EventHandler(this.LoadJson_Click);
            // 
            // JsonUrltextBox
            // 
            this.JsonUrltextBox.Location = new System.Drawing.Point(158, 13);
            this.JsonUrltextBox.Name = "JsonUrltextBox";
            this.JsonUrltextBox.ReadOnly = true;
            this.JsonUrltextBox.Size = new System.Drawing.Size(284, 21);
            this.JsonUrltextBox.TabIndex = 29;
            this.JsonUrltextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(253, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 31;
            this.label6.Text = "绑定按键：";
            // 
            // ButtontextBox
            // 
            this.ButtontextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ButtontextBox.Location = new System.Drawing.Point(311, 43);
            this.ButtontextBox.Name = "ButtontextBox";
            this.ButtontextBox.Size = new System.Drawing.Size(40, 21);
            this.ButtontextBox.TabIndex = 32;
            this.ButtontextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ButtontextBox_KeyDown);
            this.ButtontextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtontextBox_KeyPress);
            // 
            // CheckButton
            // 
            this.CheckButton.Location = new System.Drawing.Point(400, 42);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(42, 23);
            this.CheckButton.TabIndex = 33;
            this.CheckButton.Text = "检查";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(179, 12);
            this.label8.TabIndex = 37;
            this.label8.Text = "注：修改/读取范围详见说明书。";
            // 
            // OpenJson
            // 
            this.OpenJson.Location = new System.Drawing.Point(110, 12);
            this.OpenJson.Name = "OpenJson";
            this.OpenJson.Size = new System.Drawing.Size(42, 23);
            this.OpenJson.TabIndex = 38;
            this.OpenJson.Text = "浏览";
            this.OpenJson.UseVisualStyleBackColor = true;
            this.OpenJson.Click += new System.EventHandler(this.OpenJson_Click);
            // 
            // RewriteAndSaveButton
            // 
            this.RewriteAndSaveButton.Location = new System.Drawing.Point(357, 70);
            this.RewriteAndSaveButton.Name = "RewriteAndSaveButton";
            this.RewriteAndSaveButton.Size = new System.Drawing.Size(85, 23);
            this.RewriteAndSaveButton.TabIndex = 39;
            this.RewriteAndSaveButton.Text = "修改并保存";
            this.RewriteAndSaveButton.UseVisualStyleBackColor = true;
            this.RewriteAndSaveButton.Click += new System.EventHandler(this.RewriteAndSaveButton_Click);
            // 
            // CheckFileChangetimer
            // 
            this.CheckFileChangetimer.Enabled = true;
            this.CheckFileChangetimer.Interval = 500;
            this.CheckFileChangetimer.Tick += new System.EventHandler(this.CheckFileChangetimer_Tick);
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(309, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 14);
            this.label7.TabIndex = 41;
            this.label7.Text = "Ctrl+";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(309, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 14);
            this.label9.TabIndex = 42;
            this.label9.Text = "Ctrl+";
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(409, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 47;
            this.label10.Text = "保存当前鼠标坐标";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(409, 97);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 12);
            this.label11.TabIndex = 48;
            this.label11.Text = "移动鼠标至保存位置";
            // 
            // QhrxlinkLabel
            // 
            this.QhrxlinkLabel.AutoSize = true;
            this.QhrxlinkLabel.Location = new System.Drawing.Point(14, 344);
            this.QhrxlinkLabel.Name = "QhrxlinkLabel";
            this.QhrxlinkLabel.Size = new System.Drawing.Size(77, 12);
            this.QhrxlinkLabel.TabIndex = 49;
            this.QhrxlinkLabel.TabStop = true;
            this.QhrxlinkLabel.Text = "By：漆黑人形";
            this.QhrxlinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.QhrxlinkLabel_LinkClicked);
            // 
            // ReadPPButton
            // 
            this.ReadPPButton.Location = new System.Drawing.Point(357, 42);
            this.ReadPPButton.Name = "ReadPPButton";
            this.ReadPPButton.Size = new System.Drawing.Size(42, 23);
            this.ReadPPButton.TabIndex = 50;
            this.ReadPPButton.Text = "读取";
            this.ReadPPButton.UseVisualStyleBackColor = true;
            this.ReadPPButton.Click += new System.EventHandler(this.ReadPPButton_Click);
            // 
            // KeysListcomboBox
            // 
            this.KeysListcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeysListcomboBox.FormattingEnabled = true;
            this.KeysListcomboBox.Location = new System.Drawing.Point(448, 43);
            this.KeysListcomboBox.Name = "KeysListcomboBox";
            this.KeysListcomboBox.Size = new System.Drawing.Size(127, 20);
            this.KeysListcomboBox.TabIndex = 51;
            this.KeysListcomboBox.DropDown += new System.EventHandler(this.KeysListcomboBox_DropDown);
            // 
            // WriteKeysButton
            // 
            this.WriteKeysButton.Location = new System.Drawing.Point(448, 71);
            this.WriteKeysButton.Name = "WriteKeysButton";
            this.WriteKeysButton.Size = new System.Drawing.Size(127, 23);
            this.WriteKeysButton.TabIndex = 52;
            this.WriteKeysButton.Text = "写入并保存常用键";
            this.WriteKeysButton.UseVisualStyleBackColor = true;
            this.WriteKeysButton.Click += new System.EventHandler(this.WriteKeysButton_Click);
            // 
            // FunctiontabControl
            // 
            this.FunctiontabControl.Controls.Add(this.tabPage1);
            this.FunctiontabControl.Controls.Add(this.tabPage2);
            this.FunctiontabControl.Controls.Add(this.tabPage3);
            this.FunctiontabControl.Location = new System.Drawing.Point(16, 160);
            this.FunctiontabControl.Name = "FunctiontabControl";
            this.FunctiontabControl.SelectedIndex = 0;
            this.FunctiontabControl.Size = new System.Drawing.Size(563, 181);
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
            this.tabPage1.Size = new System.Drawing.Size(555, 155);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "坐标计算";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.NCXtextBox);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.NCYtextBox);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.SCXtextBox);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.LoadKeybutton);
            this.tabPage2.Controls.Add(this.SCYtextBox);
            this.tabPage2.Controls.Add(this.SaveKeybutton);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.ResetKeytextBox);
            this.tabPage2.Controls.Add(this.CcheckBox);
            this.tabPage2.Controls.Add(this.FindKeytextBox);
            this.tabPage2.Controls.Add(this.EcheckBox);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(555, 155);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "鼠标回溯";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.DeleteRangeLTRDkeysButton);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.RangeRDXtextBox);
            this.tabPage3.Controls.Add(this.RangeRDYtextBox);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.RangeLTXtextBox);
            this.tabPage3.Controls.Add(this.RangeLTYtextBox);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(555, 155);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "区域键位清空";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(521, 36);
            this.label15.TabIndex = 22;
            this.label15.Text = "这个模块是对 右下清空 按钮的功能补充。原按钮只对通用的16:9比例的右下角进行键位清除。\r\n\r\n该模块录入指定区域左上角X,Y坐标，右下角X,Y坐标后，按 区域" +
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "区域左上坐标X：";
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
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "区域左上坐标Y：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(221, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 12);
            this.label13.TabIndex = 17;
            this.label13.Text = "区域右下坐标X：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(221, 101);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 12);
            this.label14.TabIndex = 19;
            this.label14.Text = "区域右下坐标Y：";
            // 
            // DeleteRepeatKeysButton
            // 
            this.DeleteRepeatKeysButton.Location = new System.Drawing.Point(448, 100);
            this.DeleteRepeatKeysButton.Name = "DeleteRepeatKeysButton";
            this.DeleteRepeatKeysButton.Size = new System.Drawing.Size(63, 23);
            this.DeleteRepeatKeysButton.TabIndex = 54;
            this.DeleteRepeatKeysButton.Text = "键位去重";
            this.DeleteRepeatKeysButton.UseVisualStyleBackColor = true;
            this.DeleteRepeatKeysButton.Click += new System.EventHandler(this.DeleteRepeatKeysButton_Click);
            // 
            // DeleteRangeRDkeysButton
            // 
            this.DeleteRangeRDkeysButton.Location = new System.Drawing.Point(515, 100);
            this.DeleteRangeRDkeysButton.Name = "DeleteRangeRDkeysButton";
            this.DeleteRangeRDkeysButton.Size = new System.Drawing.Size(63, 23);
            this.DeleteRangeRDkeysButton.TabIndex = 55;
            this.DeleteRangeRDkeysButton.Text = "右下清空";
            this.DeleteRangeRDkeysButton.UseVisualStyleBackColor = true;
            this.DeleteRangeRDkeysButton.Click += new System.EventHandler(this.DeleteRangeRDkeysButton_Click);
            // 
            // Button2textBox
            // 
            this.Button2textBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Button2textBox.Location = new System.Drawing.Point(484, 127);
            this.Button2textBox.Name = "Button2textBox";
            this.Button2textBox.Size = new System.Drawing.Size(40, 21);
            this.Button2textBox.TabIndex = 57;
            this.Button2textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Button2textBox_KeyDown);
            this.Button2textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Button2textBox_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(446, 130);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 12);
            this.label16.TabIndex = 56;
            this.label16.Text = "按键：";
            // 
            // ReadPP2Button
            // 
            this.ReadPP2Button.Location = new System.Drawing.Point(530, 126);
            this.ReadPP2Button.Name = "ReadPP2Button";
            this.ReadPP2Button.Size = new System.Drawing.Size(48, 23);
            this.ReadPP2Button.TabIndex = 58;
            this.ReadPP2Button.Text = "读取";
            this.ReadPP2Button.UseVisualStyleBackColor = true;
            this.ReadPP2Button.Click += new System.EventHandler(this.ReadPP2Button_Click);
            // 
            // WriteKeyButton
            // 
            this.WriteKeyButton.Location = new System.Drawing.Point(448, 155);
            this.WriteKeyButton.Name = "WriteKeyButton";
            this.WriteKeyButton.Size = new System.Drawing.Size(127, 23);
            this.WriteKeyButton.TabIndex = 59;
            this.WriteKeyButton.Text = "写入并保存单个键";
            this.WriteKeyButton.UseVisualStyleBackColor = true;
            this.WriteKeyButton.Click += new System.EventHandler(this.WriteKeyButton_Click);
            // 
            // autoReadcheckBox
            // 
            this.autoReadcheckBox.AutoSize = true;
            this.autoReadcheckBox.Location = new System.Drawing.Point(255, 122);
            this.autoReadcheckBox.Name = "autoReadcheckBox";
            this.autoReadcheckBox.Size = new System.Drawing.Size(96, 16);
            this.autoReadcheckBox.TabIndex = 60;
            this.autoReadcheckBox.Text = "自动读取坐标";
            this.autoReadcheckBox.UseVisualStyleBackColor = true;
            this.autoReadcheckBox.CheckedChanged += new System.EventHandler(this.autoReadcheckBox_CheckedChanged);
            // 
            // Button3textBox
            // 
            this.Button3textBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Button3textBox.Location = new System.Drawing.Point(363, 99);
            this.Button3textBox.Name = "Button3textBox";
            this.Button3textBox.Size = new System.Drawing.Size(40, 21);
            this.Button3textBox.TabIndex = 62;
            this.Button3textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Button3textBox_KeyDown);
            this.Button3textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Button3textBox_KeyPress);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(253, 104);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(113, 12);
            this.label17.TabIndex = 61;
            this.label17.Text = "待自动读取的按键：";
            // 
            // keyTypelistcomboBox
            // 
            this.keyTypelistcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyTypelistcomboBox.FormattingEnabled = true;
            this.keyTypelistcomboBox.Location = new System.Drawing.Point(254, 71);
            this.keyTypelistcomboBox.Name = "keyTypelistcomboBox";
            this.keyTypelistcomboBox.Size = new System.Drawing.Size(97, 20);
            this.keyTypelistcomboBox.TabIndex = 63;
            // 
            // Undobutton
            // 
            this.Undobutton.Location = new System.Drawing.Point(490, 12);
            this.Undobutton.Name = "Undobutton";
            this.Undobutton.Size = new System.Drawing.Size(41, 21);
            this.Undobutton.TabIndex = 64;
            this.Undobutton.Text = "撤销";
            this.Undobutton.UseVisualStyleBackColor = true;
            this.Undobutton.Click += new System.EventHandler(this.Undobutton_Click);
            // 
            // Redobutton
            // 
            this.Redobutton.Location = new System.Drawing.Point(534, 12);
            this.Redobutton.Name = "Redobutton";
            this.Redobutton.Size = new System.Drawing.Size(41, 21);
            this.Redobutton.TabIndex = 65;
            this.Redobutton.Text = "重做";
            this.Redobutton.UseVisualStyleBackColor = true;
            this.Redobutton.Click += new System.EventHandler(this.Redobutton_Click);
            // 
            // OpenJsonFolderbutton
            // 
            this.OpenJsonFolderbutton.Location = new System.Drawing.Point(254, 144);
            this.OpenJsonFolderbutton.Name = "OpenJsonFolderbutton";
            this.OpenJsonFolderbutton.Size = new System.Drawing.Size(97, 23);
            this.OpenJsonFolderbutton.TabIndex = 66;
            this.OpenJsonFolderbutton.Text = "快捷打开文件夹";
            this.OpenJsonFolderbutton.UseVisualStyleBackColor = true;
            this.OpenJsonFolderbutton.Click += new System.EventHandler(this.OpenJsonFolderbutton_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 366);
            this.Controls.Add(this.OpenJsonFolderbutton);
            this.Controls.Add(this.Redobutton);
            this.Controls.Add(this.Undobutton);
            this.Controls.Add(this.keyTypelistcomboBox);
            this.Controls.Add(this.Button3textBox);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.autoReadcheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.WriteKeyButton);
            this.Controls.Add(this.ReadPP2Button);
            this.Controls.Add(this.Button2textBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.DeleteRangeRDkeysButton);
            this.Controls.Add(this.DeleteRepeatKeysButton);
            this.Controls.Add(this.FunctiontabControl);
            this.Controls.Add(this.WriteKeysButton);
            this.Controls.Add(this.KeysListcomboBox);
            this.Controls.Add(this.ReadPPButton);
            this.Controls.Add(this.QhrxlinkLabel);
            this.Controls.Add(this.RewriteAndSaveButton);
            this.Controls.Add(this.OpenJson);
            this.Controls.Add(this.CheckButton);
            this.Controls.Add(this.ButtontextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.JsonUrltextBox);
            this.Controls.Add(this.LoadJson);
            this.Controls.Add(this.TOPcheckBox);
            this.Controls.Add(this.FLoad);
            this.Controls.Add(this.FSave);
            this.Controls.Add(this.FcheckBox);
            this.Controls.Add(this.FYlabel);
            this.Controls.Add(this.FYtextBox);
            this.Controls.Add(this.FXlabel);
            this.Controls.Add(this.FXtextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "MuMu摸点小助手2.8";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.Leave += new System.EventHandler(this.Form1_Leave);
            this.FunctiontabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
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
        private System.Windows.Forms.Label FXlabel;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NCYtextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SCXtextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SCYtextBox;
        private System.Windows.Forms.CheckBox CcheckBox;
        private System.Windows.Forms.Timer Ctimer;
        private System.Windows.Forms.OpenFileDialog JsonopenFileDialog;
        private System.Windows.Forms.Button LoadJson;
        private System.Windows.Forms.TextBox JsonUrltextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ButtontextBox;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button OpenJson;
        private System.Windows.Forms.Button RewriteAndSaveButton;
        private System.Windows.Forms.Timer CheckFileChangetimer;
        private System.Windows.Forms.CheckBox EcheckBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox FindKeytextBox;
        private System.Windows.Forms.TextBox ResetKeytextBox;
        private System.Windows.Forms.Button SaveKeybutton;
        private System.Windows.Forms.Button LoadKeybutton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel QhrxlinkLabel;
        private System.Windows.Forms.Button ReadPPButton;
        private System.Windows.Forms.ComboBox KeysListcomboBox;
        private System.Windows.Forms.Button WriteKeysButton;
        private System.Windows.Forms.TabControl FunctiontabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button DeleteRepeatKeysButton;
        private System.Windows.Forms.Button DeleteRangeRDkeysButton;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox RangeRDXtextBox;
        private System.Windows.Forms.TextBox RangeRDYtextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RangeLTXtextBox;
        private System.Windows.Forms.TextBox RangeLTYtextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button DeleteRangeLTRDkeysButton;
        private System.Windows.Forms.TextBox Button2textBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button ReadPP2Button;
        private System.Windows.Forms.Button WriteKeyButton;
        private System.Windows.Forms.CheckBox autoReadcheckBox;
        private System.Windows.Forms.TextBox Button3textBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox keyTypelistcomboBox;
        private System.Windows.Forms.Button Undobutton;
        private System.Windows.Forms.Button Redobutton;
        private System.Windows.Forms.Button OpenJsonFolderbutton;
    }
}

