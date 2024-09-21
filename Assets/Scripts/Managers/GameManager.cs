using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GeneralReferences GeneralReferences;
    public CameraManager CameraManager;
    public CoinsManager CoinsManager;
    public UpgradesManager UpgradesManager;
    public MachinesManager MachinesManager;
    public WorkersManager WorkersManager;
    public AudioManager AudioManager;
    public SalmonManager SalmonManager;
    public ShopManager ShopManager;
    public TimeManager TimeManager;
    public ScreenTextAndTransitions Stats;

    public GameObject MainCanvas;
    public bool HasTutorial = true;

    private void Awake()
    {
        Instance = this;
    }
}

