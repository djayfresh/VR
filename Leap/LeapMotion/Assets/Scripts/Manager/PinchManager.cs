using UnityEngine;
using System.Collections;
using Leap.Unity;
using Game.Leap;
using FingerModel =  Game.Leap.FingerModel;

namespace Game.Manager
{
    public class PinchManager : MonoBehaviour
    {
        public delegate void FingerPinch(PinchEvent evnt);
        public static event FingerPinch OnPinch;

        public FingerModel[] FingersRight = new FingerModel[2];
        public FingerModel[] FingersLeft = new FingerModel[2];

        public float PinchDistanceBuffer = 0.5f;
        // Use this for initialization
        void Start () {
            if (FingersRight.Length > 2 || FingersLeft.Length > 2)
                throw new System.Exception("Pinch Fingers Array only supports 2 fingers");
	    }
	
	    // Update is called once per frame
	    void Update () {
            Vector3? loc;
            if ((loc = FingerPinchCheck(FingersLeft)).HasValue)
            {
                if (OnPinch != null)
                    OnPinch(new PinchEvent()
                    {
                        Fingers = FingersLeft,
                        Handedness = Chirality.Left,
                        Location = loc.Value
                    });

                Debug.Log("Pinch Left");
            }

            if ((loc = FingerPinchCheck(FingersRight)).HasValue)
            {
                if (OnPinch != null)
                    OnPinch(new PinchEvent()
                    {
                        Fingers = FingersRight,
                        Handedness = Chirality.Right,
                        Location = loc.Value
                    });

                Debug.Log("Pinch Right");
            }
        }

        Vector3? FingerPinchCheck(FingerModel[] fingers)
        {
            FingerModel finger1 = fingers[0];
            FingerModel finger2 = fingers[1];

            if (finger1 == null || finger2 == null)
                return null;

            Vector3 tip1 = finger1.GetTipPosition();
            Vector3 tip2 = finger2.GetTipPosition();

            float distance = Vector3.Distance(tip1, tip2);
            if (distance > PinchDistanceBuffer || distance == 0)
                return null;
            
            //Debug.Log(string.Format("Distance: {0}", distance));
            Vector3 midLocation = tip1 + (tip1 - tip2) / 2;
            //Debug.Log(string.Format("Location: {0}", midLocation));
            return midLocation;
        }
    }
}
