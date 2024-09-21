using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private GameObject shopMachine;
    [SerializeField] private GameObject shopWorkers;
    [SerializeField] private Image machine;
    [SerializeField] private Image employee;
    [SerializeField] private Sprite machineSprite;
    [SerializeField] private Sprite employeeSprite;
    [Header("Positions")]
    [SerializeField] private RectTransform m_Money;
    [SerializeField] private RectTransform m_Time;
    [SerializeField] private RectTransform m_Shop;
    [SerializeField] private RectTransform m_MachinesSection;
    [SerializeField] private RectTransform m_WorkersSection;
    [SerializeField] private RectTransform m_CloseShopButton;

    private bool visitedShop = false;
    private bool visitedFishing = false;
    private bool visitedFisrstScene = false;
    private bool visitedButchering = false;
    private bool visitedWashing = false;
    private bool visitedSecondScene = false;
    private bool visitedCanning = false;
    private bool visitedThirdScene = false;
    private bool visitedTransport = false;
    private Button[] buttons;

    void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        buttons = FindObjectsOfType<Button>();
        arrow.gameObject.SetActive(false);
        dialogueRunner.AddCommandHandler("ShowMoney", ShowMoney);
        dialogueRunner.AddCommandHandler("ShowShop", ShowShop);
        dialogueRunner.AddCommandHandler("ShowTime", ShowTime);
        dialogueRunner.AddCommandHandler("ShowShopMachine", ShowShopMachine);
        dialogueRunner.AddCommandHandler("ShowShopEmployee", ShowShopEmployee);
        dialogueRunner.AddCommandHandler("ShowCloseShop", ShowCloseShop);
        dialogueRunner.AddCommandHandler("StartGame", StartGame);
    }
    public void PauseGame()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
    public void ResumeGame()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
    private void ShowMoney()
    {
        arrow.gameObject.SetActive(true);
        arrow.position = m_Money.position; //new Vector3(-428, 443, 0);
    }
    private void ShowTime()
    {
        arrow.position = m_Time.position; // new Vector3(-566, 367, 0);
    }

    private void ShowShop()
    {
        arrow.position = m_Shop.position;// new Vector3(-652, 271, 0);
    }
    private void ShowShopMachine()
    {
        GameManager.Instance.ShopManager.OpenShop();
        arrow.position = m_MachinesSection.position; //new Vector3(91, 355, 0);
    }
    private void ShowShopEmployee()
    {
        shopMachine.SetActive(false);
        shopWorkers.SetActive(true);
        machine.sprite = machineSprite;
        employee.sprite = employeeSprite;
        arrow.position = m_WorkersSection.position;//new Vector3(592, 355, 0);
    }

    private void ShowCloseShop()
    {
        arrow.position = m_CloseShopButton.position;// new Vector3(701, 355, 0);
    }
    private void StartGame()
    {
        GameManager.Instance.MachinesManager.Machines.Find(m => m.Info.Type == MachinesType.Boat).Info.StartCatchingInTime(0f);
        GameManager.Instance.TimeManager.StartCounting = true;
    }
    public void StartDialogue(string node)
    {
        if (dialogueRunner == null) return;

        dialogueRunner.Stop();
        arrow.gameObject.SetActive(false);

        switch (node)
        {
            case "Fishing":
                if (!visitedShop)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedShop = true;
                }
                break;
            case "MoveToButchering":
                if (!visitedFishing)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedFishing = true;
                }
                break;
            case "Butchering":
                if (!visitedFisrstScene)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedFisrstScene = true;
                }
                break;
            case "Washing":
                if (!visitedButchering)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedButchering = true;
                }
                break;
            case "MoveToCanning":
                if (!visitedWashing)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedWashing = true;
                }
                break;
            case "Canning":
                if (!visitedSecondScene)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedSecondScene = true;
                }
                break;
            case "Transport":
                if (!visitedThirdScene)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedThirdScene = true;
                }
                break;
            case "End":
                if (!visitedTransport)
                {
                    dialogueRunner.StartDialogue(node);
                    visitedTransport = true;
                }
                break;
        }
    }
}
