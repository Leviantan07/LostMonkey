using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostMonkey.Core.Progression
{
    /// <summary>
    /// PlayerPrefs-backed persistence for <see cref="ProgressData"/>, stored as a
    /// small JSON blob under a single key.
    /// </summary>
    public class PlayerPrefsSaveStore : ISaveStore
    {
        private const string Key = "lostmonkey.progress.v1";

        [Serializable]
        private class Dto
        {
            public int highestUnlockedLevel = 1;
            public List<int> levels = new List<int>();
            public List<int> bestBananas = new List<int>();
        }

        public ProgressData Load(int totalLevels)
        {
            var data = new ProgressData();
            string json = PlayerPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return data;
            }

            Dto dto;
            try
            {
                dto = JsonUtility.FromJson<Dto>(json);
            }
            catch (Exception)
            {
                Debug.LogWarning("[LostMonkey] Corrupt save data — starting fresh.");
                return data;
            }

            if (dto == null)
            {
                return data;
            }

            var best = new Dictionary<int, int>();
            int count = Mathf.Min(dto.levels?.Count ?? 0, dto.bestBananas?.Count ?? 0);
            for (int i = 0; i < count; i++)
            {
                best[dto.levels[i]] = dto.bestBananas[i];
            }
            data.LoadFrom(dto.highestUnlockedLevel, best);
            return data;
        }

        public void Save(ProgressData data)
        {
            var dto = new Dto { highestUnlockedLevel = data.HighestUnlockedLevel };
            foreach (KeyValuePair<int, int> entry in data.BestBananasSnapshot)
            {
                dto.levels.Add(entry.Key);
                dto.bestBananas.Add(entry.Value);
            }
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(dto));
            PlayerPrefs.Save();
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(Key);
            PlayerPrefs.Save();
        }
    }
}
