using Game.Manager;
using Leap.Unity;

namespace Game.Manager
{
    using UnityEngine;

    public class HandManager : MonoBehaviour
    {

    }

    public enum HandState
    {
        NotTracking,
        Down,
        Up,
        Left,
        Right,
        Forward,
        Back
    }

    public enum FingersState
    {
        NotTracked,
        Open,
        Closed,
        Pinch,
        Pointing,
        Grasp
    }
}