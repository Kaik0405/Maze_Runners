using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InfoCharacters : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject infoCharacterPanel;
    public GameObject CurrentToken;
    public TMP_Text Name;
    public TMP_Text StaminaValue;
    public TMP_Text CooldownValue;
    public TMP_Text SkillValue;
    public GameObject Image;
    void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoCharacterPanel.SetActive(true);
        UpdateInfo();
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        infoCharacterPanel.SetActive(false);
    }
    private void UpdateInfo()
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
