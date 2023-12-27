using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
   [SerializeField] private ClearCounter clearCounter;
   [SerializeField] private GameObject visualGameObject;
   private void Start()
   {
      // Player.Instance  <-- refering to the player instance we set in Awake() function of player
      Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged; // subscribing to an event(this function will be called when the event is triggered)

      
   }

   private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
   {
       /*
        AGAR SELECTED COUNTER JO EVENT SE TRIGGER HUA HAI VO REFERENCED CLEAR COUNTER HAI TOH EFFECT SHOW KARDO
        NAHI TOH EFFECT HIDE KAR DO
        */
       if (e.selectedCounter == clearCounter)
       {
            Show();
       }
       else
       {
           Hide();
       }
   }

   private void Show()
   {
       visualGameObject.SetActive(true);
   }
   private void Hide()
   {
       visualGameObject.SetActive(false);
   }
}
