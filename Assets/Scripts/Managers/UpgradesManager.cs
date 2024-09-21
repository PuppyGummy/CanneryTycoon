using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [System.Serializable]
    public struct UpgradeValue
    {
        public int Cost;
        public float ProcessingTime;
        public Sprite Icon;
        public List<Sprite> MachineSprites;
    }

    [System.Serializable]
    public class MachineUpgradeInfo
    {
        public MachinesType MachineType;
        [HideInInspector] public int CurrentLevel = -1;
        [SerializeField] private List<UpgradeValue> m_UpgradesValues;

        public void Upgrade()
        {
            CurrentLevel++;

            if(CurrentLevel > MAX_UPGRADE_LEVEL - 1)
            {
                CurrentLevel = MAX_UPGRADE_LEVEL - 1;
            }
        }

        public float GetProcessingTime()
        {
            return m_UpgradesValues[CurrentLevel].ProcessingTime;
        }

        public int GetCost()
        {
            return m_UpgradesValues[CurrentLevel].Cost;
        }

        public int GetNextCost()
        {
            return m_UpgradesValues[CurrentLevel + 1].Cost;
        }

        public Sprite GetIcon()
        {
            return m_UpgradesValues[CurrentLevel].Icon;
        }

        public bool IsMaxedOut()
        {
            return CurrentLevel >= MAX_UPGRADE_LEVEL - 1;
        }

        public List<Sprite> GetMachineSprites()
        {
            return m_UpgradesValues[CurrentLevel].MachineSprites;
        }
    }

    [HideInInspector] public const int MAX_UPGRADE_LEVEL = 3;
    public List<MachineUpgradeInfo> UpgradesInfo;

    private void Awake()
    {
        UpgradesInfo.ForEach(i => i.CurrentLevel = -1);
    }
}
