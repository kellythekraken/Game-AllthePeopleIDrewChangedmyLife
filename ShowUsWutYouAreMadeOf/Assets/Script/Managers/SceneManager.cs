using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    [Header("Canvas")] 
    [SerializeField] private GameObject startSceneObjects;
    public CanvasFade startCanvasFade, blackoutFade, buttonCanvas;
    [Header("UI")]
    [SerializeField] private TextModifier titleText;
    [SerializeField] private Button startBtn, restartBtn, continueBtn, quitBtn;
    internal UnityEvent GamestartEvent;
    void Awake() => Instance = this;
    void Start()
    {
        blackoutFade.gameObject.SetActive(true);
        StartCoroutine(FadeInStartUI());
        LoadMainGameUponStart();
        restartBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);

        startBtn.onClick.AddListener(StartFromBeginning);
        continueBtn.onClick.AddListener(DeactivateStartMenu);
        restartBtn.onClick.AddListener(ReloadGame);
        quitBtn.onClick.AddListener(QuitGame);
        if (GamestartEvent == null)
            GamestartEvent = new UnityEvent();
    }

#region Scene Loading
    public void LoadMainGameUponStart()
    {
        AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main",LoadSceneMode.Additive);
    }
    public void StartFromBeginning()
    {
        startBtn.interactable = false;
        startSceneObjects.SetActive(false);
        startBtn.gameObject.SetActive(false);

        GamestartEvent.Invoke();
        //StartCoroutine(SwitchToMain());
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
        var load = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Main");
        yield return load;
        StartFromBeginning();
    }
#endregion

#region Menu/Canvas
    //load the start scene from main game, called by button in setting screen
    public void ActivateStartMenu()
    {
        StartCoroutine(FadeInStartUI());
        continueBtn.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
        continueBtn.interactable = true;
        GameManager.Instance.currMode = CurrentMode.StartMenu;
        startSceneObjects.SetActive(true);
    }

    void DeactivateStartMenu()
    {
        continueBtn.interactable = false;
        InputManager.Instance.EnableAllInput(true);
        GameManager.Instance.BackToLastMode();
        startSceneObjects.SetActive(false);
    }

    IEnumerator FadeInStartUI()
    {
        buttonCanvas.gameObject.SetActive(false);
        var fade = FadeInStart(1.5f);
        yield return fade;
        StartCoroutine(titleText.Typewrite());
        while(titleText.typing == true)
        {yield return null;}
        buttonCanvas.gameObject.SetActive(true);
        StartCoroutine(buttonCanvas.ChangeAlphaOverTime(0f,1f,1f));
    }
    public void DisplayStartScreen()
    {
        FadeInStart(2f);
        startCanvasFade.BlockRayCast(true);
        GameManager.Instance.currMode = CurrentMode.StartMenu;
        GameManager.Instance.PauseGame();
    }    

    private Coroutine FadeOutStart(float time = 1f) {
        return StartCoroutine(blackoutFade.ChangeAlphaOverTime(0f,1f, time));
    }
    private Coroutine FadeInStart(float time = 1f) {
        return StartCoroutine(blackoutFade.ChangeAlphaOverTime(1f,0f, time));
    }
#endregion
}
