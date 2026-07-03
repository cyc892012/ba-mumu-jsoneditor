using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    /// <summary>
    /// 统一的 ComboBox 初始化辅助类，消除 InitializeKeysComboBox / InitializeFileNamecomboBox 中的重复模式。
    /// </summary>
    public static class ComboBoxInitializer
    {
        public static readonly Dictionary<string, string> PredefinedKeyTypes = new Dictionary<string, string>
        {
            { "点击按键", "Click" },
            { "宏指牌按键", "Macro" }
        };

        /// <summary>
        /// 尝试从指定文件夹获取所有 .json 文件。
        /// 若文件夹不存在，自动设置 ComboBox 为 "数据目录不存在" 状态并返回 null。
        /// </summary>
        public static string[] TryGetJsonFiles(string folderPath, SearchableComboBox comboBox)
        {
            if (!Directory.Exists(folderPath))
            {
                ShowEmptyMessage(comboBox, "数据目录不存在");
                return null;
            }
            try { return Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly); }
            catch { return new string[0]; }
        }

        /// <summary>
        /// 将 ComboBox 设置为仅显示一条提示消息的状态（清除 DataSource，使用 Items）。
        /// </summary>
        public static void ShowEmptyMessage(SearchableComboBox comboBox, string message)
        {
            comboBox.ValueMember = null;
            comboBox.DisplayMember = null;
            comboBox.DataSource = null;
            comboBox.Items.Clear();
            comboBox.Items.Add(message);
            comboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 用标准模式将文件条目列表绑定到 ComboBox（DisplayMember="Value", ValueMember="Key"）。
        /// </summary>
        public static void BindFileItems(SearchableComboBox comboBox, List<KeyValuePair<string, string>> items)
        {
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
            comboBox.DataSource = items;
        }

        /// <summary>
        /// 尝试根据 selectedKey 恢复 ComboBox 的之前选中项。
        /// 若找不到匹配项，自动选中第一个。
        /// </summary>
        public static void RestoreSelection(SearchableComboBox comboBox,
            List<KeyValuePair<string, string>> items, string selectedKey)
        {
            if (!string.IsNullOrEmpty(selectedKey))
            {
                var item = items.FirstOrDefault(i => i.Key == selectedKey);
                if (item.Key != null)
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
            if (items.Count > 0)
                comboBox.SelectedIndex = 0;
        }
    }
}
