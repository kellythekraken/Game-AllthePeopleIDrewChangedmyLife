using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WardrobeButton : MonoBehaviour
{
    //attached to the ui wardrobe button. 
    //control everything wardrobe outside of the wardrobe canvas
    //also control the popup ui to display received item

    public static WardrobeButton Instance;
    public GameObject wardrobeUI;
    public Button closeBtn;
    public Image newIndicator;
    [SerializeField] private WardrobeManager wardrobeManager;
    [SerializeField] private Transform wardrobeParent;
    [SerializeField] private GameObject itemPrefab;
    private List<Transform> wardrobeSections;
    private Button openBtn;

    bool newItem = false;
    void Awake() => Instance = this;
    private void Start()
    {
        newIndicator.enabled = newItem;
        openBtn = GetComponent<Button>();
        InputManager.Instance.wardrobeAction.performed += ctx => { OpenCloseWardrobe();};

        closeBtn.onClick.AddListener(() => OpenCloseWardrobe());
        wardrobeManager.WardrobeInit();
        wardrobeUI.SetActive(false);
    }

    bool wardrobeOpen = false;
    void OpenCloseWardrobe()
    {
        wardrobeOpen = !wardrobeOpen;
        wardrobeUI.SetActive(wardrobeOpen);
        if(wardrobeOpen)
        {
            GameManager.Instance.currMode = CurrentMode.Changing;
            if(newItem)
            {
                newItem = false;
                newIndicator.enabled = newItem;
            }
        }
        else{
            GameManager.Instance.BackToLastMode();
        }
    }
    public void DisplayReceivedItem(string npcName, GiftItem gift) //load a list of gift?
    {
        newIndicator.enabled = true;
        newItem = true;
        string text = string.Format("You received {0} from {1}!", gift.name, npcName);
        UIManager.Instance.DisplayItem(text,gift.icon);
    }

}
