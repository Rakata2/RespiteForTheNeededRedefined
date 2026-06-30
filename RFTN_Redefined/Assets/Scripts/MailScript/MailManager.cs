using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class MailManager : MonoBehaviour
{
    public static MailManager Instance;
    public GameObject MailWindow;
    public Image MailIconButton;
    public Transform ContentBox;

    public Sprite NormalMailSprite;
    public Sprite HoveredMailSprite;
    public Sprite AlertMailSprite;
    public Sprite HoveredAlertMailSprite;

    public GameObject MailPrefab;

    private bool HasUnreadMail = false;
    private bool IsMouseHovering = false;

    public ClickableObject ComputerObject;

    private void Awake()
    {
        Instance = this;
    }

    public void ReceiveViolationMail(string subject, string descripition)
    {
        HasUnreadMail = true;
       if(IsMouseHovering)
       {
            MailIconButton.sprite = HoveredAlertMailSprite;
       }
       else
       {
            MailIconButton.sprite = AlertMailSprite; 
       }

       if(ComputerObject != null)
       {
            ComputerObject.TriggerComputerAlert();
       }

        GameObject newMail = Instantiate(MailPrefab, ContentBox);
        newMail.transform.SetAsFirstSibling();
        MailItem mailscript = newMail.GetComponent<MailItem>();
        mailscript.SetupMail(subject, descripition);

        LayoutRebuilder.ForceRebuildLayoutImmediate(ContentBox.GetComponent<RectTransform>());
    }

    public void ShowMailWindow()
    {
        MailWindow.SetActive(true);
        if(HasUnreadMail)
        {
            HasUnreadMail = false;
            MailIconButton.sprite = IsMouseHovering ? HoveredMailSprite : NormalMailSprite;
        }
    }

    public void OnHoverEnter()
    {
        IsMouseHovering = true;
        MailIconButton.sprite = HasUnreadMail ? HoveredAlertMailSprite : HoveredMailSprite;
    }

    public void OnHoverExit()
    {
        IsMouseHovering = false;
        MailIconButton.sprite = HasUnreadMail ? AlertMailSprite : NormalMailSprite;
    }

    public void CloseMailWindow()
    {
        MailWindow.SetActive(false);
    }
}
