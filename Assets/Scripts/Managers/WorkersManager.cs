using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    // the price and any explanation text about the worker type
    [System.Serializable]
    public class WorkerInfo
    {
        public MachinesType Type;
        public string Name;
        [TextArea(0, 10)] public string AboutInfo;
        [SerializeField] private int Cost;

        public int GetCost()
        {
            return Cost;
        }
    }

    public List<WorkerInfo> WorkersInfos;


}
