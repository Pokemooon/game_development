using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void QuitGame() {
#if UNITY_EDITOR //������ڱ༭��������
        UnityEditor.EditorApplication.isPlaying = false;
#else//�ڴ�������Ļ�����
        Application.Quit();
#endif    
    }
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);    
    }
    public void ReturnMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);    
        Time.timeScale = 1f;
    }

    //��ȡ���ݿ�Ľ�ɫλ�ý�����Ϸ
    public void ResumeGame() {
        
    }
}
