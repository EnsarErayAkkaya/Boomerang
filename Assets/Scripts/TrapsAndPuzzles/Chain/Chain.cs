using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public ChainLink[] chainLinks;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        chainLinks = new ChainLink[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            chainLinks[i] = transform.GetChild(i).GetComponent<ChainLink>();
            chainLinks[i].index = i;
        }
    }

    public void OnChainGethit(int index)
    {
        audioSource.Play();

        for (int i = 0; i < chainLinks.Length; i++)
        {
            if(index == i) // hitted chainlink
            {
                chainLinks[i].Unlink();
            }
            else if( i > index)
            {
                chainLinks[i].SetCollidersWithoutUnlink();
            }
        }
    }
}
