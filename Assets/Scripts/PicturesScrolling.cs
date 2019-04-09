using System.Collections;
using System.Collections.Generic;
using UnityGoogleDrive;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine;

public class PicturesScrolling : MonoBehaviour
{
    [Header("Prefab for pan in scroll view")]
    public GameObject panPrefab;
    public int panOffset;

    [Range (0f, 20f)]
    public float snapSpeed;

    [Header("Resources method")]
    private Sprite[] pictures;
    private GameObject[] instPans;
    private Vector2[] pansPos;
    private RectTransform contentRect;
    private Vector2 contentVector;

    private int selectedPanID;
    private bool isScrolling;

    [Header ("Google Drive API")]
    [Range(1, 1000)]
    public int ResultsPerPage = 100;
    private GoogleDriveFiles.DownloadTextureRequest requestTexture;
    private GoogleDriveFiles.ListRequest request;
    private string query = string.Empty;
    private int i = -1;
    private int pressedButton;

    // Start is called before the first frame update
    void Start()
    {
        // Using Google Drive API

        ListFiles();

        // Using resources 
        /*
        pictures = Resources.LoadAll<Sprite>("Pictures");
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[pictures.Length];
        pansPos = new Vector2[pictures.Length];
        for (int i = 0; i < pictures.Length; i++)
        {
            panPrefab.GetComponentInChildren<Image>().GetComponent<Image>().sprite = pictures[i];
            panPrefab.GetComponentInChildren<Text>().text = pictures[i].name;
            panPrefab.name = pictures[i].name;
            instPans[i] = Instantiate(panPrefab, transform, false);
            instPans[i].name = panPrefab.name;
            instPans[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { PictureClick(selectedPanID); });
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i].transform.localPosition.x, instPans[i - 1].transform.localPosition.y - panPrefab.GetComponent<RectTransform>().localPosition.y + panOffset);
            pansPos[i] = instPans[i].transform.localPosition; 
        }
        */
    }

    void FixedUpdate()
    {
        // Inertia for scroll view 
       /*
        float nearestPos = float.MaxValue;
        for (int a = 0; a < 2; a++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.y - pansPos[a].y);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = a;
            }
        }
        if (isScrolling) return;
        contentVector.y = Mathf.SmoothStep(contentRect.anchoredPosition.y, pansPos[selectedPanID].y, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Request list of files from Google Drive
    private void ListFiles (string nextPageToken = null)
    {
        request = GoogleDriveFiles.List();
        request.Fields = new List<string> { "nextPageToken, files(id, name, thumbnailLink)" };
        request.PageSize = ResultsPerPage;
        request.Q = "mimeType contains 'image' and '1-NymqviIpcoKleDdG1OPkadwhAwCPZhn' in parents";
        if (!string.IsNullOrEmpty(query))
            request.Q = string.Format("name contains '{0}'", query);
        if (!string.IsNullOrEmpty(nextPageToken))
            request.PageToken = nextPageToken;
        request.Send().OnDone += BuildResults;
    }

    // Drawing list from Google Drive in menu UI
    private void BuildResults (UnityGoogleDrive.Data.FileList fileList)
    {
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[fileList.Files.Count];
        pansPos = new Vector2[fileList.Files.Count];

        foreach (var file in fileList.Files)
        {
            
            i++;
            instPans[i] = Instantiate(panPrefab, transform, false);
            StartCoroutine(DownloadThumbnail(file.ThumbnailLink));
            instPans[i].name = file.Id;
            instPans[i].GetComponentInChildren<Text>().text = file.Name;

            instPans[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { DownloadTexture(instPans[i].name); });

            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i].transform.localPosition.x, instPans[i - 1].transform.localPosition.y - panPrefab.GetComponent<RectTransform>().localPosition.y + panOffset);
            pansPos[i] = instPans[i].transform.localPosition;
        }
    }

    private bool NextPageExists ()
    {
        return request != null && 
            request.ResponseData != null && 
            !string.IsNullOrEmpty(request.ResponseData.NextPageToken);
    }

    // Download thumbnail from URL
    private IEnumerator DownloadThumbnail(string id)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(id);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var texture = DownloadHandlerTexture.GetContent(www);
            var rect = new Rect(0, 0, texture.width, texture.height);
            instPans[i].GetComponentInChildren<Image>().GetComponent<Image>().sprite = Sprite.Create(texture, rect, Vector2.one * .5f);
        }
    }

    
    // Download texture from Google Drive
    private void DownloadTexture(string id)
    {
        pressedButton = i;
        requestTexture = GoogleDriveFiles.DownloadTexture(id, true);
        Debug.Log(pressedButton);
        requestTexture.Send().OnDone += RenderImage;
    }

    // Render texture from Google Drive
    private void RenderImage(UnityGoogleDrive.Data.TextureFile textureFile)
    {
        var texture = textureFile.Texture;
        var rect = new Rect(0, 0, texture.width, texture.height);
        Store.vrPicture = Sprite.Create(texture, rect, Vector2.one * .5f);
        GameObject DownloadButton = instPans[pressedButton].transform.Find("DownloadButton").gameObject;
        GameObject StartButton = instPans[pressedButton].transform.Find("StartButton").gameObject;
        DownloadButton.SetActive(false);
        StartClick();
        StartButton.SetActive(true);
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }
   
    public void StartClick()
    {
        XRSettings.enabled = true;
        SceneManager.LoadScene("VR");
    }

    public void BackClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
