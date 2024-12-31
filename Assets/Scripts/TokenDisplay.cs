using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Token;

public class TokenDisplay : MonoBehaviour
{
    public Token Token;
    public GameObject SpriteTramp;
    public Image SpriteTrampImage;
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
        SpriteTrampImage = SpriteTramp.GetComponent<Image>();
        SpriteTramp.transform.position = gameObject.transform.position;
    }
    void Update()
    {
        if(Token.CurrentCountLock == 0)
        {
            Token.Lock = false;
            if(SpriteTramp != null && !Token.Available)
                DecreaseA();
        }
    }
    public void IncreaseA()
    {
        Image image = SpriteTramp.GetComponent<Image>();
        Color color = image.color;
        color.a = 1.0f;
        image.color = color;
    }
    public void DecreaseA()
    {
        Image image = SpriteTramp.GetComponent<Image>();
        Color color = image.color;
        color.a = 0.0f;
        image.color = color;
    }
}
