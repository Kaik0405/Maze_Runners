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
    int Speed;
    int Cooldown;
    public bool Available = false;
    public int PosX = 1;
    public int PosY = 1;
    public int PrePosX = 1; 
    public int PrePosY = 1;
    public int CurrentSpeed;
    public int CurrentCooldown;
    public bool Lock = false;
    int CountLock = 3;
    public int CurrentCountLock = 0;
    public delegate void TokenSkill(params object[] param);
    public TokenSkill Skill { get; set;}
    public Sprite SpriteToken;
    public Sprite SpriteTokenNormal;
    public Sprite SpriteTokenSelect;
    public Sprite SpriteTokenFull;
    public Sprite SpriteTokenSkillActive;
    public Sprite SpriteAux;
    public AudioClip AudioSkill;
  
    public Token(string name, string infoHability,int speed,int cooldown,TokenSkill skill,Sprite spriteToken,
    Sprite spriteTokenSelect,Sprite spriteTokenFull,Sprite spriteTokenNormal,Sprite spriteTokenSkillActive,AudioClip audioSkill)

    {
        Name = name;
        InfoHability = infoHability;
        Speed = speed;
        Cooldown = cooldown;
        Skill = skill;
        SpriteToken = spriteToken;
        SpriteTokenSelect = spriteTokenSelect;
        SpriteTokenFull = spriteTokenFull;
        SpriteTokenNormal = spriteTokenNormal;
        SpriteTokenSkillActive = spriteTokenSkillActive;
        AudioSkill = audioSkill;
        CurrentSpeed = speed;
        CurrentCooldown = 0;
        SpriteAux = spriteTokenFull;
   }
   public Token(){

   }
   public int GetSpeed() => Speed;
   public int GetCooldown() => Cooldown;
   public int GetCountLock() => CountLock;  
}
