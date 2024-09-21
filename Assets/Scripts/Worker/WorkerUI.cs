using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerUI : MonoBehaviour
{
    [System.Serializable]
    public struct WorkerInfo
    {
        public Sprite Icon;
        public string Name;
        public string Nationality;
        public MachinesType Type;
        [TextArea(3, 20)] public string BackGround;
    }

    [SerializeField] private Image m_IconUI;
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_TypeText;
    [SerializeField] private TextMeshProUGUI m_NationalityText;
    [SerializeField] private List<WorkerInfo> m_Icons;
    private int m_CurrentWorker = -1;

    public void SetRandomInfo()
    {
        m_CurrentWorker = Random.Range(0, m_Icons.Count);
        var info = m_Icons[m_CurrentWorker];
        m_IconUI.sprite = info.Icon;
        m_NameText.text = info.Name;
        m_TypeText.text = info.Type.ToString();
        m_NationalityText.text = info.Nationality;
    }

    public void OpenInfo()
    {
        GameManager.Instance.GeneralReferences.WorkersInfo.text = m_Icons[m_CurrentWorker].BackGround;
        GameManager.Instance.GeneralReferences.WorkersInfoName.text = $"{m_Icons[m_CurrentWorker].Nationality} workers";
        GameManager.Instance.GeneralReferences.WorkersInfoWindow.SetActive(true);
    }
}
