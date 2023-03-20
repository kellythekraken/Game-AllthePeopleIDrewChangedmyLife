using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public CanvasFade startCanvasFade, blackoutFade;
    [SerializeField] private Button startBtn, restartBtn, continueBtn, quitBtn;
    [SerializeField] private EventSystem eventsystemInStartScene;
    [SerializeField] private GameObject startSceneObjects;
    [SerializeField] private Camera startMenuCam;
    [SerializeField] private TextModifier titleText;
    public UnityEvent GamestartEvent;
    bool startFromBeginning = true;

    void Awake() => Instance = this;
    void Start()
    {
        StartCoroutine(FadeInStartMenu());
        //load the main game scene from the very beginning
        LoadMainGameUponStart();

        restartBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);

        startBtn.onClick.AddListener(LoadMainScene);
        continueBtn.onClick.AddListener(DeactivateStartMenu);
        restartBtn.onClick.AddListener(ReloadGame);
        quitBtn.onClick.AddListener(QuitGame);
        if (GamestartEvent == null)
            GamestartEvent = new UnityEvent();
    }

    public void LoadMainGameUponStart()
    {
        AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);
    }

    //start the game from start scene
    public void LoadMainScene()
    {
        startBtn.interactable = false;
        DeactivateStartMenu();
        startBtn.gameObject.SetActive(false);

        if(startFromBeginning) GamestartEvent.Invoke();
        //StartCoroutine(SwitchToMain());
    }
    IEnumerator SwitchToMain()  //first time loading main scene
    {
        var fade = FadeOutStart();
        yield return fade;

        AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);
        yield return load;
        //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
        startSceneObjects.SetActive(false);
        startBtn.gameObject.SetActive(false);

        var eventList = FindObjectsOfType<EventSystem>();
        foreach(var i in eventList) if(i != eventsystemInStartScene) Destroy(i.gameObject);
    }

    void DeactivateStartMenu()
    {
        continueBtn.interactable = false;
        InputManager.Instance.EnableAllInput(true);
        GameManager.Instance.BackToLastMode();
        startSceneObjects.SetActive(false);
    }

    //load the start scene from main game, called by button in setting screen
    public void ActivateStartMenu()
    {
        startFromBeginning = false;
        StartCoroutine(FadeInStartMenu());
        continueBtn.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
        continueBtn.interactable = true;
        GameManager.Instance.currMode = CurrentMode.StartMenu;
        startSceneObjects.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("quit game!");
        Application.Quit();
    }

    public void StartOver() //reload the game at start 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void ReloadGame()    //reload the game at main scene
    {
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        startFromBeginning = true;
        var load = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(1);
        yield return load;
        LoadMainScene();
    }
    IEnumerator FadeInStartMenu()
    {
        var fade = FadeInStart(1.5f);
        yield return fade;
        StartCoroutine(titleText.Typewrite());
    }
    public void DisplayStartScreen()
    {
        FadeInStart(2f);
        startCanvasFade.BlockRayCast(true);
        GameManager.Instance.LockCursor(false);
        GameManager.Instance.PauseGame();
    }    

    private Coroutine FadeOutStart(float time = 1f) {
        return StartCoroutine(blackoutFade.ChangeAlphaOverTime(0f,1f, time));
    }
    private Coroutine FadeInStart(float time = 1f) {
        return StartCoroutine(blackoutFade.ChangeAlphaOverTime(1f,0f, time));
    }
}
