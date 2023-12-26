using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioClip bgmusic;
    public AudioClip match;
    public AudioClip fail;
    public AudioClip gameOverSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = bgmusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void MatchFail()
    {
        audioSource.PlayOneShot(fail);
    }
    public void Match()
    {
        audioSource.PlayOneShot(match);
    }
    public void GameOver()
    {
        audioSource1.PlayOneShot(gameOverSound);
    }
}
