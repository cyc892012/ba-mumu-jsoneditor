using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MuMu坐标计算
{
    internal class IndexBackupInfo
    {
        public string Timestamp { get; set; }
        public string IndexFileName { get; set; }
        public int SchemeCount { get; set; }
        public List<string> Schemes { get; set; }
    }

    internal class IndexFileBackupManager
    {
        private readonly string _baseDirectory;
        private readonly Dictionary<string, string> _packageNameTypes;

        public string IndexDirectory { get; set; }

        private static readonly string[] KnownServerKeys = { "官服", "B服", "日服", "国际服" };

        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);

        public IndexFileBackupManager(string baseDirectory, Dictionary<string, string> packageNameTypes)
        {
            _baseDirectory = baseDirectory;
            _packageNameTypes = new Dictionary<string, string>(packageNameTypes);
            IndexDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @"AppData\Roaming\Netease\MuMuPlayer\data\keymapConfig");
        }

        public string BackupDirectory
        {
            get { return Path.Combine(_baseDirectory, "data", "index_backups"); }
        }

        public string ResolveIndexFilePath(string schemeFilePath)
        {
            if (string.IsNullOrEmpty(schemeFilePath))
                return null;

            string dir = Path.GetDirectoryName(schemeFilePath);
            string fileName = Path.GetFileName(schemeFilePath);
            if (string.IsNullOrEmpty(dir) || string.IsNullOrEmpty(fileName))
                return null;

            var sortedKeys = KnownServerKeys
                .Select(k => new { Key = k, Prefix = _packageNameTypes.ContainsKey(k) ? _packageNameTypes[k] : null })
                .Where(x => x.Prefix != null)
                .OrderByDescending(x => x.Prefix.Length)
                .ToList();

            foreach (var item in sortedKeys)
            {
                string prefix = item.Prefix;
                string prefixWithoutDash = prefix.TrimEnd('-');
                if (fileName.StartsWith(prefixWithoutDash, StringComparison.OrdinalIgnoreCase))
                {
                    return Path.Combine(dir, prefixWithoutDash + ".json");
                }
            }

            return null;
        }

        public static bool ContainsScheme(string indexContent, string schemeFilePath)
        {
            try
            {
                JObject json = JObject.Parse(indexContent);
                JArray config = json["Config"] as JArray;
                if (config == null) return false;

                string normalizedPath = schemeFilePath.Replace('/', '\\');
                
                foreach (var item in config)
                {
                    var entry = item as JObject;
                    if (entry == null) continue;
                    foreach (var kv in entry)
                    {
                        string path = kv.Value?["path"]?.Value<string>();
                        if (path != null)
                        {
                            string normalized = path.Replace('/', '\\');
                            if (string.Equals(normalized, normalizedPath, StringComparison.OrdinalIgnoreCase))
                                return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public class OrphanSchemeInfo
        {
            public string SchemeName { get; set; }
            public string FilePath { get; set; }
        }

        public List<OrphanSchemeInfo> ScanOrphanSchemes(string indexFileName)
        {
            var result = new List<OrphanSchemeInfo>();
            if (string.IsNullOrEmpty(indexFileName))
                return result;

            string indexFilePath = Path.Combine(IndexDirectory, indexFileName);
            if (!File.Exists(indexFilePath))
                return result;

            string prefix = Path.GetFileNameWithoutExtension(indexFileName);
            JObject indexJson;
            try
            {
                string content = RetryIO(() => File.ReadAllText(indexFilePath, Encoding.UTF8));
                indexJson = JObject.Parse(content);
            }
            catch { return result; }

            var existingSchemes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            JArray config = indexJson["Config"] as JArray;
            if (config != null)
            {
                foreach (var item in config)
                {
                    var entry = item as JObject;
                    if (entry == null) continue;
                    foreach (var kv in entry)
                    {
                        existingSchemes.Add(kv.Key);
                    }
                }
            }

            if (!Directory.Exists(IndexDirectory))
                return result;

            var allFiles = Directory.GetFiles(IndexDirectory, prefix + "-*.json", SearchOption.TopDirectoryOnly);
            foreach (string file in allFiles)
            {
                string fileName = Path.GetFileName(file);
                int sepIndex = fileName.IndexOf('-', prefix.Length);
                if (sepIndex < 0) continue;
                string schemeName = fileName.Substring(sepIndex + 1);
                if (schemeName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    schemeName = schemeName.Substring(0, schemeName.Length - 5);

                if (!existingSchemes.Contains(schemeName))
                {
                    result.Add(new OrphanSchemeInfo
                    {
                        SchemeName = schemeName,
                        FilePath = file
                    });
                }
            }

            return result;
        }

        public int AddSchemesToIndex(string indexFileName, List<OrphanSchemeInfo> schemes)
        {
            if (schemes == null || schemes.Count == 0 || string.IsNullOrEmpty(indexFileName))
                return 0;

            string indexFilePath = Path.Combine(IndexDirectory, indexFileName);
            if (!File.Exists(indexFilePath))
                return 0;

            string content = RetryIO(() => File.ReadAllText(indexFilePath, Encoding.UTF8));
            JObject indexJson;
            try { indexJson = JObject.Parse(content); }
            catch { return 0; }

            JArray config = indexJson["Config"] as JArray;
            if (config == null)
            {
                config = new JArray();
                indexJson["Config"] = config;
            }

            int added = 0;
            foreach (var scheme in schemes)
            {
                string path = scheme.FilePath.Replace('\\', '/');
                var schemeEntry = new JObject();
                schemeEntry["configType"] = "Customer";
                schemeEntry["copiedNum"] = 0;
                schemeEntry["moveTop"] = false;
                schemeEntry["path"] = path;
                schemeEntry["status"] = "";
                schemeEntry["transparency"] = 100;
                schemeEntry["type"] = 0;
                schemeEntry["xSensitivity"] = 1;
                schemeEntry["ySensitivity"] = 1;

                var container = new JObject();
                container[scheme.SchemeName] = schemeEntry;

                config.Add(container);
                added++;
            }

            if (added > 0)
            {
                string newContent = SerializeJson(indexJson);
                string tmpPath = indexFilePath + ".tmp";
                RetryIO(() => { AtomicWriteAllText(tmpPath, indexFilePath, newContent); return 0; });
            }

            return added;
        }

        public bool BackupIndex(string schemeFilePath)
        {
            if (string.IsNullOrEmpty(schemeFilePath))
                return false;

            try
            {
                string indexFilePath = ResolveIndexFilePath(schemeFilePath);
                if (indexFilePath == null || !File.Exists(indexFilePath))
                    return false;

                string indexContent = RetryIO(() => File.ReadAllText(indexFilePath, Encoding.UTF8));

                try
                {
                    JObject.Parse(indexContent);
                }
                catch
                {
                    return false;
                }

                if (!ContainsScheme(indexContent, schemeFilePath))
                    return false;

                string newHash;
                try
                {
                    newHash = ComputeSha256(indexContent);
                }
                catch
                {
                    var fileInfo = new FileInfo(indexFilePath);
                    newHash = fileInfo.Length + "_" + fileInfo.LastWriteTimeUtc.Ticks;
                }

                string lastHash = GetLastBackupHash(Path.GetFileName(indexFilePath));
                if (lastHash != null && lastHash == newHash)
                    return false;

                string indexFileName = Path.GetFileName(indexFilePath);
                string indexName = Path.GetFileNameWithoutExtension(indexFileName);
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
                string backupDir = Path.Combine(BackupDirectory, indexName, timestamp);

                if (Directory.Exists(backupDir))
                {
                    int suffix = 1;
                    while (Directory.Exists(backupDir + "_" + suffix))
                        suffix++;
                    backupDir = backupDir + "_" + suffix;
                }

                Directory.CreateDirectory(backupDir);
                string tmpPath = Path.Combine(backupDir, "index.json.tmp");
                string finalPath = Path.Combine(backupDir, "index.json");

                RetryIO(() => { AtomicWriteAllText(tmpPath, finalPath, indexContent); return 0; });
                return true;
            }
            catch (Exception ex)
            {
                LogService.Error("IndexFileBackupManager", ex, "BackupIndex 失败, schemeFilePath=" + (schemeFilePath ?? "(null)"));
                return false;
            }
        }

        public List<IndexBackupInfo> GetBackups(string indexFileName)
        {
            var result = new List<IndexBackupInfo>();
            string indexName = Path.GetFileNameWithoutExtension(indexFileName);
            string backupBaseDir = Path.Combine(BackupDirectory, indexName);

            if (!Directory.Exists(backupBaseDir))
                return result;

            string[] dirs;
            try
            {
                dirs = Directory.GetDirectories(backupBaseDir);
            }
            catch
            {
                return result;
            }

            foreach (var dir in dirs.OrderByDescending(d => d))
            {
                string indexJsonPath = Path.Combine(dir, "index.json");
                if (!File.Exists(indexJsonPath))
                    continue;

                try
                {
                    string content = RetryIO(() => File.ReadAllText(indexJsonPath, Encoding.UTF8));

                    JObject json = JObject.Parse(content);
                    JArray config = json["Config"] as JArray;
                    var schemes = new List<string>();

                    if (config != null)
                    {
                        foreach (var item in config)
                        {
                            var entry = item as JObject;
                            if (entry == null) continue;
                            foreach (var kv in entry)
                            {
                                schemes.Add(kv.Key);
                            }
                        }
                    }

                    string ts = Path.GetFileName(dir);
                    result.Add(new IndexBackupInfo
                    {
                        Timestamp = ts,
                        IndexFileName = indexFileName,
                        SchemeCount = schemes.Count,
                        Schemes = schemes
                    });
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public string GetLastBackupHash(string indexFileName)
        {
            string indexName = Path.GetFileNameWithoutExtension(indexFileName);
            string backupBaseDir = Path.Combine(BackupDirectory, indexName);

            if (!Directory.Exists(backupBaseDir))
                return null;

            string[] dirs;
            try
            {
                dirs = Directory.GetDirectories(backupBaseDir);
            }
            catch
            {
                return null;
            }

            string latestDir = null;
            foreach (var dir in dirs)
            {
                string indexJsonPath = Path.Combine(dir, "index.json");
                if (!File.Exists(indexJsonPath))
                    continue;

                if (latestDir == null || string.CompareOrdinal(dir, latestDir) > 0)
                    latestDir = dir;
            }

            if (latestDir != null)
            {
                try
                {
                    string content = RetryIO(() => File.ReadAllText(Path.Combine(latestDir, "index.json"), Encoding.UTF8));
                    return ComputeSha256(content);
                }
                catch
                {
                }
            }

            return null;
        }

        public string GetLastBackupTimestamp(string indexFileName)
        {
            string indexName = Path.GetFileNameWithoutExtension(indexFileName);
            string backupBaseDir = Path.Combine(BackupDirectory, indexName);

            if (!Directory.Exists(backupBaseDir))
                return null;

            string[] dirs;
            try
            {
                dirs = Directory.GetDirectories(backupBaseDir);
            }
            catch
            {
                return null;
            }

            foreach (var dir in dirs.OrderByDescending(d => d))
            {
                string indexJsonPath = Path.Combine(dir, "index.json");
                if (File.Exists(indexJsonPath))
                    return Path.GetFileName(dir);
            }

            return null;
        }

        public List<string> GetAvailableIndexFiles()
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (Directory.Exists(BackupDirectory))
            {
                string[] subDirs;
                try { subDirs = Directory.GetDirectories(BackupDirectory); }
                catch { subDirs = new string[0]; }

                foreach (var subDir in subDirs)
                {
                    string indexName = Path.GetFileName(subDir);
                    try
                    {
                        string[] innerDirs = Directory.GetDirectories(subDir);
                        foreach (var inner in innerDirs)
                        {
                            if (File.Exists(Path.Combine(inner, "index.json")))
                            {
                                result.Add(indexName + ".json");
                                break;
                            }
                        }
                    }
                    catch { continue; }
                }
            }

            if (Directory.Exists(IndexDirectory))
            {
                foreach (var kv in _packageNameTypes)
                {
                    if (kv.Value == "other" || kv.Value == "萌新666sssaaa") continue;
                    string prefix = kv.Value.TrimEnd('-');
                    string indexPath = Path.Combine(IndexDirectory, prefix + ".json");
                    if (File.Exists(indexPath))
                        result.Add(prefix + ".json");
                }
            }

            return result.OrderBy(f => f).ToList();
        }

        public bool BackupIndexDirect(string indexFileName)
        {
            if (string.IsNullOrEmpty(indexFileName))
                return false;

            try
            {
                string indexFilePath = Path.Combine(IndexDirectory, indexFileName);
                if (!File.Exists(indexFilePath))
                    return false;

                string indexContent = RetryIO(() => File.ReadAllText(indexFilePath, Encoding.UTF8));

                try { JObject.Parse(indexContent); }
                catch { return false; }

                string newHash;
                try { newHash = ComputeSha256(indexContent); }
                catch
                {
                    var fi = new FileInfo(indexFilePath);
                    newHash = fi.Length + "_" + fi.LastWriteTimeUtc.Ticks;
                }

                string lastHash = GetLastBackupHash(indexFileName);
                if (lastHash != null && lastHash == newHash)
                    return false;

                string indexName = Path.GetFileNameWithoutExtension(indexFileName);
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
                string backupDir = Path.Combine(BackupDirectory, indexName, timestamp);

                if (Directory.Exists(backupDir))
                {
                    int suffix = 1;
                    while (Directory.Exists(backupDir + "_" + suffix))
                        suffix++;
                    backupDir = backupDir + "_" + suffix;
                }

                Directory.CreateDirectory(backupDir);
                string tmpPath = Path.Combine(backupDir, "index.json.tmp");
                string finalPath = Path.Combine(backupDir, "index.json");
                RetryIO(() => { AtomicWriteAllText(tmpPath, finalPath, indexContent); return 0; });
                return true;
            }
            catch (Exception ex)
            {
                LogService.Error("IndexFileBackupManager", ex, "BackupIndexDirect 失败, indexFileName=" + (indexFileName ?? "(null)"));
                return false;
            }
        }

        public List<string> GetSchemesInBackup(string indexFileName, string timestamp)
        {
            var result = new List<string>();
            string indexName = Path.GetFileNameWithoutExtension(indexFileName);
            string backupPath = Path.Combine(BackupDirectory, indexName, timestamp, "index.json");

            if (!File.Exists(backupPath))
                return result;

            try
            {
                string content = RetryIO(() => File.ReadAllText(backupPath, Encoding.UTF8));
                if (content == null) return result;

                JObject json = JObject.Parse(content);
                JArray config = json["Config"] as JArray;
                if (config != null)
                {
                    foreach (var item in config)
                    {
                        var entry = item as JObject;
                        if (entry == null) continue;
                        foreach (var kv in entry)
                        {
                            result.Add(kv.Key);
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        public bool RestoreBackup(string indexFileName, string timestamp)
        {
            string indexDir = !string.IsNullOrEmpty(IndexDirectory) ? IndexDirectory : Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @"AppData\Roaming\Netease\MuMuPlayer\data\keymapConfig");
            string indexFilePath = Path.Combine(indexDir, indexFileName);

            string indexName = Path.GetFileNameWithoutExtension(indexFileName);
            string backupFilePath = Path.Combine(BackupDirectory, indexName, timestamp, "index.json");

            if (!File.Exists(backupFilePath))
                return false;

            string content = RetryIO(() => File.ReadAllText(backupFilePath, Encoding.UTF8));

            string tmpPath = indexFilePath + ".tmp";
            try
            {
                RetryIO(() => { AtomicWriteAllText(tmpPath, indexFilePath, content); return 0; });
                return true;
            }
            catch (Exception ex)
            {
                try { File.Delete(tmpPath); } catch { }
                System.Windows.Forms.MessageBox.Show("还原失败：" + ex.Message,
                    "还原失败", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
        }

        public void CleanupOldBackups(int maxKeep)
        {
            if (maxKeep < 1) maxKeep = 1;
            if (!Directory.Exists(BackupDirectory))
                return;

            string[] subDirs;
            try
            {
                subDirs = Directory.GetDirectories(BackupDirectory);
            }
            catch
            {
                return;
            }

            foreach (var subDir in subDirs)
            {
                string[] backupDirs;
                try
                {
                    backupDirs = Directory.GetDirectories(subDir);
                }
                catch
                {
                    continue;
                }

                foreach (var dir in backupDirs.OrderBy(d => d))
                {
                    string tmpFile = Path.Combine(dir, "index.json.tmp");
                    string finalFile = Path.Combine(dir, "index.json");

                    if (File.Exists(tmpFile) && !File.Exists(finalFile))
                    {
                        try { Directory.Delete(dir, true); } catch { }
                        continue;
                    }

                    if (!File.Exists(finalFile))
                    {
                        try { Directory.Delete(dir, true); } catch { }
                    }
                }

                try
                {
                    backupDirs = Directory.GetDirectories(subDir);
                }
                catch
                {
                    continue;
                }
                var orderedDirs = backupDirs
                    .Where(d => File.Exists(Path.Combine(d, "index.json")))
                    .OrderByDescending(d => d)
                    .ToList();

                for (int i = maxKeep; i < orderedDirs.Count; i++)
                {
                    try { Directory.Delete(orderedDirs[i], true); } catch { }
                }
            }
        }

        public static string ComputeSha256(string content)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static T RetryIO<T>(Func<T> action, int maxRetries = 3, int delayMs = 500)
        {
            Exception lastException = null;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return action();
                }
                catch (IOException ex)
                {
                    lastException = ex;
                }
                catch (UnauthorizedAccessException ex)
                {
                    lastException = ex;
                }

                if (i < maxRetries - 1)
                    Thread.Sleep(delayMs);
            }
            if (lastException != null) throw lastException;
            throw new InvalidOperationException("RetryIO 失败：maxRetries 为 0 或操作未抛出异常。");
        }

        public static string SerializeJson(JObject json)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb) { NewLine = "\n" })
            using (var jtw = new JsonTextWriter(sw))
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

        public static void AtomicWriteAllText(string tmpPath, string finalPath, string content)
        {
            File.WriteAllText(tmpPath, content, Utf8NoBom);
            if (File.Exists(finalPath))
                File.Replace(tmpPath, finalPath, null);
            else
                File.Move(tmpPath, finalPath);
        }

        public void CheckAndNotifyDamage(string schemeFilePath, string schemeDisplayPath,
            ref System.Collections.Generic.HashSet<string> notifiedSet,
            System.Windows.Forms.ToolStripStatusLabel statusBox, ref bool suppressNext)
        {
            if (string.IsNullOrEmpty(schemeFilePath) || !System.IO.File.Exists(schemeFilePath))
                return;

            string indexFilePath = ResolveIndexFilePath(schemeFilePath);
            if (indexFilePath == null || !System.IO.File.Exists(indexFilePath))
                return;

            try
            {
                string indexContent = RetryIO(() => System.IO.File.ReadAllText(indexFilePath, Encoding.UTF8));
                bool contains = ContainsScheme(indexContent, schemeFilePath);
                string indexName = System.IO.Path.GetFileName(indexFilePath);

                if (!contains)
                {
                    if (notifiedSet == null)
                        notifiedSet = new System.Collections.Generic.HashSet<string>();
                    if (!notifiedSet.Contains(indexName))
                    {
                        notifiedSet.Add(indexName);
                        suppressNext = true;
                        statusBox.Text = "警告：索引文件可能已损坏，方案可能未写入索引！";
                    }
                }
            }
            catch { }
        }
    }
}
