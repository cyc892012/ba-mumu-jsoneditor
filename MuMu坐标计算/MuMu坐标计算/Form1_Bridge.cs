using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace MuMu坐标计算
{
    public partial class Form1
    {
        private readonly Dictionary<Type, Form> _openChildForms = new Dictionary<Type, Form>();

        public void ShowChildForm(Form childForm)
        {
            Type formType = childForm.GetType();
            if (_openChildForms.TryGetValue(formType, out var existing) && !existing.IsDisposed) { existing.BringToFront(); childForm.Dispose(); return; }
            _openChildForms[formType] = childForm;
            childForm.FormClosed += (s2, e2) => _openChildForms.Remove(formType);
            childForm.StartPosition = FormStartPosition.Manual;
            childForm.Location = CalculateChildPosition(childForm);
            childForm.Show(this);
        }

        private System.Drawing.Point CalculateChildPosition(Form child)
        {
            int x = Left + (Width - child.Width) / 2;
            int y = Top + (Height - child.Height) / 2;
            var scr = Screen.FromControl(this).WorkingArea;
            return new System.Drawing.Point(Math.Max(scr.Left, Math.Min(x, scr.Right - child.Width)), Math.Max(scr.Top, Math.Min(y, scr.Bottom - child.Height)));
        }

        private void fileNamecomboBox2_DropDown(object sender, EventArgs e) { fileNameSearchtextBox2.Visible = true; fileNameSearchtextBox2.BringToFront(); fileNameSearchtextBox2.Focus(); }
        private void fileNamecomboBox2_DropDownClosed(object sender, EventArgs e) { fileNameSearchtextBox2.Visible = false; }

        private void fileNameSearchtextBox2_TextChanged(object sender, EventArgs e) { if (!string.IsNullOrWhiteSpace(fileNameSearchtextBox2.Text)) InitializeFileNamecomboBox2(fileNamecomboBox2, false, fileNameSearchtextBox2.Text); }
        private void fileNameSearchtextBox2_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { fileNamecomboBox2.DroppedDown = false; e.Handled = true; } else if (e.KeyCode == Keys.Up && fileNamecomboBox2.SelectedIndex > 0) { fileNamecomboBox2.SelectedIndex--; e.Handled = true; } else if (e.KeyCode == Keys.Down && fileNamecomboBox2.SelectedIndex < fileNamecomboBox2.Items.Count - 1) { fileNamecomboBox2.SelectedIndex++; e.Handled = true; } }

        private void ktckPListcheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e) { if (isUpdatingCheckState) return; isUpdatingCheckState = true; if (e.Index == 0) { bool c = e.NewValue == CheckState.Checked; for (int i = 1; i < ktckPListcheckedListBox.Items.Count; i++) ktckPListcheckedListBox.SetItemChecked(i, c); } isUpdatingCheckState = false; }
        private void ktckPListcheckedListBox_SelectedIndexChanged(object sender, EventArgs e) { if (ktckPListcheckedListBox.SelectedIndex >= 0 && ktckPListcheckedListBox.SelectedIndex < ktckPListcheckedListBox.Items.Count) { if (ktckPListcheckedListBox.Items[ktckPListcheckedListBox.SelectedIndex].ToString() != "全选" && !string.IsNullOrEmpty(_mumuJson)) { var coord = MuMuJsonEditor.ReadKeyPP(_mumuJson, new KeyEventArgs(Keys.None)); if (coord != null) { ktckKXtextBox.Text = coord[0]; ktckKYtextBox.Text = coord[1]; } } } }

        private void packageNamecomboBox_SelectedIndexChanged(object sender, EventArgs e) { if (flagFlushingFilename) InitializeFileNamecomboBox2(fileNamecomboBox2, false); }
        private void resolutionTypecomboBox_SelectedIndexChanged(object sender, EventArgs e) { if (flagFlushingResolutionType) InitializeResolutioncomboBox(resolutioncomboBox, resolutionTypecomboBox.SelectedValue?.ToString()); }
        private void resolutionTypecomboBox2_SelectedIndexChanged(object sender, EventArgs e) { if (flagFlushingResolutionType) InitializeResolutioncomboBox(resolutioncomboBox2, resolutionTypecomboBox2.SelectedValue?.ToString()); }
        private void statusAuthor_Click(object sender, EventArgs e) { try { using (System.Diagnostics.Process.Start("https://space.bilibili.com/251589")) { } } catch (Exception ex) { MessageBox.Show("无法打开链接：" + ex.Message); } }

        private void InitializeFileNamecomboBox2(ComboBox cb, bool flagback, string searchText = null)
        { try { flagFlushingFilename = false; var folder = _config.JsonFolderPath; if (!Directory.Exists(folder)) { flagFlushingFilename = true; return; } var files = Directory.GetFiles(folder, "*.json", SearchOption.TopDirectoryOnly); var items = new List<KeyValuePair<string, string>>(); foreach (var f in files) { var n = Path.GetFileNameWithoutExtension(f); if (string.IsNullOrEmpty(searchText) || n.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) items.Add(new KeyValuePair<string, string>(f, n)); } if (items.Count == 0 && !string.IsNullOrEmpty(searchText)) { cb.ValueMember = ""; cb.DisplayMember = ""; cb.DataSource = null; cb.Items.Clear(); cb.Items.Add("N/A"); } else { cb.DisplayMember = "Value"; cb.ValueMember = "Key"; cb.DataSource = items; if (flagback && items.Count > 0) cb.SelectedIndex = 0; } flagFlushingFilename = true; } catch { flagFlushingFilename = true; } }
    }
}