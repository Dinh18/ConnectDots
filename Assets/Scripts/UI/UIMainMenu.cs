using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour, IMenu
{
    private UIController uIController;
    [SerializeField] Button btnPlay;
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnQuit;

    void Start()
    {
        btnPlay.onClick.AddListener(() => uIController.OnclikPlay());
        btnSetting.onClick.AddListener(() => uIController.OnClickSetting());
    }

    public void SetUp(UIController uIController)
    {
        this.uIController = uIController;
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
