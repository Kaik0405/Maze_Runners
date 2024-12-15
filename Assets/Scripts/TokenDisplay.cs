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
    public Sprite SpriteToken;
    public Image ImageToken;

    public void Asignate()
    {
        SpriteToken = Token.SpriteToken;
        ImageToken.sprite = SpriteToken;
    }
}
