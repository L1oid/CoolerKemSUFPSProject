using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

	float randomTime;
	bool routineStarted = false;
	public bool isHit = false;

	[Header("Customizable Options")]
	public float minTime;
	public float maxTime;

	[Header("Audio")]
	public AudioClip upSound;
	public AudioClip downSound;

	public AudioSource audioSource;
	
	private void Update () {
		
		randomTime = Random.Range (minTime, maxTime);

		if (isHit == true) 
		{
			if (routineStarted == false) 
			{
				gameObject.GetComponent<Animation> ().Play("target_down");

				audioSource.GetComponent<AudioSource>().clip = downSound;
				audioSource.Play();

				StartCoroutine(DelayTimer());
				routineStarted = true;
			} 
		}
	}

	private IEnumerator DelayTimer () {
		yield return new WaitForSeconds(randomTime);
		gameObject.GetComponent<Animation> ().Play ("target_up");

		audioSource.GetComponent<AudioSource>().clip = upSound;
		audioSource.Play();

		isHit = false;
		routineStarted = false;
	}
}