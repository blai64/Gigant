using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance = null;

	// Enemy Audio
	public AudioClip EnemyCrumble;

	private AudioSource crumbleAudio;

	// Environment Audio
	public AudioClip BackgroundMusic;

	private AudioSource backgroundAudio;

	// Player Audio
	public AudioClip SwordSlash;
	public AudioClip JumpSound;

	private AudioSource swordAudio;
	private AudioSource jumpAudio;


	private Dictionary<string, AudioSource> sounds;

	void Awake () {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		sounds = new Dictionary<string, AudioSource> ();

		// Enemy Audio
		crumbleAudio = gameObject.AddComponent<AudioSource>();
		crumbleAudio.clip = EnemyCrumble;
		sounds ["enemy crumble"] = crumbleAudio;

		// Player Audio
		swordAudio = gameObject.AddComponent<AudioSource>();
		swordAudio.clip = SwordSlash;
		sounds ["sword slash"] = swordAudio;
		jumpAudio = gameObject.AddComponent<AudioSource> ();
		jumpAudio.clip = JumpSound;
		sounds ["jump"] = jumpAudio;

		// Environment Audio
		backgroundAudio = gameObject.AddComponent<AudioSource>();
		backgroundAudio.clip = BackgroundMusic;
		sounds ["background"] = backgroundAudio;
		backgroundAudio.loop = true;


	}

	public void PlaySound(string soundName) {
		AudioSource temp;
		if (sounds.TryGetValue (soundName, out temp)) {
			temp.Play ();
		}
	}

	public void StopSound(string soundName) {
		AudioSource temp;
		if (sounds.TryGetValue (soundName, out temp)) {
			temp.Stop ();
		}
	}
}
