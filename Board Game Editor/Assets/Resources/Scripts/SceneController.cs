using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneController : MonoBehaviour
{
    public SaveController saveCtrl;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

    }

    public void GoToEditor()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        SceneManager.LoadScene("BoardEditor", LoadSceneMode.Single);
        SFXController.SFXInstance.Audio.PlayOneShot(SFXController.SFXInstance.Click);
    }


    public void GoToMainMenu()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        SFXController.SFXInstance.Audio.PlayOneShot(SFXController.SFXInstance.Click);

    }

    public void GoToPlayGame()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        SceneManager.LoadScene("PlayGame", LoadSceneMode.Single);
        SFXController.SFXInstance.Audio.PlayOneShot(SFXController.SFXInstance.Click);

    }
}
