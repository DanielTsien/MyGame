using System.Collections.Generic;
using MyGame;
using MyGame.Proto;
using Network;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectCharacterPanel : PanelBase
{
    public List<CharacterSoltItem> CharacterItem;
    public GameObject CreatePanelGo;
    public GameObject CharactersGo;
    public List<GameObject> CharacterGos;

    private int m_selectedIdx = -1;
    private List<Character> m_characters;

    private void Start()
    {
        CharactersGo.SetActive(false);
        m_characters = this.GetModel<IUserModel>().GetCharacters();
        for (int i = 0; i < CharacterItem.Count; i++)
        {
            int index = i;
            CharacterItem[i].SetSelected(false);
            CharacterItem[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (CharacterItem[index].IsEmpty())
                {
                    CreatePanelGo.SetActive(true);
                    gameObject.SetActive(false);
                }
                else
                {
                    SelectCharacter(index);
                }
            });
            if (i < m_characters.Count)
            {
                if (m_selectedIdx == -1)
                {
                    CharactersGo.SetActive(true);
                    SelectCharacter(i);
                }
                CharacterItem[i].Init(m_characters[i]);
            }
            else
            {
                CharacterItem[i].Empty();
            }
        }
    }

    public void OnStartClicked()
    {
        if (m_selectedIdx == -1)
        {
            return;
        }
        this.SendNetMessage(PacketId.UserGameEnterRequest, new UserGameEnterRequest
        {
            CharacterIdx = m_selectedIdx
        });
    }

    private void SelectCharacter(int index)
    {
        if(m_selectedIdx == index) return;
        int idx;
        if (m_selectedIdx != -1)
        {
            idx = (int) m_characters[m_selectedIdx].Class;
            CharacterItem[idx].SetSelected(false);
            CharacterGos[idx].SetActive(false);
        }
        m_selectedIdx = index;
        
        idx = (int) m_characters[m_selectedIdx].Class;
        CharacterItem[idx].SetSelected(true);
        CharacterGos[idx].SetActive(true);
    }
     
}
