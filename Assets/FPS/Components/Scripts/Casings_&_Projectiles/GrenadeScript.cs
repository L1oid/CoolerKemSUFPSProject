using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour {

	[Header("Timer")]

	[Tooltip("Time before the grenade explodes")]
	public float grenadeTimer = 5.0f;

	[Header("Explosion Prefabs")]
	public Transform explosionPrefab;

	[Header("Explosion Options")]
	[Tooltip("The radius of the explosion force")]
	public float radius = 25.0F;
	[Tooltip("The intensity of the explosion force")]
	public float power = 350.0F;

	[Header("Throw Force")]
	[Tooltip("Minimum throw force")]
	public float minimumForce = 1500.0f;
	[Tooltip("Maximum throw force")]
	public float maximumForce = 2500.0f;
	private float throwForce;

	[Header("Audio")]
	public AudioSource impactSound;

	private void Awake () 
	{
		throwForce = Random.Range
			(minimumForce, maximumForce);

		GetComponent<Rigidbody>().AddRelativeTorque 
		   (Random.Range(500, 1500), //X Axis
			Random.Range(0,0), 		 //Y Axis
			Random.Range(0,0)  		 //Z Axis
			* Time.deltaTime * 5000);
	}

	private void Start () 
	{
		GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * throwForce);

		StartCoroutine (ExplosionTimer ());
	}

	private void OnCollisionEnter (Collision collision) 
	{
		impactSound.Play ();
	}

	private IEnumerator ExplosionTimer () 
	{
		yield return new WaitForSeconds(grenadeTimer);

		RaycastHit checkGround;
		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			Instantiate (explosionPrefab, checkGround.point, 
				Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (power * 5, explosionPos, radius, 3.0F);
			
			if (hit.GetComponent<Collider>().tag == "Target" 
			    	&& hit.gameObject.GetComponent<TargetScript>().isHit == false) 
			{
				hit.gameObject.GetComponent<Animation> ().Play("target_down");
				hit.gameObject.GetComponent<TargetScript>().isHit = true;
			}

			if (hit.GetComponent<Collider>().tag == "ExplosiveBarrel") 
			{
				hit.gameObject.GetComponent<ExplosiveBarrelScript> ().explode = true;
			}
		}

		Destroy (gameObject);
	}
}