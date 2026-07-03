using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MuMu坐标计算
{
    internal class ConfigManager
    {
        private readonly Dictionary<string, object> _values;
        private readonly object _saveLock;
        private readonly string _primaryConfigPath;
        private readonly string _fallbackConfigPath;
        private string _activeConfigPath;
        private bool _warnedAboutFallback;
        private readonly bool _isDesignTime;

        public ConfigManager()
        {
            _values = new Dictionary<string, object>();
            _saveLock = new object();
            _primaryConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            _fallbackConfigPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MuMu摸点小助手",
                "config.json");
            _activeConfigPath = null;
            _warnedAboutFallback = false;

            _isDesignTime = IsDesignTime();

            InitializeDefaults();

            if (_isDesignTime)
                return;

            try
            {

                if (!TryLoadConfig())
                {
                    string existing = FindExistingConfigPath();
                    if (existing != null)
                    {
                        try
                        {
                            string backupPath = existing + ".corrupted_backup";
                            File.Copy(existing, backupPath, true);
                            System.Diagnostics.Debug.WriteLine("ConfigManager: 配置文件无法读取，已备份到: " + backupPath);
                        }
                        catch { }
                    }
                    else
                    {
                        SaveInternal();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ConfigManager 构造函数失败: " + ex.Message);
            }
        }

        private static bool IsDesignTime()
        {
            try
            {
                return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
            catch
            {
                return false;
            }
        }

        private void InitializeDefaults()
        {
            _values["FX"] = "1280";
            _values["FY"] = "720";
            _values["FindKey"] = "D";
            _values["ResetKey"] = "F";
            _values["JsonFolderPath"] = "";
            _values["Resolution4String"] = "";
            _values["AdbPort"] = "16384";
            _values["AdbPath"] = "";
            _values["AdbPortsHistory"] = "16384";
        }

        private bool TryLoadConfig()
        {
            string path = FindExistingConfigPath();
            if (path == null)
                return false;

            try
            {
                string json = File.ReadAllText(path, new UTF8Encoding(false));
                var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (config != null)
                {
                    foreach (var kv in config)
                    {
                        _values[kv.Key] = kv.Value;
                    }
                }
                _activeConfigPath = path;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TryLoadConfig 失败, 路径=" + path + ", 错误=" + ex.Message);
                return false;
            }
        }

        private string FindExistingConfigPath()
        {
            if (_activeConfigPath != null && File.Exists(_activeConfigPath))
                return _activeConfigPath;
            if (File.Exists(_primaryConfigPath))
                return _primaryConfigPath;
            if (File.Exists(_fallbackConfigPath))
                return _fallbackConfigPath;
            return null;
        }

        private bool SaveInternal()
        {
            if (_isDesignTime)
                return false;

            lock (_saveLock)
            {
                try
                {
                    var data = new Dictionary<string, object>();
                    data["ConfigVersion"] = 1;
                    data["FX"] = _values["FX"];
                    data["FY"] = _values["FY"];
                    data["FindKey"] = _values["FindKey"];
                    data["ResetKey"] = _values["ResetKey"];
                    data["JsonFolderPath"] = _values["JsonFolderPath"];
                    data["Resolution4String"] = _values["Resolution4String"];
                    data["AdbPort"] = _values["AdbPort"];
                    data["AdbPath"] = _values["AdbPath"];
                    data["AdbPortsHistory"] = _values["AdbPortsHistory"];

                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    string targetPath = _activeConfigPath ?? _primaryConfigPath;

                    if (!TryAtomicWrite(targetPath, json))
                    {
                        if (targetPath != _fallbackConfigPath && TryAtomicWrite(_fallbackConfigPath, json))
                        {
                            _activeConfigPath = _fallbackConfigPath;
                            if (!_warnedAboutFallback)
                            {
                                Debug.WriteLine(
                                    "config.json 写入程序目录失败，已降级保存到: " + _fallbackConfigPath);
                                _warnedAboutFallback = true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        _activeConfigPath = targetPath;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("config.json 保存失败: " + ex.Message);
                    return false;
                }
            }
        }

        private static bool TryAtomicWrite(string finalPath, string json)
        {
            try
            {
                string dir = Path.GetDirectoryName(finalPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string tmpPath = finalPath + ".tmp";
                File.WriteAllText(tmpPath, json, new UTF8Encoding(false));

                try
                {
                    if (File.Exists(finalPath))
                        File.Replace(tmpPath, finalPath, null);
                    else
                        File.Move(tmpPath, finalPath);
                }
                catch (IOException)
                {
                    File.Copy(tmpPath, finalPath, true);
                    File.Delete(tmpPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TryAtomicWrite 失败: " + ex.Message);
                return false;
            }
        }

        private string GetString(string key)
        {
            object val;
            if (_values.TryGetValue(key, out val))
                return val != null ? val.ToString() : "";
            return "";
        }

        private void SetString(string key, string value)
        {
            _values[key] = value;
            SaveInternal();
        }

        private Keys GetKeys(string key)
        {
            object val;
            if (_values.TryGetValue(key, out val) && val is string)
            {
                string s = (string)val;
                Keys parsed;
                if (Enum.TryParse(s, out parsed))
                    return parsed;
            }
            if (key == "ResetKey")
                return Keys.F;
            return Keys.D;
        }

        private void SetKeys(string key, Keys value)
        {
            _values[key] = value.ToString();
            SaveInternal();
        }

        public bool Save()
        {
            return SaveInternal();
        }

        public bool Reload()
        {
            if (_isDesignTime)
                return false;

            try
            {
                string path = FindExistingConfigPath();
                if (path == null)
                    return false;

                string json = File.ReadAllText(path, new UTF8Encoding(false));
                var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (config == null)
                    return false;

                foreach (var kv in config)
                    _values[kv.Key] = kv.Value;

                _activeConfigPath = path;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Reload 失败: " + ex.Message);
                return false;
            }
        }

        public string FX
        {
            get { return GetString("FX"); }
            set { SetString("FX", value); }
        }

        public string FY
        {
            get { return GetString("FY"); }
            set { SetString("FY", value); }
        }

        public Keys FindKey
        {
            get { return GetKeys("FindKey"); }
            set { SetKeys("FindKey", value); }
        }

        public Keys ResetKey
        {
            get { return GetKeys("ResetKey"); }
            set { SetKeys("ResetKey", value); }
        }

        public string JsonFolderPath
        {
            get { return GetString("JsonFolderPath"); }
            set { SetString("JsonFolderPath", value); }
        }

        public string Resolution4String
        {
            get { return GetString("Resolution4String"); }
            set { SetString("Resolution4String", value); }
        }

        public string AdbPort
        {
            get { return GetString("AdbPort"); }
            set { SetString("AdbPort", value); }
        }

        public string AdbPath
        {
            get { return GetString("AdbPath"); }
            set { SetString("AdbPath", value); }
        }

        public string AdbPortsHistory
        {
            get { return GetString("AdbPortsHistory"); }
            set { SetString("AdbPortsHistory", value); }
        }

        public void LoadIntoKeyboardHandler(KeyboardBindingHandler handler)
        {
            handler.FindKey = FindKey;
            handler.ResetKey = ResetKey;
        }

        public void SaveFromKeyboardHandler(KeyboardBindingHandler handler)
        {
            FindKey = handler.FindKey;
            ResetKey = handler.ResetKey;
        }
    }
}