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
        Character.Add(new Token("Natsu","Y",40,0,TokenSkill.DestroyTrampInRadius));
        Character.Add(new Token("Lucy","Y",40,0,TokenSkill.SimultaneousMovement));
        Character.Add(new Token("Gray","Y",6,0,TokenSkill.IncreseSpeed));
        Character.Add(new Token("Erza","Y",4,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Wendy","Y",7,0,TokenSkill.Exchange));
        Character.Add(new Token("Laxus","Y",10,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Mirajane","Y",10,0,TokenSkill.IncreseSpeed));
        Character.Add(new Token("Gajeel","Y",3,0,TokenSkill.DestroyTrampInRadius));
        Character.Add(new Token("Juvia","Y",6,0,TokenSkill.SendTeleportZone));
        Character.Add(new Token("Efman","Y",3,0,TokenSkill.XD));
    }


}
