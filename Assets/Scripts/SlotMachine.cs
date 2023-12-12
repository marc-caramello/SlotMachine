#pragma warning disable 0649

using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameControl.PlaySound += PlayClip;
    }
    private void PlayClip(int clipNo)
    {
        audioSource.clip = clips[clipNo];
        audioSource.PlayOneShot(audioSource.clip);
    }
    private void OnDestroy()
    {
        GameControl.PlaySound -= PlayClip;
    }
}