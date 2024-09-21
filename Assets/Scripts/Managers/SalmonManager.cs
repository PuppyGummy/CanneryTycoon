using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalmonManager : MonoBehaviour
{
    public float FirstRottingPeriod = 5f;
    public float SecondRottingPeriod = 10f;
    [SerializeField] private int m_ExcellentSalmonPrice = 200;
    [SerializeField] private int m_GoodSalmonPrice = 100;

    public int GetSalmonPrice(SalmonQuality _quality)
    {
        return _quality == SalmonQuality.Excellent ? m_ExcellentSalmonPrice : m_GoodSalmonPrice;
    }
}
