using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour {

    public int gameSceneId = 1;

    public void NewGame() {
        SceneManager.LoadScene(gameSceneId);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}
