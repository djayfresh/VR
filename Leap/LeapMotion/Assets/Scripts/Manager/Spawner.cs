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
        public float PinchDistance = 0.5f;
        
        public float spawnRate = 0.5f;
        private float lastSpawn = 1;

        private GameObject lastSpawned;
        // Use this for initialization
        void Start()
        {
            PinchManager.OnPinch += OnPinch;
        }

        // Update is called once per frame
        void Update()
        {
            if (PinchEvent[0] == null && PinchEvent[1] == null)
            {
                lastSpawn += Time.deltaTime;

                if (lastSpawned != null)
                {
                    lastSpawned.GetComponent<Collider>().enabled = true;
                    lastSpawned.GetComponent<Rigidbody>().useGravity = Gravity.GravityEnabled;
                    lastSpawned = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Block.Create(Instantiate(Item, SpawnPoint.position, Quaternion.identity) as GameObject);
            }

            if (PinchEvent[0] != null && PinchEvent[1] != null)
            {
                float distance = Vector3.Distance(PinchEvent[0].Location, PinchEvent[1].Location);
                //Debug.Log(distance);
                if (distance < PinchDistance && lastSpawn >= spawnRate)
                {
                    Vector3 center = PinchEvent[0].Location + ((PinchEvent[1].Location - PinchEvent[0].Location) / 2);
                    lastSpawned = Instantiate(Item, center, Quaternion.identity) as GameObject;
                    Block.Create(lastSpawned);
                    lastSpawn = 0;
                }

                if (lastSpawn < spawnRate && lastSpawned != null)
                {
                    lastSpawned.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (distance);
                    lastSpawned.GetComponent<Collider>().enabled = false;
                    lastSpawned.GetComponent<Rigidbody>().useGravity = false;
                }

                PinchEvent[0] = null;
                PinchEvent[1] = null;
            }

            if(PinchEvent[0] != null || PinchEvent[1] != null)
            {
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
