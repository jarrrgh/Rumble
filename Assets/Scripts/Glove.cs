using UnityEngine;
using System.Collections;

public class Glove : MonoBehaviour {

	private float powDuration = 1f;
	private Object powInstance;

	public GameObject pow;
	public GameManager gameManager;
	public SoundManager soundManager;

	void Start () {
	
	}

	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D other) {
		Debug.Log ("Collision " + tag + " hit " + other.gameObject.tag);

		if (other.gameObject.CompareTag ("Glove") || other.gameObject.CompareTag ("Untagged")) {
			return;
		}

		var boxerLayer = (BoxerLayer)other.gameObject.layer;

		if (other.gameObject.CompareTag ("Body")) {
			Debug.Log ("Body");
			gameManager.Damage (boxerLayer, BodyPart.Body);
			soundManager.Play (SoundEffect.Body);
		} else if (other.gameObject.CompareTag ("Head")) {
			Debug.Log ("Head");
			gameManager.Damage (boxerLayer, BodyPart.Head);
			soundManager.Play (SoundEffect.Head);
		} else if (other.gameObject.CompareTag ("Chin")) {
			Debug.Log ("Chin");
			gameManager.Damage (boxerLayer, BodyPart.Chin);
			soundManager.Play (SoundEffect.Chin);
		}

		var contactPoint = other.contacts[0];
		ShowPow (contactPoint);

		/*
		if (CompareTag ("RedBoxerGlove")) {
			if (other.gameObject.CompareTag ("BlueBoxerBody")) {
				Debug.LogError ("BlueBoxerBody");
				ShowPow (contactPoint);
			} else if (other.gameObject.CompareTag ("BlueBoxerHead")) {
				Debug.LogError ("BlueBoxerHead");
				ShowPow (contactPoint);
			}


		} else if (CompareTag ("BlueBoxerGlove")) {
			if (other.gameObject.CompareTag ("RedBoxerBody")) {
				Debug.LogError ("RedBoxerBody");
				ShowPow (contactPoint);
			} else if (other.gameObject.CompareTag ("RedBoxerHead")) {
				Debug.LogError ("RedBoxerHead");
				ShowPow (contactPoint);
			}
		}
		*/
	}

	void ShowPow (ContactPoint2D contactPoint) {
		
		DestroyPow ();
		powInstance = Instantiate(pow, contactPoint.point, Quaternion.identity);
		Invoke ("DestroyPow", powDuration);
	}

	void DestroyPow () {
		Debug.LogError ("POW!!");
		var previousInstance = powInstance;

		if (previousInstance != null) {
			Destroy (previousInstance);
		}
	}
}
