using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Cell", menuName = "Cell")]
public class Cell: ScriptableObject
{
    public bool Obstacle = true;
    public bool Tramp = false;
    public bool cellTeleport = false;
    public bool EnergyCell = false;
    public bool Start = false;
    public bool FinishLine = false;
    public int PosX = 0;
    public int PosY = 0;
    public delegate void TrampEffect(params object[] param);
    public TrampEffect trampEffect;
    public TrampType trampType = TrampType.Unknown;
    public AudioClip AudioTramp;
    public Cell(bool Obstacle)
    {
        this.Obstacle = Obstacle;  
    }
    public void AsignateTeleportDelegate()
    {
        if (cellTeleport)
        {
            trampEffect = TrampEffects.Teleport;
            AudioTramp = Resources.Load<AudioClip>("TeleportSound");
        }
        if (EnergyCell)
        {
            AudioTramp = Resources.Load<AudioClip>("SoundEng");
        }
    }
    public void AsignateTrampDelegate()
    {
        switch (trampType)
        {
            case TrampType.Sleep:
                trampEffect = TrampEffects.Sleep;
                AudioTramp = Resources.Load<AudioClip>("SleepSound");
                break;
            case TrampType.Freeze:
                trampEffect = TrampEffects.Freeze;
                AudioTramp = Resources.Load<AudioClip>("IceSound");
                break;
            case TrampType.returnInit:
                trampEffect = TrampEffects.ReturnInit;
                AudioTramp = Resources.Load<AudioClip>("TeleportSound");
                break;
            case TrampType.Spines:
                trampEffect = TrampEffects.Spines;
                AudioTramp = Resources.Load<AudioClip>("SpikeSound");
                break;        
            default:
                break;
        }
    }
}
public enum TrampType{
    returnInit, //mueve la ficha para la salida
    Freeze,     //evita que la ficha se mueva mas
    Sleep,      //evita que la ficha se mueva por 2 turnos
    Spines,     //hace que la ficha se mueva la mitad de los pasos restantes
    Unknown
}
