using UnityEngine;

[System.Serializable]
public enum SFXType
{
	Jump,
	Walk,
	Land,
	LightUp,
	Win,
	Bounce
}

public class AudioManager : MonoBehaviour
{
	[Header("Audio Sources")]
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	[Header("Music Clips")]
	[SerializeField] private AudioClip backgroundMusic;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip jumpClip;
	[SerializeField] private AudioClip walkClip;
	[SerializeField] private AudioClip landClip;
	[SerializeField] private AudioClip lightUpClip; // checkpoint or light
	[SerializeField] private AudioClip winClip;
	[SerializeField] private AudioClip bounceClip;

	// -----------------------------
	// MUSIC
	// -----------------------------
	public void PlayMusic(AudioClip clip = null, bool loop = true)
	{
		if (clip == null)
			clip = backgroundMusic;

		if (musicSource.clip == clip) return;

		musicSource.clip = clip;
		musicSource.loop = loop;
		musicSource.Play();
	}

	// -----------------------------
	// SOUND EFFECTS
	// -----------------------------
	public void PlaySFX(SFXType type)
	{
		AudioClip clip = GetClip(type);
		if (clip != null)
			sfxSource.PlayOneShot(clip);
	}

	private AudioClip GetClip(SFXType type)
	{
		return type switch
		{
			SFXType.Jump => jumpClip,
			SFXType.Walk => walkClip,
			SFXType.Land => landClip,
			SFXType.LightUp => lightUpClip,
			SFXType.Win => winClip,
			SFXType.Bounce => bounceClip,
			_ => null
		};
	}
}
