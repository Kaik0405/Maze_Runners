using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "new Token", menuName = "Token")]
public class Token : ScriptableObject
{
    string Name;
    string InfoHability;
    int Speed;
    int Cooldown;
    bool Available = false;
    public int PosX = 1;
    public int PosY = 1;
    //Delegado de la Habilidad...
    Sprite spriteToken;
     
}
