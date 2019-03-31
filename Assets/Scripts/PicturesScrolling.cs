using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine;

public class PicturesScrolling : MonoBehaviour
{
    public GameObject panPrefab;
    public int panOffset;

    [Range (0f, 20f)]
    public float snapSpeed;

    private Sprite[] pictures;
    private GameObject[] instPans;
    private Vector2[] pansPos;
    private RectTransform contentRect;
    private Vector2 contentVector;

    private int selectedPanID;
    private bool isScrolling;

    // Start is called before the first frame update
    void Start()
    {
        pictures = Resources.LoadAll<Sprite>("Pictures");
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[pictures.Length];
        pansPos = new Vector2[pictures.Length];
        for (int i = 0; i < pictures.Length; i++)
        {
            panPrefab.GetComponent<Image>().sprite = pictures[i];
            panPrefab.name = pictures[i].name;
            instPans[i] = Instantiate(panPrefab, transform, false);
            instPans[i].name = panPrefab.name;
            instPans[i].GetComponent<Button>().onClick.AddListener(delegate { PictureClick(selectedPanID); });
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, instPans[i].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition; 
        }
    }

    void FixedUpdate()
    {
        float nearestPos = float.MaxValue;
        for (int i = 0; i < pictures.Length; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
        }
        if (isScrolling) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }
    
    public void RightClick()
    {

    }

    public void LeftClick()
    {

    }

    public void PictureClick(int id)
    {
        Store.pictureName = instPans[id].name;
        XRSettings.enabled = true;
        SceneManager.LoadScene("VR");
    }

    public void BackClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
