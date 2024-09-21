using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemInfo : MonoBehaviour
{
    public float ProcessingTime;
    public MachinesType Type;
    [Header("References")]
    [SerializeField] private SystemComponents m_Components;
    // systems connected to the current system
    [SerializeField] private SystemComponents m_PreviousSystem;
    [SerializeField] private SystemComponents m_NextSystem;
    [HideInInspector] public SalmonMovement CurrentSalmonMovement;
    [HideInInspector] public SalmonInfo CurrentSalmonInfo;
    [SerializeField] private List<Image> m_MachineImages;
    [SerializeField] private List<ParticleSystem> m_GoldAndSilverParticles;
    public Transform SalmonCatchingPoint;
    public Transform SalmonSendingPoint;

    // when the system recieves something e.g., the buthers recieve the salmon raw, the car recieves the salmon canned, etc.
    public void OnSalmonRecieved(SalmonMovement _salmon, SalmonInfo _salmonInfo)
    {
        CurrentSalmonMovement = _salmon;
        CurrentSalmonInfo = _salmonInfo;
        // hide the salmon
        CurrentSalmonMovement.gameObject.SetActive(false);
        m_Components.Performance.StartPreparing();
    }

    public void MoveSalmon()
    {
        // if it's the car i.e. shipping, then we stop the cycle and call sendToCosumers
        if (Type == MachinesType.Shipping)
        {
            SendToConsumers();
            return;
        }

        CurrentSalmonMovement.MoveSalmon(SalmonSendingPoint.position, m_NextSystem.Info.SalmonCatchingPoint.position, m_NextSystem);
    }

    public bool NextSystemIsAvailable()
    {
        return m_NextSystem.Performance.IsAvailable;
    }

    public MachinesType GetNextMachineType()
    {
        return m_NextSystem.Info.Type;
    }

    public void SetMachineSprite(List<Sprite> _sprites, int _level)
    {
        for (int i = 0; i < m_MachineImages.Count; i++)
        {
            m_MachineImages[i].sprite = _sprites[i];
        }

        if (_level == 0 || m_GoldAndSilverParticles.Count == 0) return;

        m_GoldAndSilverParticles.ForEach(p => p.Stop());
        m_GoldAndSilverParticles[_level - 1].Play();
    }

    #region Machine Specific Funcitonality
    public void StartCatchingInTime(float _time)
    {
        Invoke(nameof(StartCatchingSalmon), _time);
    }

    public void StartCatchingSalmon()
    {
        if (Type == MachinesType.Boat)
        {
            GameObject salmonPrefab = GameManager.Instance.GeneralReferences.SalmonPrefab;
            SalmonMovement salmonFromSea = Instantiate(salmonPrefab, GameManager.Instance.MainCanvas.transform).GetComponent<SalmonMovement>();
            OnSalmonRecieved(salmonFromSea, salmonFromSea.GetComponent<SalmonInfo>());
        }
    }

    private void SendToConsumers()
    {
        // add the currency

        // play car animation or something

    }
    #endregion
}

public enum MachinesType
{
    Boat = 0, Butchering = 1, Cleaning = 2, Canning = 3, Shipping = 4
}