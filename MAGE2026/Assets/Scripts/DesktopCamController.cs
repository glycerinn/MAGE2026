using UnityEngine;

public class DesktopCamController : MonoBehaviour
{
    public Camera main;
    public Camera[] cams;

    bool inDesktop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inDesktop == false && Input.GetKeyDown(KeyCode.E))
        {
            main.gameObject.SetActive(false);
            cams[0].gameObject.SetActive(true);
            inDesktop = true;
        }
        else if(inDesktop == true && Input.GetKeyDown(KeyCode.E))
        {   
            cams[0].gameObject.SetActive(false);
            main.gameObject.SetActive(true);
            inDesktop = false;
        }
    }

    public void onCamButtonClick(string button)
    {
  
    }
}
