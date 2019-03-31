using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour
{

    public int visitorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetButton("Vertical"))
        {
            transform.position = transform.position + Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
        }

        if (Input.GetButton("Horizontal"))
        {
            transform.position = transform.position - Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
        }
    
    }
}
