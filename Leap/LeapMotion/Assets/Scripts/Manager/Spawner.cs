using Leap.Unity;
using Game.Leap;
using UnityEngine;

namespace Game.Manager
{
    public class Spawner : MonoBehaviour
    {
        public Transform SpawnPoint;

        public GameObject Item;

        private PinchEvent[] PinchEvent = new PinchEvent[2];
        // Use this for initialization
        void Start()
        {
            PinchManager.OnPinch += OnPinch;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Block.Create(Instantiate(Item, SpawnPoint.position, Quaternion.identity) as GameObject);
            }

            if (PinchEvent[0] != null && PinchEvent[1] != null)
            {
                float distance = Vector3.Distance(PinchEvent[0].Location, PinchEvent[1].Location);
                Debug.Log(distance);
                if (distance < .5)
                {
                    Block.Create(Instantiate(Item, SpawnPoint.position, Quaternion.identity) as GameObject);
                }
                PinchEvent[0] = null;
                PinchEvent[1] = null;
            }
        }

        void OnPinch(PinchEvent evnt)
        {
            if(evnt.Handedness == Chirality.Left)
            {
                PinchEvent[0] = evnt;
            }
            else
            {
                PinchEvent[1] = evnt;
            }
        }
    }
}
