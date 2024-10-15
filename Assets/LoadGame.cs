using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadGame : MonoBehaviour
{
    public void StartSave(string save)
	{
		if (PlayerPrefs.HasKey(save) && PlayerPrefs.GetString(save) != "")
		{
			//load
		}
		else
		{
			//start new game
			SceneManager.LoadScene("Create-with-VR-Starter-Scene");
		}
	}
}
