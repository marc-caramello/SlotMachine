#pragma warning disable 0649

using UnityEngine;

public class BetMaxButton : MonoBehaviour
{
    private AudioSource coinInsertSound;

    private void Start()
    {
        coinInsertSound = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (GameControl.totalCoins >= Constants.COINS_PER_BUTTON_PRESS)
        {
            coinInsertSound.PlayOneShot(coinInsertSound.clip);
            GameControl.currentBet += GameControl.totalCoins;
            GameControl.totalCoins = 0;
        }
    }
}