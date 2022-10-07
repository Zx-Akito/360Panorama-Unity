using System.Collections;
using System.IO;
using UnityEngine;
using SimpleJSON;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MainService : MonoBehaviour
{
    /// ======================================================
    /// Get JSON in Server
    /// ======================================================
    public IEnumerator GetRequest(string uri, Action<JSONNode> result)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                break;

                case UnityWebRequest.Result.Success:
                    var json = JSONNode.Parse(webRequest.downloadHandler.text);
                    result.Invoke(json);
                break;
            }
        }
    }

    /// ======================================================
    /// Get Texture in JSON
    /// ======================================================
    public IEnumerator GetTexture(string url, Action<Texture2D, byte[]> actionSuccess, Action<float> actionProgress)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            // hit request
            AsyncOperation request = webRequest.SendWebRequest();
            
            // Response Progress
            while (webRequest.result == UnityWebRequest.Result.InProgress)
            {
                if(actionProgress != null)
                {
                    float progress = webRequest.downloadProgress * 100;
                    actionProgress.Invoke(progress);
                } 

                yield return null;
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("Loading...");
                break;

                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(": Conenction Error: " + webRequest.error);
                    Debug.LogError(url);
                break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                break;

                case UnityWebRequest.Result.Success:
                    byte[] bytes = webRequest.downloadHandler.data;
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    actionSuccess.Invoke(texture, bytes);
                break;
            }
        }
    }
    
    /// ======================================================
    /// Get Texture From Local
    /// ======================================================
    public Texture2D GetTextureLocal(string path)
    {
        if (File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(16, 16, TextureFormat.PVRTC_RGBA4, false);
            tex.LoadImage(bytes);

            return tex;
        }
        
        else
        {
            return null;
        }
    }

    /// ======================================================
    /// Save JSON to Local Storage
    /// ======================================================
    public void SaveJson(string folder, string fileName, string jsonData)
    {
        var path = folder + "/" + fileName;

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        File.AppendAllText(path, jsonData);
    }

    /// ======================================================
    /// Save Thumbnail to Local Storage
    /// ======================================================
    public void SaveThumbnail(string folder, string fileName, byte[] bytes)
    {
        var path = folder + "/" + fileName;

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        File.WriteAllBytes(path, bytes);
    }

    /// ======================================================
    /// Save Texture to Local Storage
    /// ======================================================
    public void SaveTexture(string folder, string fileName, byte[] bytes)
    {
        var path = folder + "/" + fileName;

        if(!Directory.Exists(folder)) 
        {
            Directory.CreateDirectory(folder);
        }

        File.WriteAllBytes(path, bytes);
    }

    /// ======================================================
    /// Fix The Inverted Gyro Position
    /// ======================================================
    public void FixGyro(GameObject fixGyro)
    {
        fixGyro.transform.localRotation = Quaternion.Euler(90,0,0);
    }

    /// ======================================================
    /// Exit Game
    /// ======================================================
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Game Close");
    }

    /// ======================================================
    /// Convert RGB to Color unity
    /// ======================================================
    public static Color RGBToColor(string rgb)
    {
        string[] array = rgb.Replace("rgb(", "").Replace(")", "").Split(',');
        return new Color(
            float.Parse(array[0]) / 255,
            float.Parse(array[1]) / 255,
            float.Parse(array[2]) / 255,
            1);
    }

    /// ======================================================
    /// Convert RGBA to Color unity
    /// ======================================================
    public static Color RGBAToColor(string rgba)
    {
        List<string> array = new List<string>(rgba.Replace("rgba(", "").Replace(")", "").Split(','));
        if(array.Count != 4) {
			array.Add("1");
		}

        string opacity = array[3].IndexOf(" .") == 0 ? array[3].Replace(" ","0") : 
                         array[3].IndexOf(".") == 0  ? "0"+array[3] : array[3];

        Color color = new Color(
            float.Parse(array[0]) / 255f,
            float.Parse(array[1]) / 255f,
            float.Parse(array[2]) / 255f,
            float.Parse(opacity, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
        
        return color;
    }

}
