using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class SearchableComboBox
    {
        private System.ComponentModel.IContainer components = null;
        internal ComboBox cmbMain;
        internal TextBox txtSearch;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (cmbMain != null)
                {
                    cmbMain.DropDown -= CmbMain_DropDown;
                    cmbMain.DropDownClosed -= CmbMain_DropDownClosed;
                    cmbMain.Dispose();
                }
                if (txtSearch != null)
                {
                    txtSearch.TextChanged -= TxtSearch_TextChanged;
                    txtSearch.KeyDown -= TxtSearch_KeyDown;
                    txtSearch.Dispose();
                }
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            cmbMain = new ComboBox();
            cmbMain.Dock = DockStyle.Fill;
            cmbMain.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMain.FormattingEnabled = true;
            cmbMain.IntegralHeight = false;

            txtSearch = new TextBox();
            txtSearch.Visible = false;

            this.Controls.Add(cmbMain);
            this.Controls.Add(txtSearch);
            this.Name = "SearchableComboBox";
            this.Size = new System.Drawing.Size(154, 21);
            this.ResumeLayout(false);
        }
    }
}
