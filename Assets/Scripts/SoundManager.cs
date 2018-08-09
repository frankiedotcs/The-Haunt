using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance;
	private AudioSource oneShotSource;
	private List<AudioSource> sources = new List<AudioSource>();
	public AudioClip[] music;
	public AudioClip thunder1;

	// Use this for initialization
	void Awake()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void PlaySingle(AudioClip clip)
	{
		oneShotSource = getFXSource();
		oneShotSource.clip = clip;
		oneShotSource.Play();
	}

	private AudioSource getFXSource()
	{
		foreach (AudioSource source in sources)
		{
			if (!source.isPlaying)
			{
				return source;
			}
		}
		AudioSource _source = gameObject.AddComponent<AudioSource>();
		_source.playOnAwake = false;
		sources.Add(_source);
		return _source;
	}
}
