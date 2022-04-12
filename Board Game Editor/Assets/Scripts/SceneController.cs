using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneController : MonoBehaviour
{
    
    private Scene activeScene;
    public SaveController saveCtrl;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        activeScene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Scene scene = SceneManager.GetSceneByName("MainMenu");
        activeScene = SceneManager.GetActiveScene();
    }

    public void GoToEditor(){
        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        SceneManager.LoadScene("BoardEditor", LoadSceneMode.Single);
        activeScene = SceneManager.GetActiveScene();
    }
    

    public void GoToMainMenu(){
        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        activeScene = SceneManager.GetActiveScene();
    }

    public void GoToPlayGame(){
        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        activeScene = SceneManager.GetActiveScene();
    } 

    public Scene GetActiveScene(){
        return activeScene;
    }
}
