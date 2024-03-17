using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private Dictionary<AudioClip, float> playingClips;

    private void Awake()
    {
        playingClips = new Dictionary<AudioClip, float>();

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        var keys = playingClips.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            if (playingClips[keys[i]] > 0f)
                playingClips[keys[i]] -= Time.deltaTime;
        }
    }

    public void Play(AudioClip clip)
    {
        if (!playingClips.Keys.ToList().Contains(clip))
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            playingClips.Add(clip, clip.length);
        }
        else if (playingClips[clip] <= 0)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            playingClips[clip] = clip.length;
        }
    }

    public void Play(AudioClip clip, float length)
    {
        if (!playingClips.Keys.ToList().Contains(clip))
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            playingClips.Add(clip, length);
        }
        else if (playingClips[clip] <= 0)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            playingClips[clip] = length;
        }
    }

}
