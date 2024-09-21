using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    public RectTransform shopPanel;
    public List<ShopItem> ShopItems;
    private const string MAXED_ITEM_TEXT = "Maxed out";
    private bool isInitialized = false;

    private void Awake()
    {
        SetUIValues();
        isInitialized = true;
    }

    // sets the initial values of the machines and update the prices of the shop
    private void SetUIValues()
    {
        for (int i = 0; i < ShopItems.Count; i++)
        {
            UpgradeMachine(i);
        }
    }

    private void Start()
    {
        // set random UI for the workers in the shop
        GameManager.Instance.MachinesManager.Machines.ForEach(m => m.Workers.WorkerUI.SetRandomInfo());
    }

    public void OpenShop()
    {
        shopPanel.gameObject.SetActive(true);
        shopPanel.DOScale(1.2f, 0.5f).From(0);
    }

    public void CloseShop()
    {
        shopPanel.DOScale(0, 0.5f).OnComplete(() => shopPanel.gameObject.SetActive(false));
    }

    // since Unity doesn't support passing Enum parameters to OnClick slots, we use ints and convert them to MachineType enums
    public void UpgradeMachine(int _machineTypeIndex)
    {
        // convert the int to a machineType
        MachinesType type = (MachinesType)_machineTypeIndex;

        // cache the current upgrade info
        var upgradeInfo = GameManager.Instance.UpgradesManager.UpgradesInfo.Find(m => m.MachineType == type);

        // check if the item is maxed out
        if (upgradeInfo.IsMaxedOut())
        {
            return;
        }

        // check if there is enough coins to upgrade
        if (!GameManager.Instance.CoinsManager.HasEnoughCoins(upgradeInfo.GetNextCost()))
        {
            // print some message in some UI informing the player there aren't enough coins for the upgrade
            print($"Not enough coins for {type}");
            return;
        }

        // upgrade the values
        upgradeInfo.Upgrade();
        // set the processing time of the machine
        var machine = GameManager.Instance.MachinesManager.Machines.Find(m => m.Info.Type == type);
        machine.Info.ProcessingTime = upgradeInfo.GetProcessingTime();

        if (upgradeInfo.GetMachineSprites().Count > 0)
        {
            machine.Info.SetMachineSprite(upgradeInfo.GetMachineSprites(), upgradeInfo.CurrentLevel);
        }

        // pay for the upgrade
        GameManager.Instance.CoinsManager.AddCoins(-upgradeInfo.GetCost());
        GameManager.Instance.Stats.MoneySpent += upgradeInfo.GetCost();

        if (isInitialized)
            GameManager.Instance.AudioManager.PlayAudio(AudioType.Purchase);

        // update the UI for the specific item purchased
        if (!upgradeInfo.IsMaxedOut())
        {
            ShopItems.Find(i => i.MachineType == type).UpdateItemInfo($"{upgradeInfo.GetNextCost()}", upgradeInfo.GetIcon());
        }
        else
        {
            // disable the button and set its text to nothing or some meaningful
            ShopItems.Find(i => i.MachineType == type).UpdateItemInfo(MAXED_ITEM_TEXT, upgradeInfo.GetIcon());
            ShopItems.Find(i => i.MachineType == type).MaxedOutGFX();
        }
    }

    // should be called by a button for a specified worker
    public void HireWorker(int _machineTypeIndex)
    {
        // convert the int to a machineType
        MachinesType type = (MachinesType)_machineTypeIndex;

        var upgradeInfo = GameManager.Instance.WorkersManager.WorkersInfos.Find(m => m.Type == type);
        SystemComponents selectedMachine = GameManager.Instance.MachinesManager.Machines.Find(m => m.Info.Type == type);

        // check if the machine can occupy more workers
        if (!selectedMachine.Workers.CanTakeMoreWorkers())
        {
            print($"{type} can't take more workers");
            return;
        }

        // check if there is enough coins to upgrade
        if (!GameManager.Instance.CoinsManager.HasEnoughCoins(upgradeInfo.GetCost()))
        {
            print($"Not enough coins to hire a worker for {type}");
            return;
        }

        // add a worker for the specified type
        selectedMachine.Workers.AddWorker();
        selectedMachine.Workers.WorkerUI.SetRandomInfo();

        // deduct the cost
        GameManager.Instance.CoinsManager.AddCoins(-upgradeInfo.GetCost());
        GameManager.Instance.Stats.MoneySpent += upgradeInfo.GetCost();

        if (isInitialized)
            GameManager.Instance.AudioManager.PlayAudio(AudioType.Purchase);
    }
}
