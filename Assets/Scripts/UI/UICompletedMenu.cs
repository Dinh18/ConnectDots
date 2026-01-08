using UnityEngine;
using UnityEngine.UI;

public class UICompletedMenu : MonoBehaviour, IMenu
{
    private UIController uIController;
    [SerializeField] private Button btnNextLevel;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btnNextLevel.onClick.AddListener(() => uIController.OnClickNextLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
