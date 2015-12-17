using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BodyPart {
	Body,
	Head,
	Chin
}

public enum BoxerLayer {
	None = -1,
	Red = 8,
	Blue = 9
}

public class GameManager : MonoBehaviour {

	public float startDelay = 3f;
	public float endDelay = 3f;
	private WaitForSeconds startWait;
	private WaitForSeconds endWait;

	public Text PrimaryMessage;
	public Text SecondaryMessage;

	private BoxerLayer gameWinner;

	public Boxer redBoxerScript;
	public Boxer blueBoxerScript;

	public GameObject redBoxerEnergyBar;
	public GameObject blueBoxerEnergyBar;

	public SoundManager soundManager;

	enum GameState {
		Staring,
		Playing,
		Ending
	}

	private GameState gameState;

	private float redPlayerEnergy;
	private float bluePlayerEnergy;

	void Start () {
		startWait = new WaitForSeconds (startDelay);
		endWait = new WaitForSeconds (endDelay);

		StartCoroutine (GameLoop ());
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			redBoxerScript.Push ();
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			blueBoxerScript.Push ();
		}

		if (Input.GetMouseButtonDown (0)) {
			if (Input.mousePosition.x < Screen.width / 2) {
				redBoxerScript.Push ();
			}
			if (Input.mousePosition.x > Screen.width / 2) {
				blueBoxerScript.Push ();
			}
		}

		for (int i = 0; i < Input.touches.Length; i++) {
			if (Input.touches[i].position.x < Screen.width / 2) {
				redBoxerScript.Push ();
			}
			if (Input.touches[i].position.x > Screen.width / 2) {
				blueBoxerScript.Push ();
			}
		}
	}

	public void EndGame (BoxerLayer winner) {
		gameWinner = winner;
		gameState = GameState.Ending;
	}

	public void Reset () {
		SetEnergy (BoxerLayer.Red, 1f);
		SetEnergy (BoxerLayer.Blue, 1f);
	}

	private IEnumerator GameLoop ()
	{
		yield return StartCoroutine (RoundStarting ());

		yield return StartCoroutine (RoundPlaying());

		yield return StartCoroutine (RoundEnding());

		if (gameWinner != BoxerLayer.None)
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
		else
		{
			StartCoroutine (GameLoop ());
		}
	}

	private IEnumerator RoundStarting ()
	{
		Debug.Log ("RoundStarting");
		DisableGameControls ();

		gameState = GameState.Staring;

		soundManager.Play (BackgroundMusic.Menu);

		Reset ();

		PrimaryMessage.text = "Are u ready to rumble!!!";
		SecondaryMessage.text = "Controls: left and right arrows or tap.";

		yield return startWait;
	}

	private IEnumerator RoundPlaying ()
	{
		Debug.Log ("RoundPlaying");

		gameState = GameState.Playing;

		EnableGameControls ();

		soundManager.Play (BackgroundMusic.Game);

		PrimaryMessage.text = string.Empty;
		SecondaryMessage.text = string.Empty;

		while (gameState == GameState.Playing)
		{
			yield return null;
		}
	}

	private IEnumerator RoundEnding ()
	{
		Debug.Log ("RoundEnding");

		DisableGameControls ();

		gameState = GameState.Ending;

		soundManager.Play (BackgroundMusic.Menu);

		var boxerBoxerColor = gameWinner == BoxerLayer.Red ? "red" : "blue";

		PrimaryMessage.text = "K.O.";
		SecondaryMessage.text = string.Format ("Boxer in {0} boxers wins!", boxerBoxerColor);

		gameWinner = BoxerLayer.None;

		yield return endWait;
	}

	private void EnableGameControls () {
		redBoxerScript.PushEnabled = true;
		blueBoxerScript.PushEnabled = true;
	}

	private void DisableGameControls () {
		redBoxerScript.PushEnabled = false;
		blueBoxerScript.PushEnabled = false;
	}

	public void Damage (BoxerLayer boxerType, BodyPart bodyPart) {
		float damage = 0f;

		switch (bodyPart) {
		case BodyPart.Head:
			damage = 0.2f;
			break;
		case BodyPart.Chin:
			damage = 0.4f;
			break;
		case BodyPart.Body:
		default:
			damage = 0.1f;
			break;
		}

		var energy = boxerType == BoxerLayer.Red ? redPlayerEnergy : bluePlayerEnergy;
		energy = Mathf.Max (0f, energy - damage);

		SetEnergy (boxerType, energy);

		Debug.Log ("damage " + damage);
		Debug.Log ("energy " + energy);
	}

	private void SetEnergy (BoxerLayer boxer, float energy) {
		GameObject energyBar;

		if (boxer == BoxerLayer.Red) {
			redPlayerEnergy = energy;
			energyBar = redBoxerEnergyBar;
		} else {
			bluePlayerEnergy = energy;
			energyBar = blueBoxerEnergyBar;
		}

		energyBar.transform.localScale = new Vector3 (energy, 1f, 1f);
		energyBar.transform.localPosition = new Vector3 ((1f - energy) / 2, 0f, -1f);

		if (energy == 0f) {
			EndGame (boxer == BoxerLayer.Red ? BoxerLayer.Blue : BoxerLayer.Red);
		}
	}
}
