using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class RangeClearForm : Form
    {
        private readonly Func<string> _getCurrentJson;
        private readonly Func<double> _getResolutionX;
        private readonly Func<double> _getResolutionY;
        private readonly Func<string, bool> _writeJsonCallback;

        public RangeClearForm(
            Func<string> getCurrentJson,
            Func<double> getResolutionX,
            Func<double> getResolutionY,
            Func<string, bool> writeJsonCallback)
        {
            _getCurrentJson = getCurrentJson ?? throw new ArgumentNullException(nameof(getCurrentJson));
            _getResolutionX = getResolutionX ?? throw new ArgumentNullException(nameof(getResolutionX));
            _getResolutionY = getResolutionY ?? throw new ArgumentNullException(nameof(getResolutionY));
            _writeJsonCallback = writeJsonCallback ?? throw new ArgumentNullException(nameof(writeJsonCallback));

            InitializeComponent();

            _topCheckBox.CheckedChanged += TopCheckBox_CheckedChanged;
            _rangeLTXtextBox.KeyPress += CheckTextBox_KeyPress;
            _rangeLTYtextBox.KeyPress += CheckTextBox_KeyPress;
            _rangeRDXtextBox.KeyPress += CheckTextBox_KeyPress;
            _rangeRDYtextBox.KeyPress += CheckTextBox_KeyPress;
            _deleteButton.Click += DeleteButton_Click;
        }

        private void CheckTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
                e.Handled = true;
            if (e.KeyChar == '.' && (sender as TextBox)?.Text?.Contains(".") == true)
                e.Handled = true;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("区域清空功能存在风险，使用前请确认选择区域不存在要保留的按键！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;

                double FX = _getResolutionX() - 1;
                double FY = _getResolutionY() - 1;
                if (FX <= 0 || FY <= 0) return;

                if (!SafeParseHelper.TryGetDouble(_rangeLTXtextBox, out double LTX) ||
                    !SafeParseHelper.TryGetDouble(_rangeLTYtextBox, out double LTY) ||
                    !SafeParseHelper.TryGetDouble(_rangeRDXtextBox, out double RDX) ||
                    !SafeParseHelper.TryGetDouble(_rangeRDYtextBox, out double RDY))
                    return;

                if (LTX >= RDX || LTY >= RDY)
                {
                    MessageBox.Show("左上角坐标必须小于右下角坐标！");
                    return;
                }
                double[] rangeLT = { (LTX / FX), (LTY / FY) };
                double[] rangeRD = { (RDX / FX), (RDY / FY) };

                string currentJson = _getCurrentJson();
                var results = MuMuJsonEditor.FindRangeKeyValues(rangeLT, rangeRD, currentJson);
                if (results.Count == 0) { MessageBox.Show("选中区域不存在按键，无需清空。"); return; }

                var keyTexts = new List<string>();
                foreach (var (_, _, text, vk) in results)
                {
                    currentJson = MuMuJsonEditor.DeleteKey(vk, currentJson);
                    keyTexts.Add(text);
                }
                if (_writeJsonCallback(currentJson))
                    MessageBox.Show("已清空：" + string.Join(",", keyTexts) + "键，如出现问题请转人工！");
                else
                    MessageBox.Show("写入失败，清空操作未完成！请重新尝试。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message + "\n请检查您的输入内容！");
            }
        }

        private void TopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = _topCheckBox.Checked;
        }
    }
}
