using UnityEngine;
using System.Collections;

public class Boxer : MonoBehaviour {

	public float pushForce = 1000f;

	private Rigidbody2D rb2d;

	private bool pushEnabled;
	public bool PushEnabled {
		get {
			return pushEnabled;
		}
		set {
			pushEnabled = value;
		}
	}

	void Start () {
		rb2d = GetComponentInChildren<Rigidbody2D>();
	}

	void Update () {
		
	}

	public void Push () {
		if (pushEnabled) {
			rb2d.AddForce (new Vector2 (pushForce, 0f));
		}
	}
}
