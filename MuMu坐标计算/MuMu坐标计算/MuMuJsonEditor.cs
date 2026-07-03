using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MuMu坐标计算
{
    internal class MuMuJsonEditor
    {
        //按键类型
        public static string typeClick = "Click";
        public static string typeMacro = "Macro";
        public static string typeBunchClick = "BunchClick";

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        // 获取扫描码（十进制）
        public static int GetScanCode(Keys key)
        {
            return (int)MapVirtualKey((uint)key, 0);
        }

        public static int GetScanCodeForMouse(Keys key)
        {
            switch (key)
            {
                case Keys.LButton: return 1;
                case Keys.RButton: return 2;
                case Keys.MButton: return 4;
                case Keys.XButton1: return 5;
                case Keys.XButton2: return 6;
                default: return 0;
            }
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
                    return key;
            }
            return Keys.None;
        }

        // ===== JSON 核心操作（基于 Newtonsoft.Json.Linq） =====

        /// <summary>解析 JSON 字符串为 JObject</summary>
        private static JObject Parse(string json) => JObject.Parse(json);

        /// <summary>序列化 JObject 为标准缩进 JSON，保持4空格缩进、LF换行并控制浮点数精度</summary>
        private static string Serialize(JObject json)
        {
            var sb = new StringBuilder();
            using (var sw = new LfStringWriter(sb))
            using (var jtw = new PrecisionJsonWriter(sw))
            {
                jtw.Formatting = Formatting.Indented;
                jtw.Indentation = 4;
                jtw.IndentChar = ' ';
                json.WriteTo(jtw);
            }
            var result = sb.ToString();
            if (result.EndsWith("\n"))
                result = result.Substring(0, result.Length - 1);
            return result;
        }

        private sealed class LfStringWriter : StringWriter
        {
            public LfStringWriter(StringBuilder sb) : base(sb) { }
            public override string NewLine => "\n";
        }

        private sealed class PrecisionJsonWriter : JsonTextWriter
        {
            public PrecisionJsonWriter(TextWriter textWriter) : base(textWriter) { }

            public override void WriteValue(double value)
            {
                WriteRawValue(value.ToString("G16", System.Globalization.CultureInfo.InvariantCulture));
            }

            public override void WriteValue(float value)
            {
                WriteRawValue(value.ToString("G9", System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>根据 virtual_key 查找按键对象，未找到返回 null</summary>
        private static JObject GetKeyByVirtualKey(JObject json, int virtualKey)
        {
            var keymaps = json["keymaps"] as JArray;
            if (keymaps == null) return null;
            foreach (var item in keymaps)
            {
                var keyObj = item as JObject;
                if (keyObj?["key"]?["virtual_key"]?.Value<int>() == virtualKey)
                    return keyObj;
            }
            return null;
        }

        /// <summary>根据 key.text 查找按键对象，未找到返回 null</summary>
        private static JObject GetKeyByText(JObject json, string keyText)
        {
            var keymaps = json["keymaps"] as JArray;
            if (keymaps == null) return null;
            foreach (var item in keymaps)
            {
                var keyObj = item as JObject;
                if (keyObj?["key"]?["text"]?.Value<string>() == keyText)
                    return keyObj;
            }
            return null;
        }

        /// <summary>根据 virtual_key 查找按键在 keymaps 数组中的索引，未找到返回 -1</summary>
        private static int GetKeyIndexByVirtualKey(JObject json, int virtualKey)
        {
            var keymaps = json["keymaps"] as JArray;
            if (keymaps == null) return -1;
            for (int i = 0; i < keymaps.Count; i++)
            {
                if (keymaps[i] is JObject keyObj
                    && keyObj["key"]?["virtual_key"]?.Value<int>() == virtualKey)
                    return i;
            }
            return -1;
        }

        /// <summary>根据 key.text 查找按键在 keymaps 数组中的索引，未找到返回 -1</summary>
        private static int GetKeyIndexByText(JObject json, string keyText)
        {
            var keymaps = json["keymaps"] as JArray;
            if (keymaps == null) return -1;
            for (int i = 0; i < keymaps.Count; i++)
            {
                if (keymaps[i] is JObject keyObj
                    && keyObj["key"]?["text"]?.Value<string>() == keyText)
                    return i;
            }
            return -1;
        }

        //定位对应按键在Json文件的位置（返回 keymaps 数组中的索引，-1=未找到）
        public static int FindKey(string myJson, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(myJson) || e == null) return -1;
            return GetKeyIndexByVirtualKey(Parse(myJson), e.KeyValue);
        }

        public static int FindKey(string myJson, string KeyText)
        {
            return GetKeyIndexByText(Parse(myJson), KeyText);
        }

        //返回按键类型
        public static string FindType(string myJson, KeyEventArgs e)
        {
            var key = GetKeyByVirtualKey(Parse(myJson), e.KeyValue);
            return key?["type"]?.Value<string>() ?? "";
        }

        public static string FindType(string myJson, string KeyText)
        {
            var key = GetKeyByText(Parse(myJson), KeyText);
            return key?["type"]?.Value<string>() ?? "";
        }

        //检查对应按键是否符合要求（目前仅支持 Click 和 Macro）
        public static bool CheckType(string myJson, KeyEventArgs e)
        {
            string type = FindType(myJson, e);
            return type == typeClick || type == typeMacro || type == typeBunchClick;
        }

        //修改按键坐标（支持 Click 和 Macro）
        public static string ReKey(string myJson, KeyEventArgs e, string X, string Y)
        {
            var json = Parse(myJson);
            var key = GetKeyByVirtualKey(json, e.KeyValue);
            if (key == null) return myJson;

            string type = key["type"]?.Value<string>();
            if (!double.TryParse(X, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dX) ||
                !double.TryParse(Y, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dY))
                return myJson;

            if (type == typeClick)
            {
                SetRelPosition(key["icon"]?["rel_position"], dX, dY);
                SetRelPosition(key["rel_work_position"], dX, dY);
            }
            else if (type == typeBunchClick)
            {
                SetRelPosition(key["icon"]?["rel_position"], dX, dY);
                SetRelPosition(key["rel_work_position"], dX, dY);
            }
            else if (type == typeMacro)
            {
                var pressActions = key["press_actions"] as JArray;
                if (pressActions == null) return null;
                for (int i = 0; i < pressActions.Count; i++)
                {
                    var action = pressActions[i]?.Value<string>();
                    if (action != null && action.StartsWith("curve_rel:mouse;("))
                    {
                        pressActions[i] = "curve_rel:mouse;(" + X + "," + Y + ")";
                        return Serialize(json);
                    }
                }
                return null;
            }
            return Serialize(json);
        }

        public static string ReKey(string myJson, string KeyText, string X, string Y)
        {
            var json = Parse(myJson);
            var key = GetKeyByText(json, KeyText);
            if (key == null) return myJson;

            string type = key["type"]?.Value<string>();
            if (!double.TryParse(X, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dX) ||
                !double.TryParse(Y, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dY))
                return myJson;

            if (type == typeClick)
            {
                SetRelPosition(key["icon"]?["rel_position"], dX, dY);
                SetRelPosition(key["rel_work_position"], dX, dY);
            }
            else if (type == typeBunchClick)
            {
                SetRelPosition(key["icon"]?["rel_position"], dX, dY);
                SetRelPosition(key["rel_work_position"], dX, dY);
            }
            else if (type == typeMacro)
            {
                var pressActions = key["press_actions"] as JArray;
                if (pressActions == null) return null;
                for (int i = 0; i < pressActions.Count; i++)
                {
                    var action = pressActions[i]?.Value<string>();
                    if (action != null && action.StartsWith("curve_rel:mouse;("))
                    {
                        pressActions[i] = "curve_rel:mouse;(" + X + "," + Y + ")";
                        return Serialize(json);
                    }
                }
                return null;
            }
            return Serialize(json);
        }

        private static void SetRelPosition(JToken posObj, double x, double y)
        {
            if (posObj == null) return;
            posObj["rel_x"] = x;
            posObj["rel_y"] = y;
        }

        //读取单击按键或宏按键的第一组坐标
        public static string[] ReadKeyPP(string myJson, KeyEventArgs e)
        {
            try
            {
                var json = Parse(myJson);
                var key = GetKeyByVirtualKey(json, e.KeyValue);
                if (key == null) return null;

                string type = key["type"]?.Value<string>();
                if (type == typeClick)
                {
                    var rwp = key["rel_work_position"];
                    if (rwp == null) return null;
                    return new[] {
                        (rwp["rel_x"]?.Value<double>() ?? 0.0).ToString(),
                        (rwp["rel_y"]?.Value<double>() ?? 0.0).ToString()
                    };
                }
                else if (type == typeBunchClick)
                {
                    var rwp = key["rel_work_position"];
                    if (rwp == null) return null;
                    return new[] {
                        (rwp["rel_x"]?.Value<double>() ?? 0.0).ToString(),
                        (rwp["rel_y"]?.Value<double>() ?? 0.0).ToString()
                    };
                }
                else if (type == typeMacro)
                {
                    var pressActions = key["press_actions"] as JArray;
                    if (pressActions == null) return null;
                    foreach (var action in pressActions)
                    {
                        var text = action?.Value<string>();
                        if (text != null && text.StartsWith("curve_rel:mouse;("))
                        {
                            // 提取 (X,Y) 中的坐标
                            int start = text.IndexOf('(') + 1;
                            int comma = text.IndexOf(',', start);
                            int end = text.IndexOf(')', comma);
                            if (start > 0 && comma > start && end > comma)
                            {
                                return new[] {
                                    text.Substring(start, comma - start),
                                    text.Substring(comma + 1, end - comma - 1)
                                };
                            }
                            break;
                        }
                    }
                    return null;
                }
                else
                {
                    throw new NotSupportedException("当前仅支持读取单击按键中的坐标或宏按键的第一组坐标，请检查您选择的按键！");
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取按键坐标失败：{ex.Message}", ex);
            }
        }


        public static string[] ReadKeyPP(string myJson, string keyText)
        {
            try
            {
                var json = Parse(myJson);
                var key = GetKeyByText(json, keyText);
                if (key == null) return null;

                string type = key["type"]?.Value<string>();
                if (type == typeClick || type == typeBunchClick)
                {
                    var rwp = key["rel_work_position"];
                    if (rwp == null) return null;
                    return new[] {
                        (rwp["rel_x"]?.Value<double>() ?? 0.0).ToString(),
                        (rwp["rel_y"]?.Value<double>() ?? 0.0).ToString()
                    };
                }
                else if (type == typeMacro)
                {
                    var pressActions = key["press_actions"] as JArray;
                    if (pressActions == null) return null;
                    foreach (var action in pressActions)
                    {
                        var text = action?.Value<string>();
                        if (text != null && text.StartsWith("curve_rel:mouse;("))
                        {
                            int start = text.IndexOf('(') + 1;
                            int comma = text.IndexOf(',', start);
                            int end = text.IndexOf(')', comma);
                            if (start > 0 && comma > start && end > comma)
                            {
                                return new[] {
                                    text.Substring(start, comma - start),
                                    text.Substring(comma + 1, end - comma - 1)
                                };
                            }
                            break;
                        }
                    }
                    return null;
                }
                else
                {
                    throw new NotSupportedException("当前仅支持读取单击按键中的坐标或宏按键的第一组坐标，请检查您选择的按键！");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取按键坐标失败：{ex.Message}", ex);
            }
        }
        //读取 JSON 文件中指定按键的完整 JSON 块（返回可作为新按键插入的 JSON 字符串）
        public static string ReadKey(string filePath, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath) || e == null) return null;
            try
            {
                string text = File.ReadAllText(filePath, Encoding.UTF8);
                var json = Parse(text);
                var key = GetKeyByVirtualKey(json, e.KeyValue);
                return key != null ? key.ToString(Formatting.Indented) : null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        public static string ReadKey(string filePath, string KeyText)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(KeyText)) return null;
            try
            {
                string text = File.ReadAllText(filePath, Encoding.UTF8);
                var json = Parse(text);
                var key = GetKeyByText(json, KeyText);
                return key != null ? key.ToString(Formatting.Indented) : null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        //读取 JSON 文件中 keymaps 数组部分
        public static string ReadKeys(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;
            try
            {
                string text = File.ReadAllText(filePath, Encoding.UTF8);
                var json = Parse(text);
                var keymaps = json["keymaps"] as JArray;
                return keymaps?.ToString(Formatting.Indented);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        //将按键代码写入 JSON 文件
        public static string WriteKeys(string keys, string myJson)
        {
            try
            {
                var target = Parse(myJson);
                var keymaps = target["keymaps"] as JArray;
                if (keymaps == null) { target["keymaps"] = new JArray(); keymaps = target["keymaps"] as JArray; }

                // 解析要写入的按键（可能是一个完整的 keymaps 数组，也可能是一个单独的按键对象）
                JToken newKeys;
                try
                {
                    newKeys = JToken.Parse(keys.Trim());
                }
                catch
                {
                    return null;
                }

                if (newKeys is JArray arr)
                {
                    foreach (var item in arr)
                    {
                        keymaps.Add(item);
                    }
                }
                else if (newKeys is JObject obj)
                {
                    // 传入的是单个按键对象，添加到现有数组
                    keymaps.Add(obj);
                }

                return Serialize(target);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        //获取待写入的按键部分的所有键值（virtual_key）
        public static string[] FindKeyValues(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return Array.Empty<string>();
            try
            {
                string text = File.ReadAllText(filePath, Encoding.UTF8);
                var json = Parse(text);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null) return Array.Empty<string>();

                return keymaps
                    .Select(k => (k as JObject)?["key"]?["virtual_key"]?.Value<int>().ToString())
                    .Where(v => v != null)
                    .ToArray();
            }
            catch (FileNotFoundException) { throw; }
            catch (IOException ex) { throw new InvalidOperationException($"读取文件时发生错误：{ex.Message}", ex); }
        }

        //检查被写入的 JSON 文件是否有按键重复
        public static bool AreAllKeysMissing(string[] keys, string myJson)
        {
            var json = Parse(myJson);
            var keymaps = json["keymaps"] as JArray;
            if (keymaps == null) return true;

            var existingKeys = new HashSet<string>(
                keymaps.Select(k => (k as JObject)?["key"]?["virtual_key"]?.Value<int>().ToString())
                       .Where(v => v != null)
            );

            return keys.All(k => !existingKeys.Contains(k));
        }

        //获取文件中的所有键位文字
        public static string[] FindKeyTexts(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return Array.Empty<string>();
            try
            {
                string text = File.ReadAllText(filePath, Encoding.UTF8);
                var json = Parse(text);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null) return Array.Empty<string>();

                return keymaps
                    .Select(k => (k as JObject)?["key"]?["text"]?.Value<string>())
                    .Where(v => v != null)
                    .ToArray();
            }
            catch (FileNotFoundException) { throw; }
            catch (IOException ex) { throw new InvalidOperationException($"读取文件时发生错误：{ex.Message}", ex); }
        }

        //检查被写入的 JSON 文件具体有什么按键文字重复
        public static string[] FindAllRepeatKeyTexts(string[] keys, string myJson)
        {
            var json = Parse(myJson);
            var keymaps = json["keymaps"] as JArray;
            if (keymaps == null) return Array.Empty<string>();

            var existingKeys = new HashSet<string>(
                keymaps.Select(k => (k as JObject)?["key"]?["text"]?.Value<string>())
                       .Where(v => v != null)
            );

            return keys.Where(k => existingKeys.Contains(k)).ToArray();
        }

        //记录重复按键的键值
        public static string[] FindAllRepeatKeyValues(string[] keys, string myJson)
        {
            var json = Parse(myJson);
            var keymaps = json["keymaps"] as JArray;
            if (keymaps == null) return Array.Empty<string>();

            var existingKeys = new HashSet<string>(
                keymaps.Select(k => (k as JObject)?["key"]?["virtual_key"]?.Value<int>().ToString())
                       .Where(v => v != null)
            );

            return keys.Where(k => existingKeys.Contains(k)).ToArray();
        }

        //寻找并返回指定区域的按键坐标、键名、键值
        public static List<(double RelX, double RelY, string Text, string VirtualKey)> FindRangeKeyValues(
            double[] rangeLT, double[] rangeRD, string myJson)
        {
            try
            {
                var json = Parse(myJson);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null) return new List<(double, double, string, string)>();

                var results = new List<(double, double, string, string)>();
                foreach (var item in keymaps)
                {
                    var k = item as JObject;
                    if (k == null) continue;

                    var relPos = k["icon"]?["rel_position"];
                    if (relPos == null) continue;

                    double relX = relPos["rel_x"]?.Value<double>() ?? 0;
                    double relY = relPos["rel_y"]?.Value<double>() ?? 0;

                    if (relX > rangeLT[0] && relX < rangeRD[0] && relY > rangeLT[1] && relY < rangeRD[1])
                    {
                        string text = k["key"]?["text"]?.Value<string>() ?? "";
                        string virtualKey = k["key"]?["virtual_key"]?.Value<int>().ToString() ?? "";
                        results.Add((relX, relY, text, virtualKey));
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        //通过键值定位按键并从 keymaps 数组中删除
        public static string DeleteKey(string key, string myJson)
        {
            try
            {
                var json = Parse(myJson);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null) return myJson;

                if (!int.TryParse(key, out int vk)) return myJson;

                int index = -1;
                for (int i = 0; i < keymaps.Count; i++)
                {
                    if (keymaps[i] is JObject k && k["key"]?["virtual_key"]?.Value<int>() == vk)
                    {
                        index = i;
                        break;
                    }
                }
                if (index >= 0)
                    keymaps.RemoveAt(index);

                return Serialize(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        //通过键值批量删除键位
        public static string DeleteKeys(string[] repeatKeyValues, string myJson)
        {
            try
            {
                var json = Parse(myJson);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null) return myJson;

                var toRemove = new HashSet<string>(repeatKeyValues);
                for (int i = keymaps.Count - 1; i >= 0; i--)
                {
                    if (keymaps[i] is JObject k)
                    {
                        var vk = k["key"]?["virtual_key"]?.Value<int>().ToString();
                        if (vk != null && toRemove.Contains(vk))
                            keymaps.RemoveAt(i);
                    }
                }
                return Serialize(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        //生成指定类型的键位 JSON 字符串
        public static string CreateKey(string keyType, KeyEventArgs bindKey, string keyX, string keyY, string scan_code)
        {
            if (bindKey == null) return null;
            try
            {
                string keyName = bindKey.KeyCode.ToString();
                string keyValue = bindKey.KeyValue.ToString();

                if (keyType == typeClick)
                {
                    return $"{{\n            \"editor_icon_scale\": 1,\n            \"icon\": {{\n                \"background_color\": \"00000066\",\n                \"description\": \"\",\n                \"radius_correction\": 1,\n                \"rel_position\": {{\n                    \"rel_x\": {keyX},\n                    \"rel_y\": {keyY}\n                }},\n                \"visibility\": true\n            }},\n            \"key\": {{\n                \"device\": \"keyboard\",\n                \"scan_code\": {scan_code},\n                \"text\": \"{keyName}\",\n                \"virtual_key\": {keyValue}\n            }},\n            \"rel_work_position\": {{\n                \"rel_x\": {keyX},\n                \"rel_y\": {keyY}\n            }},\n            \"type\": \"Click\"\n        }}";
                }
                else if (keyType == typeMacro)
                {
                    // 调整宏指牌按键位置：触底向反方向偏移3%，未触底向正方向偏移3%
                    double keyPositionSet = 0.03;
                    if (!double.TryParse(keyX, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dKeyX) ||
                        !double.TryParse(keyY, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dKeyY))
                        return null;
                    double keyPositionX = (dKeyX + keyPositionSet) < 1
                        ? dKeyX + keyPositionSet
                        : dKeyX - keyPositionSet;
                    double keyPositionY = (dKeyY + keyPositionSet) < 1
                        ? dKeyY + keyPositionSet
                        : dKeyY - keyPositionSet;

                    return $"{{\n            \"editor_icon_scale\": 1,\n            \"icon\": {{\n                \"background_color\": \"00000066\",\n                \"description\": \"\",\n                \"radius_correction\": 1,\n                \"rel_position\": {{\n                    \"rel_x\": {keyPositionX},\n                    \"rel_y\": {keyPositionY}\n                }},\n                \"visibility\": true\n            }},\n            \"key\": {{\n                \"device\": \"keyboard\",\n                \"scan_code\": {scan_code},\n                \"text\": \"{keyName}\",\n                \"virtual_key\": {keyValue}\n            }},\n            \"press_actions\": [\n                \"start_loop:until_release\",\n                \"curve_first_point_sleep_time:1\",\n                \"curve_last_point_sleep_time:1\",\n                \"curve_rel:mouse;({dKeyX},{dKeyY})\",\n                \"curve_release\",\n                \"stop_loop\"\n            ],\n            \"rel_work_position\": {{\n                \"rel_x\": {keyPositionX},\n                \"rel_y\": {keyPositionY}\n            }},\n            \"release_actions\": [\n\n            ],\n            \"type\": \"Macro\"\n        }}";
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
            return null;
        }

        //分辨率字典序列化/反序列化（非 JSON 相关）
        public static string ResolutionToString(Dictionary<string, string> resolution)
        {
            try
            {
                if (resolution.Count == 0) return "";
                string resolutionString = "";
                foreach (var item in resolution)
                {
                    string[] value = item.Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (value.Length < 2) continue;
                    resolutionString += item.Key + "," + value[0] + "," + value[1] + "V";
                }
                return resolutionString;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"发生错误：{ex.Message}", ex);
            }
        }

        public static Dictionary<string, string> StringToResolution(string resolutionString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(resolutionString)) return null;
                string[] temp = resolutionString.Split(new[] { "V" }, StringSplitOptions.RemoveEmptyEntries);
                var resolution = new Dictionary<string, string>();
                foreach (var item in temp)
                {
                    string[] temp2 = item.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp2.Length < 3) continue;
                    resolution.Add(temp2[0], temp2[1] + "," + temp2[2]);
                }
                return resolution;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"发生错误：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 坐标转换方法。
        /// 基于"小分辨率放大后居中放入大分辨率"模型：
        ///   将较小分辨率内容等比例放缩后居中放入较大分辨率，多余空间为黑边。
        ///   大分辨率永远不会缩小，只有小分辨率放大。
        /// KeymapToKeymap  方向：源 → 目标。若源是小则正向(+scale+offset)，若源是大则反向(-offset/scale)
        /// MouseToSimulator 方向：窗口(大)→模拟器(小)，反向(-offset/scale)
        /// result[2] = -1 表示转换后坐标超出目标边界（仅大→小方向时可能发生）
        /// </summary>
        public static double[] CalculateCoordinates(int FX, int FY, int SX, int SY, double mX, double mY,
            CoordinateConversionDirection direction)
        {
            double[] result = new double[3] { 0.0, 0.0, 0.0 };
            if (FX <= 0 || FY <= 0 || SX <= 0 || SY <= 0) return result;

            int smallW, smallH, bigW, bigH;
            bool sourceIsSmall;

            if (direction == CoordinateConversionDirection.KeymapToKeymap)
            {
                double ratio1 = (double)FX / FY;
                double ratio2 = (double)SX / SY;
                if (ratio1 <= ratio2) { smallW = FX; smallH = FY; bigW = SX; bigH = SY; sourceIsSmall = true; }
                else                 { smallW = SX; smallH = SY; bigW = FX; bigH = FY; sourceIsSmall = false; }
            }
            else
            {
                smallW = SX; smallH = SY;
                bigW = FX; bigH = FY;
                sourceIsSmall = false;
            }

            double smallAspect = (double)smallW / smallH;
            double bigAspect = (double)bigW / bigH;

            double fittedW, fittedH;
            if (bigAspect >= smallAspect)
            {
                fittedH = bigH;
                fittedW = bigH * smallAspect;
            }
            else
            {
                fittedW = bigW;
                fittedH = bigW / smallAspect;
            }

            double scale = fittedW / smallW;
            double offsetX = (bigW - fittedW) / 2.0;
            double offsetY = (bigH - fittedH) / 2.0;

            if (sourceIsSmall)
            {
                result[0] = mX * scale + offsetX;
                result[1] = mY * scale + offsetY;
                if (result[0] < 0) result[0] = 0.0;
                if (result[1] < 0) result[1] = 0.0;
                if (result[0] > bigW - 1) result[0] = bigW - 1.0;
                if (result[1] > bigH - 1) result[1] = bigH - 1.0;
                result[2] = 0.0;
            }
            else
            {
                result[0] = (mX - offsetX) / scale;
                result[1] = (mY - offsetY) / scale;
                result[2] = 0.0;
                if (result[0] < 0 || result[1] < 0 ||
                    result[0] > smallW - 1 || result[1] > smallH - 1)
                {
                    result[2] = -1.0;
                    if (result[0] < 0) result[0] = 0.0;
                    if (result[1] < 0) result[1] = 0.0;
                    if (result[0] > smallW - 1) result[0] = smallW - 1.0;
                    if (result[1] > smallH - 1) result[1] = smallH - 1.0;
                }
            }

            return result;
        }

        public static double[] CalculateCoordinatesMouseToSimulator(int FX, int FY, int SX, int SY, double mX, double mY)
        {
            return CalculateCoordinates(FX, FY, SX, SY, mX, mY, CoordinateConversionDirection.MouseToSimulator);
        }

        public static double[] CalculateCoordinatesKToCK(int FX, int FY, int SX, int SY, double mX, double mY)
        {
            return CalculateCoordinates(FX, FY, SX, SY, mX, mY, CoordinateConversionDirection.KeymapToKeymap);
        }

        /// <summary>坐标转换方向枚举</summary>
        public enum CoordinateConversionDirection
        {
            /// <summary>鼠标屏幕坐标 → 模拟器内部坐标</summary>
            MouseToSimulator,
            /// <summary>键位坐标 → 另一分辨率的键位坐标</summary>
            KeymapToKeymap
        }
        //获取指定 JSON 文件所有单击按键
        public static PBClass.ClickKeyInfo[] GetClickKeys(string kJson, int X, int Y)
        {
            if (X <= 0 || Y <= 0) return Array.Empty<PBClass.ClickKeyInfo>();
            double KX = X - 1.0;
            double KY = Y - 1.0;

            try
            {
                var json = Parse(kJson);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null) return Array.Empty<PBClass.ClickKeyInfo>();

                var result = new List<PBClass.ClickKeyInfo>();
                foreach (var item in keymaps)
                {
                    var k = item as JObject;
                    if (k == null || k["type"]?.Value<string>() != typeClick) continue;

                    var rwp = k["rel_work_position"];
                    if (rwp == null) continue;

                    result.Add(new PBClass.ClickKeyInfo
                    {
                        KeyText = k["key"]?["text"]?.Value<string>() ?? "",
                        RelX = (rwp["rel_x"]?.Value<double>() ?? 0) * KX,
                        RelY = (rwp["rel_y"]?.Value<double>() ?? 0) * KY
                    });
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"错误：{ex.Message}", ex);
            }
        }

        public static string[] ReadAllKeys(string myJson)
        {
            if (string.IsNullOrEmpty(myJson)) return Array.Empty<string>();
            try
            {
                var json = JObject.Parse(myJson);
                var keymaps = json["keymaps"] as JArray;
                if (keymaps == null) return Array.Empty<string>();
                return keymaps
                    .Select(k => (k as JObject)?["key"]?["text"]?.Value<string>())
                    .Where(v => v != null)
                    .ToArray();
            }
            catch (Exception)
            {
                return Array.Empty<string>();
            }
        }

        public static string GetKeyText(string keyText)
        {
            return keyText ?? "";
        }

        public static string GetKeyText(System.Windows.Forms.CheckedListBox clb)
        {
            return clb?.SelectedItem?.ToString() ?? "";
        }
    }

    public static class StringCompressor
    {
        public static byte[] Compress(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                return outputStream.ToArray();
            }
        }

        public static string CompressToBase64(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            byte[] compressedBytes = Compress(text);
            return Convert.ToBase64String(compressedBytes);
        }
    }

    public static class StringDecompressor
    {
        public static string Decompress(byte[] compressedData)
        {
            if (compressedData == null || compressedData.Length == 0) return string.Empty;
            using (var inputStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzipStream))
                return reader.ReadToEnd();
        }

        public static string DecompressFromBase64(string base64Data)
        {
            byte[] compressedBytes = Convert.FromBase64String(base64Data);
            return Decompress(compressedBytes);
        }
    }
}
