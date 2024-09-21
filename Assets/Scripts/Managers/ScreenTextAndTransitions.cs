using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScreenTextAndTransitions : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] private Animation m_TransitionAnim;
    [SerializeField] private AnimationClip m_FadeOut;
    [Header("Stats Animation")]
    [SerializeField] private Animation m_Anim;
    [SerializeField] private AnimationClip m_WinningClip;
    [SerializeField] private AnimationClip m_LosingClip;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI m_TimeText;
    [SerializeField] private TextMeshProUGUI m_MoneyLostText;
    [SerializeField] private TextMeshProUGUI m_WorkersText;
    [SerializeField] private TextMeshProUGUI m_MoneySpentText;
    [SerializeField] private TextMeshProUGUI m_FinishCansText;
    [SerializeField] private TextMeshProUGUI m_UnfinishedCanText;
    public int LostMoney = 0;
    public int Workers = 0;
    public int MoneySpent = 0;
    public int SalmonFinished = 0;
    public int SalmonUnfinished = 0;

    public void ShowWinningScreen()
    {
        SetStats();
        GameManager.Instance.TimeManager.StartCounting = false;
        GameManager.Instance.MachinesManager.Machines.ForEach(m => m.Performance.StopWorking());
        m_Anim.clip = m_WinningClip;
        m_Anim.Play();
    }

    public void ShowLosingScreen()
    {
        SetStats();
        GameManager.Instance.TimeManager.StartCounting = false;
        GameManager.Instance.MachinesManager.Machines.ForEach(m => m.Performance.StopWorking());
        m_Anim.clip = m_LosingClip;
        m_Anim.Play();
    }

    public void SetStats()
    {
        m_TimeText.text = GameManager.Instance.TimeManager.GetTime();
        m_MoneyLostText.text = LostMoney.ToString();
        m_WorkersText.text = Workers.ToString();
        m_MoneySpentText.text = MoneySpent.ToString();
        m_FinishCansText.text = SalmonFinished.ToString();
        m_UnfinishedCanText.text = SalmonUnfinished.ToString();
    }

    public void ContinueButton()
    {
        m_TransitionAnim.clip = m_FadeOut;
        m_TransitionAnim.Play();
        Invoke(nameof(ReloadScene), m_FadeOut.length);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
