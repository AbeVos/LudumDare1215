using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public struct ClipWithName
    {
        public static string name;
        public static AudioClip clip;
    }

    
    public static ClipWithName[] Clips;
    private AudioSource source;

    public void CrossFadeToNewClip()
    {

    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
