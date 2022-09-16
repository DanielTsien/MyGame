using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Proto;
using Network;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class CreateCharacterPanel : PanelBase
{
    public TMP_InputField NameInput;
    public List<GameObject> ClassViewGos;
    public List<Image> ClassAvatars;
    public List<Button> ClassButtons;
    private int m_curIdx;

    private void Start()
    {
        for (int i = 0; i < ClassButtons.Count; i++)
        {
            var idx = i;
            ClassButtons[i].onClick.AddListener(() => SelectClass(idx));
            ClassAvatars[i].SetGray(true);
            ClassViewGos[i].SetActive(false);
        }

        SelectClass(m_curIdx);
    }

    private void SelectClass(int idx)
    {
        ClassAvatars[m_curIdx].SetGray(true);
        ClassViewGos[m_curIdx].SetActive(false);
        m_curIdx = idx;
        ClassAvatars[m_curIdx].SetGray(false);
        ClassViewGos[m_curIdx].SetActive(true);
    }
    
    public void OnCreateClick()
    {
        if (string.IsNullOrEmpty(NameInput.text)) return;
        
        this.SendNetMessage(PacketId.UserCreateCharacterRequest, new UserCreateCharacterRequest
        {
            Class = (CharacterClass)m_curIdx,
            Name = NameInput.text,
        });

        NameInput.text = "";
    }
}
