using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void QuitGame() {
#if UNITY_EDITOR //如果是在编辑器环境下
        UnityEditor.EditorApplication.isPlaying = false;
#else//在打包出来的环境下
        Application.Quit();
#endif    
    }
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);    
    }

}
