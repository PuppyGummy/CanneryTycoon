using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemPerformance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SystemComponents m_Components;
    [SerializeField] private Image m_ProgressBarImage;
    [SerializeField] private Gradient m_ProgressBarColor;
    [SerializeField] private Animation m_Anim;
    [SerializeField] private GameObject m_ProgressBar;
    [SerializeField] private Image m_CollectButton;
    [SerializeField] private Button m_CollectionButtonTheButton;
    [HideInInspector] public bool IsAvailable = true;
    [Header("Salmon References")]
    [SerializeField] private Sprite m_Excellent;
    [SerializeField] private Sprite m_Good;
    [SerializeField] private Sprite m_Bad;
    private Coroutine m_SalmonRottingProcess;
    private const float ICON_TRANSITION = 0.2f;

    private void Awake()
    {
        m_CollectButton.gameObject.SetActive(false);
        m_ProgressBar.SetActive(false);
    }

    public void StartPreparing()
    {
        StartCoroutine(PreparationProcess());
    }

    private IEnumerator PreparationProcess()
    {
        if (m_Components.Info.Type == MachinesType.Boat) yield return new WaitForSeconds(2f);

        float time = 0f;
        IsAvailable = false;
        float duration = m_Components.Info.ProcessingTime * m_Components.Workers.TimeMultiplier;
        m_ProgressBar.SetActive(true);
        m_ProgressBarImage.color = m_ProgressBarColor.Evaluate(0f);
        // to show it from the beginning
        ChangeIcon(m_Components.Info.CurrentSalmonInfo.Quality == SalmonQuality.Excellent ? m_Excellent : m_Good);
        // show the button when the process is finished
        m_CollectButton.gameObject.SetActive(true);
        m_CollectionButtonTheButton.interactable = false;
        m_CollectButton.color = Color.white;

        // start the process
        while (time < duration)
        {
            m_CollectButton.color = Color.white;
            m_CollectButton.color = Color.Lerp(Color.black, Color.white, time / duration);
            time += Time.deltaTime;
            // update the progress bar
            m_ProgressBarImage.fillAmount = time / duration;
            yield return null;
        }

        m_Anim.Play();
        m_ProgressBar.SetActive(false);
        m_CollectionButtonTheButton.interactable = true;
        GameManager.Instance.AudioManager.PlayAudio(AudioType.Notification);

        if (m_Components.Info.Type != MachinesType.Shipping)
        {
            // start salmon progress
            if (m_Components.Info.CurrentSalmonInfo.Quality == SalmonQuality.Excellent)
            {
                ChangeIcon(m_Excellent);
                m_SalmonRottingProcess = StartCoroutine(SalmonRottingProcess(GameManager.Instance.SalmonManager.FirstRottingPeriod));
            }
            else
            {
                ChangeIcon(m_Good);
                m_SalmonRottingProcess = StartCoroutine(SalmonRottingProcess(GameManager.Instance.SalmonManager.SecondRottingPeriod, false));
            }
        }
        else
        {
            ChangeIcon(m_Good);
        }
    }

    private IEnumerator SalmonRottingProcess(float _duration, bool _repeat = true)
    {
        float time = 0f;
        m_ProgressBar.SetActive(true);

        // start the process
        while (time < _duration)
        {
            time += Time.deltaTime;
            float t = time / _duration;
            m_ProgressBarImage.fillAmount = 1 - t;
            m_ProgressBarImage.color = m_ProgressBarColor.Evaluate(t);
            yield return null;
        }

        // decrease salmon quality
        if (_repeat)
        {
            m_Components.Info.CurrentSalmonInfo.Quality = SalmonQuality.Good;
            ChangeIcon(m_Good);
        }
        else
        {
            m_Components.Info.CurrentSalmonInfo.Quality = SalmonQuality.Bad;
            ChangeIcon(m_Bad);
        }

        // whether to repeat or to stop the progress
        if (_repeat)
        {
            m_SalmonRottingProcess = StartCoroutine(SalmonRottingProcess(GameManager.Instance.SalmonManager.SecondRottingPeriod, false));
        }
        else
        {
            m_Anim.Stop();
            m_ProgressBar.SetActive(false);
        }
    }

    private void ChangeIcon(Sprite _sprite)
    {
        m_Components.Info.CurrentSalmonInfo.SetSprite(_sprite);
        m_CollectButton.sprite = _sprite;
    }

    #region Buttons Functionality
    public void ReadyButton()
    {
        // if there is no next system i.e., this is the shipping system
        // if it's a shipping phase then we add coins
        if (m_Components.Info.Type == MachinesType.Shipping)
        {
            GameManager.Instance.Stats.SalmonFinished++;
            GameManager.Instance.GeneralReferences.CoinsParticles.Play();
            // the value will change if different types of salmon are introduced
            int money = GameManager.Instance.SalmonManager.GetSalmonPrice(m_Components.Info.CurrentSalmonInfo.Quality);
            GameManager.Instance.CoinsManager.AddCoins(money);
            IsAvailable = true;
            StartCoroutine(ShrinkIcon());

            // must destroy or pool the salmon. This will depend on how far we take it
            if (m_SalmonRottingProcess != null) StopCoroutine(m_SalmonRottingProcess);

            m_Components.Message.DisplayMessage($"+{money}$", GameManager.Instance.GeneralReferences.MoneyColor);
            m_ProgressBar.SetActive(false);
            return;
        }

        // if the salmon is bad, then don't process
        if (m_Components.Info.CurrentSalmonInfo.Quality == SalmonQuality.Bad)
        {
            GameManager.Instance.Stats.SalmonUnfinished++;
            m_ProgressBar.SetActive(false);
            StartCoroutine(ShrinkIcon());
            IsAvailable = true;
            Destroy(m_Components.Info.CurrentSalmonInfo.gameObject);
            // if it's a boat then it starts the process again
            m_Components.Info.StartCatchingSalmon();
            int cost = GameManager.Instance.GeneralReferences.BadSalmonCost;
            GameManager.Instance.CoinsManager.AddCoins(-cost);
            GameManager.Instance.AudioManager.PlayAudio(AudioType.Trash);
            GameManager.Instance.Stats.LostMoney += GameManager.Instance.GeneralReferences.BadSalmonCost;
            // play some trash audio
            m_Components.Message.DisplayMessage($"-{cost}$", Color.red);
            return;
        }

        // if the next system is busy
        if (!m_Components.Info.NextSystemIsAvailable())
        {
            string message = $"{m_Components.Info.GetNextMachineType()} section is busy";
            m_Components.Message.DisplayMessage(message, Color.white);
            return;
        }

        StopCoroutine(m_SalmonRottingProcess);
        IsAvailable = true;
        StartCoroutine(ShrinkIcon());
        // move the salmon to the next machine
        m_Components.Info.MoveSalmon();
        m_ProgressBar.SetActive(false);
        m_Components.Info.StartCatchingSalmon();
        // get some xp or something
        // play some particles
        // play some animation
    }

    private IEnumerator ShrinkIcon()
    {
        m_Anim.Stop();
        float time = 0f;
        float size = m_CollectButton.transform.localScale.x;
        m_CollectionButtonTheButton.enabled = false;

        while (time < ICON_TRANSITION)
        {
            time += Time.deltaTime;
            m_CollectButton.transform.localScale = Vector3.one * Mathf.Lerp(size, 0f, time / ICON_TRANSITION);
            yield return null;
        }

        m_CollectButton.gameObject.SetActive(false);
        m_CollectButton.transform.localScale = Vector3.one * size;
        m_CollectionButtonTheButton.enabled = true;
    }

    public void StopWorking()
    {
        StopAllCoroutines();
        m_CollectButton.gameObject.SetActive(false);
        m_ProgressBarImage.gameObject.SetActive(false);
    }
    #endregion
}
