using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalmonInfo : MonoBehaviour
{
    [SerializeField] private Image m_SalmonGraphics;
    public SalmonQuality Quality = SalmonQuality.Excellent;

    public void SetSprite(Sprite _sprite)
    {
        m_SalmonGraphics.sprite = _sprite;
    }
}

public enum SalmonQuality
{
    Bad = 2, Good = 1, Excellent = 0
}
