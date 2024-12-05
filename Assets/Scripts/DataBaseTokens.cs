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
        Character.Add(new Token("Natsu","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Lucy","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Gray","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Erza","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Wendy","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Laxus","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Mirajane","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Gajeel","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Juvia","Y",0,0,TokenSkill.AdversaryTokenInit));
        Character.Add(new Token("Efman","Y",0,0,TokenSkill.AdversaryTokenInit));
    }


}
