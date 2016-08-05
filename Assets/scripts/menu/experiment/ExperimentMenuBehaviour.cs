﻿using UnityEngine;

namespace menu.experiment
{
    public class ExperimentMenuBehaviour : MonoBehaviour
    {
        private UIPanTransition uiPanTransition;

        void Awake()
        {
            uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();
        }

        public void StartConsentForm()
        {
            uiPanTransition.TransitionTo(1);
        }

        public void StartExperiment()
        {
            Application.LoadLevel("experiment-load");
        }

        public void SkipToGame()
        {
            Application.LoadLevel("experiment-load");
        }

        public void ToPane(int paneID)
        {
            uiPanTransition.TransitionTo(paneID);
        }

        public void ConsentButton()
        {
            uiPanTransition.TransitionTo(3);
        }
    }   
}