using System;
using System.Collections.Generic;

namespace MuMu坐标计算
{
    internal class BackupManager
    {
        private readonly List<string> _snapshots = new List<string>();
        private int _currentIndex;
        private readonly object _lock = new object();
        private const int MaxSnapshots = 50;

        public bool CanUndo
        {
            get { return _currentIndex > 0 && _snapshots.Count > 1; }
        }

        public bool CanRedo
        {
            get { return _currentIndex < _snapshots.Count - 1; }
        }

        public bool HasHistory
        {
            get { return _snapshots.Count > 0; }
        }

        public void RecordInitial(string jsonContent)
        {
            lock (_lock)
            {
                _snapshots.Clear();
                _currentIndex = 0;
                _snapshots.Add(jsonContent);
#if DEBUG
                System.Diagnostics.Debug.WriteLine("BackupManager.RecordInitial: 快照数={0}, 索引={1}, JSON长度={2}",
                    _snapshots.Count, _currentIndex, jsonContent.Length);
#endif
            }
        }

        public void RecordChange(string jsonContent)
        {
            lock (_lock)
            {
                if (_currentIndex == _snapshots.Count - 1)
                {
                    _snapshots.Add(jsonContent);
                }
                else
                {
                    int startIndex = _currentIndex + 1;
                    int count = _snapshots.Count - startIndex;
                    if (count > 0)
                        _snapshots.RemoveRange(startIndex, count);
                    _snapshots.Add(jsonContent);
                }
                while (_snapshots.Count > MaxSnapshots)
                    _snapshots.RemoveAt(0);
                _currentIndex = _snapshots.Count - 1;
#if DEBUG
                System.Diagnostics.Debug.WriteLine("BackupManager.RecordChange: 快照数={0}, 索引={1}, JSON长度={2}",
                    _snapshots.Count, _currentIndex, jsonContent.Length);
#endif
            }
        }

        public string Undo()
        {
            lock (_lock)
            {
                if (!CanUndo) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("BackupManager.Undo: 无法撤销 (CanUndo=false, 快照数={0}, 索引={1})", _snapshots.Count, _currentIndex);
#endif
                    return null;
                }
                _currentIndex--;
                var result = _snapshots[_currentIndex];
#if DEBUG
                System.Diagnostics.Debug.WriteLine("BackupManager.Undo: 撤销到索引={0}/{1}, JSON长度={2}", _currentIndex, _snapshots.Count, result.Length);
#endif
                return result;
            }
        }

        public string Redo()
        {
            lock (_lock)
            {
                if (!CanRedo) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("BackupManager.Redo: 无法重做 (CanRedo=false, 快照数={0}, 索引={1})", _snapshots.Count, _currentIndex);
#endif
                    return null;
                }
                _currentIndex++;
                var result = _snapshots[_currentIndex];
#if DEBUG
                System.Diagnostics.Debug.WriteLine("BackupManager.Redo: 重做到索引={0}/{1}, JSON长度={2}", _currentIndex, _snapshots.Count, result.Length);
#endif
                return result;
            }
        }

        public void UpdateButtonStates(
            System.Windows.Forms.Button undoButton,
            System.Windows.Forms.Button redoButton)
        {
            undoButton.Enabled = CanUndo;
            redoButton.Enabled = CanRedo;
        }
    }
}
