using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "new Token", menuName = "Token")]
public class Token : ScriptableObject
{
    public string Name;
    public string InfoHability;
    public int Speed;
    public int Cooldown;
    public bool Available = false;
    public int PosX = 1;
    public int PosY = 1;
    public delegate void TokenSkill(params object[] param);
    public TokenSkill Skill { get; set;}
    Sprite spriteToken;

    public Token(string name, string infoHability,int speed,int cooldown,TokenSkill skill)
    {
        Name = name;
        InfoHability = infoHability;
        Speed = speed;
        Cooldown = cooldown;
        Skill = skill;
   }
     
}
