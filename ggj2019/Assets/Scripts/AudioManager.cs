using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] Whistles;

    private void Awake()
    {
        Instance = this;
    }
    
    public void PlayWhistle(Vector3 pos)
    {
        GameObject obj = new GameObject();
        obj.transform.position = pos;
        AudioSource aS = obj.AddComponent<AudioSource>();
        aS.spatialBlend = 1f;
        aS.PlayOneShot(Whistles[Random.Range(0, Whistles.Length)]);
        Destroy(obj, 3f);
    }
}
