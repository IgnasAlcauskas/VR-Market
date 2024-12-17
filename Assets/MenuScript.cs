using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    public void Play()
	{
		Debug.Log("Play PRESSED");
		SceneManager.LoadScene(0);
	}

	public void QuitGame()
	{
		Debug.Log("QuitGame PRESSED");
		Application.Quit();
	}

	public void QuitToMainMenu()
	{
		Debug.Log("QuitToMainMenu PRESSED");
		SceneManager.LoadScene(1);
	}
}
