using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFromTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ExitFromTut()
	{
		StartCoroutine(EnumExitFromTut());
	}

	public IEnumerator EnumExitFromTut()
	{
		AudioManager.instance.PlaySFX(0);

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene("Level_Select");
	}
}
