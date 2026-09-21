using UnityEngine;

public class DesktopCamController : MonoBehaviour
{
    public Camera main;
    public Camera[] cams;

    public GameObject desktopbuttons;
    public GameObject checkbutton;
    public int currentCam;

    bool inDesktop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCam = 0;
        desktopbuttons.SetActive(false);
        checkbutton.SetActive(true);

        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inDesktop == false && Input.GetKeyDown(KeyCode.E))
        {
            openDesktop();
        }
        else if(inDesktop == true && Input.GetKeyDown(KeyCode.E))
        {   
            closeDesktop();
        }
    }

    public void closeDesktop()
    {
        cams[currentCam].gameObject.SetActive(false);
        main.gameObject.SetActive(true);
        checkbutton.SetActive(true);
        desktopbuttons.SetActive(false);
        inDesktop = false;
    }

    public void openDesktop()
    {
        cams[currentCam].gameObject.SetActive(true);
        main.gameObject.SetActive(false);
        checkbutton.SetActive(false);
        desktopbuttons.SetActive(true);
        inDesktop = true;
    }

    public void onNextButtonClick()
    {
        cams[currentCam].gameObject.SetActive(false);
        currentCam++;
        if (currentCam >= cams.Length)
        {
            currentCam = 0;
        }

        cams[currentCam].gameObject.SetActive(true);
    }

    public void onPrevButtonClick()
    {
        cams[currentCam].gameObject.SetActive(false);
        currentCam--;
        if(currentCam < 0)
        {
            currentCam = cams.Length - 1;
        }
        
        cams[currentCam].gameObject.SetActive(true);
    }
}
