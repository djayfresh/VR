using UnityEngine;
using System.Collections;
using Leap.Unity;

namespace Game.Leap
{
    public class PinchEvent
    {
        public FingerModel[] Fingers;

        public Vector3 Location;
        public Chirality Handedness;
    }
}
