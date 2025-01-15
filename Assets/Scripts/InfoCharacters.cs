using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InfoCharacters : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject infoCharacterPanel; //Referencia al panel que muestra la informacion de la ficha/personaje
    public GameObject CurrentToken;       //Referencia al boton que se esta señalando con el cursor
    public TMP_Text Name;                 //Nombre de la ficha
    public TMP_Text StaminaValue;         //Valor de la Velocidad/Stamina
    public TMP_Text CooldownValue;        //Valor del tiempo de enfriamiento de la ficha
    public TMP_Text SkillValue;           //Texto de la habilidad de la ficha
    public GameObject Image;              //Objeto de la imagen de la ficha
    void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData) //Evento de entrada del cursor
    {
        infoCharacterPanel.SetActive(true);
        UpdateInfo();
        
    }
    public void OnPointerExit(PointerEventData eventData)  //Evento de salida
    {
        infoCharacterPanel.SetActive(false);
    }
    private void UpdateInfo() //Metodo que actualiza la informacion del panel donde se muestra la informacio de la ficha 
    {
        Token tokenValue = Find(CurrentToken.name);

        Name.text = tokenValue.Name;
        StaminaValue.text = tokenValue.GetSpeed().ToString();
        CooldownValue.text = tokenValue.GetCooldown().ToString();
        SkillValue.text = tokenValue.InfoHability.ToString();

        Image image = Image.GetComponent<Image>();
        image.sprite = tokenValue.SpriteTokenFull;
        
    }
    private Token Find(string value)
    {
        foreach(var item in DataBaseTokens.Character)
            if(item.Name == value) return item;
        
        return null;
    }
}
