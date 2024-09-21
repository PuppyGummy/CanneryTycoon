using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Animation m_Anim;
    [SerializeField] private GameObject m_HUD;
    [SerializeField] private GameObject m_Tutorial;
    [SerializeField] private Button m_StartButton;
    [SerializeField] private SystemInfo m_BoatSystem;

    private void Awake()
    {
        m_HUD.SetActive(false);
    }

    public void StartButton()
    {
        m_Anim.Play();
        GameManager.Instance.AudioManager.PlayAudio(AudioType.Click);
        m_Tutorial.SetActive(GameManager.Instance.HasTutorial);
        m_HUD.SetActive(true);
        m_StartButton.enabled = false;

        if (!GameManager.Instance.HasTutorial)
        {
            GameManager.Instance.MachinesManager.Machines.Find(m => m.Info.Type == MachinesType.Boat).Info.StartCatchingInTime(2f);
            GameManager.Instance.TimeManager.StartCounting = true;
        }
    }
}
