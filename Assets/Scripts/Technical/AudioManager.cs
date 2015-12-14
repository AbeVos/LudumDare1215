using UnityEngine;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public struct ClipWithName
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField]
    private ClipWithName[] _clips;

    private static Dictionary<string, string> Clips;
    private static AudioSource source;

    public static float GetClipLegth(string name)
    {
        if (Clips != null)
        {
            var clip = Resources.Load<AudioClip>("Audio/" + Clips[name]);
            clip.LoadAudioData();
            return clip.length;
        }
        else
        {
            return 0;
        }
    }


    public void CrossFadeToNewClip()
    {

    }

    public static float PlayClip(string name, bool ShortSound)
    {
        if (source != null && Clips != null)
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + Clips[name]);
            
            if (ShortSound)
            {
                clip.LoadAudioData();
                Debug.Log("Playing :" + "Audio/" + Clips[name]);
                source.PlayOneShot(clip);
            }
            else
            {
                clip.LoadAudioData();
                source.clip = clip;
                source.Play();
            }
            return clip.length;
        }
        return 0;
    }

    void Awake()
    {
        source = Camera.main.gameObject.GetComponent<AudioSource>();
        Clips = new Dictionary<string, string>(_clips.Length);
        for (int i = 0; i < _clips.Length; i++)
        {
            Clips.Add(_clips[i].name, _clips[i].clip.name);
        }
    }

}
