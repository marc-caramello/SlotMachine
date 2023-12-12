#pragma warning disable 0649

using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Coin;

    // Start is called before the first frame update
    void Start()
    {
        GameControl.CoinEarned += StartCoinDrop;
    }
    private void StartCoinDrop(int count)
    {
        StartCoroutine(CoinDrop(count));
    }
    private IEnumerator CoinDrop(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(Coin, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }
    private void OnDestroy()
    {
        GameControl.CoinEarned -= StartCoinDrop;
    }
}