using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.XR;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class ControlScript : MonoBehaviour
{
    [Header("Wall Pictures")]
    public GameObject[] spawnPoints;
    public GameObject saveManager;

    [Header("UI")]
    public GameObject menu;
    public int visitorSpeed;

    private bool isPaused = false;
    private bool isPauseIn = false;
    private bool isReturnIn = false;
    private bool isNextIn = false;
    private bool isBackIn = false;

    // Start is called before the first frame update
    void Start()
    {
        XRSettings.enabled = true;
        PicturesDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");


        if (!isPaused)
        {
            Vector3 movement = Camera.main.transform.TransformDirection(new Vector3(hAxis, 0, vAxis) * visitorSpeed * Time.deltaTime);
            transform.position = transform.position + movement;
        }

        if (Input.GetButtonDown("Submit") && (isPauseIn))
        {
            PauseClick();
        }

        if (Input.GetButtonDown("Submit") && (isReturnIn))
        {
            ReturnToMenuClick();
        }

        if (Input.GetButtonDown("Submit") && (isNextIn))
        {
            NextClick();
        }

        if (Input.GetButtonDown("Submit") && (isBackIn))
        {
            BackClick();
        }
    }

    public void PicturesDisplay ()
    {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/");
        var files = di.GetFiles().Where(o => o.Name.EndsWith(".json")).ToArray();
        spawnPoints[0].GetComponent<Image>().sprite = saveManager.GetComponent<SaveLoad>().Load(files[0].Name);
        spawnPoints[1].GetComponent<Image>().sprite = saveManager.GetComponent<SaveLoad>().Load(files[1].Name);
        spawnPoints[2].GetComponent<Image>().sprite = saveManager.GetComponent<SaveLoad>().Load(files[2].Name);
    }

    public void NextClick ()
    {

    }

    public void BackClick ()
    {

    }

    public void PauseClick ()
    {
        if (isPaused)
        {
            menu.gameObject.SetActive(false);
            isPaused = false;
        }
        else
        {
            isPaused = true;
            menu.gameObject.SetActive(true);
        }
    }

    public void PauseIn ()
    {
        isPauseIn = true;
    }

    public void PauseOut ()
    {
        isPauseIn = false;
    }

    public void ReturnIn()
    {
        isReturnIn = true;
    }

    public void ReturnOut()
    {
        isReturnIn = false;
    }

    public void NextIn()
    {
        isNextIn = true;
    }

    public void NextOut()
    {
        isNextIn = false;
    }

    public void BackIn()
    {
        isBackIn = true;
    }

    public void BackOut()
    {
        isBackIn = false;
    }

    public void ReturnToMenuClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
