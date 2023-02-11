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
    [SerializeField] private GameObject itemIconPrefab;
	[SerializeField] private GridLayoutGroup iconLayoutParent;
	RectTransform iconLayoutrect;
	public float iconGridHeight;
	public int iconGridCellCount = 2;
    void Awake() => Instance = this;
    private void Start()
    {
        newIndicator.enabled = newItem;
        openBtn = GetComponent<Button>();
        closeBtn.onClick.AddListener(() => OpenCloseWardrobe());
        wardrobeManager.WardrobeInit();
        wardrobeUI.SetActive(false);

        //set up the image layout
        iconLayoutrect = iconLayoutParent.GetComponent<RectTransform> ();
        ClearIconGridLayout();
    }

    public void WardrobeAction(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        OpenCloseWardrobe();
    }
    bool wardrobeOpen = false;
    void OpenCloseWardrobe()
    {
        wardrobeOpen = !wardrobeOpen;
        wardrobeUI.SetActive(wardrobeOpen);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.curtain);
        
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
