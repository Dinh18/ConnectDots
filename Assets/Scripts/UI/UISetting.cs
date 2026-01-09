using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour, IMenu
{
    private UIController uIController;
    [SerializeField] private Button btnMainMenu;
    [SerializeField] private Button btnSound;
    [SerializeField] private Button btnMusic;
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SetUp(UIController uIController)
    {
        this.uIController = uIController;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    void Start()
    {
        btnMainMenu.onClick.AddListener(() => uIController.OnClickMainMenu());
        btnSound.onClick.AddListener(() => uIController.OnClickSound());
        btnMusic.onClick.AddListener(() => uIController.OnClickMusic());
    }
}
