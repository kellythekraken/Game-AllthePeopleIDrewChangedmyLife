using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public CanvasFade startCanvasFade;
    [SerializeField] private Button startBtn, restartBtn, continueBtn, quitBtn;
    [SerializeField] private EventSystem eventsystemInStartScene;
    [SerializeField] private GameObject startSceneObjects;
    [SerializeField] private Camera startMenuCam;
    enum CurrentScene {START,MAIN};
    CurrentScene currScene;
    void Awake() => Instance = this;
    void Start()
    {
        FadeInStart(2f);
        //startCanvasFade.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);

        startBtn.onClick.AddListener(LoadMainScene);
        continueBtn.onClick.AddListener(DeactivateStartMenu);
        restartBtn.onClick.AddListener(ReloadGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    //start the game from start scene
    public void LoadMainScene()
    {
        startBtn.interactable = false;
        StartCoroutine(SwitchToMain());
    }
    IEnumerator SwitchToMain()  //first time loading main scene
    {
        AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);
        yield return load;
        //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
        startSceneObjects.SetActive(false);
        var eventList = FindObjectsOfType<EventSystem>();
        foreach(var i in eventList) if(i != eventsystemInStartScene) Destroy(i.gameObject);
        startBtn.gameObject.SetActive(false);
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
        FadeInStart(1f);
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
        var load = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(1);
        yield return load;
        LoadMainScene();
    }

    public void DisplayStartScreen()
    {
        FadeInStart(2f);
        startCanvasFade.BlockRayCast(true);
        GameManager.Instance.LockCursor(false);
        GameManager.Instance.PauseGame();
    }    

    private Coroutine FadeOutStart(float time = 1f) {
        return StartCoroutine(startCanvasFade.ChangeAlphaOverTime(1f,0f, time));
    }
    private Coroutine FadeInStart(float time = 1f) {
        return StartCoroutine(startCanvasFade.ChangeAlphaOverTime(0f,1f, time));
    }
}
