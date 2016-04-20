using Leap.Unity;

namespace Game.Manager
{
    using Component.Leap;
    using UnityEngine;

    public class HandManager : MonoBehaviour
    {
        public delegate void HandGrab(HandModel hand);
        public static event HandGrab OnGrab;
        public static event HandGrab ReleaseGrip;

        public HandModel[] Hands;

        void Start()
        {

        }

        void Update()
        {
            foreach(var hand in Hands)
            {
                if (hand.FingersState == FingersState.Closed && hand.IsTracked)
                {
                    if(OnGrab != null)
                    {
                        OnGrab(hand);
                    }
                }
                else
                {
                    if (ReleaseGrip != null)
                        ReleaseGrip(hand);
                }
            }
        }
    }

    public enum HandState
    {
        NotTracking,
        Down,
        Up,
    }

    public enum FingersState
    {
        NotTracked,
        Open,
        Closed,
        Grasp
    }
}