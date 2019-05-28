using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ControlScript : MonoBehaviour
{

    public GameObject Menu;
    public int visitorSpeed;

    private bool isMoving = false;
    private bool isPaused = false;
    private bool isPauseIn = false;
    private bool returnIn = false;

    // Start is called before the first frame update
    void Start()
    {
        
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

        if (Input.GetButtonDown("Submit") && (returnIn))
        {
            ReturnToMenuClick();
        }

        /*
        if (Input.GetButton("Fire1") && (!isPaused))
        {
            transform.position = transform.position + Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
        }
        */
    }

    public void PauseClick ()
    {
        if (isPaused)
        {
            Menu.gameObject.SetActive(false);
            isPaused = false;
        }
        else
        {
            isPaused = true;
            Menu.gameObject.SetActive(true);
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
        returnIn = true;
    }

    public void ReturnOut()
    {
        returnIn = false;
    }

    public void ReturnToMenuClick ()
    {
        Store.vrPicture = null;
        SceneManager.LoadScene("Menu");
    }
}
