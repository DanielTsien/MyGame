using MyGame.Proto;
using TMPro;
using UnityEngine;

public class CharacterSoltItem : MonoBehaviour
{
    private TMP_Text m_nameTxt;
    private TMP_Text m_classTxt;
    private TMP_Text m_levelTxt;
    private GameObject m_addGo;
    private GameObject m_selectedGo;
    private bool m_isEmpty;

    private void Awake()
    {
        m_nameTxt = transform.Find("NameTxt").GetComponent<TMP_Text>();
        m_classTxt = transform.Find("ClassTxt").GetComponent<TMP_Text>();
        m_levelTxt = transform.Find("LevelTxt").GetComponent<TMP_Text>();
        m_addGo = transform.Find("Add").gameObject;
        m_selectedGo = transform.Find("Selected").gameObject;
    }
    
    public void Init(NCharacterInfo info)
    {
        m_isEmpty = false;
        m_addGo.SetActive(false);
        m_nameTxt.text = info.Name;
        m_classTxt.text = info.Class.ToString();
        m_levelTxt.text = $"Lv.{info.Level}";
    }

    public void Empty()
    {
        m_isEmpty = true;
        m_addGo.SetActive(true);
        m_nameTxt.text = "";
        m_classTxt.text = "";
        m_levelTxt.text = "";
    }

    public void SetSelected(bool isSelected)
    {
        m_selectedGo.SetActive(isSelected);
    }

    public bool IsEmpty()
    {
        return m_isEmpty;
    }
}
