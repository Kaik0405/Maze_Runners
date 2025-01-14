using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InfoCharacterInScene : MonoBehaviour,IPointerEnterHandler
{
    GameObject infoCharacterPanel; //Referencia al panel de informacion en escena
    public GameObject CurrentToken; //Referencia a la ficha actual en la escena
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoCharacterPanel = GameObject.Find("PanelInfo");
        UpdateInfo();
    }
    private void UpdateInfo()
    {
        Token tokenValue = CurrentToken.GetComponent<TokenDisplay>().Token;
        //asigna las propiedades de la ficha al panel de la escena en tiempo real cuando se le pasa por encima con el cursor
        infoCharacterPanel.transform.Find("NameToken").GetComponent<TMP_Text>().text = tokenValue.Name;
        infoCharacterPanel.transform.Find("CurrentStaminaValue").GetComponent<TMP_Text>().text = tokenValue.CurrentSpeed.ToString();
        infoCharacterPanel.transform.Find("CurrentCooldownValue").GetComponent<TMP_Text>().text = tokenValue.CurrentCooldown.ToString();
        infoCharacterPanel.transform.Find("InfoSkillValue").GetComponent<TMP_Text>().text = tokenValue.InfoHability.ToString();

        //Asigna el sprite de la ficha
        Image image = infoCharacterPanel.transform.Find("Image").GetComponent<Image>();
        image.sprite = tokenValue.SpriteTokenFull;
        
        Color color = image.color;
        color.a = 1.0f;
        image.color = color;
        
    }
    private Token Find(string value)
    {
        foreach(var item in DataBaseTokens.Character)
            if(item.Name == value) return item;
        
        return null;
    }

}