using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System;

public class PanoControl : MonoBehaviour
{
    // Start Property
    List<JSONNode> tours = new List<JSONNode>();
    public JSONNode currentPanoJson;
    public MainService mainService;
    public UiControl uiControl;
    public GameObject loading;
    public GameObject popUp;
    public Texture2D thumbnailTexture;
    public Texture2D defaultTexture;
    public byte[] bytes;
    // End Property

    /// ======================================================
    /// Get Panorama Texture & Set to Sphere
    /// ======================================================
    public void SetPanoramas(JSONNode tour, Texture2D thumbnail)
    {
        StartCoroutine(mainService.GetTexture(tour["image"], 
            (texture, bytes)=>{
                currentPanoJson = tour;
                this.bytes = bytes;
                SetTexture(texture);
                thumbnailTexture = thumbnail;
                loading.SetActive(false);
            },
            (progress)=>{
                var load = loading.GetComponentInChildren<Text>();
                load.text = "Loading " + Math.Round(progress) + "%";
                loading.SetActive(true);
            })
        );
    }

    /// ======================================================
    /// Set Panorama Texture Local to Sphere
    /// ======================================================
    public void SetPanoLocal(JSONNode tour, Texture2D thumbnail, string path)
    {
        var texture = mainService.GetTextureLocal(path + "/image_pano");
        SetTexture(texture);

    }

    public void SetTexture(Texture texture)
    {
        GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", texture);
    }

    /// ======================================================
    /// Save All Data Panoramas to Local
    /// ======================================================
    public void SavePanoramas()
    {
        var folder = Application.persistentDataPath + "/Panoramas/" + currentPanoJson["id"];
        mainService.SaveJson(folder, "data.json", currentPanoJson.ToString());
        mainService.SaveThumbnail(folder, "thumbnail", thumbnailTexture.EncodeToJPG());
        mainService.SaveTexture(folder, "image_pano", bytes);

        StartCoroutine(uiControl.ShowAlert());
    }

    /// ======================================================
    /// Restore The Texture Sphere to Default
    /// ======================================================
    public void SetTextureDefault()
    {
        SetTexture(defaultTexture);
    } 
}
