using Game.Manager;
using Leap.Unity;
using UnityEngine;

namespace Game.Leap
{
    public class HandModel : RigidHand
    {
        public HandState HandState = HandState.NotTracking;
        public FingersState FingersState = FingersState.NotTracked;
        public Vector3 UP = Vector3.up;
        
        public override void InitHand()
        {
            base.InitHand();
        }

        public override void UpdateHand()
        {
            base.UpdateHand();

            Vector3 handNormal = GetPalmNormal();
            float dotNormal = Vector3.Dot(handNormal, UP);

            if(dotNormal > 0.5)
            {
                HandState = HandState.Up;
            }
            if(dotNormal < -0.5)
            {
                HandState = HandState.Down;
            }
        }
    }


}