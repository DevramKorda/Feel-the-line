using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioSource[] sfx;



	private void Awake()
	{
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PlaySFX(int sfxToPlay)
	{
		sfx[sfxToPlay].Stop();
		sfx[sfxToPlay].Play();
	}

	public void StopSFX(int sfxToStop)
	{
		sfx[sfxToStop].Stop();
	}
}
