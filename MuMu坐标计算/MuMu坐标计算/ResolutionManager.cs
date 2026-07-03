using System;
using System.Collections.Generic;
using System.Linq;

namespace MuMu坐标计算
{
    /// <summary>
    /// 统一管理分辨率分类、分辨率预设和自定义分辨率持久化。
    /// 消除 Form1 中 4 处重复的分辨率初始化/分类代码。
    /// </summary>
    internal class ResolutionManager
    {
        // === 分辨率类型 ===
        public const string TypeTablet = "1";   // 平板
        public const string TypePhone = "2";    // 手机
        public const string TypeUltraWide = "3"; // 超宽屏
        public const string TypeCustom = "4";   // 自定义

        // === 预设分辨率 ===
        private static readonly Dictionary<string, string> TabletResolutions = new Dictionary<string, string>
        {
            { "2560x1440", "2560,1440" },
            { "1920x1080", "1920,1080" },
            { "1600x900",  "1600,900"  },
            { "1280x720",  "1280,720"  }
        };

        private static readonly Dictionary<string, string> PhoneResolutions = new Dictionary<string, string>
        {
            { "1440x2560", "1440,2560" },
            { "1080x1920", "1080,1920" },
            { "900x1600",  "900,1600"  },
            { "720x1280",  "720,1280"  }
        };

        private static readonly Dictionary<string, string> UltraWideResolutions = new Dictionary<string, string>
        {
            { "3440x1440", "3440,1440" },
            { "3200x1440", "3200,1440" },
            { "2560x1080", "2560,1080" },
            { "2400x1080", "2400,1080" },
            { "1920x864",  "1920,864"  },
            { "1600x720",  "1600,720"  }
        };

        // === 属性 ===
        public IReadOnlyDictionary<string, string> TabletRes => TabletResolutions;
        public IReadOnlyDictionary<string, string> PhoneRes => PhoneResolutions;
        public IReadOnlyDictionary<string, string> UltraWideRes => UltraWideResolutions;

        /// <summary>
        /// 根据 FX x FY 键值判断分辨率类型。
        /// 返回 (typeCode, resolutionDict) 或 (TypeCustom, customDict)。
        /// </summary>
        public (string typeCode, Dictionary<string, string> resolutionDict) ClassifyResolution(
            string fxText, string fyText, string customResolutionString)
        {
            if (string.IsNullOrEmpty(fxText) || string.IsNullOrEmpty(fyText))
                return (TypeCustom, new Dictionary<string, string>());

            string key = $"{fxText}x{fyText}";

            if (TabletResolutions.ContainsKey(key))
                return (TypeTablet, TabletResolutions);

            if (PhoneResolutions.ContainsKey(key))
                return (TypePhone, PhoneResolutions);

            if (UltraWideResolutions.ContainsKey(key))
                return (TypeUltraWide, UltraWideResolutions);

            // 自定义分辨率
            var customDict = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(customResolutionString))
            {
                try { customDict = MuMuJsonEditor.StringToResolution(customResolutionString); }
                catch { customDict = new Dictionary<string, string>(); }
                if (customDict == null) customDict = new Dictionary<string, string>();
            }
            var resultDict = new Dictionary<string, string>(customDict);
            if (!resultDict.ContainsKey(key))
            {
                key = "*" + key;
                resultDict[key] = $"{fxText},{fyText}";
            }
            return (TypeCustom, resultDict);
        }

        /// <summary>根据类型代码获取分辨率字典</summary>
        public Dictionary<string, string> GetResolutionDictByType(string typeCode, string customResolutionString)
        {
            switch (typeCode)
            {
                case TypeTablet: return new Dictionary<string, string>(TabletResolutions);
                case TypePhone: return new Dictionary<string, string>(PhoneResolutions);
                case TypeUltraWide: return new Dictionary<string, string>(UltraWideResolutions);
                case TypeCustom:
                {
                    if (string.IsNullOrWhiteSpace(customResolutionString))
                        return new Dictionary<string, string>();
                    try
                    {
                        return MuMuJsonEditor.StringToResolution(customResolutionString) ?? new Dictionary<string, string>();
                    }
                    catch
                    {
                        return new Dictionary<string, string>();
                    }
                }
                default: return new Dictionary<string, string>();
            }
        }

        /// <summary>获取分辨率字典中匹配 key 的项</summary>
        public static KeyValuePair<string, string> FindItem(Dictionary<string, string> dict, string key)
        {
            if (dict == null || string.IsNullOrEmpty(key)) return default;
            return dict.FirstOrDefault(i => i.Key == key);
        }
    }
}
