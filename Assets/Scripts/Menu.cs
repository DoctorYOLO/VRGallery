using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject AddImage;
    public GameObject Gallery;

    // Start is called before the first frame update
    void Start()
    {
        XRSettings.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClick()
    {
        MainMenu.gameObject.SetActive(false);
        Gallery.gameObject.SetActive(true);
    }

    public void SettingsClick()
    {
        MainMenu.gameObject.SetActive(false);
        AddImage.gameObject.SetActive(true);
    }

    public void BackClick()
    {
        MainMenu.gameObject.SetActive(true);
        AddImage.gameObject.SetActive(false);
    }

    public void StartVR()
    {
        XRSettings.enabled = true;
        SceneManager.LoadScene("VR");
    }
}
