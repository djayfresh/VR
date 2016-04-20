using System;
using Game.Manager;
using Leap.Unity;
using UnityEngine;
using Leap;

namespace Game.Component.Leap
{
    public class HandModel : RigidHand
    {
        public HandState HandState = HandState.NotTracking;
        public FingersState FingersState = FingersState.NotTracked;
        public Vector3 UP = Vector3.up;

        private FingerModel[] Fingers;
        public override void InitHand()
        {
            base.InitHand();

            Fingers = new FingerModel[fingers.Length];
            InitFingers();
        }

        private void InitFingers()
        {
            for(int i = 0; i < fingers.Length; i++)
            {
                Fingers[i] = (FingerModel)fingers[i];
            }
        }

        public override void UpdateHand()
        {
            base.UpdateHand();

            HandFacingState();

            FingerStateCheck();
        }

        private void FingerStateCheck()
        {
            bool openHand = true;
            bool closedHand = true;
            bool graspHand = true;
            foreach (var finger in Fingers)
            {
                if (finger.FingersState != FingersState.Open)
                {
                    openHand = false;
                }

                if (finger.FingersState != FingersState.Closed)
                {
                    closedHand = false;
                }

                if(finger.FingersState != FingersState.Grasp)
                {
                    graspHand = false;
                }
            }

            if (graspHand)
            {
                FingersState = FingersState.Grasp;
            }
            if (closedHand)
            {
                FingersState = FingersState.Closed;
            }
            if (openHand)
            {
                FingersState = FingersState.Open;
            }
            //One or two fingers may be "Closed/Open" but the hand is in a grasp position
            if(!graspHand && !closedHand && !openHand)
            {
                FingersState = FingersState.Grasp;
            }
        }

        private void HandFacingState()
        {
            Vector3 handNormal = GetPalmNormal();
            float dotNormal = Vector3.Dot(handNormal, UP);

            if (dotNormal > 0.5)
            {
                HandState = HandState.Up;
            }
            if (dotNormal < -0.5)
            {
                HandState = HandState.Down;
            }

            if (dotNormal == -1)
            {
                HandState = HandState.NotTracking;
            }

            if (dotNormal > -0.5 && dotNormal < 0.5)
            {
                HandState = HandState.NotTracking;
            }
        }
    }
}