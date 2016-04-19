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
        public Vector3 UP = Vector3.up;

        public override void InitHand()
        {
            base.InitHand();
        }

        public override void UpdateHand()
        {
            base.UpdateHand();

            HandFacingState();
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