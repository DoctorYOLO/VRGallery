﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityGoogleDrive;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Linq;

public class PicturesScrolling : MonoBehaviour
{
    [Header("Prefab for pan in scroll view")]
    public RectTransform panPrefab;
    [Header("Container for pans in scroll view")]
    public RectTransform content;

    [Header("Loading circle")]
    public GameObject loadingWindow;
    public RectTransform loadingTransform;
    public GameObject loadingText;
    private float rotateSpeed = -100f;

    [Header ("Google Drive API")]
    [Range(1, 1000)]
    public int ResultsPerPage = 100;
    private GoogleDriveFiles.DownloadTextureRequest requestTexture;
    private GoogleDriveFiles.ListRequest request;
    private string query = string.Empty;
    private int isDownloadind = 0;

    [Header("Save/Load")]
    public static Sprite vrPicture;
    //---------------------------------------------------------------------TODO--------------------------------------------------
    private string tempName;

    // Start is called before the first frame update
    void Start()
    {
        ListFiles();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDownloadind == 0)
        {
            loadingWindow.gameObject.SetActive(false);
            loadingText.SetActive(true);
            loadingWindow.transform.Find("LoadingCircle").transform.gameObject.SetActive(true);
            loadingWindow.transform.Find("SaveButton").transform.gameObject.SetActive(false);
        }
        if (isDownloadind == 1)
        {
            loadingWindow.gameObject.SetActive(true);
            loadingText.GetComponent<Text>().text = string.Format("{0:P2}", requestTexture.Progress);
            loadingTransform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
        if (isDownloadind == 2)
        {
            loadingText.SetActive(false);
            loadingWindow.transform.Find("LoadingCircle").transform.gameObject.SetActive(false);
            loadingWindow.transform.Find("SaveButton").transform.gameObject.SetActive(true);
        }
    }

    // Request list of files from Google Drive
    private void ListFiles (string nextPageToken = null)
    {
        request = GoogleDriveFiles.List();
        request.Fields = new List<string> { "nextPageToken, files(id, name, thumbnailLink)" };
        request.PageSize = ResultsPerPage;
        request.Q = "mimeType contains 'image' and '" + Store.folderId + "' in parents";
        if (!string.IsNullOrEmpty(query))
            request.Q = string.Format("name contains '{0}'", query);
        if (!string.IsNullOrEmpty(nextPageToken))
            request.PageToken = nextPageToken;
        request.Send().OnDone += BuildResults;
    }

    // Drawing list from Google Drive in menu UI
    private void BuildResults (UnityGoogleDrive.Data.FileList fileList)
    {
        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (var file in fileList.Files)
        {
            var instance = GameObject.Instantiate(panPrefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializePanView(instance, file);
        }
    }

    // Write data using TestItemView
    private async void InitializePanView(GameObject viewPrefab, UnityGoogleDrive.Data.File file)
    {
        TestItemView view = new TestItemView(viewPrefab.transform);
        view.titleText.text = file.Name.Remove(file.Name.IndexOf('.'));
        view.imageThumbnail.sprite = await DownloadThumbnail(file.ThumbnailLink);
        view.downloadButton.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                tempName = file.Name.Remove(file.Name.IndexOf('.'));
                DownloadTexture(file.Id);
            }
        );
    }

    // Class for store data
    public class TestItemView
    {
        public Text titleText;
        public Image imageThumbnail;
        public Image progressBar;
        public Transform downloadButton;

        public TestItemView(Transform rootView)
        {
            titleText = rootView.Find("TitleText").GetComponent<Text>();
            imageThumbnail = rootView.Find("Thumbnail").GetComponent<Image>();
            downloadButton = rootView.Find("DownloadButton");
        }
    }

    private bool NextPageExists ()
    {
        return request != null && 
            request.ResponseData != null && 
            !string.IsNullOrEmpty(request.ResponseData.NextPageToken);
    }

    // Download thumbnail from URL
    private async Task<Sprite> DownloadThumbnail(string id)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(id))
        {
            var asyncOp = www.SendWebRequest();
            while (asyncOp.isDone == false)
            {
                await Task.Delay(1000 / 30);
            }
            var texture = DownloadHandlerTexture.GetContent(www);
            var rect = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, rect, Vector2.one * .5f);
        }

    }

    // Download texture from Google Drive
    private void DownloadTexture(string id)
    {
        isDownloadind = 1;
        requestTexture = GoogleDriveFiles.DownloadTexture(id, true);
        requestTexture.Send().OnDone += RenderImage;
    }

    // Render texture from Google Drive
    private void RenderImage(UnityGoogleDrive.Data.TextureFile textureFile)
    {
        var texture = textureFile.Texture;
        var rect = new Rect(0, 0, texture.width, texture.height);
        vrPicture = Sprite.Create(texture, rect, Vector2.one * .5f);
        vrPicture.name = tempName;
        isDownloadind = 2;
    }

    public void SaveClick()
    {
        isDownloadind = 0;
        ListFiles();
    }

    public void BackClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
