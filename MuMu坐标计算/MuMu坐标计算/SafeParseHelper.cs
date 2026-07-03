using System.Windows.Forms;

namespace MuMu坐标计算
{
    /// <summary>
    /// 统一的输入安全解析工具类，封装 TryParse 以避免裸 Parse 导致的异常崩溃。
    /// </summary>
    internal static class SafeParseHelper
    {
        /// <summary>
        /// 尝试将 TextBox 文本解析为 int，失败时返回 false。
        /// </summary>
        public static bool TryGetInt(TextBox textBox, out int value)
        {
            value = 0;
            if (textBox == null) return false;
            return int.TryParse(textBox.Text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// 尝试将 TextBox 文本解析为 double，失败时返回 false。
        /// </summary>
        public static bool TryGetDouble(TextBox textBox, out double value)
        {
            value = 0.0;
            if (textBox == null) return false;
            return double.TryParse(textBox.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// 尝试将 TextBox 文本解析为 int，失败时返回默认值。
        /// </summary>
        public static int GetIntOrDefault(TextBox textBox, int defaultValue = 0)
        {
            if (textBox == null) return defaultValue;
            return int.TryParse(textBox.Text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int value) ? value : defaultValue;
        }

        /// <summary>
        /// 尝试将 TextBox 文本解析为 double，失败时返回默认值。
        /// </summary>
        public static double GetDoubleOrDefault(TextBox textBox, double defaultValue = 0.0)
        {
            if (textBox == null) return defaultValue;
            return double.TryParse(textBox.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double value) ? value : defaultValue;
        }
    }
}
