using UnityEngine;
using System.Collections;

public enum BackgroundMusic {
	Menu,
	Game
}

public enum SoundEffect {
	Body,
	Head,
	Chin
}

public class SoundManager : ScriptableObject {

	public AudioSource menu;
	public AudioSource game;
	private AudioSource backgroundMusic;

	public AudioSource body;
	public AudioSource head;
	public AudioSource chin;

	public void Play (SoundEffect sound) {
		AudioSource audioSource;

		switch (sound) {
		case SoundEffect.Head:
			audioSource = head;
			break;
		case SoundEffect.Chin:
			audioSource = chin;
			break;
		case SoundEffect.Body:
		default:
			audioSource = body;
			break;
		}

		audioSource.Play ();
	}

	public void Play (BackgroundMusic music) {
		AudioSource audioSource;

		switch (music) {
		case BackgroundMusic.Menu:
			audioSource = menu;
			break;
		case BackgroundMusic.Game:
		default:
			audioSource = game;
			break;
		}

		if (!audioSource.isPlaying) {
			if (backgroundMusic != null) {
				backgroundMusic.Stop ();
			}

			backgroundMusic = audioSource;
			backgroundMusic.Play ();
		}
	}
}
