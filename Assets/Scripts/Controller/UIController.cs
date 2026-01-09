using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using static System.Net.Mime.MediaTypeNames;
using NUnit.Framework;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    private SoundController soundController;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelCompleted;
    [SerializeField] private GameObject panelCompletedMenu;
    [SerializeField] private GameObject uiLevel;
    [SerializeField] private GameObject uiSetting;
    private UIMainMenu mainMenu;
    private UICompleted completed;
    private UICompletedMenu completedMenu;
    private UILevel level;
    private UISetting setting;
    void Awake()
    {
        mainMenu = panelMainMenu.GetComponent<UIMainMenu>();
        completed = panelCompleted.GetComponent<UICompleted>();
        completedMenu = panelCompletedMenu.GetComponent<UICompletedMenu>();
        level = uiLevel.GetComponent<UILevel>();
        setting = uiSetting.GetComponent<UISetting>();

        mainMenu.SetUp(this);
        completed.SetUp(this);
        completedMenu.SetUp(this);
        level.SetUp(this);
        setting.SetUp(this); 
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
        level.Hide();
        setting.Hide();
        switch(state)
        {
            case GameController.GameState.MainMenu:
                mainMenu.Show();
                break;
            case GameController.GameState.Playing:
                level.Show();
                break;
            case GameController.GameState.Setting:
                setting.Show();
                break;
            case GameController.GameState.LevelCompleted:
                Camera.main.transform.DOShakePosition(0.5f, 0.3f, 10, 90);
                StartCoroutine(DelayDisplayCompleltedMenu(0.2f));
                break;
        }
    }

    private IEnumerator DelayDisplayCompleltedMenu(float time)
    {
        yield return new WaitForSeconds(time);
        completedMenu.Show();
        panelCompletedMenu.transform.localScale = Vector3.zero;
        panelCompletedMenu.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
    public void Setup(GameController gameController, SoundController soundController)
    {
        this.gameController = gameController;
        this.soundController = soundController;
    }

    public void OnclikPlay()
    {
        gameController.LoadLevel();
    }

    public void OnClickNextLevel()
    {
        gameController.LoadNextLevel();
    }
    public void OnClickSetting()
    {
        gameController.SetState(GameController.GameState.Setting);
    }
    public void OnClickMainMenu()
    {
        gameController.SetState(GameController.GameState.MainMenu);
    }

    public void OnClickSound()
    {
        soundController.ChangeEnableSoundEffectSource();
    }
    public void OnClickMusic()
    {
        if(soundController.IsPlayBackGround())
        {
            soundController.MuteBackGroundSound();
        }
        else soundController.UnMuteBackGroundSound();
    }
    public int SetLevelText()
    {
        return gameController.CurrentLevelIndex + 1;
    }
}
