using System.Windows.Forms;

namespace MuMu坐标计算
{
    internal class StatusNotifier
    {
        private readonly Label _tip1;
        private readonly Label _tip2;

        public StatusNotifier(Label tip1, Label tip2)
        {
            _tip1 = tip1;
            _tip2 = tip2;
        }

        public void ShowListeningActive()
        {
            _tip1.Text = "提示：已开启键盘监听";
        }

        public void ShowListeningStopped()
        {
            _tip1.Text = "提示：已关闭键盘监听";
        }

        public void ShowKeyCreated(string keyType, string keyName)
        {
            _tip2.Text = keyType + keyName + "生成并写入成功！如出现问题请转人工。";
        }

        public void ShowKeyExists(string keyName)
        {
            _tip2.Text = "当前文件已存在按键：" + keyName + "！禁止重复生成！";
        }
    }
}
