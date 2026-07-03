using System;
using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    public partial class SearchableComboBox : UserControl
    {
        public event EventHandler DropDown { add { cmbMain.DropDown += value; } remove { cmbMain.DropDown -= value; } }
        public event EventHandler DropDownClosed { add { cmbMain.DropDownClosed += value; } remove { cmbMain.DropDownClosed -= value; } }
        public event EventHandler SelectedIndexChanged { add { cmbMain.SelectedIndexChanged += value; } remove { cmbMain.SelectedIndexChanged -= value; } }
        public ComboBox InnerComboBox => cmbMain;
        public string CurrentSearchText => txtSearch.Text;
        public event EventHandler<string> FilterRequested;

        public SearchableComboBox()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                cmbMain.DropDown += CmbMain_DropDown;
                cmbMain.DropDownClosed += CmbMain_DropDownClosed;
                txtSearch.TextChanged += TxtSearch_TextChanged;
                txtSearch.KeyDown += TxtSearch_KeyDown;
            }
        }

        private void CmbMain_DropDown(object s, EventArgs e) { txtSearch.Location = cmbMain.Location; txtSearch.Size = cmbMain.Size; txtSearch.Visible = true; txtSearch.BringToFront(); txtSearch.Focus(); }
        private void CmbMain_DropDownClosed(object s, EventArgs e) { txtSearch.Visible = false; }
        private void TxtSearch_TextChanged(object s, EventArgs e) { FilterRequested?.Invoke(this, txtSearch.Text); }
        private void TxtSearch_KeyDown(object s, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { cmbMain.DroppedDown = false; Application.DoEvents(); e.Handled = true; } else if (e.KeyCode == Keys.Up && cmbMain.SelectedIndex > 0) { cmbMain.SelectedIndex -= 1; e.Handled = true; } else if (e.KeyCode == Keys.Down && cmbMain.SelectedIndex < cmbMain.Items.Count - 1) { cmbMain.SelectedIndex += 1; e.Handled = true; } }

        public void ClearSearchText() { txtSearch.Clear(); }
        public int DropDownHeight { get => cmbMain.DropDownHeight; set => cmbMain.DropDownHeight = value; }
        public object DataSource { get => cmbMain.DataSource; set => cmbMain.DataSource = value; }
        public string DisplayMember { get => cmbMain.DisplayMember; set => cmbMain.DisplayMember = value; }
        public string ValueMember { get => cmbMain.ValueMember; set => cmbMain.ValueMember = value; }
        public bool DroppedDown { get => cmbMain.DroppedDown; set => cmbMain.DroppedDown = value; }
        public object SelectedValue => cmbMain.SelectedValue;
        public object SelectedItem { get => cmbMain.SelectedItem; set => cmbMain.SelectedItem = value; }
        public int SelectedIndex { get => cmbMain.SelectedIndex; set => cmbMain.SelectedIndex = value; }
        public ComboBox.ObjectCollection Items => cmbMain.Items;
    }
}