using UnityEngine;

namespace PowerPrank3D.Gameplay
{
    [System.Serializable]
    public class EnemyRosterEntry
    {
        public string entryId = "meeting_tyrant";
        public string displayName = "Meeting Tyrant";
        public EnemyPresetData preset;
        public string recommendedSlotId = "enemy_slot_01";
        public bool enabled = true;
    }

    [CreateAssetMenu(
        fileName = "enemy_roster_data",
        menuName = "PowerPrank3D/Enemy/Enemy Roster Data")]
    public class EnemyRosterData : ScriptableObject
    {
        [Header("Identity")]
        public string rosterId = "main_enemy_roster";
        public string displayName = "Main Enemy Roster";

        [Header("Entries")]
        public EnemyRosterEntry[] entries;

        public EnemyRosterEntry GetEntryById(string entryId)
        {
            if (string.IsNullOrWhiteSpace(entryId) || entries == null)
            {
                return null;
            }

            for (int i = 0; i < entries.Length; i++)
            {
                EnemyRosterEntry entry = entries[i];
                if (entry == null)
                {
                    continue;
                }

                if (string.Equals(entry.entryId, entryId, System.StringComparison.Ordinal))
                {
                    return entry;
                }
            }

            return null;
        }
    }
}
