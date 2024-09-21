using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemWorkers : MonoBehaviour
{
    [System.Serializable]
    public class WorkerPosition
    {
        public Transform Spot;
        public bool IsAvailable = true;
    }
    // the value that combination of all the workers' effect on the processing time, 1 means that the workers have no effect, 0 means the work gets done instantly (not possible)
    public float TimeMultiplier = 1;
    private List<Worker> m_CurrentWorkers = new List<Worker>();
    [Header("References")]
    [Tooltip("workers that can only work for this system")]
    [SerializeField] private Worker[] m_WorkersPrefabs;
    [SerializeField] private List<WorkerPosition> m_WorkersPositions;
    public WorkerUI WorkerUI;
    [SerializeField] private bool m_StickToOriginalParent = false;

    private void Start()
    {
        AddWorker();
    }

    public void AddWorker()
    {
        GameManager.Instance.Stats.Workers++;
        Worker workerPrefab = m_WorkersPrefabs[Random.Range(0, m_WorkersPrefabs.Length)];
        WorkerPosition spot = m_WorkersPositions.Find(p => p.IsAvailable);
        spot.IsAvailable = false;
        var worker = Instantiate(workerPrefab, spot.Spot.position, Quaternion.identity, spot.Spot);
        m_CurrentWorkers.Add(worker);
        // the first worker won't have any effect on the calculation since the work can't be done in the first place without him/her
        TimeMultiplier = 1 - (m_CurrentWorkers.Count - 1) * worker.Efficiency;
        Invoke(nameof(SetLastWorkerParent), 0.05f);
    }

    private void SetLastWorkerParent()
    {
        if (!m_StickToOriginalParent)
        {
            m_CurrentWorkers[m_CurrentWorkers.Count - 1].transform.SetParent(GameManager.Instance.GeneralReferences.GraphicsCanvas);
        }

        m_CurrentWorkers.ForEach(w => w.transform.localPosition = new Vector3(w.transform.localPosition.x, w.transform.localPosition.y, 0f));
    }

    public bool CanTakeMoreWorkers()
    {
        return m_CurrentWorkers.Count != m_WorkersPositions.Count;
    }
}
