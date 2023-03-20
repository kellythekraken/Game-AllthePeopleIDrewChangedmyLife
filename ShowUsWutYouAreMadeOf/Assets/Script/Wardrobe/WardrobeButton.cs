using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WardrobeButton : MonoBehaviour
{
    //attached to the ui wardrobe button. 
    //control everything wardrobe outside of the wardrobe canvas
    //also control the popup ui to display received item

    [Header("Wardrobe Canvas")]
    public GameObject wardrobeUI;   //the entire wardrobe canvas
    public Image newIndicator;
    [SerializeField] private WardrobeManager wardrobeManager;
    [SerializeField] GameObject changingScreen,customizeScreen; //show customize screen on start
    [SerializeField] Button closeBtn, customizeFinishBtn;
    [SerializeField] private Transform wardrobeParent;
    [SerializeField] private GameObject itemPrefab;

    [Header("Pop up UI")]
    [SerializeField] private GameObject itemIconPrefab;
	[SerializeField] private GridLayoutGroup iconLayoutParent;
	RectTransform iconLayoutrect;
	public float iconGridHeight;
	public int iconGridCellCount = 2;
    InputManager inputManager;
    private List<Transform> wardrobeSections;
    private Button openBtn;
    bool newItem = false;
    GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        inputManager = InputManager.Instance;
        newIndicator.enabled = newItem;
        openBtn = GetComponent<Button>();
        closeBtn.onClick.AddListener(() => OpenCloseWardrobe());
        customizeFinishBtn.onClick.AddListener(()=>CustomizeFinished());
        wardrobeManager.WardrobeInit();
        wardrobeUI.SetActive(false);

        //default set customize to false
        customizeScreen.SetActive(false);
        changingScreen.SetActive(true);
        
        //set up the image layout
        iconLayoutrect = iconLayoutParent.GetComponent<RectTransform> ();
        ClearIconGridLayout();
    }
    void OnEnable() 
    {
        if(inputManager != null) inputManager.wardrobeAction.Enable();
    }
    void OnDisable() => inputManager.wardrobeAction.Disable();
    public void WardrobeAction(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        OpenCloseWardrobe();
    }

#region GameStartCustomization
    //called by gamemanager
    public void PlayerCustomization()
    {
        //load the wardrobe init!
        
        Debug.Log("show player customize screen");
        customizeScreen.SetActive(true);
        changingScreen.SetActive(false);
        wardrobeUI.SetActive(true);

        // you can customize your skin, hair and eye color
    }
    //done button is clicked
    void CustomizeFinished()
    {
        gm.FadeIn();
        gm.StartGameCinematic();
        wardrobeUI.SetActive(false);
        customizeScreen.SetActive(false);
        changingScreen.SetActive(true);
    }
#endregion

    bool wardrobeOpen = false;
    void OpenCloseWardrobe()
    {
        wardrobeOpen = !wardrobeOpen;
        wardrobeUI.SetActive(wardrobeOpen);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.curtain);
        
        if(wardrobeOpen)
        {
            gm.currMode = CurrentMode.Changing;
            if(newItem)
            {
                newItem = false;
                newIndicator.enabled = newItem;
            }
        }
        else{
            gm.BackToLastMode();
        }
    }
    //load a list of gift
    public void DisplayReceivedItem(QueerNPC npc) 
    {
        //clear child
        foreach(Transform i in iconLayoutParent.transform) Destroy(i.gameObject);

        newIndicator.enabled = true;
        newItem = true;
        GiftItem[] gifts = npc.queerID.items;
        
        //for multiple items
       for (int i =0 ; i < gifts.Length ; i++)
        {
            GameObject icon = Instantiate(itemIconPrefab, iconLayoutParent.transform);
            Image iconImage = icon.GetComponent<Image>();
            iconImage.sprite = gifts[i].icon;
            wardrobeManager.AddGiftToWardrobe(gifts[i], npc.queerID.npcName);
        }
        
        //manipulate the item icon scales based on numbers
        iconGridCellCount = iconLayoutParent.GetComponentsInChildren<Image>().Length;
        if(iconGridCellCount> 4) iconGridCellCount = 4; else if(iconGridCellCount < 2) iconGridCellCount = 2;
        var cellSize = iconLayoutrect.rect.width / iconGridCellCount; 
        iconLayoutParent.cellSize = new Vector2 (cellSize, cellSize);
        UIManager.Instance.DisplayItem(npc);
    }

    void ClearIconGridLayout()
    {
        foreach(Transform i in iconLayoutParent.transform) Destroy(i.gameObject); 
    }

}
