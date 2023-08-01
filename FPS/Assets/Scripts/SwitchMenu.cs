using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMenu : MonoBehaviour
{
    [SerializeField] KeyCode MenuKey;
    [SerializeField] Canvas TutorialPanel;

    void Update()
    {
        if(Input.GetKeyDown(MenuKey))
        {
            TutorialPanel.enabled = !TutorialPanel.enabled;
        }
    }
}
