using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinsManager : MonoBehaviour
{
    private int m_CurrentCoins;
    // for display
    private int m_FakeCoins;
    [SerializeField] private float m_MaxFillingCoinsTime = 0.2f;
    [SerializeField] private AnimationCurve m_CoinsAdditionCurve;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI m_CoinsText;
    [SerializeField] private Image m_MoneyBar;

    private void Start()
    {
        m_CoinsText.text = $"0 / {GameManager.Instance.TimeManager.MoneyGoal}";
        m_MoneyBar.fillAmount = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddCoins(200);// Random.Range(100, 200));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddCoins(-200);// Random.Range(100, 200));
        }
    }

    public void AddCoins(int _amount)
    {
        if (m_CurrentCoins + _amount >= GameManager.Instance.TimeManager.MoneyGoal)
        {
            GameManager.Instance.Stats.ShowWinningScreen();
        }
        else if (m_CurrentCoins + _amount < 0)
        {
            GameManager.Instance.Stats.ShowLosingScreen();
        }

        if (_amount < 0)
        {
            // GameManager.Instance.AudioManager.PlayAudio(AudioType.Trash);
        }
        else if (_amount > 0)
        {
            GameManager.Instance.AudioManager.PlayAudio(AudioType.CollectCoins);
        }

        // if no coins are being added at the moment, then start the process of adding new coins
        StartCoroutine(AddingCoinsProcess(Mathf.Clamp(_amount, -m_CurrentCoins, 10000000)));
    }

    public int GetCurrentCoins()
    {
        return m_CurrentCoins;
    }

    public bool HasEnoughCoins(int _amount)
    {
        return _amount <= m_CurrentCoins;
    }

    private IEnumerator AddingCoinsProcess(int _coinsToAdd)
    {
        m_CurrentCoins += _coinsToAdd;
        m_CurrentCoins = Mathf.Clamp(m_CurrentCoins, 0, 10000000);

        int addition = (int)Mathf.Sign(_coinsToAdd);
        _coinsToAdd = Mathf.Abs(_coinsToAdd);
        float totalCoinsToAdd = _coinsToAdd;
        float currentCoins = 0;

        // for display
        float previousMoney = m_CurrentCoins - _coinsToAdd;

        while (currentCoins < totalCoinsToAdd)
        {
            m_FakeCoins += addition;
            currentCoins++;
            m_CoinsText.text = $"{m_FakeCoins} / {GameManager.Instance.TimeManager.MoneyGoal}";
            float t = (float)currentCoins / totalCoinsToAdd;
            float coins = (float)m_FakeCoins;
            m_MoneyBar.fillAmount = coins / GameManager.Instance.TimeManager.MoneyGoal;
            float waitTime = m_CoinsAdditionCurve.Evaluate(t) * m_MaxFillingCoinsTime;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
