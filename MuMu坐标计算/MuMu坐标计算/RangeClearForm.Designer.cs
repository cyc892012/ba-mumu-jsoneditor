using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class RangeClearForm
    {
        private System.ComponentModel.IContainer components = null;
        internal TextBox _rangeLTXtextBox;
        internal TextBox _rangeLTYtextBox;
        internal TextBox _rangeRDXtextBox;
        internal TextBox _rangeRDYtextBox;
        internal Button _deleteButton;
        internal Label _lblRangeClearTip;
        internal Label _lblRangeLTX;
        internal Label _lblRangeLTY;
        internal Label _lblRangeRDX;
        internal Label _lblRangeRDY;
        internal CheckBox _topCheckBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangeClearForm));
            this._lblRangeClearTip = new System.Windows.Forms.Label();
            this._topCheckBox = new System.Windows.Forms.CheckBox();
            this._lblRangeLTX = new System.Windows.Forms.Label();
            this._rangeLTXtextBox = new System.Windows.Forms.TextBox();
            this._lblRangeLTY = new System.Windows.Forms.Label();
            this._rangeLTYtextBox = new System.Windows.Forms.TextBox();
            this._lblRangeRDX = new System.Windows.Forms.Label();
            this._rangeRDXtextBox = new System.Windows.Forms.TextBox();
            this._lblRangeRDY = new System.Windows.Forms.Label();
            this._rangeRDYtextBox = new System.Windows.Forms.TextBox();
            this._deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lblRangeClearTip
            // 
            this._lblRangeClearTip.AutoSize = true;
            this._lblRangeClearTip.Location = new System.Drawing.Point(10, 11);
            this._lblRangeClearTip.Name = "_lblRangeClearTip";
            this._lblRangeClearTip.Size = new System.Drawing.Size(520, 51);
            this._lblRangeClearTip.TabIndex = 0;
            this._lblRangeClearTip.Text = "这个模块是对 右下清空 按钮的功能补充。原按钮只对通用的16:9比例的右下角进行键位清除。\r\n\r\n该模块录入指定区域左上角X,Y坐标，右下角X,Y坐标后，按 区域" +
    "清空 按钮清空该区域所有键位。";
            // 
            // _topCheckBox
            // 
            this._topCheckBox.AutoSize = true;
            this._topCheckBox.Location = new System.Drawing.Point(10, 45);
            this._topCheckBox.Name = "_topCheckBox";
            this._topCheckBox.Size = new System.Drawing.Size(75, 21);
            this._topCheckBox.TabIndex = 1;
            this._topCheckBox.Text = "窗口置顶";
            this._topCheckBox.UseVisualStyleBackColor = true;
            // 
            // _lblRangeLTX
            // 
            this._lblRangeLTX.AutoSize = true;
            this._lblRangeLTX.Location = new System.Drawing.Point(10, 68);
            this._lblRangeLTX.Name = "_lblRangeLTX";
            this._lblRangeLTX.Size = new System.Drawing.Size(100, 17);
            this._lblRangeLTX.TabIndex = 2;
            this._lblRangeLTX.Text = "区域左上坐标X：";
            // 
            // _rangeLTXtextBox
            // 
            this._rangeLTXtextBox.Location = new System.Drawing.Point(110, 65);
            this._rangeLTXtextBox.Name = "_rangeLTXtextBox";
            this._rangeLTXtextBox.Size = new System.Drawing.Size(100, 23);
            this._rangeLTXtextBox.TabIndex = 3;
            this._rangeLTXtextBox.Text = "0";
            // 
            // _lblRangeLTY
            // 
            this._lblRangeLTY.AutoSize = true;
            this._lblRangeLTY.Location = new System.Drawing.Point(10, 101);
            this._lblRangeLTY.Name = "_lblRangeLTY";
            this._lblRangeLTY.Size = new System.Drawing.Size(99, 17);
            this._lblRangeLTY.TabIndex = 4;
            this._lblRangeLTY.Text = "区域左上坐标Y：";
            // 
            // _rangeLTYtextBox
            // 
            this._rangeLTYtextBox.Location = new System.Drawing.Point(110, 98);
            this._rangeLTYtextBox.Name = "_rangeLTYtextBox";
            this._rangeLTYtextBox.Size = new System.Drawing.Size(100, 23);
            this._rangeLTYtextBox.TabIndex = 5;
            this._rangeLTYtextBox.Text = "0";
            // 
            // _lblRangeRDX
            // 
            this._lblRangeRDX.AutoSize = true;
            this._lblRangeRDX.Location = new System.Drawing.Point(221, 68);
            this._lblRangeRDX.Name = "_lblRangeRDX";
            this._lblRangeRDX.Size = new System.Drawing.Size(100, 17);
            this._lblRangeRDX.TabIndex = 6;
            this._lblRangeRDX.Text = "区域右下坐标X：";
            // 
            // _rangeRDXtextBox
            // 
            this._rangeRDXtextBox.Location = new System.Drawing.Point(322, 65);
            this._rangeRDXtextBox.Name = "_rangeRDXtextBox";
            this._rangeRDXtextBox.Size = new System.Drawing.Size(100, 23);
            this._rangeRDXtextBox.TabIndex = 7;
            this._rangeRDXtextBox.Text = "0";
            // 
            // _lblRangeRDY
            // 
            this._lblRangeRDY.AutoSize = true;
            this._lblRangeRDY.Location = new System.Drawing.Point(221, 101);
            this._lblRangeRDY.Name = "_lblRangeRDY";
            this._lblRangeRDY.Size = new System.Drawing.Size(99, 17);
            this._lblRangeRDY.TabIndex = 8;
            this._lblRangeRDY.Text = "区域右下坐标Y：";
            // 
            // _rangeRDYtextBox
            // 
            this._rangeRDYtextBox.Location = new System.Drawing.Point(322, 98);
            this._rangeRDYtextBox.Name = "_rangeRDYtextBox";
            this._rangeRDYtextBox.Size = new System.Drawing.Size(100, 23);
            this._rangeRDYtextBox.TabIndex = 9;
            this._rangeRDYtextBox.Text = "0";
            // 
            // _deleteButton
            // 
            this._deleteButton.Location = new System.Drawing.Point(438, 63);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(74, 25);
            this._deleteButton.TabIndex = 10;
            this._deleteButton.Text = "区域清空";
            this._deleteButton.UseVisualStyleBackColor = true;
            // 
            // RangeClearForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(524, 180);
            this.Controls.Add(this._lblRangeClearTip);
            this.Controls.Add(this._topCheckBox);
            this.Controls.Add(this._lblRangeLTX);
            this.Controls.Add(this._rangeLTXtextBox);
            this.Controls.Add(this._lblRangeLTY);
            this.Controls.Add(this._rangeLTYtextBox);
            this.Controls.Add(this._lblRangeRDX);
            this.Controls.Add(this._rangeRDXtextBox);
            this.Controls.Add(this._lblRangeRDY);
            this.Controls.Add(this._rangeRDYtextBox);
            this.Controls.Add(this._deleteButton);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RangeClearForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "区域键位清空";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
