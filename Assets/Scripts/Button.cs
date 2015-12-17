using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public GameObject boxer;

	void Awake () {
	}

	void Start () {
	
	}

	void Update () {
	
	}

	void OnMouseDown() {
		var boxerScript = (Boxer) boxer.GetComponent(typeof(Boxer));
		boxerScript.Push ();
	}
}
