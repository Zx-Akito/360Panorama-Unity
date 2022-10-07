using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{
    // Start Property
    List<JSONNode> tours = new List<JSONNode>();
    public JSONNode toursJson;
    public MainService mainService;
    public PanoControl panoControl;
    public GameObject prefab;
    public GameObject logo;
    public GameObject loading;
    public GameObject loadTour;
    public GameObject firstMoreBtn;
    public GameObject secondMoreBtn;
    public GameObject moreDemoBtn;
    public GameObject sidebarMenu;
    public GameObject selectMenu;
    public GameObject downloadMenu;
    public GameObject demoMenu;
    public GameObject alert;
    public GameObject menuSelected;
    public Transform contentScroll;
    public Transform contentDownload;
    public Transform contentTour;
    public Transform contentTourLocal;
    public Transform tourContentLocal;
    public Transform tourContent;
    public Transform tourContentLocalMore;
    public Transform tourContentMore;
    public CameraRotate cam;
    public RawImage bgMenu;
    public RawImage bgSide;
    // End Property

    /// ======================================================
    /// Load Tour Server
    /// ======================================================
    public void LoadTourJson()
    {
        if (toursJson == null)
        {
            StartCoroutine(mainService.GetRequest("https://raw.githubusercontent.com/esa08/panoramas/main/db.json", (result)=>{
                toursJson = result;
                SetGrid(); 
            }));
        }

        else
        {
            SetGrid();
        }
    }

    /// ======================================================
    /// Load Tour Online in Main Menu
    /// ======================================================
    public void SetGrid()
    {
        JSONNode panoramas = toursJson["panoramas"];

        foreach (JSONNode item in panoramas)
        {
            var itemObject = Instantiate<GameObject>(prefab, contentScroll);

            var textObject = itemObject.GetComponentInChildren<Text>();
            textObject.text = item["name"];

            StartCoroutine(mainService.GetTexture(item["thumbnail"], 
                (texture, bytes)=> {
                    var rawImage = itemObject.GetComponent<RawImage>();
                    rawImage.texture = texture;

                    var button = itemObject.GetComponent<Button>();
                    button.onClick.AddListener(() => {

                        panoControl.SetPanoramas(item, texture);

                        sidebarMenu.SetActive(false);
                        selectMenu.SetActive(false);
                        firstMoreBtn.SetActive(true);
                        logo.SetActive(true);
                        cam.enabled = true;
                        bgMenu.enabled = false;
                        bgSide.enabled = false;
                    });
                        loading.SetActive(false);
                },

                (progress) => {
                    loading.SetActive(true);
                } )); 

            tours.Add(item);
        }
   }

    /// ======================================================
    /// Load Tour Online in Game
    /// ======================================================
   public void SetGridMore()
    {
        JSONNode panoramas = toursJson["panoramas"];

        foreach (JSONNode item in panoramas)
        {
            var itemObject = Instantiate<GameObject>(prefab, contentTour);

            var textObject = itemObject.GetComponentInChildren<Text>();
            textObject.text = item["name"];

            StartCoroutine(mainService.GetTexture(item["thumbnail"], 
                (texture, bytes)=> {
                    var rawImage = itemObject.GetComponent<RawImage>();
                    rawImage.texture = texture;

                    var button = itemObject.GetComponent<Button>();
                    button.onClick.AddListener(() => {

                        panoControl.SetPanoramas(item, texture);

                        sidebarMenu.SetActive(false);
                        selectMenu.SetActive(false);
                        firstMoreBtn.SetActive(false);
                        logo.SetActive(true);
                        cam.enabled = false;
                    });
                        loadTour.SetActive(false);
                },

                (progress) => {
                    loadTour.SetActive(true);
                } )); 

            tours.Add(item);
        }
   }

    /// ======================================================
    /// Load Tour Local in Main Menu
    /// ======================================================
    public void LoadTourLocal()
    {
       var directores = Directory.GetDirectories(Application.persistentDataPath + "/Panoramas/");
       foreach (var item in directores)
       {
            var itemObject = Instantiate<GameObject>(prefab, contentDownload);
            var data = File.ReadAllText(item + "/data.json");
            var dataJson = JSON.Parse(data);

            var texture = mainService.GetTextureLocal(item + "/thumbnail");
            var rawImage = itemObject.GetComponent<RawImage>();
            rawImage.texture = texture;
            
            var textObject = itemObject.GetComponentInChildren<Text>();
            textObject.text = dataJson["name"];

            var button = itemObject.GetComponent<Button>();
            button.onClick.AddListener(() => {

                panoControl.SetPanoLocal(item, texture, item);

                sidebarMenu.SetActive(false);
                selectMenu.SetActive(false);
                downloadMenu.SetActive(false);
                secondMoreBtn.SetActive(true);
                logo.SetActive(true);
                cam.enabled = true;
                bgMenu.enabled = false;
                bgSide.enabled = false;
            });
       }
    }

    /// ======================================================
    /// Load Tour Local in Game
    /// ======================================================
    public void LoadTourLocalMore()
    {
       var directores = Directory.GetDirectories(Application.persistentDataPath + "/Panoramas/");
       foreach (var item in directores)
       {
            var itemObject = Instantiate<GameObject>(prefab, contentTourLocal);
            var data = File.ReadAllText(item + "/data.json");
            var dataJson = JSON.Parse(data);

            var texture = mainService.GetTextureLocal(item + "/thumbnail");
            var rawImage = itemObject.GetComponent<RawImage>();
            rawImage.texture = texture;
            
            var textObject = itemObject.GetComponentInChildren<Text>();
            textObject.text = dataJson["name"];

            var button = itemObject.GetComponent<Button>();
            button.onClick.AddListener(() => {

                panoControl.SetPanoLocal(item, texture, item);

                sidebarMenu.SetActive(false);
                selectMenu.SetActive(false);
                downloadMenu.SetActive(false);
                secondMoreBtn.SetActive(false);
                logo.SetActive(true);
                cam.enabled = false;
            });
       }
    }

    /// ======================================================
    /// Alert Saved Success
    /// ======================================================
    public IEnumerator ShowAlert()
    {
        alert.SetActive(true);
        yield return new WaitForSeconds(2);
        alert.SetActive(false);
    }

    /// ======================================================
    /// Destroy Content Online in Main Menu
    /// ======================================================
    public void DestroyContent()
    {
        foreach (Transform item in tourContent)
        {
            Destroy(item.gameObject);
        }
    }

    /// ======================================================
    /// Destroy Content Local in Main Menu
    /// ======================================================
    public void DestroyContentLocal()
    {
        foreach (Transform item in tourContentLocal)
        {
            Destroy(item.gameObject);
        }
    }

    /// ======================================================
    /// Destroy Content Online in Game
    /// ======================================================
    public void DestroyContentMore()
    {
        foreach (Transform item in tourContentMore)
        {
            Destroy(item.gameObject);
        }
    }

    /// ======================================================
    /// Destroy content local in game
    /// ======================================================
    public void DestroyContentLocalMore()
    {
        foreach (Transform item in tourContentLocalMore)
        {
            Destroy(item.gameObject);
        }
    }

    /// ======================================================
    /// Demo Button in Main Menu
    /// ======================================================
    public void DemoBtn(GameObject active)
    {
        if (menuSelected != null)
        {
            menuSelected.transform.Find("Title").GetComponent<Text>()
            .color = MainService.RGBToColor("rgb(255, 255, 255)");
            menuSelected.transform.Find("Icon").GetComponent<Image>()
            .color = MainService.RGBToColor("rgb(120, 120, 120)");
        }

        menuSelected = active;
        active.transform.Find("Title").GetComponent<Text>()
        .color = MainService.RGBToColor("rgb(70, 125, 252)");
        active.transform.Find("Icon").GetComponent<Image>()
        .color = MainService.RGBToColor("rgb(70, 125, 252)");

        demoMenu.SetActive(true);
        selectMenu.SetActive(false);
        downloadMenu.SetActive(false);
    }

    /// ======================================================
    /// Select Button in Main Menu
    /// ======================================================
    public void SelectBtn(GameObject active)
    {
        if (menuSelected != null)
        {
            menuSelected.transform.Find("Title").GetComponent<Text>()
            .color = MainService.RGBToColor("rgb(255, 255, 255)");
            menuSelected.transform.Find("Icon").GetComponent<Image>()
            .color = MainService.RGBToColor("rgb(120, 120, 120)");
        }

        menuSelected = active;
        active.transform.Find("Title").GetComponent<Text>()
        .color = MainService.RGBToColor("rgb(70, 125, 252)");
        active.transform.Find("Icon").GetComponent<Image>()
        .color = MainService.RGBToColor("rgb(70, 125, 252)");

        demoMenu.SetActive(false);
        selectMenu.SetActive(true);
        downloadMenu.SetActive(false);
        DestroyContent();
        LoadTourJson();
    }

    /// ======================================================
    /// Download Button in Main Menu
    /// ======================================================
    public void DownloadedBtn(GameObject active)
    {
        if (menuSelected != null)
        {
            menuSelected.transform.Find("Title").GetComponent<Text>()
            .color = MainService.RGBToColor("rgb(255, 255, 255)");
            menuSelected.transform.Find("Icon").GetComponent<Image>()
            .color = MainService.RGBToColor("rgb(120, 120, 120)");
        }

        menuSelected = active;
        active.transform.Find("Title").GetComponent<Text>()
        .color = MainService.RGBToColor("rgb(70, 125, 252)");
        active.transform.Find("Icon").GetComponent<Image>()
        .color = MainService.RGBToColor("rgb(70, 125, 252)");

        demoMenu.SetActive(false);
        selectMenu.SetActive(false);
        downloadMenu.SetActive(true);
        DestroyContentLocal();
        LoadTourLocal();
    }

    /// ======================================================
    /// Image Button in Demo Menu
    /// ======================================================
    public void ImageBtn()
    {
        cam.enabled = true;
        moreDemoBtn.SetActive(true);
        logo.SetActive(true);
        bgMenu.enabled = false;
        bgSide.enabled = false;
        demoMenu.SetActive(false);
        sidebarMenu.SetActive(false);
    }

}
