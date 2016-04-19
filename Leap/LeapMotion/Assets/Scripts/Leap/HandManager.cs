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
    }

    public enum FingersState
    {
        NotTracked,
        Open,
        Closed,
        Grasp
    }
}