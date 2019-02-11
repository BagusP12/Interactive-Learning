using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour {

    public ObjectManager objectManager;
    public CameraController cameraController;
    public CameraImage cameraImage;

    [Space]
    public AudioSource audioSource;

    [Header("UI Component")]
    public GameObject focusButton;
    public GameObject hideOtherObjectButton;
    public GameObject descriptionButton;
    public GameObject unselectObjectButton;
    public GameObject descriptionPanel;
    public GameObject menuPanel;

    [Space]
    public Button rotateCameraButton;
    public Button panCameraButton;
    public Button orbitCameraButton;
    public Button crossSectionButton;
    public Button rotateObjectButton;
    public Button dragObjectButton;

    [Space]
    public TextMeshProUGUI selectedObjectText;
    public TextMeshProUGUI descriptionTitle;
    public TextMeshProUGUI descriptionContent;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseApp();
        }

        if (objectManager.selectedGameObject)
        {
            focusButton.SetActive(true);
            descriptionButton.SetActive(true);
            hideOtherObjectButton.SetActive(true);
            unselectObjectButton.SetActive(true);

            selectedObjectText.text = objectManager.selectedGameObject.name;
        }
        else
        {
            focusButton.SetActive(false);
            descriptionButton.SetActive(false);
            hideOtherObjectButton.SetActive(false);
            unselectObjectButton.SetActive(false);

            selectedObjectText.text = string.Empty;
        }
    }

    public void OpenDescription()
    {
        Debug.Log("Open description");
        descriptionPanel.SetActive(true);

        ObjectScript objectScript = objectManager.selectedGameObject.GetComponent<ObjectScript>();

        descriptionTitle.text = objectScript.name;
        descriptionContent.text = objectScript.objectDescription;

        if (objectScript.audioVoice != null)
        {
            audioSource.clip = objectScript.audioVoice;
        }

        cameraImage.MoveCameraImageToObject();
    }


    public void CloseDescription()
    {
        descriptionPanel.SetActive(false);

        StopAudioSource();
        audioSource.clip = null;

        //Return focused object back to original layer
        objectManager.selectedGameObject.layer = 9;
    }

    public void PlayAudioSource()
    {
        audioSource.Play();
    }

    public void StopAudioSource()
    {
        audioSource.Stop();
    }

    public void PauseAudioSource()
    {
        audioSource.Pause();
    }

    public void PauseApp()
    {
        menuPanel.active = !menuPanel.active;
    }

    public void QuitApp()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }

}
