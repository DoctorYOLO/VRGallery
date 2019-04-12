using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("App screens")]
    public GameObject MainMenu;
    public GameObject Settings;
    public GameObject Gallery;

    [Header("Settings screen")]
    public GameObject IdField;
    private string folderId = "1-NymqviIpcoKleDdG1OPkadwhAwCPZhn";

    // Start is called before the first frame update
    void Start()
    {
        XRSettings.enabled = false;
        if (Store.folderId == null)
        {
            IdField.GetComponentInChildren<InputField>().text = folderId;
            Store.folderId = folderId;
        }
        else
        {
            IdField.GetComponentInChildren<InputField>().text = Store.folderId;
        }
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
        Settings.gameObject.SetActive(true);
    }

    public void BackClick()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SaveClick()
    {
        Store.folderId = IdField.GetComponentInChildren<InputField>().text;
        MainMenu.gameObject.SetActive(true);
        Settings.gameObject.SetActive(false);
    }
}
