using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseTokens : MonoBehaviour
{
    public static List<Token> Character = new List<Token>();

    string[] Descritions = {"",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            ""};
    
    void Awake()
    {
        Character.Add(new Token("Natsu"   ,"Y",5,2,TokenSkill.DestroyTrampInRadius,Resources.Load<Sprite>("NatsuF")));
        Character.Add(new Token("Lucy"    ,"Y",3,3,TokenSkill.SendTeleportZone,    Resources.Load<Sprite>("LucyF")));
        Character.Add(new Token("Gray"    ,"Y",6,2,TokenSkill.IncreseSpeed,        Resources.Load<Sprite>("GrayF")));
        Character.Add(new Token("Erza"    ,"Y",4,5,TokenSkill.AdversaryTokenInit,  Resources.Load<Sprite>("ErzaF")));
        Character.Add(new Token("Wendy"   ,"Y",7,2,TokenSkill.Exchange,            Resources.Load<Sprite>("WendyF")));
        Character.Add(new Token("Laxus"   ,"Y",5,5,TokenSkill.AdversaryTokenInit,  Resources.Load<Sprite>("LaxusF")));
        Character.Add(new Token("Mirajane","Y",7,2,TokenSkill.IncreseSpeed,        Resources.Load<Sprite>("MirajaneF")));
        Character.Add(new Token("Gajeel"  ,"Y",3,2,TokenSkill.DestroyTrampInRadius,Resources.Load<Sprite>("GajeelF")));
        Character.Add(new Token("Juvia"   ,"Y",6,3,TokenSkill.SendTeleportZone,    Resources.Load<Sprite>("JuviaF")));
        Character.Add(new Token("Mystogan","Y",8,1,TokenSkill.MoveBackTokensAdv,   Resources.Load<Sprite>("JellalF")));
    }


}
