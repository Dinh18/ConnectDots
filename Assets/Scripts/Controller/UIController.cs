using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelCompleted;
    [SerializeField] private GameObject panelCompletedMenu;
    private UIMainMenu mainMenu;
    private UICompleted completed;
    private UICompletedMenu completedMenu;
    void Awake()
    {
        mainMenu = panelMainMenu.GetComponent<UIMainMenu>();
        completed = panelCompleted.GetComponent<UICompleted>();
        completedMenu = panelCompletedMenu.GetComponent<UICompletedMenu>();

        mainMenu.SetUp(this);
        completed.SetUp(this);
        completedMenu.SetUp(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(GameController.GameState state)
    {
        mainMenu.Hide();
        completed.Hide();
        completedMenu.Hide();
        switch(state)
        {
            case GameController.GameState.MainMenu:
                mainMenu.Show();
                break;
            case GameController.GameState.LevelCompleted:
                
                StartCoroutine(DelayDisplayCompleltedMenu(1f));
                break;
        }
    }

    private IEnumerator DelayDisplayCompleltedMenu(float time)
    {
        completed.Show();
        yield return new WaitForSeconds(time);
        completed.Hide();
        completedMenu.Show();
    }
    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnclikPlay()
    {
        gameController.LoadLevel();
    }

    public void OnClickNextLevel()
    {
        gameController.LoadNextLevel();
    }
}
