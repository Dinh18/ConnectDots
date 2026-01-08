using UnityEngine;

public class UICompleted : MonoBehaviour, IMenu
{
    private UIController uIController;
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
