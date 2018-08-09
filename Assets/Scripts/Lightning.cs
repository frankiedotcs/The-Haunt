using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lightning : MonoBehaviour
{
	private System.Random rnd = new System.Random();
	//public GameObject whitePanel;
	//public Image lightningFlash;
	public float calmDuration;
	public float lastFlash;
	public float flashTimeRemaining;
	public Animator anim;
	private void Start()
	{
		calmDuration = (float)rnd.Next(3, 15);
		lastFlash = Time.time;
	}
	private void Update()
	{
		if (Time.time - lastFlash > calmDuration)
		{
			anim.SetTrigger("Flash1");
			lastFlash = Time.time;
			calmDuration = (float)rnd.Next(10);
			SoundManager.instance.PlaySingle(SoundManager.instance.thunder1);
		}
		flashTimeRemaining = Time.time - lastFlash;
	}
}