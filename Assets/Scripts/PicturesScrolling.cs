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
    public RectTransform panPrefab;
    [Header("Container for pans in scroll view")]
    public RectTransform content;

    //public Image testImage;

    [Header ("Google Drive API")]
    [Range(1, 1000)]
    public int ResultsPerPage = 100;
    private GoogleDriveFiles.DownloadTextureRequest requestTexture;
    private GoogleDriveFiles.ListRequest request;
    private string query = string.Empty;
    private Sprite thumbnailPicture;


    // Start is called before the first frame update
    void Start()
    {
        ListFiles();
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
        int panCount = fileList.Files.Count;

        foreach (var file in fileList.Files)
        {
            var instance = GameObject.Instantiate(panPrefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializePanView(instance, file);
        }
    }

    // Write data using TestItemView
    private void InitializePanView(GameObject viewPrefab, UnityGoogleDrive.Data.File file)
    {
        TestItemView view = new TestItemView(viewPrefab.transform);
        view.titleText.text = file.Name;
        StartCoroutine(DownloadThumbnail(file.ThumbnailLink));
        view.imageThumbnail.sprite = thumbnailPicture;
        view.downloadButton.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                DownloadTexture(file.Id);
            }
        );
    }

    // Class for store data
    public class TestItemView
    {
        public Text titleText;
        public Image imageThumbnail;
        public Transform downloadButton;
        public Button startButton;

        public TestItemView(Transform rootView)
        {
            titleText = rootView.Find("TitleText").GetComponent<Text>();
            imageThumbnail = rootView.Find("Thumbnail").GetComponent<Image>();
            downloadButton = rootView.Find("DownloadButton");
            //startButton = rootView.Find("StartButton").GetComponent<Button>();
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
        var www = UnityWebRequestTexture.GetTexture(id);
        yield return www.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(www);
        var rect = new Rect(0, 0, texture.width, texture.height);
        thumbnailPicture = Sprite.Create(texture, rect, Vector2.one * .5f);
    }

    // Download texture from Google Drive
    private void DownloadTexture(string id)
    {
        requestTexture = GoogleDriveFiles.DownloadTexture(id, true);
        requestTexture.Send().OnDone += RenderImage;
    }

    // Render texture from Google Drive
    private void RenderImage(UnityGoogleDrive.Data.TextureFile textureFile)
    {
        var texture = textureFile.Texture;
        var rect = new Rect(0, 0, texture.width, texture.height);
        Store.vrPicture = Sprite.Create(texture, rect, Vector2.one * .5f);
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
