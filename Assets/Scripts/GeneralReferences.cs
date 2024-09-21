using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneralReferences : MonoBehaviour
{
    public Color MoneyColor;
    public int BadSalmonCost = 40;
    public ParticleSystem CoinsParticles;
    public Transform GraphicsCanvas;
    [Header("Worker Info")]
    public TextMeshProUGUI WorkersInfo;
    public TextMeshProUGUI WorkersInfoName;
    public GameObject WorkersInfoWindow;
    [Header("Worker Movement Points")]
    public List<Transform> StartingPoints;
    public List<Transform> EndingPoints;
    [Header("Others")]
    public GameObject SalmonPrefab;

}
