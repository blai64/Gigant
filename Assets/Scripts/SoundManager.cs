using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance = null;

	// BG Music
	public AudioClip BackgroundMusic;
	private AudioSource backgroundAudio;

	// Enemy Audio
	public AudioClip EnemyCrumble;
	private AudioSource crumbleAudio;

	public AudioClip EnemyAttack;
	private AudioSource attackAudio;

	public AudioClip GolemStep;
	private AudioSource golemStepAudio;

	// Boulder Falling Audio
	public AudioClip PebbleFall;
	private AudioSource pebbleFallAudio;

	public AudioClip RockFall;
	private AudioSource rockFallAudio;

	public AudioClip BoulderFall1;
	private AudioSource boulderAudio1;

	public AudioClip BoulderFall2;
	private AudioSource boulderAudio2;

	// Player Audio
	public AudioClip SwordSlash;
	private AudioSource swordAudio;

	public AudioClip JumpSound;
	private AudioSource jumpAudio;

	public AudioClip Death;
	private AudioSource deathAudio;

	public AudioClip SwordSlash1;
	private AudioSource swordSlashAudio1;

	public AudioClip SwordSlash2;
	private AudioSource swordSlashAudio2;

	public AudioClip Footstep1;
	private AudioSource footstepAudio1;

	public AudioClip Footstep2;
	private AudioSource footstepAudio2;

	// Beanstalk Audio
	public AudioClip StalkGrow;
	private AudioSource stalkGrowAudio;

	public AudioClip StalkDie;
	private AudioSource stalkDieAudio;

	// Hermit Audio
	public AudioClip Moo;
	private AudioSource mooAudio;

	public AudioClip Talking1;
	private AudioSource talkingAudio1;

	public AudioClip Talking2;
	private AudioSource talkingAudio2;

	public AudioClip Talking3;
	private AudioSource talkingAudio3;

	public AudioClip Talking4;
	private AudioSource talkingAudio4;

	private Dictionary<string, AudioSource> sounds;

	void Awake () {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		sounds = new Dictionary<string, AudioSource> ();

		// BG Music
		backgroundAudio = gameObject.AddComponent<AudioSource>();
		backgroundAudio.clip = BackgroundMusic;
		sounds ["background"] = backgroundAudio;
		backgroundAudio.loop = true;

		// Enemy Audio
		crumbleAudio = gameObject.AddComponent<AudioSource>();
		crumbleAudio.clip = EnemyCrumble;
		sounds ["enemy crumble"] = crumbleAudio;

		attackAudio = gameObject.AddComponent<AudioSource>();
		attackAudio.clip = EnemyAttack;
		sounds["enemy attack"] = attackAudio;

		golemStepAudio = gameObject.AddComponent<AudioSource>();
		golemStepAudio.clip = GolemStep;
		sounds["enemy step"] = golemStepAudio;

		// Boulder Falling Audio
		pebbleFallAudio = gameObject.AddComponent<AudioSource>();
		pebbleFallAudio.clip = PebbleFall;
		sounds["pebble fall"] = pebbleFallAudio;

		rockFallAudio = gameObject.AddComponent<AudioSource>();
		rockFallAudio.clip = RockFall;
		sounds["rock fall"] = rockFallAudio;

		boulderAudio1 = gameObject.AddComponent<AudioSource>();
		boulderAudio1.clip = BoulderFall1;
		sounds["boulder fall 1"] = boulderAudio1;

		boulderAudio2 = gameObject.AddComponent<AudioSource>();
		boulderAudio2.clip = BoulderFall2;
		sounds["boulder fall 2"] = boulderAudio2;

		// Player Audio
		swordAudio = gameObject.AddComponent<AudioSource>();
		swordAudio.clip = SwordSlash;
		sounds ["sword slash"] = swordAudio;

		jumpAudio = gameObject.AddComponent<AudioSource> ();
		jumpAudio.clip = JumpSound;
		sounds ["jump"] = jumpAudio;

		deathAudio = gameObject.AddComponent<AudioSource>();
		deathAudio.clip = Death;
		sounds["death"] = deathAudio;

		swordSlashAudio1 = gameObject.AddComponent<AudioSource>();
		swordSlashAudio1.clip = SwordSlash1;
		sounds["sword hit 1"] = swordSlashAudio1;

		swordSlashAudio2 = gameObject.AddComponent<AudioSource>();
		swordSlashAudio2.clip = SwordSlash2;
		sounds["sword hit 2"] = swordSlashAudio2;

		footstepAudio1 = gameObject.AddComponent<AudioSource>();
		footstepAudio1.clip = Footstep1;
		sounds["footstep 1"] = footstepAudio1;

		footstepAudio2 = gameObject.AddComponent<AudioSource>();
		footstepAudio2.clip = Footstep2;
		sounds["footstep 2"] = footstepAudio2;

		// Beanstalk Audio
		stalkGrowAudio = gameObject.AddComponent<AudioSource>();
		stalkGrowAudio.clip = StalkGrow;
		sounds["stalk grow"] = stalkGrowAudio;

		stalkDieAudio = gameObject.AddComponent<AudioSource>();
		stalkDieAudio.clip = StalkDie;
		sounds["stalk die"] = stalkDieAudio;

		// Hermit Audio
		mooAudio = gameObject.AddComponent<AudioSource>();
		mooAudio.clip = Moo;
		sounds["moo"] = mooAudio;

		talkingAudio1 = gameObject.AddComponent<AudioSource>();
		talkingAudio1.clip = Talking1;
		sounds["talking 1"] = talkingAudio1;

		talkingAudio2 = gameObject.AddComponent<AudioSource>();
		talkingAudio2.clip = Talking2;
		sounds["talking 2"] = talkingAudio2;

		talkingAudio3 = gameObject.AddComponent<AudioSource>();
		talkingAudio3.clip = Talking3;
		sounds["talking 3"] = talkingAudio3;

		talkingAudio4 = gameObject.AddComponent<AudioSource>();
		talkingAudio4.clip = Talking4;
		sounds["talking 4"] = talkingAudio4;
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
