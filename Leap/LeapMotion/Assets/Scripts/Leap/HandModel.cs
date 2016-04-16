using System;
using Game.Manager;
using Leap.Unity;
using UnityEngine;
using Leap;

namespace Game.Leap
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

            FingersStatus();
        }

        private void FingersStatus()
        {
            foreach (var finger in fingers)
            {
                float stretch = finger.GetFingerJointStretchMecanim((int)Bone.BoneType.TYPE_METACARPAL);
                //Debug.Log(String.Format("Finger: {0}, Stretch: {1}", GetFingerName(finger.fingerType), stretch));
            }
        }

        private Finger.FingerType[] FingersForClose()
        {
            return new Finger.FingerType[]
            {
                Finger.FingerType.TYPE_INDEX,
                Finger.FingerType.TYPE_MIDDLE,
                Finger.FingerType.TYPE_RING,
                Finger.FingerType.TYPE_PINKY,
                Finger.FingerType.TYPE_THUMB
            };
        }

        private string GetFingerName(Finger.FingerType finger)
        {
            switch(finger)
            {
                case Finger.FingerType.TYPE_INDEX:
                    return "Index";
                case Finger.FingerType.TYPE_MIDDLE:
                    return "Middle";
                case Finger.FingerType.TYPE_PINKY:
                    return "Pinky";
                case Finger.FingerType.TYPE_RING:
                    return "Ring";
                case Finger.FingerType.TYPE_THUMB:
                    return "Thumb";
                default:
                    return "N/A";
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