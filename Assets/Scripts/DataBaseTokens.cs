using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseTokens : MonoBehaviour
{
    public static List<Token> Character = new List<Token>();

    string[] Descritions = {"Destruye todas las trampas que estan en el radio de desplazamiento de la ficha",
                            "Envia la ficha hacia una zona de teletransporte aleatoria",
                            "Incrementa la energia de la ficha en 5",
                            "Manda una ficha aleatoria del adversario en el campo para el inicio",
                            "Intercambia la ficha actual con una ficha aleatoria del adversario en el campo",
                            "Manda una ficha aleatoria del adversario en el campo para el inicio",
                            "Incrementa la energia de la ficha en 5",
                            "Destruye todas las trampas que estan en el radio de desplazamiento de la ficha",
                            "Envia la ficha hacia una zona de teletransporte aleatoria",
                            "Manda todas la fichas en el campo para su posicion anterior"};
    
    void Awake()
    {
        Character.Add(new Token("Natsu"   ,Descritions[0],8,2,TokenSkill.DestroyTrampInRadius,Resources.Load<Sprite>("NatsuF"),   Resources.Load<Sprite>("NatsuFSelect"),   Resources.Load<Sprite>("NatsuFull")));
        Character.Add(new Token("Lucy"    ,Descritions[1],7,3,TokenSkill.SendTeleportZone,    Resources.Load<Sprite>("LucyF"),    Resources.Load<Sprite>("LucyFSelect"),    Resources.Load<Sprite>("LucyFull")));
        Character.Add(new Token("Gray"    ,Descritions[2],9,2,TokenSkill.IncreseSpeed,        Resources.Load<Sprite>("GrayF"),    Resources.Load<Sprite>("GrayFSelect"),    Resources.Load<Sprite>("GrayFull")));
        Character.Add(new Token("Erza"    ,Descritions[3],8,5,TokenSkill.AdversaryTokenInit,  Resources.Load<Sprite>("ErzaF"),    Resources.Load<Sprite>("ErzaFSelect"),    Resources.Load<Sprite>("ErzaFull")));
        Character.Add(new Token("Wendy"   ,Descritions[4],10,2,TokenSkill.Exchange,           Resources.Load<Sprite>("WendyF"),   Resources.Load<Sprite>("WendyFSelect"),   Resources.Load<Sprite>("WendyFull")));
        Character.Add(new Token("Laxus"   ,Descritions[5],8,5,TokenSkill.AdversaryTokenInit,  Resources.Load<Sprite>("LaxusF"),   Resources.Load<Sprite>("LaxusFSelect"),   Resources.Load<Sprite>("LaxusFull")));
        Character.Add(new Token("Mirajane",Descritions[6],10,2,TokenSkill.IncreseSpeed,       Resources.Load<Sprite>("MirajaneF"),Resources.Load<Sprite>("MirajaneFSelect"),Resources.Load<Sprite>("MirajaneFull")));
        Character.Add(new Token("Gajeel"  ,Descritions[7],8,2,TokenSkill.DestroyTrampInRadius,Resources.Load<Sprite>("GajeelF"),  Resources.Load<Sprite>("GajeelFSelect"),  Resources.Load<Sprite>("GajeelFull")));
        Character.Add(new Token("Juvia"   ,Descritions[8],9,3,TokenSkill.SendTeleportZone,    Resources.Load<Sprite>("JuviaF"),   Resources.Load<Sprite>("JuviaFSelect"),   Resources.Load<Sprite>("JuviaFull")));
        Character.Add(new Token("Mystogan",Descritions[9],6,1,TokenSkill.MoveBackTokensAdv,   Resources.Load<Sprite>("JellalF"),  Resources.Load<Sprite>("JellalFSelect"),  Resources.Load<Sprite>("JellalFull")));
    }


}
