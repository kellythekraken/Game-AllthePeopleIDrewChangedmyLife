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
        InputManager.Instance.wardrobeAction.performed += ctx => { OpenCloseWardrobe();};

        closeBtn.onClick.AddListener(() => OpenCloseWardrobe());
        wardrobeManager.WardrobeInit();
        wardrobeUI.SetActive(false);

        //set up the image layout
        iconLayoutrect = iconLayoutParent.GetComponent<RectTransform> ();
        ClearIconGridLayout();
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
    public void DisplayReceivedItem(string giftLine, GiftItem[] gifts) //load a list of gift?
    {
        newIndicator.enabled = true;
        newItem = true;
        string text = giftLine;
        //for multiple items
       for (int i =0 ; i < gifts.Length ; i++)
        {
            GameObject icon = Instantiate(itemIconPrefab, iconLayoutParent.transform);
            Image iconImage = icon.GetComponent<Image>();
            iconImage.sprite = gifts[i].icon;
            wardrobeManager.AddItemToWardrobe(gifts[i]);
        }

        iconGridCellCount = iconLayoutParent.GetComponentsInChildren<Image>().Length;
        if(iconGridCellCount> 4) iconGridCellCount = 4; else if(iconGridCellCount < 2) iconGridCellCount = 2;
        var cellSize = iconLayoutrect.rect.width / iconGridCellCount; 
        iconLayoutParent.cellSize = new Vector2 (cellSize, cellSize);
        UIManager.Instance.DisplayItem(text);
    }

    void OnRectTransformDimensionsChange()
	{
        Debug.Log("change dimension");
			if (iconLayoutParent != null && iconLayoutrect != null)
			if ((iconLayoutrect.rect.height + (iconLayoutParent.padding.horizontal * 2)) * iconGridCellCount < iconLayoutrect.rect.width)
					iconLayoutParent.cellSize = new Vector2 (iconLayoutrect.rect.height, iconLayoutrect.rect.height);
	}

    void ClearIconGridLayout()
    {
        foreach(Transform i in iconLayoutParent.transform) Destroy(i.gameObject); 
    }

}
