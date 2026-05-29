using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ShelterDiversionManager : MonoBehaviour
{
    [Header("Selection settings")]
    public List<string> SelectedNeeds = new List<string>();

    public Sprite DefaultMedical;
    public Sprite DefaultIsolation;
    public Sprite DefaultBehavioral;

    public Sprite SelectedMedical;
    public Sprite SelectedIsolation;
    public Sprite SelectedBehavioral;

    [Header("UI reference")]
    public GameObject ClarificationWindow;
    public GameObject ShelterDiversionWarningWindow;
    public TMP_Text PopupBodyText;

    public Image MedicalImage;
    public Image IsolationImage;
    public Image BehavioralImage;

    public void ToggleNeed(string needName)
    {
        if(SelectedNeeds.Contains(needName))
        {
            SelectedNeeds.Remove(needName);
            UpdateVisual(needName, false);
        }
        else
        {
            SelectedNeeds.Add(needName);
            UpdateVisual(needName, true);
        }
    }

    private void UpdateVisual(string needName, bool active)
    {
        if (needName == "Medical")
            MedicalImage.sprite = active ? SelectedMedical : DefaultMedical;
        if(needName == "Isolation")
            IsolationImage.sprite = active ? SelectedIsolation : DefaultIsolation;
        if (needName == "Behavioral")
            BehavioralImage.sprite = active ? SelectedBehavioral : DefaultBehavioral;
    }

    public void ClearAll()
    {
        SelectedNeeds.Clear();
        MedicalImage.sprite = DefaultMedical;
        IsolationImage.sprite = DefaultIsolation;
        BehavioralImage.sprite= DefaultBehavioral;
    }

    public void HandleShelterDiversionClick()
    {
        if (NPCMovement.CurrentClient == null)
        {
            ShowWarningWindow();
            return;
        }

        if (NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.MovingToCenter || NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.Interact)
        {
            ShowWarningWindow();
            return;
        }

        if (NPCMovement.CurrentClient.CurrentState == NPCMovement.NPCState.WaitingForDecision)
        {
            ShowClarificationWindow();
        }
    }

    public void ShowClarificationWindow()
    {
        string NeedsDisplay = SelectedNeeds.Count == 0 ? "None" : string.Join(", ", SelectedNeeds);
        PopupBodyText.text = $"Needs selected: {NeedsDisplay}";
        ClarificationWindow.SetActive(true);
    }

    public void CancelAssignment()
    {
        ClarificationWindow.SetActive(false);
    }

    public void ShowWarningWindow()
    {
        ShelterDiversionWarningWindow.SetActive(true);
    }

    public void CloseWarningWindow()
    {
        ShelterDiversionWarningWindow.SetActive(false);
    }

    public void Divert()
    {
        ClarificationWindow.SetActive(false);
        GameUIManager.instance.CloseComputer();
        ClearAll();
    }

}
