using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PowerPrank3D.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private const string DEFAULT_LOCALE = "en";
        private const string FALLBACK_LOCALE = "en";

        private readonly Dictionary<string, Dictionary<string, string>> _table = new Dictionary<string, Dictionary<string, string>>();
        private readonly HashSet<string> _supportedLocales = new HashSet<string>
        {
            "zh-CN", "zh-TW", "en", "ja", "ko", "de", "fr", "es", "pt-BR", "ru"
        };

        public string CurrentLocale { get; private set; } = DEFAULT_LOCALE;

        public event Action<string> OnLocaleChanged;

        private void Awake()
        {
            LoadCsv();
        }

        public void SetLocale(string locale)
        {
            if (string.IsNullOrEmpty(locale) || !_supportedLocales.Contains(locale))
            {
                CurrentLocale = DEFAULT_LOCALE;
            }
            else
            {
                CurrentLocale = locale;
            }

            OnLocaleChanged?.Invoke(CurrentLocale);
        }

        public string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            if (!_table.TryGetValue(key, out var row))
            {
                return key;
            }

            if (TryGetLocaleValue(row, CurrentLocale, out var value))
            {
                return value;
            }

            if (TryGetLocaleValue(row, FALLBACK_LOCALE, out value))
            {
                return value;
            }

            return key;
        }

        private bool TryGetLocaleValue(Dictionary<string, string> row, string locale, out string value)
        {
            value = string.Empty;
            return row.TryGetValue(locale, out value) && !string.IsNullOrEmpty(value);
        }

        private void LoadCsv()
        {
            _table.Clear();

            var csvPath = Path.Combine(Application.dataPath, "Localization/localization_table.csv");
            if (!File.Exists(csvPath))
            {
                Debug.LogError($"Localization CSV not found: {csvPath}");
                return;
            }

            var lines = File.ReadAllLines(csvPath);
            if (lines.Length <= 1)
            {
                Debug.LogWarning("Localization CSV has no data rows.");
                return;
            }

            var headers = lines[0].Split(',');
            for (var i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var cols = lines[i].Split(',');
                if (cols.Length != headers.Length)
                {
                    Debug.LogWarning($"Skip invalid localization row at line {i + 1}: {lines[i]}");
                    continue;
                }

                var key = cols[0].Trim();
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                var row = new Dictionary<string, string>();
                for (var c = 0; c < headers.Length; c++)
                {
                    row[headers[c].Trim()] = cols[c].Trim();
                }

                _table[key] = row;
            }
        }
    }
}
