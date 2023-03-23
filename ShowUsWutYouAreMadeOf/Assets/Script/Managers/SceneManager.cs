using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    [Header("Canvas")] 
    [SerializeField] GameObject sceneGroup;
    [SerializeField] CanvasFade startCanvasFade, endCanvasFade;
    public CanvasFade blackoutFade, buttonCanvas;

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
        ShowStartCanvas(true);
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
        sceneGroup.SetActive(false);
        startBtn.gameObject.SetActive(false);

        GamestartEvent.Invoke();
        UIManager.Instance.BackToStartEvent.AddListener(ActivateStartMenu);
        //StartCoroutine(SwitchToMain());
    }

    public void QuitGame()
    {
        Debug.Log("quit game!");
        Application.Quit();
    }

    public void ReloadGame()    //reload the game at main scene
    {
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        var load = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Main");
        yield return load;
        var start = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main",LoadSceneMode.Additive);
        yield return start;
        blackoutFade.ChangeAlphaOverTime(0f,1f,1f);
        yield return new WaitForSeconds(2f);
        StartFromBeginning();
    }
#endregion

#region Menu/Canvas
    //load the start scene from main game, called by button in setting screen
    public void ActivateStartMenu()
    {
        blackoutFade.ChangeAlphaOverTime(1f,0f,1f);
        StartCoroutine(FadeInStartUI());
        continueBtn.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
        continueBtn.interactable = true;
        GameManager.Instance.currMode = CurrentMode.StartMenu;
        ShowStartCanvas(true);
    }

    void DeactivateStartMenu()
    {
        blackoutFade.ChangeAlphaOverTime(0f,1f,1f);
        continueBtn.interactable = false;
        InputManager.Instance.EnableAllInput(true);
        GameManager.Instance.BackToLastMode();
        sceneGroup.SetActive(false);
    }

    IEnumerator FadeInStartUI()
    {
        titleText.ClearText();
        buttonCanvas.gameObject.SetActive(false);
        var fade = FadeInStart(1.5f);
        yield return fade;
        StartCoroutine(titleText.Typewrite());
        while(titleText.typing == true)
        {yield return null;}
        buttonCanvas.gameObject.SetActive(true);
        StartCoroutine(buttonCanvas.ChangeAlphaOverTime(0f,1f,.5f));
    }

    public void DisplaysceneGroup()
    {
        FadeInStart(2f);
        startCanvasFade.BlockRayCast(true);
        GameManager.Instance.currMode = CurrentMode.StartMenu;
        GameManager.Instance.PauseGame();
    }    

    public void EndGame()
    {
        Debug.Log("end game");
        ShowStartCanvas(false);
        StartCoroutine(FadeInEndUI());
    }
    IEnumerator FadeInEndUI()
    {
        var continueBtn = endCanvasFade.transform.Find("Continue").GetComponent<CanvasFade>();
        continueBtn.gameObject.SetActive(false);

        var fade = FadeInStart(1.5f);
        yield return fade;
        TextModifier thankTxt = endCanvasFade.transform.Find("Thank").GetComponent<TextModifier>();
        TextModifier endTxt = endCanvasFade.transform.Find("End").GetComponent<TextModifier>();
        StartCoroutine(thankTxt.Typewrite());
        while(thankTxt.typing)
        {yield return null;}
        yield return new WaitForSeconds(2f);
        StartCoroutine(endTxt.Typewrite());
        while(endTxt.typing)
        {yield return null;}
        yield return new WaitForSeconds(1f);
        continueBtn.ChangeAlphaOverTime(0f,1f,1f);
        GameManager.Instance.currMode = CurrentMode.StartMenu;
    }
    void ShowStartCanvas(bool start)
    {
        sceneGroup.SetActive(true);
        startCanvasFade.gameObject.SetActive(start);
        endCanvasFade.gameObject.SetActive(!start);
    }
    private Coroutine FadeOutStart(float time = 1f) {
        return StartCoroutine(blackoutFade.ChangeAlphaOverTime(0f,1f, time));
    }
    private Coroutine FadeInStart(float time = 1f) {
        return StartCoroutine(blackoutFade.ChangeAlphaOverTime(1f,0f, time));
    }
#endregion
}
