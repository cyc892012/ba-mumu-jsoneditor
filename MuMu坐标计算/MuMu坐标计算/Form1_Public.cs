using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    public partial class Form1
    {
#pragma warning disable CS0649
        private MuMu坐标计算.SearchableComboBox searchKeysCombo;
#pragma warning restore CS0649
        private System.Windows.Forms.TextBox Button2textBox;

        public void EnsureSearchControls()
        {
            if (searchKeysCombo == null)
            {
                searchKeysCombo = new MuMu坐标计算.SearchableComboBox();
                searchKeysCombo.Name = "searchKeysCombo";
                searchKeysCombo.Location = new Point(0, 0);
                searchKeysCombo.Size = new Size(120, 21);
                searchKeysCombo.Visible = false;
                this.Controls.Add(searchKeysCombo);
            }
            if (Button2textBox == null)
            {
                Button2textBox = new TextBox();
                Button2textBox.Name = "Button2textBox";
                Button2textBox.Location = new Point(0, 0);
                Button2textBox.Size = new Size(100, 21);
                Button2textBox.Visible = false;
                this.Controls.Add(Button2textBox);
            }
        }

        public string GetMuMuJson()
        {
            return _mumuJson;
        }

        public void SetMuMuJson(string json)
        {
            _mumuJson = json;
        }

        public bool SaveJsonAndBackup()
        {
            return WriteToJsonAndBackup();
        }

        public string GetJsonFilePath()
        {
            return JsonUrltextBox.Text;
        }

        public string GetCurrentKeyType()
        {
            return keyTypelistcomboBox.SelectedValue?.ToString() ?? MuMuJsonEditor.typeClick;
        }

        public void SetCurrentKeyType(string keyType)
        {
            if (string.IsNullOrEmpty(keyType)) return;
            keyTypelistcomboBox.SelectedValue = keyType;
        }

        public void ApplyPostLoadLayout()
        {
            gbKeyEdit.Height = 55;
        }

        public string StripPackageName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return fileName;
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            foreach (var prefix in PackageNameTypes.Values)
            {
                if (prefix != "other" && prefix != "萌新666sssaaa" && nameWithoutExt.StartsWith(prefix))
                    return nameWithoutExt.Substring(prefix.Length) + ext;
            }
            return fileName;
        }

        private void SetupKeyPresetFeature()
        {
            _featureToolTip.SetToolTip(featureBtnKeyPreset, "管理基础键位预设，支持写入、去重、导入导出等操作。");
            featureBtnKeyPreset.Click += (s, e) => ShowChildForm(new KeyPresetForm(
                _keyboardHandler,
                () => _mumuJson,
                json => _mumuJson = json,
                () => WriteToJsonAndBackup(),
                flagBack => { InitializeFileNamecomboBox(searchFileCombo, flagBack); }
            ));
        }
    }
}