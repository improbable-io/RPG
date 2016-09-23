using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Improbable;
using Improbable.Actions;
using Improbable.Checks;
using Improbable.Unity.Visualizer;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionOptionsUIBehaviour : UIBehaviour
{
    [Require] CheckIsClientSideWriter clientSideCheck; // Ensures that this runs on the client only

    private List<GameObject> interactionOptionsButtons = new List<GameObject>();
    public Dictionary<Button, ActionTypeEnum> buttonActionMap = new Dictionary<Button, ActionTypeEnum>();

    // Set during initialisation of PlayerUIController
    public PlayerController playerController;
    public GameObject interactionButtonPrefab;
    public Canvas hudCanvas;

    protected override void Start()
    {
        hideInteractionOptions();
    }

    public void showInteractionOptions(GameObject interactionObject, List<ActionTypeEnumWrapper> availableActions)
    {
        // (Re-)Populate tray with a child button for each available action
        destroyButtons();
        for (int i = 0; i < availableActions.Count; i++)
        {
            GameObject nextButton = Instantiate(interactionButtonPrefab);
            nextButton.gameObject.SetActive(true);
            interactionOptionsButtons.Add(nextButton);
            // Tray element's HorizontalLayoutGroup will specify exact positioning
            nextButton.transform.SetParent(this.transform, false);

            Button button = nextButton.GetComponent<Button>();
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = availableActions[i].ActionType.ToString();

            buttonActionMap[button] = availableActions[i].ActionType;

            button.onClick.AddListener(() =>
            {
                Button thisButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                Debug.Log("Selected interaction: name= " + thisButton.name + ", action=" + buttonActionMap[thisButton]);
                
                playerController.MoveToAndInteract(interactionObject, buttonActionMap[thisButton]);
            });
        }
        // Show
        transform.gameObject.SetActive(true);
    }

    public void hideInteractionOptions()
    {
        this.transform.gameObject.SetActive(false);
        destroyButtons();
    }

    // TODO: Pool buttons instead of re-instantiating
    private void destroyButtons()
    {
        while (interactionOptionsButtons.Count > 0)
        {
            GameObject forDestruction = interactionOptionsButtons[0];
            interactionOptionsButtons.RemoveAt(0);
            Destroy(forDestruction);
        }
    }
}
