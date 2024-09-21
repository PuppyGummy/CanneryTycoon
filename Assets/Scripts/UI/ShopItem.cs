using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public MachinesType MachineType;
    [SerializeField] private Image m_UpgradeImage;
    [SerializeField] private TextMeshProUGUI m_CostText;
    [SerializeField] private GameObject m_BuyButton;
    [SerializeField] private GameObject m_CheckButton;
    public void UpdateItemInfo(string _newCost, Sprite _newIcon)
    {
        m_CostText.text = _newCost;
        m_UpgradeImage.sprite = _newIcon;
    }

    public void MaxedOutGFX()
    {
        m_BuyButton.SetActive(false);
        m_CheckButton.SetActive(true);
    }
}
