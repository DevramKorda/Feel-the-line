using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {

	void Start ()
	{
		//!!! очистка сохранённых пересенных
		//PlayerPrefs.DeleteAll();

		//чтобы музыка продолжала играть
		GameObject.FindGameObjectWithTag("Music").GetComponent<MusicControl>().PlayMusic();
	}

	void Update ()
	{	
		
	}

	public void StartGame()
	{
		StartCoroutine(EnumStartGame());	
	}

	public IEnumerator EnumStartGame()
	{
		AudioManager.instance.PlaySFX(0);

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene("Level_Select");
	}
}
