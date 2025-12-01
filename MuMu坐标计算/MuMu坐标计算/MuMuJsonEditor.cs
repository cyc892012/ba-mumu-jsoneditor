using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO.Compression;

namespace MuMu坐标计算
{
    internal class MuMuJsonEditor
    {
        //按键类型
        public static string typeClick = "Click";
        public static string typeMacro = "Macro";
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        // 获取扫描码（十进制）
        public static int GetScanCode(Keys key)
        {
            return (int)MapVirtualKey((uint)key, 0);
        }
        // 获取当前按下键的扫描码（十进制）
        public static int GetCurrentScanCode()
        {
            Keys keyPressed = GetPressedKey();
            return keyPressed != Keys.None ? GetScanCode(keyPressed) : -1;
        }
        // 获取当前按下的键（处理多按键情况）
        private static Keys GetPressedKey()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (GetAsyncKeyState(key) < 0)
                {
                    return key;
                }
            }
            return Keys.None;
        }
        //压缩/解压



        //定位对应按键在Json文件的位置
        public static int FindKey(string myJson, KeyEventArgs e) {
            string searchText = "\"virtual_key\": " + e.KeyValue;
            int startPosition = 0;
            startPosition = myJson.IndexOf(searchText);
            return startPosition;
        }
        //返回按键类型
        public static string FindType(string myJson, KeyEventArgs e)
        {
            string Type = "";
            string searchType = "\"type\":";
            int key = FindKey(myJson, e);//定位按键坐标
            int type = myJson.IndexOf(searchType, key);//定位type属性坐标
            int end = myJson.IndexOf("\"", type + 10);
            Type = myJson.Substring(type + 9, end - (type + 9));
            return Type;
        }
        //检查对应按键是否符合要求（根据功能需求，目前仅支持单击按键、宏按键）
        public static bool CheckType(string myJson, KeyEventArgs e) {
            string Type = FindType(myJson, e);
            if (Type == typeClick || Type == typeMacro)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //定位上方X坐标位置
        public static int FindPreX(string myJson, KeyEventArgs e) {
            string searchX = "\"rel_x\":";
            int key = FindKey(myJson, e);
            int relX = myJson.LastIndexOf(searchX, key);
            if (relX == -1)
            {
                return relX;
            }
            else
            {
                return relX + 9;
            }
        }
        //定位上方Y坐标位置
        public static int FindPreY(string myJson, KeyEventArgs e)
        {
            string searchY = "\"rel_y\":";
            int key = FindKey(myJson, e);
            int relY = myJson.LastIndexOf(searchY, key);
            if (relY == -1)
            {
                return relY;
            }
            else
            {
                return relY + 9;
            }
        }
        //定位下方X坐标位置
        public static int FindAftX(string myJson, KeyEventArgs e)
        {
            string searchX = "\"rel_x\":";
            int key = FindKey(myJson, e);
            int relX = myJson.IndexOf(searchX, key);
            if (relX == -1)
            {
                return relX;
            }
            else
            {
                return relX + 9;
            }
        }
        //定位下方Y坐标位置
        public static int FindAftY(string myJson, KeyEventArgs e)
        {
            string searchY = "\"rel_y\":";
            int key = FindKey(myJson, e);
            int relY = myJson.IndexOf(searchY, key);
            if (relY == -1)
            {
                return relY;
            }
            else
            {
                return relY + 9;
            }
        }
        //定位X,Y之间分隔符
        public static int FindSep(string myJson, int start) {
            string searchSep = ",";
            return myJson.IndexOf(searchSep, start);
        }
        //定位Y坐标结束
        public static int FindEnd(string myJson, int start) {
            string searchEnd = "},";
            return myJson.IndexOf(searchEnd, start);
        }
        //定位宏指牌的X起始坐标
        public static int FindMacroX(string myJson, KeyEventArgs e) {
            int MacroX = 0;
            string searchX = "mouse;(";
            int key = FindKey(myJson, e);
            MacroX = myJson.IndexOf(searchX, key);
            if (MacroX == -1)
            {
                return MacroX;
            }
            else
            {
                return MacroX + 7;
            }
        }
        //定位宏指牌的Y坐标结束
        public static int FindMacroEnd(string myJson, int start) {
            int MacroEnd = 0;
            string searchEnd = ")";
            MacroEnd = myJson.IndexOf(searchEnd, start);
            return MacroEnd;
        }
        //修改单击按键坐标
        public static string ReKey(string myJson, KeyEventArgs e, string X, string Y) {

            string Type = FindType(myJson, e);
            //修改单击按键
            if (Type == typeClick)
            {
                //生成两个坐标模板
                string rwp = $"\"rel_work_position\": {{\r\n                \"rel_x\": {X},\r\n                \"rel_y\": {Y}\r\n            }},";
                string rp = $"\"rel_position\": {{\r\n                    \"rel_x\": {X},\r\n                    \"rel_y\": {Y}\r\n                }},";
                //定位按键位置
                int key = FindKey(myJson, e);
                //定位下方"rel_work_position":
                int rwp_start = myJson.IndexOf("\"rel_work_position\":", key);
                //定位"rel_work_position":后的第一个},
                int rwp_end = myJson.IndexOf("},", rwp_start);
                //调整位置
                rwp_end += 2;
                myJson = myJson.Substring(0, rwp_start) + rwp + myJson.Substring(rwp_end);
                //定位上方的"rel_position":
                int rp_start = myJson.LastIndexOf("\"rel_position\":", key);
                //定位"rel_position":后的第一个},
                int rp_end = myJson.IndexOf("},", rp_start);
                //调整位置
                rp_end += 2;
                myJson = myJson.Substring(0, rp_start) + rp + myJson.Substring(rp_end);
            }
            //修改宏指牌按键
            else if (Type == typeMacro) {
                //定位宏指牌按键坐标
                int KeyMacroX = FindMacroX(myJson, e);
                int keyMacroSep = FindSep(myJson, KeyMacroX);
                int keyMacroEnd = FindMacroEnd(myJson, keyMacroSep);
                //修改宏指牌按键
                myJson = myJson.Substring(0, KeyMacroX) + X + "," + Y + myJson.Substring(keyMacroEnd);
            }


            return myJson;
        }
        //读取单击按键坐标
        public static string[] ReadKeyPP(string myJson, KeyEventArgs e) {

            try
            {
                string type = FindType(myJson, e);
                if (type == typeClick)
                {
                    //单击键位时查找坐标
                    string keyX = "";
                    string keyY = "";
                    //寻找X坐标
                    int keyAftX = FindAftX(myJson, e);
                    int keyAftSep = FindSep(myJson, keyAftX);
                    keyX = myJson.Substring(keyAftX, keyAftSep - keyAftX);
                    //寻找Y坐标
                    int keyAftY = FindAftY(myJson, e);
                    int keyAftEnd = FindEnd(myJson, keyAftY) - 13;
                    keyY = myJson.Substring(keyAftY, keyAftEnd - keyAftY);
                    return new string[] { keyX, keyY };
                }
                else if (type == typeMacro)
                {
                    //宏按键时查找坐标，多组坐标时返回第一组
                    //查找 "virtual_key": +键值 的起始位置
                    int baseIndex = myJson.IndexOf("\"virtual_key\": " + e.KeyValue);
                    //查找 ( 定位坐标起始位置
                    int StartIndex = myJson.IndexOf("(",baseIndex);
                    //查找 "type" 定位键位终点位置
                    int NextIndex = myJson.IndexOf("\"type\"", baseIndex);
                    //坐标起始位置越界，该宏键位不存在坐标。
                    if (StartIndex > NextIndex) { return null; }
                    //查找 , 定位坐标中间位置
                    int MidIndex = myJson.IndexOf(",",StartIndex);
                    //查找 ) 定位坐标结束位置
                    int EndIndex = myJson.IndexOf(")", MidIndex);
                    string keyX = myJson.Substring(StartIndex + 1, MidIndex - StartIndex - 1);
                    string keyY = myJson.Substring(MidIndex + 1, EndIndex - MidIndex - 1);

                    return new string[] {keyX,keyY };
                }
                else
                {
                    MessageBox.Show("当前仅支持读取单击按键中的坐标或宏按键的第一组坐标，请检查您选择的按键！");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
            return null;  
        }
        //读取Json文件指定按键代码
        public static string ReadKey(string filePath, KeyEventArgs e) {
            try
            {
                string text = File.ReadAllText(filePath);
                string keystart = "\r\n        ";
                //查找 "virtual_key": +键值 的起始位置
                int baseIndex = text.IndexOf("\"virtual_key\": " + e.KeyValue);
                if (baseIndex == -1) { return null; }
                //尝试向上查找"type"
                int lastTypeIndex = text.LastIndexOf("\"type\"", baseIndex);
                int startIndex = -1;
                if (lastTypeIndex == -1)
                {
                    //该键位为第一个按键，定位"keymaps"后的第一个{
                    startIndex = text.IndexOf("{", text.IndexOf("\"keymaps\""));
                }
                else
                {
                    //该按键不为第一个按键，定位上一个"type"后的{
                    startIndex = text.IndexOf("{", lastTypeIndex);
                }
                if (startIndex == -1) { return null; }
                //向下查找 "type" ，再向下查找 } 定位结尾
                int endIndex = text.IndexOf("}", text.IndexOf("\"type\":", baseIndex));
                if (endIndex == -1) { return null; }
                return keystart+text.Substring(startIndex, endIndex - startIndex + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
            return null;
        }
        //读取Json文件按键代码部分
        public static string ReadKeys(string filePath)
        {
            try
            {
                string text = File.ReadAllText(filePath);

                // 查找"keymaps":的起始位置
                int startIndex = text.IndexOf("\"keymaps\":");
                if (startIndex == -1) { return null; } else { startIndex +=  12; }
                // 查找"param":的起始位置
                int endIndex = text.IndexOf("\"param\":");
                if (endIndex == -1) { return null; }
                //向上找到结尾的“}”
                endIndex = text.LastIndexOf("}", endIndex);
                if (endIndex == -1) { return null; }
                return text.Substring(startIndex, endIndex-startIndex+1);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
            return null;
        }
        //将完整按键写入Json文件
        public static string WriteKeys(string keys,string myJson) {

            try
            {
                // 查找"keymaps":的起始位置
                int startIndex = myJson.IndexOf("\"keymaps\":");
                int endIndex = myJson.IndexOf("\"param\":");
                if (startIndex == -1) { return null; } else { startIndex += 12; }
                if (endIndex == -1) { return null; }
                if (endIndex - startIndex < 100) {
                    //判定为没有任何按键的空文件
                    return myJson.Substring(0, startIndex) + keys + myJson.Substring(startIndex);
                }
                return myJson.Substring(0,startIndex)+keys+ ","+myJson.Substring(startIndex);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
            return null;
        }
        //获取待写入的按键部分的所有键值
        public static string[] FindKeyValues(string filePath)
        {
            List<string> keyvalues = new List<string>();

            try
            {
                // 读取文件内容
                string text = File.ReadAllText(filePath);

                // 正则表达式匹配模式："virtual_key"后紧跟数字（支持整数/小数）
                Regex regex = new Regex(@"""virtual_key""\s*:\s*(\d+)", RegexOptions.IgnoreCase);

                // 查找所有匹配项
                MatchCollection matches = regex.Matches(text);

                // 提取数字部分
                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1)
                    {
                        keyvalues.Add(match.Groups[1].Value);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"错误：文件 '{filePath}' 未找到。");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"读取文件时发生错误：{ex.Message}");
            }

            return keyvalues.ToArray();
        }
        //检查被写入的Json文件是否有按键重复
        public static bool AreAllKeysMissing(string[] keys, string myJson)
        {
            // 使用正则表达式提取所有虚拟键值（支持带/不带引号的数字）
            var regex = new Regex(@"""virtual_key""\s*:\s*(\d+)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(myJson);

            // 将所有匹配到的键值存入哈希集合（去重）
            var existingKeys = new HashSet<string>();
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    existingKeys.Add(match.Groups[1].Value);
                }
            }

            // 检查所有待查键值是否均不在集合中
            foreach (var key in keys)
            {
                if (existingKeys.Contains(key))
                {
                    return false; // 存在冲突，立即返回false
                }
            }

            return true; // 所有键值均不存在
        }
        //如果有按键重复，以下两个函数用于检测重复的按键
        //获取文件中的所有键位
        public static string[] FindKeyTexts(string filePath)
        {
            List<string> keytexts = new List<string>();

            try
            {
                // 读取文件内容
                string text = File.ReadAllText(filePath);

                // 正则表达式匹配模式："text": "" 中""中的键位内容（支持英文/数字）
                Regex regex = new Regex(@"""text""\s*:\s*""([^""]+)""", RegexOptions.IgnoreCase);

                // 查找所有匹配项
                MatchCollection matches = regex.Matches(text);

                // 提取数字部分
                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1)
                    {
                        keytexts.Add(match.Groups[1].Value);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"错误：文件 '{filePath}' 未找到。");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"读取文件时发生错误：{ex.Message}");
            }

            return keytexts.ToArray();
        }
        //检查被写入的Json文件具体有什么按键重复
        public static string[] FindAllRepeatKeyTexts(string[] keys, string myJson)
        {
            // 使用正则表达式提取所有虚拟键值（支持带/不带引号的数字）
            var regex = new Regex(@"""text""\s*:\s*""([^""]+)""", RegexOptions.IgnoreCase);
            var matches = regex.Matches(myJson);
            List<string> repeatKeyTexts = new List<string>();
            // 将所有匹配到的键值存入哈希集合（去重）
            var existingKeys = new HashSet<string>();
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    existingKeys.Add(match.Groups[1].Value);
                }
            }

            // 检查所有待查键值是否均不在集合中
            foreach (var key in keys)
            {
                if (existingKeys.Contains(key))
                {
                    repeatKeyTexts.Add(key);// 存在冲突，记录冲突键位
                }
            }

            return repeatKeyTexts.ToArray(); // 返回冲突的键位
        }
        //记录重复按键的键值
        public static string[] FindAllRepeatKeyValues(string[] keys, string myJson)
        {
            // 使用正则表达式提取所有虚拟键值（支持带/不带引号的数字）
            var regex = new Regex(@"""virtual_key""\s*:\s*(\d+)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(myJson);
            List<string> repeatKeyValues = new List<string>();
            // 将所有匹配到的键值存入哈希集合（去重）
            var existingKeys = new HashSet<string>();
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    existingKeys.Add(match.Groups[1].Value);
                }
            }

            // 检查所有待查键值是否均不在集合中
            foreach (var key in keys)
            {
                if (existingKeys.Contains(key))
                {
                    repeatKeyValues.Add(key);// 存在冲突，记录冲突键值
                }
            }
            return repeatKeyValues.ToArray(); // 返回冲突键值
        }
        //寻找并返回指定区域的按键坐标、键名、键值
        public static List<(double RelX, double RelY, string Text, string VirtualKey)> FindRangeKeyValues(double[] rangeLT, double[]rangeRD,string myJson) {
            List<string> rangeKeyValues = new List<string>();
            try
            {
                var results = new List<(double, double, string, string)>();
                string pattern = @"""rel_position""[\s\S]*?""rel_x"":\s*(\d+\.\d+)[\s\S]*?""rel_y"":\s*(\d+\.\d+)[\s\S]*?""key""[\s\S]*?""text"":\s*""([^""]+)""[\s\S]*?""virtual_key"":\s*(\d+)";

                foreach (Match match in Regex.Matches(myJson, pattern, RegexOptions.Multiline))
                {
                    double relX = double.Parse(match.Groups[1].Value);
                    double relY = double.Parse(match.Groups[2].Value);
                    string text = match.Groups[3].Value;
                    string virtualKey = match.Groups[4].Value;
                    if (relX > rangeLT[0] && relX < rangeRD[0] && relY > rangeLT[1] && relY < rangeRD[1])
                    {
                        results.Add((relX, relY, text, virtualKey));
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
                return null;
            }
        }
        //通过键值定位按键并删除
        public static string DeleteKey(string key,string myJson) {
            try
            {
                //查找 "virtual_key": +键值 的起始位置
                int baseIndex = myJson.IndexOf("\"virtual_key\": "+key);
                //查找 "param": 定位尾端
                int fileEndIndex = myJson.IndexOf("\"param\":");
                //通过起始位置，定位上下边界的位置：
                //尝试向上定位 "type" 
                int lastTypeIndex = myJson.LastIndexOf("\"type\"", baseIndex);
                int startIndex = -1;
                //区分情况，如果向上找不到"type" ，按第一个键处理，定位"keymaps"后的"["，
                if (lastTypeIndex == -1)
                {
                    //该键位为第一个按键，定位"keymaps"后的"["
                    startIndex = myJson.IndexOf("[", myJson.IndexOf("\"keymaps\""));
                }
                else {
                    //该按键不为第一个按键，定位上一个"type"后的 ,
                    startIndex = myJson.IndexOf(",", lastTypeIndex);
                }
                //向下定位 "type": 后分情况，
                int endIndex = myJson.IndexOf("}", myJson.IndexOf("\"type\":", baseIndex));
                if (fileEndIndex - myJson.IndexOf("\"type\":", baseIndex) > 100)
                {
                    //定位非尾端按键
                    endIndex += 1;//后移一位覆盖逗号
                }
                else {
                    //定位尾端按键
                    startIndex -= 1; //删除范围覆盖逗号
                }
                //处理特殊情况，当文件中仅有一个按键时会变成特殊格式：
                if ((fileEndIndex - myJson.IndexOf("\"type\":", baseIndex) < 100) && lastTypeIndex == -1)
                {
                    startIndex += 1;//特殊情况时虽然是尾端按键但是范围不需要覆盖逗号
                }
                if (startIndex == -1||endIndex == -1) { return null; } 
                return myJson.Substring(0, startIndex+1) +  myJson.Substring(endIndex+1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
            return null;
        }
        //通过键值批量删除键位
        public static string DeleteKeys(string[]repeatKeyValues,string myJson)
        {
            try
            {
                foreach (string key in repeatKeyValues) {
                    myJson = DeleteKey(key, myJson);
                }
                return myJson;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
            return null;
        }
        //生成指定类型的键位
        public static string CreateKey(string keyType, KeyEventArgs bindKey, string keyX, string keyY,string scan_code) {
            try
            {
                string keystart = "\r\n        ";
                if (keyType == typeClick)
                {
                    //生成单击按键
                    return keystart+$"{{\r\n            \"editor_icon_scale\": 1,\r\n            \"icon\": {{\r\n                \"background_color\": \"00000066\",\r\n                \"description\": \"\",\r\n                \"radius_correction\": 1,\r\n                \"rel_position\": {{\r\n                    \"rel_x\": {keyX},\r\n                    \"rel_y\": {keyY}\r\n                }},\r\n                \"visibility\": true\r\n            }},\r\n            \"key\": {{\r\n                \"device\": \"keyboard\",\r\n                \"scan_code\": {scan_code},\r\n                \"text\": \"{bindKey.KeyCode.ToString()}\",\r\n                \"virtual_key\": {bindKey.KeyValue.ToString()}\r\n            }},\r\n            \"rel_work_position\": {{\r\n                \"rel_x\": {keyX},\r\n                \"rel_y\": {keyY}\r\n            }},\r\n            \"type\": \"Click\"\r\n        }}";
                }
                else if (keyType == typeMacro) {
                    //调整一下宏指牌按键的位置，触底向反方向偏移3%，未触底向正方向偏移3%
                    double keyPositionSet = 0.03;//偏移量
                    double keyPositionX = 0;
                    double keyPositionY = 0;
                    if ((double.Parse(keyX) + keyPositionSet) < 1) { keyPositionX = double.Parse(keyX) + keyPositionSet; } else { keyPositionX = double.Parse(keyX) - keyPositionSet; }
                    if ((double.Parse(keyY) + keyPositionSet) < 1) { keyPositionY = double.Parse(keyY) + keyPositionSet; } else { keyPositionY = double.Parse(keyY) - keyPositionSet; }
                    //生成宏指牌按键
                    return keystart+$"{{\r\n            \"editor_icon_scale\": 1,\r\n            \"icon\": {{\r\n                \"background_color\": \"00000066\",\r\n                \"description\": \"\",\r\n                \"radius_correction\": 1,\r\n                \"rel_position\": {{\r\n                    \"rel_x\": {keyPositionX},\r\n                    \"rel_y\": {keyPositionY}\r\n                }},\r\n                \"visibility\": true\r\n            }},\r\n            \"key\": {{\r\n                \"device\": \"keyboard\",\r\n                \"scan_code\": {scan_code},\r\n                \"text\": \"{bindKey.KeyCode.ToString()}\",\r\n                \"virtual_key\": {bindKey.KeyValue.ToString()}\r\n            }},\r\n            \"press_actions\": [\r\n                \"start_loop:until_release\",\r\n                \"curve_first_point_sleep_time:1\",\r\n                \"curve_last_point_sleep_time:1\",\r\n                \"curve_rel:mouse;({keyX},{keyY})\",\r\n                \"curve_release\",\r\n                \"stop_loop\"\r\n            ],\r\n            \"rel_work_position\": {{\r\n                \"rel_x\": {keyPositionX},\r\n                \"rel_y\": {keyPositionY}\r\n            }},\r\n            \"release_actions\": [\r\n\r\n            ],\r\n            \"type\": \"Macro\"\r\n        }}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
            return null;
        }
        public static string ResolutionToString(Dictionary<string, string> resolution) {
            try
            {
                if (resolution.Count == 0) { return ""; }
                else {
                    string resolutiongString="";
                    foreach (var item in resolution) {
                        string[] value = item.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        resolutiongString += item.Key + "," + value[0] + "," + value[1] + "V";
                    }
                    return resolutiongString;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return "";
            }
        }
        public static Dictionary<string, string> StringToResolution(string resolutionString) {
            try
            {
                if (string.IsNullOrWhiteSpace(resolutionString)) { return null; }
                else
                {
                    string[] temp = resolutionString.Split(new string[] { "V" }, StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<string, string> resolution = new Dictionary<string, string> { };
                    foreach (var item in temp)
                    {
                        string[] temp2 = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        resolution.Add(temp2[0], temp2[1]+ "," + temp2[2]);
                    }
                    return resolution;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
                return null;
            }

            
        }
        /*废案备份：
         * 之前考虑的是覆写一定位数的坐标，这样不用定位后续内容，但mumu模拟器保存下来的坐标位数不确定，最终还是舍弃了这个方案
         * 其实最开始想用的是把Json反序列化成字典再操作，但想想只是改个坐标罢了，又要引第三方库，没必要吧，字符串操作一下得了。
         public static string ReKey(string myJson, KeyEventArgs e, string X, string Y, int Len) {

            string reX = X.Substring(0, Len);
            string reY = Y.Substring(0, Len);
            int keyPreX = FindPreX(myJson, e);
            int keyPreY = FindPreY(myJson, e);
            int keyAftX = FindAftX(myJson, e);
            int keyAftY = FindAftY(myJson, e);
            myJson = myJson.Substring(0, keyPreX) + reX + myJson.Substring(keyPreX + Len);
            myJson = myJson.Substring(0, keyPreY) + reY + myJson.Substring(keyPreY + Len);
            myJson = myJson.Substring(0, keyAftX) + reX + myJson.Substring(keyAftX + Len);
            myJson = myJson.Substring(0, keyAftY) + reY + myJson.Substring(keyAftY + Len);
            return myJson;
        }
        */


    }
    public static class StringCompressor
    {
        // 压缩为字节数组
        public static byte[] Compress(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                }
                return outputStream.ToArray();
            }
        }

        // 压缩为Base64字符串（适合存储或传输）
        public static string CompressToBase64(string text)
        {
            byte[] compressedBytes = Compress(text);
            return Convert.ToBase64String(compressedBytes);
        }
    }
    public static class StringDecompressor
    {
        // 从字节数组解压
        public static string Decompress(byte[] compressedData)
        {
            if (compressedData == null || compressedData.Length == 0) return string.Empty;

            using (MemoryStream inputStream = new MemoryStream(compressedData))
            {
                using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(gzipStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        // 从Base64字符串解压
        public static string DecompressFromBase64(string base64Data)
        {
            byte[] compressedBytes = Convert.FromBase64String(base64Data);
            return Decompress(compressedBytes);
        }
    }
}
