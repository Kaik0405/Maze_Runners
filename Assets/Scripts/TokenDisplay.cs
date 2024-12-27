using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Token;
using Unity.VisualScripting;

public class TokenDisplay : MonoBehaviour
{
    public Token Token;
    public Sprite SpriteToken;
    public Image ImageToken;
    public AudioSource audioSource;

    public void Asignate()
    {
        SpriteToken = Token.SpriteTokenSelect;
        ImageToken.sprite = SpriteToken;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = Token.AudioSkill;
    }
    void Update()
    {
        if(Token.CurrentCountLock == 0)
        {
            Token.Lock = false;
        }
    }
}
