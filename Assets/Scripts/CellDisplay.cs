using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellDisplay : MonoBehaviour
{
    public Cell cell;
    Image ImageCell;
    public AudioSource audioSource;
    void Update()
    {
    }
    public void Asignate()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = cell.AudioTramp;
    }
}
