using Game.Leap;
using UnityEngine;

namespace Game.Manager
{
    public class Spawner : MonoBehaviour
    {
        public Transform SpawnPoint;

        public GameObject Item;

        private PinchEvent[] PinchEvent;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Block.Create(Instantiate(Item, SpawnPoint.position, Quaternion.identity) as GameObject);
            }
        }
    }
}
