using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimeText;
    [SerializeField] private TextMeshProUGUI m_GoalText;
    [SerializeField] private Image m_TimeImage;
    [SerializeField] private float m_MaxTime = 1091f;
    public int MoneyGoal = 1000;
    public bool StartCounting = false;
    private float m_StartingTime = 360f; // in minutes
    private float m_TotalTime = 0f;

    private void Start()
    {
        m_TotalTime = m_StartingTime;
        m_GoalText.text = $"Goal: collect {MoneyGoal}$ before 18:30";
    }

    private void Update()
    {
        if (!StartCounting) return;

        UpdateTime();
    }

    private void UpdateTime()
    {
        if(m_TotalTime >= m_MaxTime)
        {
            GameManager.Instance.Stats.ShowLosingScreen();
        }

        m_TotalTime += Time.deltaTime * 2;
        int hours = Mathf.FloorToInt(m_TotalTime / 60f);
        float minutes = m_TotalTime - (hours * 60f);
        m_TimeText.text = $"{hours:00}:{minutes:00}";
        m_TimeImage.fillAmount = 1 - (m_TotalTime - m_StartingTime) / (m_MaxTime - m_StartingTime);
    }

    public string GetTime()
    {
        float time = m_TotalTime - m_StartingTime;
        float hours = time / 60f;
        float minutes = time % 59f;
        return $"{hours:00}:{minutes:00}";
    }
}
