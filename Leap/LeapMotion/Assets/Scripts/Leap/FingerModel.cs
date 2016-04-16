using UnityEngine;
using System.Collections;
using Leap.Unity;
using Game.Manager;
using Leap;

namespace Game.Leap
{
    public class FingerModel : RigidFinger
    {
        public FingersState FingersState = FingersState.NotTracked;
        public float ClosedLowerBounds;
        public float OpenUpperBounds;

        public override void InitFinger()
        {
            base.InitFinger();
        }

        public override void UpdateFinger()
        {
            base.UpdateFinger();
            FingerStateCheck();
        }

        private void FingerStateCheck()
        {
            float fingerStretch = GetFingerJointStretchMecanim((int)Bone.BoneType.TYPE_METACARPAL);

            if(fingerStretch < ClosedLowerBounds)
            {
                FingersState = FingersState.Closed;
            }

            if(fingerStretch > OpenUpperBounds)
            {
                FingersState = FingersState.Open;
            }

            if(fingerStretch < OpenUpperBounds && fingerStretch > ClosedLowerBounds)
            {
                FingersState = FingersState.Grasp;
            }

            Debug.Log(string.Format("Finger: {0}, Stretch: {1}, State: {2}", GetFingerName(fingerType), fingerStretch, FingersState));
        }

        private string GetFingerName(Finger.FingerType finger)
        {
            switch (finger)
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
    }
}