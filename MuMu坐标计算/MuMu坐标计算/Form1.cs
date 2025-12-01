using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MuMu坐标计算.Properties;
using System.Diagnostics;

namespace MuMu坐标计算
{
    public partial class Form1 : Form
    {
        //防止四个文本框对应的坐标计算冲突
        int flag = 1;
        //控制下拉框刷新时的选项变动，防止重复调用
        bool flagFlushingFilename = true;
        //控制下拉框搜索功能延迟刷新防止重复刷新
        bool flagSleepFlushingFilename = true;
        //控制分辨率下拉框刷新时的选项变动，防止重复调用
        bool flagFlushingResolution = true;
        //控制分辨率类型下拉框刷新时的选项变动，防止重复调用
        bool flagFlushingResolutionType = true;
        //控制坐标文本框变动时候的函数调用，防止重复更新
        bool flagFXFYChanged = true;
        //控制重载提示语句，防止重复加载
        bool flagReloadingTip = true;
        //记录上方文件要读取/修改的按键
        KeyEventArgs bindKey =null;
        string bindKeyScan_code = "";
        //记录下拉框要读取的第二个按键
        KeyEventArgs bindKey2 = null;
        //记录要自动读取的第三个按键
        KeyEventArgs bindKey3 = null;
        //加载Json文件字符串
        String MyMuMuJosn= "";
        //记录最后修改文件的时间
        DateTime lastWriteTime;
        //记录找到鼠标坐标绑定按键
        Keys FindKay = 0;
        //记录回溯鼠标坐标绑定按键
        Keys ResetKey = 0;
        //保留的小数位数
        String FDP = "F16";
        // 保存上次选中文件的路径
        string lastSelectedFilePath = "";
        //重载文件时的语句
        string reloadingTip = "该操作会导致对当前文件的编辑无法还原，是否继续？";
        //预设按键类型的数据源
        Dictionary<string, string> KeyTypes { get; } = new Dictionary<string, string>{
            {"点击按键","Click" },
            { "宏指牌按键","Macro"}
        };
        //预设包名分类的数据源
        Dictionary<string, string> PackageNameTypes{ get; } = new Dictionary<string, string>{
            {"官服","com.RoamingStar.BlueArchive-" },
            { "B服","com.RoamingStar.BlueArchive.bilibili-"},
            { "日服","com.YostarJP.BlueArchive-"},
            { "国际服","com.nexon.bluearchive-"},
            { "其他","other"},
            { "宇宙服","萌新666sssaaa"}
        };
        //预设分辨率类型分类的数据源
        Dictionary<string, string> resolutionTypes { get; } = new Dictionary<string, string>{
            {"平板","1" },
            { "手机","2"},
            { "超宽屏","3"},
            { "自定义","4"}
        };
        //预设各个分辨率下的数据源
        Dictionary<string, string> resolution1 { get; } = new Dictionary<string, string>{
            {"2560x1440","2560,1440"  },
            {"1920x1080","1920,1080"  },
            {"1600x900","1600,900"  },
            {"1280x720","1280,720"  }
        };
        Dictionary<string, string> resolution2 { get; } = new Dictionary<string, string>{
            {"1440x2560","1440,2560"  },
            {"1080x1920","1080,1920"  },
            {"900x1600","900,1600"  },
            {"720x1280","720,1280" }
        };
        Dictionary<string, string> resolution3 { get; } = new Dictionary<string, string>{
            {"3200x1440","3200,1440"  },
            {"2400x1080","2400,1080"  },
            {"1920x864","1920,864"  },
            {"1600x720","1600,720"  }
        };
        //存储备份文件
        List<string> JsonTemp = new List<string>();
        //指向当前文件备份位置
        int JsonTempNowFlag = 0;
        public Form1()
        {
            InitializeComponent();
            InitializeHiddenMenu();
        }
        //初始化，窗口加载时读取配置文件
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FXtextBox.Text = Properties.Settings.Default.FX;
            this.FYtextBox.Text = Properties.Settings.Default.FY;
            FindKay = Properties.Settings.Default.FindKey;
            ResetKey = Properties.Settings.Default.ResetKey;
            FindKeytextBox.Text = FindKay.ToString().ToUpper(CultureInfo.InvariantCulture);
            ResetKeytextBox.Text = ResetKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            InitializeKeysComboBox(KeysListcomboBox);
            //初始化包名选择框
            packageNamecomboBox.DataSource = PackageNameTypes.ToList();
            packageNamecomboBox.DisplayMember = "Key";
            packageNamecomboBox.ValueMember = "Value";
            //初始化分辨率类型选择框
            resolutionTypecomboBox.DataSource = resolutionTypes.ToList();
            resolutionTypecomboBox.DisplayMember = "Key";
            resolutionTypecomboBox.ValueMember = "Value";
            //初始化分辨率选择框
            InitializeResolutioncomboBox(resolutioncomboBox);
            //尝试获取mumu目录
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.JsonFolderPath)) {
                TryGetJsonFileFolder();
            }
            InitializeFileNamecomboBox(fileNamecomboBox,false);
            //初始化后加载
            if (fileNamecomboBox.SelectedValue != null)
            {
                updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                string filePath = @JsonUrltextBox.Text;
                if (File.Exists(filePath))
                {
                    Properties.Settings.Default.JsonFolderPath = Path.GetDirectoryName(filePath);
                    Properties.Settings.Default.Save();
                    lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                    MyMuMuJosn = File.ReadAllText(filePath);
                    BackupAfterJsonReading();
                }
            }
            //初始化按键类型选项框
            keyTypelistcomboBox.DataSource = KeyTypes.ToList();
            keyTypelistcomboBox.DisplayMember = "Key";
            keyTypelistcomboBox.ValueMember = "Value";
            //初始化按键启用
            SetUndobtnAndRedobtnState();
        }
        //创建隐藏菜单用于绑定快捷键
        private void InitializeHiddenMenu()
        {
            // 创建隐藏的MenuStrip
            MenuStrip hiddenMenuStrip = new MenuStrip();
            hiddenMenuStrip.Visible = false;
            this.Controls.Add(hiddenMenuStrip);

            // 创建撤销菜单项（Ctrl+Z）
            ToolStripMenuItem undoToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem.Text = "撤销(&U)";
            undoToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.Z);
            undoToolStripMenuItem.Click += Undobutton_Click;

            // 创建重做菜单项（Ctrl+Y）
            ToolStripMenuItem redoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem.Text = "重做(&R)";
            redoToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.Y);
            redoToolStripMenuItem.Click += Redobutton_Click;

            // 将菜单项添加到MenuStrip
            hiddenMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(), // 分隔线（可选）
                undoToolStripMenuItem,
                redoToolStripMenuItem
            });
        }
        //高复用频率代码整理区域
        //检测空输入
        private bool CheckEmptyText() {
            if (FXtextBox.Text == "" ||
                    FYtextBox.Text == "" ||
                    KXtextBox.Text == "" ||
                    KYtextBox.Text == "" ||
                    JSXtextBox.Text == "" ||
                    JSYtextBox.Text == "") { 
                return true; 
            };
            return false;
        }
        //自动读工具人按键点位
        private void autoReadBindkey3PP()
        {
            if (autoReadcheckBox.Checked)
            {
                try
                {
                    if (MyMuMuJosn == "")
                    {
                        MessageBox.Show("请先加载一个Json文件！");
                        if (autoReadcheckBox.Checked) { autoReadcheckBox.Checked = false; }
                        return;
                    }
                    if (MuMuJsonEditor.FindKey(MyMuMuJosn, bindKey3) == -1)
                    {
                        MessageBox.Show("当前Json文件中未找到按键" + Button3textBox.Text);
                        if (autoReadcheckBox.Checked) { autoReadcheckBox.Checked = false; }
                    }
                    else
                    {
                        string[] key = MuMuJsonEditor.ReadKeyPP(MyMuMuJosn, bindKey3);
                        if (key == null) { 
                            MessageBox.Show("查找坐标失败，请检查您指定的按键中是否有坐标存在！");
                            if (autoReadcheckBox.Checked) { autoReadcheckBox.Checked = false; }
                            return; 
                        }
                        JSXtextBox.Text = key[0];
                        JSYtextBox.Text = key[1];
                    }
                }
                catch
                {
                    MessageBox.Show("当前未绑定按键，请检查您的设置！");
                    if (autoReadcheckBox.Checked) { autoReadcheckBox.Checked = false; }
                }
            }
        }
        //基础校验
        private bool WriteKeysCheck()
        {
            try
            {
                if (MyMuMuJosn == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return false;
                }
                // 获取程序所在目录的"data"子文件夹路径
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string dataFolder = Path.Combine(baseDir, "data");
                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    MessageBox.Show("程序目录下无“data”文件夹，请检查您的配置文件！");
                    return false;
                }
                //检查文件夹下是否有json文件
                if (Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly) == null)
                {
                    MessageBox.Show("“data”文件夹中无json文件，请检查您的配置文件！");
                    return false;
                }
                //检查是否删除json文件后又还原但是未重新加载列表（真有人能无聊到触发这个bug吗？？？）
                if (KeysListcomboBox.SelectedItem.ToString() == "数据目录不存在"|| KeysListcomboBox.SelectedItem.ToString()== "未找到符合条件的文件！")
                {
                    MessageBox.Show("文件不存在，请重新选择你的基础键位！");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        //写入修改后的mumu文件并备份
        private bool WriteToJsonAndBackup()
        {
            try
            {
                string filePath = @JsonUrltextBox.Text;
                if (MyMuMuJosn == "" || filePath == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return false;
                }
                File.WriteAllText(filePath, MyMuMuJosn);
                lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                BackupAfterJsonWriting();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        //纯粹写入mumu文件
        private bool WriteToJson()
        {
            try
            {
                string filePath = @JsonUrltextBox.Text;
                if (MyMuMuJosn == "" || filePath == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return false;
                }
                File.WriteAllText(filePath, MyMuMuJosn);
                lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        //根据不同情况设置撤销/重做按钮可交互性
        private void SetUndobtnAndRedobtnState()
        {
            try
            {
                if (JsonTempNowFlag == 0 && (JsonTemp.Count == 1 || JsonTemp.Count == 0))
                {
                    //初始化状态，此时仅有初始文件备份或未加载文件，两个按键均不可用
                    Undobutton.Enabled = false;
                    Redobutton.Enabled = false;
                    return;
                }
                else if (JsonTempNowFlag == 0 && JsonTemp.Count > 1)
                {
                    //回退到原始状态，此时有多个备份文件，可以重做但无法继续撤销
                    Undobutton.Enabled = false;
                    Redobutton.Enabled = true;
                    return;
                }
                else if (JsonTempNowFlag > 0 && JsonTempNowFlag < (JsonTemp.Count - 1))
                {
                    //中间态，可以重做也可以撤销
                    Undobutton.Enabled = true;
                    Redobutton.Enabled = true;
                    return;
                }
                else if (JsonTempNowFlag > 0 && JsonTempNowFlag == (JsonTemp.Count - 1))
                {
                    //到达list尾部，只可撤销不可重做
                    Undobutton.Enabled = true;
                    Redobutton.Enabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //读取json文件后进行备份
        private bool BackupAfterJsonReading() {
            try
            {
                if (JsonTempNowFlag==0&&JsonTemp.Count==0) {
                    //第一次加载
                    JsonTemp.Add(StringCompressor.CompressToBase64(MyMuMuJosn));
                    SetUndobtnAndRedobtnState();
                    return true;
                }
                else
                {
                    //重新加载新文件
                    JsonTemp.Clear();
                    JsonTempNowFlag = 0;
                    JsonTemp.Add(StringCompressor.CompressToBase64(MyMuMuJosn));
                    SetUndobtnAndRedobtnState();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        //修改json文件后进行新的备份
        private bool BackupAfterJsonWriting()
        {
            try
            {
                if (JsonTempNowFlag == (JsonTemp.Count - 1)) {
                    //当前指针在列表末尾
                    JsonTemp.Add(StringCompressor.CompressToBase64(MyMuMuJosn));
                    JsonTempNowFlag = JsonTemp.Count - 1;
                    SetUndobtnAndRedobtnState();
                    return true;
                }
                else if(JsonTempNowFlag<(JsonTemp.Count-1))
                {
                    //当前指针不在末端
                    //先清空当前备份之后的备份
                    int startIndex = JsonTempNowFlag + 1;
                    JsonTemp.RemoveRange(startIndex,JsonTemp.Count-startIndex);
                    //开始备份新的内容
                    JsonTemp.Add(StringCompressor.CompressToBase64(MyMuMuJosn));
                    JsonTempNowFlag = JsonTemp.Count - 1;
                    SetUndobtnAndRedobtnState();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return false;
            }
        }
        //区域结束

        //读取/保存按钮整理区域
        //保存默认分辨率
        private void FSave_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.FX = FXtextBox.Text;
                Properties.Settings.Default.FY = FYtextBox.Text;
                Properties.Settings.Default.Save();
                //防止重复触发selecteditemchanged
                flagFlushingResolution = false;
                flagFlushingResolutionType = false;
                //获取预设类型
                string[] ResolutionTypesValues = resolutionTypes.Values.ToArray();
                //判断当前输入框中的分辨率，进行分类
                if (!string.IsNullOrEmpty(FXtextBox.Text) & !string.IsNullOrEmpty(FYtextBox.Text))
                {
                    string key = FXtextBox.Text + "x" + FYtextBox.Text;
                    if (resolution1.ContainsKey(key))
                    {
                        //属于平板
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[0];
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution1.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution1.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }
                    }
                    else if (resolution2.ContainsKey(key))
                    {
                        //属于手机
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[1];
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution2.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution2.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }
                    }
                    else if (resolution3.ContainsKey(key))
                    {
                        //属于超宽屏
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[2];
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution3.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution3.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }
                    }
                    else
                    {
                        //属于自定义
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[3];
                        deleteUDResolutionbutton.Visible = true;
                        Dictionary<string, string> resolution4 = new Dictionary<string, string> { };
                        if (!string.IsNullOrWhiteSpace(Settings.Default.resolution4String))
                        {
                            resolution4 = MuMuJsonEditor.StringToResolution(Settings.Default.resolution4String);
                        }
                        if (!resolution4.ContainsKey(key))
                        {
                            resolution4.Add(key, FXtextBox.Text + "," + FYtextBox.Text);
                        }
                        resolutioncomboBox.DataSource = resolution4.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution4.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }
                        Settings.Default.resolution4String = MuMuJsonEditor.ResolutionToString(resolution4);
                        Settings.Default.Save();
                    }
                }
                //退出时恢复开关
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
            }
            catch (Exception ex)
            {
                // 异常处理
                resolutioncomboBox.DataSource = null;
                resolutioncomboBox.Items.Add($"加载失败: {ex.Message}");
                resolutioncomboBox.SelectedIndex = 0;
                //退出时恢复开关
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //读取默认分辨率
        private void FLoad_Click_1(object sender, EventArgs e)
        {
            FXtextBox.Text = Properties.Settings.Default.FX;
            FYtextBox.Text = Properties.Settings.Default.FY;
        }
        //保存默认快捷键
        private void SaveKeybutton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FindKey = FindKay;
            Properties.Settings.Default.ResetKey = ResetKey;
            Properties.Settings.Default.Save();
        }
        //读取默认快捷键
        private void LoadKeybutton_Click(object sender, EventArgs e)
        {
            FindKay = Properties.Settings.Default.FindKey;
            ResetKey = Properties.Settings.Default.ResetKey;
            FindKeytextBox.Text = FindKay.ToString().ToUpper(CultureInfo.InvariantCulture);
            ResetKeytextBox.Text = ResetKey.ToString().ToUpper(CultureInfo.InvariantCulture);
            //读取完快捷键后重置一下坐标Timer，加载新快捷键
            if (CcheckBox.Checked)
            {
                CcheckBox.Checked = false;
                CcheckBox.Checked = true;
            }
        }
        //区域结束

        //获取按键部分
        //获取修改按键
        private void ButtontextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ButtontextBox.Text = "";
            e.Handled = true;
            if (bindKey != null)
            {
                ButtontextBox.Text = bindKey.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
                if (bindKey.KeyCode == Keys.Escape)
                {
                    ButtontextBox.Text = "Esc";
                }
            }
        }
        private void ButtontextBox_KeyDown(object sender, KeyEventArgs e)
        {
            ButtontextBox.Text = "";
            bindKey = e;
            e.Handled = true;
            ButtontextBox.Text = e.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
            bindKeyScan_code=MuMuJsonEditor.GetScanCode(e.KeyCode).ToString();
        }
        private void Button2textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Button2textBox.Text = "";
            e.Handled = true;
            if (bindKey2 != null)
            {
                Button2textBox.Text = bindKey2.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
                if (bindKey2.KeyCode == Keys.Escape)
                {
                    Button2textBox.Text = "Esc";
                }
            }
        }
        private void Button2textBox_KeyDown(object sender, KeyEventArgs e)
        {
            Button2textBox.Text = "";
            bindKey2 = e;
            e.Handled = true;
            Button2textBox.Text = e.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
        }
        private void Button3textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Button3textBox.Text = "";
            e.Handled = true;
            if (bindKey3 != null)
            {
                Button3textBox.Text = bindKey3.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
                if (bindKey3.KeyCode == Keys.Escape)
                {
                    Button3textBox.Text = "Esc";
                }
            }
        }

        private void Button3textBox_KeyDown(object sender, KeyEventArgs e)
        {
            Button3textBox.Text = "";
            bindKey3 = e;
            e.Handled = true;
            Button3textBox.Text = e.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
            autoReadBindkey3PP();
        }
        //获取保存鼠标坐标快捷键
        private void FindKeytextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (FindKeytextBox.ReadOnly) { return; }
            if (ResetKey == e.KeyCode) { MessageBox.Show("快捷键冲突！"); return; }
            FindKeytextBox.Text = "";
            FindKay = e.KeyCode;
            e.Handled = true;
            FindKeytextBox.Text = e.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
            if (CcheckBox.Checked)
            {
                CcheckBox.Checked = false;
                CcheckBox.Checked = true;
            }
        }

        private void FindKeytextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            FindKeytextBox.Text = "";
            e.Handled = true;
            if (FindKay != 0)
            {
                FindKeytextBox.Text = FindKay.ToString().ToUpper(CultureInfo.InvariantCulture);
                if (FindKay == Keys.Escape)
                {
                    FindKeytextBox.Text = "Esc";
                }
            }
        }
        //获取回溯鼠标坐标快捷键
        private void ResetKeytextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (FindKeytextBox.ReadOnly) { return; }
            if (FindKay == e.KeyCode) { MessageBox.Show("快捷键冲突！"); return; }
            ResetKeytextBox.Text = "";
            ResetKey = e.KeyCode;
            e.Handled = true;
            ResetKeytextBox.Text = e.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture);
            if (CcheckBox.Checked)
            {
                CcheckBox.Checked = false;
                CcheckBox.Checked = true;
            }
        }

        private void ResetKeytextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ResetKeytextBox.Text = "";
            e.Handled = true;
            if (ResetKey != 0)
            {
                ResetKeytextBox.Text = ResetKey.ToString().ToUpper(CultureInfo.InvariantCulture);
                if (ResetKey == Keys.Escape)
                {
                    ResetKeytextBox.Text = "Esc";
                }
            }
        }
        //区域结束

        //打开下拉框时刷新数据
        private void KeysListcomboBox_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (KeysListcomboBox.SelectedValue != null)
                {
                    lastSelectedFilePath = KeysListcomboBox.SelectedValue.ToString();
                }
                KeysListcomboBox.DataSource = null;
                InitializeKeysComboBox(KeysListcomboBox);
                KeysListSearchtextBox.Visible = true;
                KeysListSearchtextBox.BringToFront();
                KeysListSearchtextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //关闭下拉框时隐藏搜索框
        private void KeysListcomboBox_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                KeysListSearchtextBox.Visible = false;
                flagSleepFlushingFilename = true;
            }
            catch (Exception ex)
            {
                flagSleepFlushingFilename = true;
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //实现搜索功能
        private void KeysListSearchtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //无论如何，先关闭计时器
                flagSleepFlushingFilename = false;
                string searchText = KeysListSearchtextBox.Text;
                InitializeKeysComboBoxAndSearch(KeysListcomboBox, searchText,true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //初始化存储的基础键位路径
        private void InitializeKeysComboBox(ComboBox KeysListcomboBox)
        {
            try
            {
                // 获取程序所在目录的"data"子文件夹路径
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string dataFolder = Path.Combine(baseDir, "data");

                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    KeysListcomboBox.DataSource = null;
                    KeysListcomboBox.Items.Clear();
                    KeysListcomboBox.Items.Add("数据目录不存在");
                    KeysListcomboBox.SelectedIndex = 0;
                    return;
                }

                // 获取所有JSON文件路径
                var jsonFiles = Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly);

                // 解析文件名和路径
                var items = new List<KeyValuePair<string, string>>();
                foreach (var file in jsonFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    items.Add(new KeyValuePair<string, string>(file, fileName));
                }

                // 数据绑定
                KeysListcomboBox.DataSource = items;
                KeysListcomboBox.DisplayMember = "Value";  // 显示文件名
                KeysListcomboBox.ValueMember = "Key";      // 存储完整路径
                KeysListcomboBox.SelectedIndex = -1;       // 取消默认选中
                // 尝试恢复之前选中项
                if (!string.IsNullOrEmpty(lastSelectedFilePath))
                {
                    var selectedItem = items.FirstOrDefault(i => i.Key==lastSelectedFilePath);
                    if (selectedItem.Key != null)
                    {
                        KeysListcomboBox.SelectedItem = selectedItem;
                    }
                    else if (jsonFiles.Any()) // 无原选项时选中第一个
                    {
                        KeysListcomboBox.SelectedIndex = 0;
                    }
                }
                else if (jsonFiles.Any()) // 首次加载时选中第一个
                {
                    KeysListcomboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                // 异常处理
                KeysListcomboBox.DataSource=null;
                KeysListcomboBox.Items.Add($"加载失败: {ex.Message}");
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //初始化存储的基础键位路径并进行检索
        private void InitializeKeysComboBoxAndSearch(ComboBox KeysListcomboBox,string searchText,bool flagBack)
        {
            try
            {
                // 获取程序所在目录的"data"子文件夹路径
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string dataFolder = Path.Combine(baseDir, "data");

                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    KeysListcomboBox.DataSource = null;
                    KeysListcomboBox.Items.Clear();
                    KeysListcomboBox.Items.Add("数据目录不存在");
                    KeysListcomboBox.SelectedIndex = 0;
                    return;
                }

                // 获取所有JSON文件路径
                var jsonFiles = Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly);

                // 解析文件名和路径
                var items = new List<KeyValuePair<string, string>>();
                foreach (var file in jsonFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (fileName.IndexOf(searchText) != -1) {
                        items.Add(new KeyValuePair<string, string>(file, fileName));
                    }
                }
                if (items.Count == 0)
                {
                    KeysListcomboBox.DataSource = null;
                    KeysListcomboBox.Items.Clear();
                    KeysListcomboBox.Items.Add("未找到符合条件的文件！");
                    KeysListcomboBox.SelectedIndex = 0;
                    return;
                }
                // 数据绑定
                string backKeysListSelectedValue = "";
                if (KeysListcomboBox.SelectedValue != null) { backKeysListSelectedValue = KeysListcomboBox.SelectedValue.ToString(); }
                KeysListcomboBox.DataSource = items;
                KeysListcomboBox.DisplayMember = "Value";  // 显示文件名
                KeysListcomboBox.ValueMember = "Key";      // 存储完整路径
                // 尝试恢复之前选中项
                if (flagBack) {
                    if (!string.IsNullOrEmpty(lastSelectedFilePath))
                    {
                        var selectedItem = items.FirstOrDefault(i => i.Key == lastSelectedFilePath);
                        if (selectedItem.Key != null)
                        {
                            KeysListcomboBox.SelectedItem = selectedItem;
                        }
                        else if (jsonFiles.Any()) // 无原选项时选中第一个
                        {
                            KeysListcomboBox.SelectedIndex = 0;
                        }
                    }
                    else if (jsonFiles.Any()) // 首次加载时选中第一个
                    {
                        KeysListcomboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    var selectedItem = items.FirstOrDefault(i => i.Key == backKeysListSelectedValue);
                    if (selectedItem.Key != null)
                    {
                        KeysListcomboBox.SelectedItem = selectedItem;
                    }
                    else if (jsonFiles.Any()) // 无原选项时选中第一个
                    {
                        KeysListcomboBox.SelectedIndex = 0;
                    }
                }
                
            }
            catch (Exception ex)
            {
                // 异常处理
                KeysListcomboBox.DataSource=null;
                KeysListcomboBox.Items.Add($"加载失败: {ex.Message}");
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //按包名分类初始化拖入文件对应文件夹中Json文件路径
        private void InitializeFileNamecomboBox(ComboBox fileNamecomboBox,bool flagback)
        {
            try
            {
                //先关闭selectIndexchanged函数的触发开关，防止初始化时重复触发
                flagFlushingFilename = false;
                // 获取文件夹路径
                string dataFolder = @Settings.Default.JsonFolderPath;
                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("数据目录不存在");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                }
                // 获取所有JSON文件路径
                var jsonFiles = Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly);
                //获取预设类型
                string[] PackageNamesValues = PackageNameTypes.Values.ToArray();
                //判断是否存在上一次选择路径，如果有的话优先将包名设置为前一次的设置，正常下拉选择时不修改该设置
                if (!string.IsNullOrEmpty(JsonUrltextBox.Text)&flagback) {
                    if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[0]) != -1)
                    {
                        //属于国服
                        packageNamecomboBox.SelectedValue = PackageNamesValues[0];
                    }
                    else if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[1]) != -1)
                    {
                        //属于B服
                        packageNamecomboBox.SelectedValue = PackageNamesValues[1];
                    }
                    else if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[2]) != -1)
                    {
                        //属于日服
                        packageNamecomboBox.SelectedValue = PackageNamesValues[2];
                    }
                    else if (JsonUrltextBox.Text.IndexOf(PackageNamesValues[3]) != -1)
                    {
                        //属于国际服
                        packageNamecomboBox.SelectedValue = PackageNamesValues[3];
                    }
                    else
                    {
                        //属于其他
                        packageNamecomboBox.SelectedValue = PackageNamesValues[4];
                    }
                }
                string PackageName = packageNamecomboBox.SelectedValue.ToString();
                // 解析文件名和路径
                var items = new List<KeyValuePair<string, string>>();
                if (PackageName == PackageNamesValues[0]|| PackageName == PackageNamesValues[1] || PackageName == PackageNamesValues[2] || PackageName == PackageNamesValues[3])
                {
                    //官服，B服，日服，国际服，正常分类
                    foreach (var file in jsonFiles)
                    {
                        //获取文件名
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        if (fileName.IndexOf(PackageName) != -1) {
                            //去掉包名后，加入列表。
                            fileName = fileName.Replace(PackageName, "");
                            items.Add(new KeyValuePair<string, string>(file, fileName));
                        }
                    }
                }
                else if (PackageName == PackageNamesValues[4])
                {
                    foreach (var file in jsonFiles)
                    {
                        //获取文件名
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        if (fileName.IndexOf(PackageNamesValues[0]) == -1& fileName.IndexOf(PackageNamesValues[1]) == -1& fileName.IndexOf(PackageNamesValues[2]) == -1 & fileName.IndexOf(PackageNamesValues[3]) == -1)
                        {
                            //不属于四个服务器任何一个包名，加入其他列表
                            items.Add(new KeyValuePair<string, string>(file, fileName));
                        }
                    }
                    //其他文件
                }
                else if (PackageName == PackageNamesValues[5]){
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("绿玩哪有宇宙服，你清醒一点。");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                    //宇宙服彩蛋
                }
                else{
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("为啥你能看到这条提示，你找到了我未曾想到的bug！");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                }
                if (items.Count == 0) {
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("该分类下没有对应的Json文件！");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                }
                // 数据绑定
                fileNamecomboBox.DataSource = items;
                fileNamecomboBox.DisplayMember = "Value";  // 显示文件名
                fileNamecomboBox.ValueMember = "Key";      // 存储完整路径
                // 尝试恢复之前选中项
                if (!string.IsNullOrEmpty(JsonUrltextBox.Text))
                {
                    var selectedItem = items.FirstOrDefault(i => i.Key == JsonUrltextBox.Text);
                    if (selectedItem.Key != null)
                    {
                        fileNamecomboBox.SelectedItem = selectedItem;
                    }
                    else if (jsonFiles.Any()) // 无原选项时选中第一个
                    {
                        fileNamecomboBox.SelectedIndex = 0;
                    }
                }
                else if (jsonFiles.Any()) // 首次加载时选中第一个
                {
                    fileNamecomboBox.SelectedIndex = 0;
                }
                //退出时恢复开关
                flagFlushingFilename = true;

            }
            catch (Exception ex)
            {
                // 异常处理
                fileNamecomboBox.DataSource=null;
                fileNamecomboBox.Items.Add($"加载失败: {ex.Message}");
                //退出时恢复开关
                flagFlushingFilename = true;
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //按包名分类初始化拖入文件对应文件夹中Json文件路径并进行检索
        private void InitializeFileNamecomboBoxAndSearch(ComboBox fileNamecomboBox, string searchText, bool flagBack) {
            try
            {
                //防止重复触发selectIndexchanged
                flagFlushingFilename = false;
                // 获取文件夹路径
                string dataFolder = @Settings.Default.JsonFolderPath;

                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("数据目录不存在");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                }
                // 获取所有JSON文件路径
                var jsonFiles = Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly);
                //获取预设类型
                string[] PackageNamesValues = PackageNameTypes.Values.ToArray();
                string PackageName = packageNamecomboBox.SelectedValue.ToString();
                // 解析文件名和路径
                var items = new List<KeyValuePair<string, string>>();
                if (PackageName == PackageNamesValues[0] || PackageName == PackageNamesValues[1] || PackageName == PackageNamesValues[2] || PackageName == PackageNamesValues[3])
                {
                    //官服，B服，日服，国际服，正常分类
                    foreach (var file in jsonFiles)
                    {
                        //获取文件名
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        if (fileName.IndexOf(PackageName) != -1)
                        {
                            //去掉包名
                            fileName = fileName.Replace(PackageName, "");
                            //符合搜索条件才加入
                            if (fileName.IndexOf(searchText) != -1) {
                                items.Add(new KeyValuePair<string, string>(file, fileName));
                            }
                        }
                    }
                }
                else if (PackageName == PackageNamesValues[4])
                {
                    foreach (var file in jsonFiles)
                    {
                        //获取文件名
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        if (fileName.IndexOf(PackageNamesValues[0]) == -1 & fileName.IndexOf(PackageNamesValues[1]) == -1 & fileName.IndexOf(PackageNamesValues[2]) == -1 & fileName.IndexOf(PackageNamesValues[3]) == -1)
                        {
                            //不属于四个服务器任何一个包名，加入其他列表
                            if (fileName.IndexOf(searchText) != -1)
                            {
                                items.Add(new KeyValuePair<string, string>(file, fileName));
                            }
                        }
                    }
                    //其他文件
                }
                else if (PackageName == PackageNamesValues[5])
                {
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("绿玩哪有宇宙服，你清醒一点。");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                    //宇宙服彩蛋
                }
                else
                {
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("为啥你能看到这条提示，你找到了我未曾想到的bug！");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                }
                if (items.Count == 0)
                {
                    fileNamecomboBox.DataSource = null;
                    fileNamecomboBox.Items.Clear();
                    fileNamecomboBox.Items.Add("未找到符合条件的文件！");
                    fileNamecomboBox.SelectedIndex = 0;
                    //退出时恢复开关
                    flagFlushingFilename = true;
                    return;
                }
                // 数据绑定
                string backSelectdValue = "";
                if (fileNamecomboBox.SelectedValue != null) { backSelectdValue = fileNamecomboBox.SelectedValue.ToString(); }
                fileNamecomboBox.DataSource = items;
                fileNamecomboBox.DisplayMember = "Value";  // 显示文件名
                fileNamecomboBox.ValueMember = "Key";      // 存储完整路径
                if (flagBack)
                {
                    // 尝试恢复之前选中项
                    if (!string.IsNullOrEmpty(JsonUrltextBox.Text))
                    {
                        var selectedItem = items.FirstOrDefault(i => i.Key == JsonUrltextBox.Text);
                        if (selectedItem.Key != null)
                        {
                            fileNamecomboBox.SelectedItem = selectedItem;
                        }
                        else if (jsonFiles.Any()) // 无原选项时选中第一个
                        {
                            fileNamecomboBox.SelectedIndex = 0;
                        }
                    }
                    else if (jsonFiles.Any()) // 首次加载时选中第一个
                    {
                        fileNamecomboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    var selectedItem = items.FirstOrDefault(i => i.Key == backSelectdValue);
                    if (selectedItem.Key != null)
                    {
                        fileNamecomboBox.SelectedItem = selectedItem;
                    }
                    else if (jsonFiles.Any()) // 无原选项时选中第一个
                    {
                        fileNamecomboBox.SelectedIndex = 0;
                    }
                }
                //退出时恢复开关
                flagFlushingFilename = true;
            }
            catch (Exception ex)
            {
                // 异常处理
                fileNamecomboBox.DataSource=null;
                fileNamecomboBox.Items.Add($"加载失败: {ex.Message}");
                //退出时恢复开关
                flagFlushingFilename = true;
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeResolutioncomboBox(ComboBox resolutioncomboBox)
        {
            try
            {
                //防止重复触发selecteditemchanged
                flagFlushingResolution = false;
                flagFlushingResolutionType = false;
                //获取预设类型
                string[] ResolutionTypesValues = resolutionTypes.Values.ToArray();
                //判断当前输入框中的分辨率，进行分类
                if (!string.IsNullOrEmpty(FXtextBox.Text)&!string.IsNullOrEmpty(FYtextBox.Text))
                {
                    string key = FXtextBox.Text + "x" + FYtextBox.Text;
                    if (resolution1.ContainsKey(key))
                    {
                        //属于平板
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[0];
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution1.ToList();
                        resolutioncomboBox.DisplayMember = "Key";  
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution1.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }

                    }
                    else if (resolution2.ContainsKey(key))
                    {
                        //属于手机
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[1];
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution2.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution2.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }
                    }
                    else if (resolution3.ContainsKey(key))
                    {
                        //属于超宽屏
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[2];
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution3.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution3.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }
                    }
                    else
                    {
                        //属于自定义
                        resolutionTypecomboBox.SelectedValue = ResolutionTypesValues[3];
                        deleteUDResolutionbutton.Visible = true;
                        Dictionary<string, string> resolution4 = new Dictionary<string, string> { };
                        if (!string.IsNullOrWhiteSpace(Settings.Default.resolution4String)) {
                            resolution4 = MuMuJsonEditor.StringToResolution(Settings.Default.resolution4String);
                        }
                        if (!resolution4.ContainsKey(key)) {
                            key = "*" + key;
                            resolution4.Add(key, FXtextBox.Text+ "," + FYtextBox.Text);
                        }
                        resolutioncomboBox.DataSource = resolution4.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        var selectedItem = resolution4.FirstOrDefault(i => i.Key == key);
                        if (selectedItem.Key != null)
                        {
                            resolutioncomboBox.SelectedItem = selectedItem;
                        }
                    }
                }
                //退出时恢复开关
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
            }
            catch (Exception ex)
            {
                // 异常处理
                resolutioncomboBox.DataSource = null;
                resolutioncomboBox.Items.Add($"加载失败: {ex.Message}");
                resolutioncomboBox.SelectedIndex = 0;
                //退出时恢复开关
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
                MessageBox.Show($"初始化ComboBox时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Activated(object sender, EventArgs e)
        {

        }
        private void Form1_Leave(object sender, EventArgs e)
        {

        }
        private void CheckTextBox_KeyPress(object sender, KeyPressEventArgs e)//输入框输入事件绑定
        {
            // 允许输入数字、小数点、Backspace和退出键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                if (e.KeyChar == (char)8 || e.KeyChar == (char)27) // 允许Backspace和退出键
                {
                    // 不做任何操作
                }
                else
                {
                    e.Handled = true; // 不允许输入其他字符
                }
            }
            // 检查是否已经输入了小数点，如果已经输入，则禁止再次输入小数点
            TextBox textBox = sender as TextBox;
            if (textBox != null && e.KeyChar == '.' && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
        //CheckBox部分
        //输入框锁定
        private void FcheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            FXtextBox.ReadOnly = FcheckBox.Checked;
            FYtextBox.ReadOnly = FcheckBox.Checked;
        }   

        private void KcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            KXtextBox.ReadOnly = KcheckBox.Checked;
            KYtextBox.ReadOnly = KcheckBox.Checked;
        }

        private void JScheckBox_CheckedChanged(object sender, EventArgs e)
        {
            JSXtextBox.ReadOnly = JScheckBox.Checked;
            JSYtextBox.ReadOnly = JScheckBox.Checked;
        }
        //输入框锁定结束
        //窗口置顶
        private void TOPcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TOPcheckBox.Checked == true && this.TopMost == false)
            {
                this.TopMost = true;
            }
            else
            if (TOPcheckBox.Checked == false && this.TopMost == true)
            {
                this.TopMost = false;
            }
        }
        //开启/关闭绑定按键的编辑
        private void EcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            FindKeytextBox.ReadOnly = !EcheckBox.Checked;
            ResetKeytextBox.ReadOnly = !EcheckBox.Checked;
        }
        //开启/关闭自动读取坐标
        private void autoReadcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            autoReadBindkey3PP();
        }
        //区域结束

        //计算部分，文本框被修改时触发计算，检测flag防止冲突，分辨率为从0开始计算，因此计算用分辨率为真实分辨率-1
        //例：1280x720的模拟器分辨率计算坐标需要分别用1279，719做除数，得到的Json坐标才是更精准的
        //经测试，大部分坐标均可完美还原，但仍有部分坐标存在0.1~0.5不等的误差，但无伤大雅。

        //修改窗口分辨率时视锁定情况进行计算
        private void FtextBox_TextChanged(object sender, EventArgs e)
        {
            try{
                if (flag==1) {//检查是否有其他事件正在执行
                    flag = 0;
                    //检查空文本框
                    if (CheckEmptyText()) { flag = 1;  return; };
                    //变量初始化
                    Double FX = Double.Parse(FXtextBox.Text)-1;
                    Double FY = Double.Parse(FYtextBox.Text)-1;
                    Double KX = Double.Parse(KXtextBox.Text);
                    Double KY = Double.Parse(KYtextBox.Text);
                    Double JSX = Double.Parse(JSXtextBox.Text);
                    Double JSY = Double.Parse(JSYtextBox.Text);
                    //初始化完毕
                    if (KcheckBox.Checked && JScheckBox.Checked)
                    {//双坐标均锁定
                        MessageBox.Show(Form1.ActiveForm, "请至少解锁一类需要得到结果的坐标再修改分辨率！");
                    }
                    else
                    if (KcheckBox.Checked)
                    { //开发者模式坐标锁定,计算Json文件坐标
                        JSXtextBox.Text = (KX / FX).ToString(FDP);
                        JSYtextBox.Text = (KY / FY).ToString(FDP);
                    }
                    else
                    if (JScheckBox.Checked)
                    { //Json文件坐标锁定,计算开发者模式坐标
                        KXtextBox.Text = (JSX * FX).ToString();
                        KYtextBox.Text = (JSY * FY).ToString();
                    }
                    else
                    {
                        //均不锁定则默认计算Json文件坐标
                        JSXtextBox.Text = (KX / FX).ToString(FDP);
                        JSYtextBox.Text = (KY / FY).ToString(FDP);
                    }
                    InitializeResolutioncomboBox(resolutioncomboBox);
                    flag = 1;
                }
                
            }
            catch(ArithmeticException ex)
            {
                flag = 1;
                MessageBox.Show("发生异常："+ex.Message+"请确保您输入的内容为数字。");
            }
            finally { 
            
            }
        }

        private void KtextBox_TextChanged(object sender, EventArgs e)//开发者坐标文本更改事件绑定
        {
            try
            {
                if (flag == 1){ //检查是否有其他事件正在执行
                    flag = 0;
                    //检查空文本框
                    if (CheckEmptyText()) { flag = 1; return; };
                    //变量初始化
                    Double FX = Double.Parse(FXtextBox.Text)-1;
                    Double FY = Double.Parse(FYtextBox.Text)-1;
                    Double KX = Double.Parse(KXtextBox.Text);
                    Double KY = Double.Parse(KYtextBox.Text);
                    //初始化完毕
                    //计算Json文件坐标
                    JSXtextBox.Text = (KX / FX).ToString(FDP);
                    JSYtextBox.Text = (KY / FY).ToString(FDP);
                    flag = 1;
                }
                
            }
            catch (ArithmeticException ex)
            {
                flag = 1;
                MessageBox.Show("发生异常：" + ex.Message + "请确保您输入的内容为数字。");
            }
            finally
            {

            }
        }

        private void JStextBox_TextChanged(object sender, EventArgs e)//Json文件坐标文本更改事件绑定
        {
            try
            {
                if (flag == 1)
                { //检查是否有其他事件正在执行
                    flag = 0;
                    //检查空文本框
                    if (CheckEmptyText()) { flag = 1;  return; };
                    //变量初始化
                    Double FX = Double.Parse(FXtextBox.Text)-1;
                    Double FY = Double.Parse(FYtextBox.Text)-1;
                    Double JSX = Double.Parse(JSXtextBox.Text);
                    Double JSY = Double.Parse(JSYtextBox.Text);
                    //初始化完毕
                    //计算开发者模式坐标
                    KXtextBox.Text = (JSX * FX).ToString();
                    KYtextBox.Text = (JSY * FY).ToString();
                    flag = 1;
                }
            }
            catch (ArithmeticException ex)
            {
                flag = 1;
                MessageBox.Show("发生异常：" + ex.Message + "请确保您输入的内容为数字。");
            }
            finally
            {

            }
        }
        //计算部分结束

        //Timer控件实时获取当前鼠标坐标，与保存坐标一致时修改背景色
        private void Ctimer_Tick(object sender, EventArgs e)
        {
            Point mousePosition = Control.MousePosition;
            NCXtextBox.Text = mousePosition.X.ToString();
            NCYtextBox.Text = mousePosition.Y.ToString();
            if (NCXtextBox.Text == SCXtextBox.Text) {
                NCXtextBox.BackColor = Color.Green;
            }
            else
            {
                NCXtextBox.BackColor = Color.White;
            }
            if (NCYtextBox.Text == SCYtextBox.Text)
            {
                NCYtextBox.BackColor = Color.Green;
            }
            else {
                NCYtextBox.BackColor = Color.White;
            }
        }
        //开启坐标捕获，同时注册/取消全局快捷键。
        private void CcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CcheckBox.Checked == true && Ctimer.Enabled == false)
            {
                Ctimer.Enabled = true;
            }
            else
            if (CcheckBox.Checked == false && Ctimer.Enabled == true) {
                Ctimer.Enabled = false;
            }

            if (CcheckBox.Checked)
            {
                HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Ctrl, FindKay);
                HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.Ctrl, ResetKey);
            }
            else {
                HotKey.UnregisterHotKey(Handle, 100);
                HotKey.UnregisterHotKey(Handle, 101);
            }
        }
        //编写快捷键的功能部分
        protected override void WndProc(ref Message m)
        {
            const int Mu_HOTKEY = 0x0312;

            switch (m.Msg)
            {
                case Mu_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        //保存坐标
                        case 100:
                            if (CcheckBox.Checked)
                            {
                                SCXtextBox.Text = NCXtextBox.Text;
                                SCYtextBox.Text = NCYtextBox.Text;
                            }
                            break;
                        //回溯鼠标位置
                        case 101:
                            if (CcheckBox.Checked)
                            {
                                MouseSimulator.MoveMouseTo(int.Parse(SCXtextBox.Text), int.Parse(SCYtextBox.Text));
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        //测试/检查按钮，详细功能看跳出提示
        private void CheckButton_Click(object sender, EventArgs e)
        {
            try {
                MessageBox.Show("当前绑定按键为：" + bindKey.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture) + Environment.NewLine + "当前绑定按键值为：" + bindKey.KeyValue.ToString()+Environment.NewLine);
                if (MyMuMuJosn == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return;
                }
                if (MuMuJsonEditor.FindKey(MyMuMuJosn, bindKey) == -1)
                {
                    MessageBox.Show("当前Json文件中未找到按键" + ButtontextBox.Text);
                }
                else {
                    if (MuMuJsonEditor.CheckType(MyMuMuJosn, bindKey))
                    {
                        if (MuMuJsonEditor.FindType(MyMuMuJosn, bindKey) == MuMuJsonEditor.typeClick)
                        {
                            MessageBox.Show("按键" + ButtontextBox.Text + "在文件中且是单击按键，可以直接修改。");
                        }
                        else if(MuMuJsonEditor.FindType(MyMuMuJosn, bindKey) == MuMuJsonEditor.typeMacro)
                        {
                            MessageBox.Show("按键" + ButtontextBox.Text + "在文件中且是宏按键，仅支持对固定格式的宏指牌修改，其余格式的坐标请自行进入Json文件修改。");
                        }
                    }
                    else {
                        MessageBox.Show("按键" + ButtontextBox.Text + "在文件中但不是支持的按键，为防止bug请自行打开Json文件手动修改！");
                    }
                }
            }
            catch {
                MessageBox.Show("当前未绑定按键，请检查您的设置！");
            }
        }
        //获取Json文件路径，不会真有人用吧，我觉得拖过来更方便啊。
        private void OpenJson_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.JsonFolderPath)) { JsonopenFileDialog.InitialDirectory = Properties.Settings.Default.JsonFolderPath; }
                if (JsonopenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (Undobutton.Enabled || Redobutton.Enabled)
                    {
                        DialogResult result = MessageBox.Show(reloadingTip, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No)
                        {
                            return;//用户选择了取消
                        }
                    }
                    updateJsonUrltextBox(JsonopenFileDialog.FileName);
                    Properties.Settings.Default.JsonFolderPath= Path.GetDirectoryName(@JsonUrltextBox.Text);
                    Properties.Settings.Default.Save();
                    string filePath = @JsonUrltextBox.Text;
                    if (File.Exists(filePath))
                    {
                        MyMuMuJosn = File.ReadAllText(filePath);
                        autoReadBindkey3PP();
                        BackupAfterJsonReading();
                        fileNameSearchtextBox.Text = "";
                        InitializeFileNamecomboBox(fileNamecomboBox,true);
                        MessageBox.Show("加载成功！");
                    }
                    else
                    {
                        MessageBox.Show("不存在的Json文件，请检查您的文件路径。");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //检查拖入窗口的文件，非json文件不接受
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        e.Effect = DragDropEffects.Copy; // 设置拖放效果为复制
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None; // 文件不是JSON格式，不接受拖放
                        break; // 如果不需要检查其他文件，可以直接退出循环
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None; // 不是文件拖放，不接受
            }
        }
        //读取拖入窗口的json文件，只取第一个
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                updateJsonUrltextBox(files[0]);
                Properties.Settings.Default.JsonFolderPath = Path.GetDirectoryName(JsonUrltextBox.Text);
                Properties.Settings.Default.Save();
                /*
                if (MyMuMuJosn != "")
                {
                    DialogResult result = MessageBox.Show("重新加载会导致尚未保存的修改被覆盖，是否继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        return;//用户选择了取消
                    }
                }
                */
                string filePath = @JsonUrltextBox.Text;
                if (File.Exists(filePath))
                {
                    MyMuMuJosn = File.ReadAllText(filePath);
                    autoReadBindkey3PP();
                    BackupAfterJsonReading();
                    fileNameSearchtextBox.Text = "";
                    InitializeFileNamecomboBox(fileNamecomboBox,true);
                }
                else
                {
                    MessageBox.Show("不存在的Json文件，请检查您的文件路径。");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
            
        }
        //独立创建指定类型按键并写入的代码，便于复用
        private void createAndwriteSetKey() {
            string[] keyValues = KeyTypes.Values.ToArray();
            if (keyTypelistcomboBox.SelectedValue.ToString() == keyValues[0])
            {
                //创建点击按键
                string key = MuMuJsonEditor.CreateKey(keyValues[0], bindKey, JSXtextBox.Text, JSYtextBox.Text, bindKeyScan_code);
                MyMuMuJosn = MuMuJsonEditor.WriteKeys(key, MyMuMuJosn);
                if (WriteToJsonAndBackup()) { MessageBox.Show($"点击按键{ButtontextBox.Text}生成并写入成功！如出现问题请转人工。"); }
            }
            else if (keyTypelistcomboBox.SelectedValue.ToString() == keyValues[1])
            {
                //创建宏按键
                string key = MuMuJsonEditor.CreateKey(keyValues[1], bindKey, JSXtextBox.Text, JSYtextBox.Text, bindKeyScan_code);
                MyMuMuJosn = MuMuJsonEditor.WriteKeys(key, MyMuMuJosn);
                if (WriteToJsonAndBackup()) { MessageBox.Show($"宏指牌按键{ButtontextBox.Text}生成并写入成功！如出现问题请转人工。"); }
            }
            else
            {
                MessageBox.Show("未知错误！我也想知道你是怎么触发这条提示的？？？");
                return;
            }
        }
        //修改并保存按键
        private void RewriteAndSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                //修改前无论如何，重置下拉框
                InitializeFileNamecomboBox(fileNamecomboBox, true);
                string filePath = @JsonUrltextBox.Text;
                if (MyMuMuJosn == "" || filePath == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return;
                }
                if (bindKey == null)
                {
                    MessageBox.Show("请先绑定一个按键！");
                    return;
                }
                if (replaceKeycheckBox.Checked) {
                    DialogResult result = MessageBox.Show($"是否按预设强制替换对应按键？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        return;//用户选择了取消
                    }
                    if (MuMuJsonEditor.FindKey(MyMuMuJosn, bindKey) != -1){
                        //文件中存在按键
                        //先删除
                        MyMuMuJosn = MuMuJsonEditor.DeleteKey(bindKey.KeyValue.ToString(), MyMuMuJosn);
                    }
                    //后写入
                    createAndwriteSetKey();
                    return;
                }
                if (MuMuJsonEditor.FindKey(MyMuMuJosn, bindKey) == -1)
                {
                    DialogResult result = MessageBox.Show($"当前Json文件中未找到按键{ButtontextBox.Text},是否新建指定类型的按键？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        return;//用户选择了取消
                    }
                    createAndwriteSetKey();
                }
                else
                {
                    if (MuMuJsonEditor.CheckType(MyMuMuJosn, bindKey))
                    {
                        try
                        {
                            MyMuMuJosn = MuMuJsonEditor.ReKey(MyMuMuJosn, bindKey, JSXtextBox.Text, JSYtextBox.Text);
                            if (WriteToJsonAndBackup()) { MessageBox.Show($"按键{ButtontextBox.Text}修改成功并保存，如出现bug请自行打开Json文件手动修改坐标！"); }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("按键修改失败！" + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("按键" + ButtontextBox.Text + "在文件中但不是单击按键或指定的宏指牌按键，为防止bug，请自行打开Json文件手动修改！");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return;
            }
            

        }
        //检查Json文件是否需要重载
        private void CheckFileChangetimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string filePath = @JsonUrltextBox.Text;
                if (File.Exists(filePath))
                {
                    if ((File.GetLastWriteTimeUtc(filePath) != lastWriteTime)&flagReloadingTip)
                    {
                        //防止重复触发
                        flagReloadingTip = false;
                        DialogResult result = MessageBox.Show("检测到Json文件被其他程序修改，是否重新加载？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            MyMuMuJosn = File.ReadAllText(filePath);
                            MessageBox.Show("加载成功！");
                            autoReadBindkey3PP();
                            BackupAfterJsonReading();
                        }
                        lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                        //本次触发结束后恢复标记
                        flagReloadingTip = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //点击跳转到作者B站主页(<ゝω·)~☆kira
        private void QhrxlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://space.bilibili.com/251589");
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开链接：" + ex.Message);
            }
        }
        //读取指定按键的坐标
        private void ReadPPButton_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("当前绑定按键为：" + bindKey.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture) + Environment.NewLine + "当前绑定按键值为：" + bindKey.KeyValue.ToString() + Environment.NewLine);
                if (MyMuMuJosn == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return;
                }
                if (MuMuJsonEditor.FindKey(MyMuMuJosn, bindKey) == -1)
                {
                    MessageBox.Show("当前Json文件中未找到按键" + ButtontextBox.Text);
                }
                else
                {
                    string[] key = MuMuJsonEditor.ReadKeyPP(MyMuMuJosn, bindKey);
                    if (key == null) { MessageBox.Show("查找坐标失败，请检查您指定的按键中是否有坐标存在！"); return; }
                    JSXtextBox.Text = key[0];
                    JSYtextBox.Text = key[1];
                }
            }
            catch
            {
                MessageBox.Show("当前未绑定按键，请检查您的设置！");
            }
        }
        //将基础键位导入其他键位中
        private void WriteKeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                //修改前无论如何，重置下拉框
                InitializeFileNamecomboBox(fileNamecomboBox, true);
                if (!WriteKeysCheck()) { return; }
                string[] text = MuMuJsonEditor.FindKeyValues(KeysListcomboBox.SelectedValue.ToString());
                if (!MuMuJsonEditor.AreAllKeysMissing(text, MyMuMuJosn)){
                    //按键重复
                    DialogResult result = MessageBox.Show("检测到待写入Json文件存在重复按键，是否继续写入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes){}
                    else
                    {
                        string[] keyText = MuMuJsonEditor.FindKeyTexts(KeysListcomboBox.SelectedValue.ToString());
                        string[] repeatKeyText = MuMuJsonEditor.FindAllRepeatKeyTexts(keyText, MyMuMuJosn);
                        string messageKey = "";
                        foreach (string key in repeatKeyText) { messageKey += key + ","; }
                        MessageBox.Show("存在重复按键:" + messageKey + "\n请修改待写入的按键文件后再操作！");
                        return;
                    }
                }
                string keys = MuMuJsonEditor.ReadKeys(KeysListcomboBox.SelectedValue.ToString());
                MyMuMuJosn = MuMuJsonEditor.WriteKeys(keys, MyMuMuJosn);
                if (WriteToJsonAndBackup()) { MessageBox.Show("基础键位注入成功！如出现问题请转人工。"); }
            }
            catch(Exception ex) {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //按键去重
        private void DeleteRepeatKeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) { return; }
                string[] text = MuMuJsonEditor.FindKeyValues(KeysListcomboBox.SelectedValue.ToString());
                if (MuMuJsonEditor.AreAllKeysMissing(text, MyMuMuJosn))
                {
                    //键位无重复
                    MessageBox.Show("无重复键位，可执行基础键位注入。");
                    return;
                }
                DialogResult result = MessageBox.Show("去重功能存在风险，使用前请确保重复的按键中不存在你要保留的按键。", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { }
                else { return; }
                while (!MuMuJsonEditor.AreAllKeysMissing(text, MyMuMuJosn))
                {
                    //存在重复键位，执行去重
                    string[] keyValue = MuMuJsonEditor.FindKeyValues(KeysListcomboBox.SelectedValue.ToString());
                    string[] repeatKeyValues = MuMuJsonEditor.FindAllRepeatKeyValues(keyValue, MyMuMuJosn);
                    MyMuMuJosn = MuMuJsonEditor.DeleteKeys(repeatKeyValues, MyMuMuJosn);
                }
                if (MuMuJsonEditor.AreAllKeysMissing(text, MyMuMuJosn))
                {
                    //键位无重复
                    if (WriteToJsonAndBackup()) { MessageBox.Show("已清除所有重复键位，可执行基础键位注入。"); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //删除对应区域的按键
        private void DeleteRangeRDkeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) { return; }
                DialogResult result = MessageBox.Show("右下区域清空功能存在风险，且当前功能仅支持16：9分辨率的键位文件。\n使用前请确认键位适配分辨率且右下角选牌区不存在要保留的按键！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { }
                else { return; }
                double[] rangeLT = { 0.661, 0.798 };
                double[] rangeRD = { 1.0, 1.0 };
                var results = new List<(double, double, string, string)>();
                results = MuMuJsonEditor.FindRangeKeyValues(rangeLT, rangeRD, MyMuMuJosn);
                if (results.Count == 0) { MessageBox.Show($"右下选牌区域中不存在按键，无需清空。");return; }
                string messageKeyTexts = "";
                foreach (var (x, y, text, vk) in results)
                {
                    MyMuMuJosn = MuMuJsonEditor.DeleteKey(vk, MyMuMuJosn);
                    messageKeyTexts += text+",";
                }
                if (WriteToJsonAndBackup()) { MessageBox.Show($"已清空：{messageKeyTexts}键，如出现问题请转人工！"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
            
        }
        //清除指定区域所有键位
        private void DeleteRangeLTRDkeysButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WriteKeysCheck()) { return; }
                DialogResult result = MessageBox.Show("区域清空功能存在风险，使用前请确认选择区域不存在要保留的按键！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { }
                else { return; }
                //变量初始化
                Double FX = Double.Parse(FXtextBox.Text) - 1;
                Double FY = Double.Parse(FYtextBox.Text) - 1;
                Double LTX = Double.Parse(RangeLTXtextBox.Text);
                Double LTY = Double.Parse(RangeLTYtextBox.Text);
                Double RDX = Double.Parse(RangeRDXtextBox.Text);
                Double RDY = Double.Parse(RangeRDYtextBox.Text);
                double[] rangeLT = { (LTX / FX),(LTY / FY) };
                double[] rangeRD = { (RDX / FX),(RDY / FY) };
                //MessageBox.Show($"发生错误：{rangeLT[0]}+{rangeLT[1]}+{rangeRD[0]}+{rangeRD[1]}\n请检查您的输入内容！");
                var results = new List<(double, double, string, string)>();
                results = MuMuJsonEditor.FindRangeKeyValues(rangeLT, rangeRD, MyMuMuJosn);
                if (results.Count == 0) { MessageBox.Show($"选中区域不存在按键，无需清空。"); return; }
                string messageKeyTexts = "";
                foreach (var (x, y, text, vk) in results)
                {
                    MyMuMuJosn = MuMuJsonEditor.DeleteKey(vk, MyMuMuJosn);
                    messageKeyTexts += text + ",";
                }
                if (WriteToJsonAndBackup()) { MessageBox.Show($"已清空：{messageKeyTexts}键，如出现问题请转人工！"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}\n请检查您的输入内容！");
            }
        }
        //读取下拉框中文件的按键坐标
        private void ReadPP2Button_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取程序所在目录的"data"子文件夹路径
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string dataFolder = Path.Combine(baseDir, "data");
                // 检查文件夹是否存在
                if (!Directory.Exists(dataFolder))
                {
                    MessageBox.Show("程序目录下无“data”文件夹，请检查您的配置文件！");
                    return;
                }
                //检查文件夹下是否有json文件
                if (Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly) == null)
                {
                    MessageBox.Show("“data”文件夹中无json文件，请检查您的配置文件！");
                    return;
                }
                //检查是否删除json文件后又还原但是未重新加载列表（真有人能无聊到触发这个bug吗？？？）
                if (KeysListcomboBox.SelectedItem.ToString() == "数据目录不存在")
                {
                    MessageBox.Show("请重新选择你的基础键位！");
                    return;
                }
                string myJson = File.ReadAllText(KeysListcomboBox.SelectedValue.ToString());
                MessageBox.Show("当前绑定按键为：" + bindKey2.KeyCode.ToString().ToUpper(CultureInfo.InvariantCulture) + Environment.NewLine + "当前绑定按键值为：" + bindKey2.KeyValue.ToString() + Environment.NewLine);
                if (MuMuJsonEditor.FindKey(myJson, bindKey2) == -1)
                {
                    MessageBox.Show("当前Json文件中未找到按键" + Button2textBox.Text);
                }
                else
                {
                    string[] key = MuMuJsonEditor.ReadKeyPP(myJson, bindKey2);
                    if (key == null) { MessageBox.Show("查找坐标失败，请检查您指定的按键中是否有坐标存在！"); return; }
                    JSXtextBox.Text = key[0];
                    JSYtextBox.Text = key[1];
                }
            }
            catch
            {
                MessageBox.Show("当前未绑定按键，请检查您的设置！");
            }
        }

        private void WriteKeyButton_Click(object sender, EventArgs e)
        {
            try
            {
                //修改前无论如何，重置下拉框
                InitializeFileNamecomboBox(fileNamecomboBox, true);
                if (!WriteKeysCheck()) { return; }
                if (bindKey2 == null) { MessageBox.Show("当前未绑定按键，请检查您的设置！");return; }
                string[] text = { bindKey2.KeyValue.ToString() };
                if (!MuMuJsonEditor.AreAllKeysMissing(text, MyMuMuJosn)){
                    //按键重复
                    DialogResult result = MessageBox.Show("检测到待写入Json文件存在重复按键，是否继续写入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes){}
                    else
                    {
                        MessageBox.Show("存在重复按键:" + bindKey2.KeyData + "\n请修改待写入的按键文件后再操作！");
                        return;
                    }
                }
                string key = MuMuJsonEditor.ReadKey(KeysListcomboBox.SelectedValue.ToString(),bindKey2);
                MyMuMuJosn = MuMuJsonEditor.WriteKeys(key, MyMuMuJosn);
                if (WriteToJsonAndBackup()) { MessageBox.Show("单键位注入成功！如出现问题请转人工。"); }
            }
            catch(Exception ex) {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //撤销按钮，返回上一步操作后的文件状态
        private void Undobutton_Click(object sender, EventArgs e)
        {
            try
            {
                //修改前无论如何，重置下拉框
                InitializeFileNamecomboBox(fileNamecomboBox, true);
                //判定备注沿用对按钮状态设置的备注，懒得改了
                if (JsonTempNowFlag == 0 && (JsonTemp.Count == 1 || JsonTemp.Count == 0))
                {
                    //初始化状态，此时仅有初始文件备份或未加载文件，两个按键均不可用
                    return;
                }
                else if (JsonTempNowFlag == 0 && JsonTemp.Count > 1)
                {
                    //回退到原始状态，此时有多个备份文件，可以重做但无法继续撤销
                    return;
                }
                else if (JsonTempNowFlag > 0 && JsonTempNowFlag < (JsonTemp.Count - 1))
                {
                    //中间态，可以重做也可以撤销
                    //指针回退一位
                    JsonTempNowFlag -= 1;
                    //还原备份
                    MyMuMuJosn = StringDecompressor.DecompressFromBase64(JsonTemp[JsonTempNowFlag]);
                    WriteToJson();
                    SetUndobtnAndRedobtnState();
                    return;
                }
                else if (JsonTempNowFlag > 0 && JsonTempNowFlag == (JsonTemp.Count - 1))
                {
                    //到达list尾部，只可撤销不可重做
                    //指针回退一位
                    JsonTempNowFlag -= 1;
                    //还原备份
                    MyMuMuJosn = StringDecompressor.DecompressFromBase64(JsonTemp[JsonTempNowFlag]);
                    WriteToJson();
                    SetUndobtnAndRedobtnState();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //重做按钮，恢复到撤销之前的文件状态
        private void Redobutton_Click(object sender, EventArgs e)
        {
            //修改前无论如何，重置下拉框
            InitializeFileNamecomboBox(fileNamecomboBox, true);
            //判定备注沿用对按钮状态设置的备注，懒得改了
            if (JsonTempNowFlag == 0 && (JsonTemp.Count == 1 || JsonTemp.Count == 0))
            {
                //初始化状态，此时仅有初始文件备份或未加载文件，两个按键均不可用
                return;
            }
            else if (JsonTempNowFlag == 0 && JsonTemp.Count > 1)
            {
                //回退到原始状态，此时有多个备份文件，可以重做但无法继续撤销
                //指针前进一位
                JsonTempNowFlag += 1;
                //还原备份
                MyMuMuJosn = StringDecompressor.DecompressFromBase64(JsonTemp[JsonTempNowFlag]);
                WriteToJson();
                SetUndobtnAndRedobtnState();
                return;
            }
            else if (JsonTempNowFlag > 0 && JsonTempNowFlag < (JsonTemp.Count - 1))
            {
                //中间态，可以重做也可以撤销
                //指针前进一位
                JsonTempNowFlag += 1;
                //还原备份
                MyMuMuJosn = StringDecompressor.DecompressFromBase64(JsonTemp[JsonTempNowFlag]);
                WriteToJson();
                SetUndobtnAndRedobtnState();
                return;
            }
            else if (JsonTempNowFlag > 0 && JsonTempNowFlag == (JsonTemp.Count - 1))
            {
                //到达list尾部，只可撤销不可重做
                return;
            }
        }
        //打开拖入json文件文件夹
        private void OpenJsonFolderbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(@Settings.Default.JsonFolderPath))
                {
                    Process.Start(@Settings.Default.JsonFolderPath); // 直接打开文件夹
                }
                else
                {
                    MessageBox.Show("文件夹不存在！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无效路径！发生错误：{ex.Message}");
            }
        }
        //打开预设键位文件夹
        private void openPresetJsonFolderbutton_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取程序根目录
                string rootPath = Application.StartupPath;

                // 拼接data文件夹路径
                string dataFolderPath = Path.Combine(rootPath, "data");

                // 如果文件夹不存在则创建
                if (!Directory.Exists(dataFolderPath))
                {
                    Directory.CreateDirectory(dataFolderPath);
                }

                // 打开文件夹
                Process.Start(new ProcessStartInfo
                {
                    FileName = dataFolderPath,
                    UseShellExecute = true,  // 启用Shell功能
                    Verb = "open"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //更新文件地址
        private void updateJsonUrltextBox(string jsonFilePath) {
            try
            {
                JsonUrltextBox.Text = jsonFilePath;
                JsonUrltextBox.TextAlign = HorizontalAlignment.Right;
                JsonUrltextBox.SelectionStart = JsonUrltextBox.TextLength;
                JsonUrltextBox.ScrollToCaret();
                if (File.Exists(jsonFilePath)) {
                    lastWriteTime = File.GetLastWriteTimeUtc(jsonFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //打开文件名下拉框
        private void fileNamecomboBox_DropDown(object sender, EventArgs e)
        {
            try
            {
                //初始化设置：
                InitializeFileNamecomboBox(fileNamecomboBox, false);
                fileNameSearchtextBox.Visible = true;
                fileNameSearchtextBox.BringToFront();
                fileNameSearchtextBox.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //关闭文件名下拉框
        private void fileNamecomboBox_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                fileNameSearchtextBox.Visible = false;
                if (fileNamecomboBox.SelectedValue != null)
                {
                    //选项不为空
                    //如果未发生变化则直接取消
                    if (fileNamecomboBox.SelectedValue.ToString() == JsonUrltextBox.Text)
                    {
                        flagSleepFlushingFilename = true;
                        return;
                    }
                    if (Undobutton.Enabled||Redobutton.Enabled)
                    {
                        DialogResult result = MessageBox.Show(reloadingTip, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No)
                        {
                            //直接初始化并利用初始化尝试还原上一个选项
                            InitializeFileNamecomboBox(fileNamecomboBox, true);
                            updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                            flagSleepFlushingFilename = true;
                            return;
                        }
                    }
                    updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                    string filePath = @JsonUrltextBox.Text;
                    if (File.Exists(filePath))
                    {
                        Properties.Settings.Default.JsonFolderPath = Path.GetDirectoryName(filePath);
                        Properties.Settings.Default.Save();
                        lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                        //bool readflag = true;
                        //if (MyMuMuJosn == "") { readflag = false; }
                        MyMuMuJosn = File.ReadAllText(filePath);
                        BackupAfterJsonReading();
                        autoReadBindkey3PP();
                        flagSleepFlushingFilename = true;
                        //if (readflag)
                        //{
                        //    MessageBox.Show("加载成功！");
                        //}
                    }
                    else
                    {
                        MessageBox.Show("不存在的Json文件，请检查您的文件路径。");
                        //直接初始化并利用初始化尝试还原上一个选项
                        fileNameSearchtextBox.Text = "";
                        InitializeFileNamecomboBox(fileNamecomboBox, true);
                        updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                        flagSleepFlushingFilename = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                flagSleepFlushingFilename = true;
                MessageBox.Show($"发生错误：{ex.Message}");
            }

        }
        //这里实现搜索功能
        private void fileNameSearchtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //无论如何，先关闭计时器
                flagSleepFlushingFilename = false;
                if (string.IsNullOrWhiteSpace(fileNameSearchtextBox.Text))
                {
                    InitializeFileNamecomboBox(fileNamecomboBox,false);
                }
                else
                {
                    InitializeFileNamecomboBoxAndSearch(fileNamecomboBox, fileNameSearchtextBox.Text,true);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //这里实现打开下拉框后再更新
        private void CheckSearchTextVtimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (fileNamecomboBox.DroppedDown & flagSleepFlushingFilename){
                    if (!string.IsNullOrWhiteSpace(fileNameSearchtextBox.Text))
                    {
                        InitializeFileNamecomboBoxAndSearch(fileNamecomboBox, fileNameSearchtextBox.Text,false);
                    }
                }
                if (KeysListcomboBox.DroppedDown & flagSleepFlushingFilename) {
                    if (!string.IsNullOrWhiteSpace(KeysListSearchtextBox.Text)) {
                        InitializeKeysComboBoxAndSearch(KeysListcomboBox, KeysListSearchtextBox.Text,false);
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //修改包名选择项
        private void packageNamecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (flagFlushingFilename){
                    InitializeFileNamecomboBox(fileNamecomboBox, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //修改文件名选择项
        private void fileNamecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (flagFlushingFilename) {
                    if (fileNamecomboBox.SelectedValue != null)
                    {
                        //如果未发生变化则直接取消
                        if (fileNamecomboBox.SelectedValue.ToString() == JsonUrltextBox.Text) { return; }
                        if (Undobutton.Enabled || Redobutton.Enabled)
                        {
                            DialogResult result = MessageBox.Show(reloadingTip, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                            {
                                //直接初始化并利用初始化尝试还原上一个选项
                                InitializeFileNamecomboBox(fileNamecomboBox, true);
                                updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                                return;
                            }
                        }
                        updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                        string filePath = @JsonUrltextBox.Text;
                        if (File.Exists(filePath))
                        {
                            Properties.Settings.Default.JsonFolderPath = Path.GetDirectoryName(filePath);
                            Properties.Settings.Default.Save();
                            lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                            //bool readflag = true;
                            //if (MyMuMuJosn == "") { readflag = false; }
                            MyMuMuJosn = File.ReadAllText(filePath);
                            BackupAfterJsonReading();
                            autoReadBindkey3PP();
                            //if (readflag)
                            //{
                            //    MessageBox.Show("加载成功！");
                            //}
                        }else{
                            MessageBox.Show("不存在的Json文件，请检查您的文件路径。");
                            //直接初始化并利用初始化尝试还原上一个选项
                            fileNameSearchtextBox.Text = "";
                            InitializeFileNamecomboBox(fileNamecomboBox, true);
                            updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //将当前按键文件导入data文件夹中
        private void importKeymapbutton_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取程序根目录
                string rootPath = Application.StartupPath;
                // 拼接data文件夹路径
                string dataFolderPath = Path.Combine(rootPath, "data");
                // 如果文件夹不存在则创建
                if (!Directory.Exists(dataFolderPath))
                {
                    Directory.CreateDirectory(dataFolderPath);
                }
                string fileName = fileNamecomboBox.Text.ToString()+".json";
                string filePath = Path.Combine(dataFolderPath, fileName);
                if (MyMuMuJosn == "" || filePath == "")
                {
                    MessageBox.Show("请先加载一个Json文件！");
                    return;
                }
                if (File.Exists(filePath))
                {
                    //已有同名文件
                    DialogResult result = MessageBox.Show("检测到data文件夹中已有同名文件，是否覆盖？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) {
                        File.WriteAllText(filePath, MyMuMuJosn);
                        MessageBox.Show("覆写成功！请点开下拉框选择你需要的文件。");
                    }
                }
                else {
                    //没有同名文件则创建并写入
                    File.WriteAllText(filePath, MyMuMuJosn,Encoding.UTF8);
                    MessageBox.Show("导入成功！请点开下拉框选择你需要的文件。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //实现文件名搜索框中回车等按键交互
        private void fileNameSearchtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //处理回车键，关闭下拉框
                if (e.KeyCode == Keys.Enter)
                {
                    fileNamecomboBox.DroppedDown = false;
                    Application.DoEvents();
                    e.Handled = true; // 阻止默认行为
                }
                // 处理方向键向上，选项向上跳一位
                else if (e.KeyCode == Keys.Up)
                {

                    if (fileNamecomboBox.SelectedIndex > 0)
                    {
                        //防止触发selecteditem更新
                        flagFlushingFilename = false;
                        fileNamecomboBox.SelectedIndex -= 1;
                    }
                    e.Handled = true;
                }
                // 处理方向键向下，选项向下跳一位
                else if (e.KeyCode == Keys.Down)
                {
                    if (fileNamecomboBox.SelectedIndex < (fileNamecomboBox.Items.Count - 1))
                    {
                        //防止触发selecteditem更新
                        flagFlushingFilename = false;
                        fileNamecomboBox.SelectedIndex += 1;
                    }
                    e.Handled = true;
                }
                //恢复判定
                flagFlushingFilename = true;
            }
            catch (Exception ex)
            {
                //恢复判定
                flagFlushingFilename = true;
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void KeysListSearchtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //处理回车键，关闭下拉框
                if (e.KeyCode == Keys.Enter)
                {
                    KeysListcomboBox.DroppedDown = false;
                    Application.DoEvents();
                    e.Handled = true; // 阻止默认行为
                }
                // 处理方向键向上，选项向上跳一位
                else if (e.KeyCode == Keys.Up)
                {

                    if (KeysListcomboBox.SelectedIndex > 0)
                    {
                        KeysListcomboBox.SelectedIndex -= 1;
                    }
                    e.Handled = true;
                }
                // 处理方向键向下，选项向下跳一位
                else if (e.KeyCode == Keys.Down)
                {
                    if (KeysListcomboBox.SelectedIndex < (KeysListcomboBox.Items.Count - 1))
                    {
                        KeysListcomboBox.SelectedIndex += 1;
                    }
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //尝试获取json文件夹
        private void TryGetJsonFileFolder()
        {
            try
            {
                string MuMuJsonFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+ @"\AppData\Roaming\Netease\MuMuPlayer\data\keymapConfig";
                if (Directory.Exists(MuMuJsonFolder))
                {
                    //Process.Start(MuMuJsonFolder); // 直接打开文件夹
                    //保存文件夹路径
                    Properties.Settings.Default.JsonFolderPath = MuMuJsonFolder;
                    Properties.Settings.Default.Save();
                    InitializeFileNamecomboBox(fileNamecomboBox, false);
                    //初始化后加载
                    if (fileNamecomboBox.SelectedValue != null)
                    {
                        updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                        string filePath = @JsonUrltextBox.Text;
                        if (File.Exists(filePath))
                        {
                            Properties.Settings.Default.JsonFolderPath = Path.GetDirectoryName(filePath);
                            Properties.Settings.Default.Save();
                            lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                            MyMuMuJosn = File.ReadAllText(filePath);
                            BackupAfterJsonReading();
                        }
                    }

                }
                else
                {
                    //MessageBox.Show("未成功获取MuMu模拟器储存按键文件的文件夹！\n请手动导入一个keymapConfig文件夹内的按键文件以获取路径！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }

        }

        private void TryGetJsonFileFolderbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMuMuJosn != "")
                {
                    DialogResult result = MessageBox.Show("该操作会导致尚未保存的修改被覆盖，是否继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No){return;}
                }
                string MuMuJsonFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Netease\MuMuPlayer\data\keymapConfig";
                if (Directory.Exists(MuMuJsonFolder))
                {
                    Process.Start(MuMuJsonFolder); // 直接打开文件夹
                    //保存文件夹路径
                    Properties.Settings.Default.JsonFolderPath = MuMuJsonFolder;
                    Properties.Settings.Default.Save();
                    InitializeFileNamecomboBox(fileNamecomboBox, false);
                    //初始化后加载
                    if (fileNamecomboBox.SelectedValue != null)
                    {
                        updateJsonUrltextBox(fileNamecomboBox.SelectedValue.ToString());
                        string filePath = @JsonUrltextBox.Text;
                        if (File.Exists(filePath))
                        {
                            Properties.Settings.Default.JsonFolderPath = Path.GetDirectoryName(filePath);
                            Properties.Settings.Default.Save();
                            lastWriteTime = File.GetLastWriteTimeUtc(filePath);
                            MyMuMuJosn = File.ReadAllText(filePath);
                            BackupAfterJsonReading();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("未成功获取MuMu模拟器储存按键文件的文件夹！\n请手动导入一个keymapConfig文件夹内的按键文件以获取路径！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
        //选择分辨率类型
        private void resolutionTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (flagFlushingResolutionType) {
                    //防止重复触发selecteditemchanged
                    flagFlushingResolution = false;
                    flagFXFYChanged = false;
                    //获取预设类型
                    string[] ResolutionTypesValues = resolutionTypes.Values.ToArray();
                    if (resolutionTypecomboBox.SelectedValue.ToString() == ResolutionTypesValues[0])
                    {
                        //属于平板
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution1.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        resolutioncomboBox.SelectedIndex = 0;

                    }
                    else if (resolutionTypecomboBox.SelectedValue.ToString() == ResolutionTypesValues[1])
                    {
                        //属于手机
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution2.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        resolutioncomboBox.SelectedIndex = 0;
                    }
                    else if (resolutionTypecomboBox.SelectedValue.ToString() == ResolutionTypesValues[2])
                    {
                        //属于超宽屏
                        deleteUDResolutionbutton.Visible = false;
                        resolutioncomboBox.DataSource = resolution3.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        resolutioncomboBox.SelectedIndex = 0;
                    }
                    else if(resolutionTypecomboBox.SelectedValue.ToString() == ResolutionTypesValues[3])
                    {
                        //属于自定义
                        deleteUDResolutionbutton.Visible = true;
                        Dictionary<string, string> resolution4 = new Dictionary<string, string> { };
                        if (!string.IsNullOrWhiteSpace(Settings.Default.resolution4String))
                        {
                            resolution4 = MuMuJsonEditor.StringToResolution(Settings.Default.resolution4String);
                        }
                        else
                        {
                            resolutioncomboBox.DataSource = null;
                            resolutioncomboBox.Items.Clear();
                            resolutioncomboBox.Items.Add("暂无自定义分辨率！");
                            resolutioncomboBox.SelectedIndex = 0;
                            //退出时恢复开关
                            flagFlushingResolution = true;
                            flagFXFYChanged = true;
                            return;
                        }
                        resolutioncomboBox.DataSource = resolution4.ToList();
                        resolutioncomboBox.DisplayMember = "Key";
                        resolutioncomboBox.ValueMember = "Value";
                        resolutioncomboBox.SelectedIndex = 0;
                    }
                    if (resolutioncomboBox.SelectedValue != null)
                    {
                        string[] value = resolutioncomboBox.SelectedValue.ToString().Split(',');
                        FXtextBox.Text = value[0];
                        FYtextBox.Text = value[1];
                    }
                    //退出时恢复开关
                    flagFlushingResolution = true;
                    flagFXFYChanged = true;
                }
            }
            catch (Exception ex)
            {
                //退出时恢复开关
                flagFlushingResolution = true;
                flagFXFYChanged = true;
                MessageBox.Show($"发生错误：{ex.Message}");
            }

            
        }
        //选择分辨率
        private void resolutioncomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //防止重复触发textChanged
                flagFXFYChanged = false;
                if (resolutioncomboBox.SelectedValue != null&flagFlushingResolution) {
                    string[] value = resolutioncomboBox.SelectedValue.ToString().Split(',');
                    FXtextBox.Text = value[0];
                    FYtextBox.Text = value[1];
                }
                //退出时恢复开关
                flagFXFYChanged = true;
            }
            catch (Exception ex)
            {
                //退出时恢复开关
                flagFXFYChanged = true;
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void deleteUDResolutionbutton_Click(object sender, EventArgs e)
        {
            try
            {
                //防止重复触发selecteditemchanged
                flagFlushingResolution = false;
                flagFlushingResolutionType = false;
                //获取预设类型
                string[] ResolutionTypesValues = resolutionTypes.Values.ToArray();
                string key = FXtextBox.Text + "x" + FYtextBox.Text;
                if (resolutionTypecomboBox.SelectedValue.ToString() == ResolutionTypesValues[3]) {
                    Dictionary<string, string> resolution4 = new Dictionary<string, string> { };
                    if (!string.IsNullOrWhiteSpace(Settings.Default.resolution4String))
                    {
                        resolution4 = MuMuJsonEditor.StringToResolution(Settings.Default.resolution4String);
                    }
                    else {
                        //恢复开关
                        flagFlushingResolution = true;
                        flagFlushingResolutionType = true;
                        return;
                    }
                    if (resolution4.ContainsKey(key))
                    {
                        resolution4.Remove(key);
                    }
                    Settings.Default.resolution4String = MuMuJsonEditor.ResolutionToString(resolution4);
                    Settings.Default.Save();
                    InitializeResolutioncomboBox(resolutioncomboBox);
                    //恢复开关
                    flagFlushingResolution = true;
                    flagFlushingResolutionType = true;
                }
                
            }
            catch (Exception ex)
            {
                //恢复开关
                flagFlushingResolution = true;
                flagFlushingResolutionType = true;
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void deleteDataJsonbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (KeysListcomboBox.SelectedValue != null) {
                    string[]item= KeysListcomboBox.SelectedItem.ToString().Split(',');
                    DialogResult result = MessageBox.Show($"该操作会导致data文件夹下的 {item[1].Substring(0, item[1].Length-1)} 文件被删除，是否继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) { return; }
                    string filePath = @KeysListcomboBox.SelectedValue.ToString();
                    if (File.Exists(filePath)) {
                        File.Delete(filePath);
                        MessageBox.Show("文件已删除");
                    }
                    else
                    {
                        MessageBox.Show("文件不存在！");
                    }
                    InitializeKeysComboBox(KeysListcomboBox);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }
    }


}
