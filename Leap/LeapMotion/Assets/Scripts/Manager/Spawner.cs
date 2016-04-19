using Leap.Unity;
using Game.Leap;
using UnityEngine;

namespace Game.Manager
{
    public class Spawner : MonoBehaviour
    {
        public Transform SpawnPoint;

        public GameObject Item;
        public GameObject TempItem;

        private PinchEvent[] PinchEvent = new PinchEvent[2];
        public float PinchDistance = 0.5f;
        
        public float spawnRate = 0.5f;
        private float lastSpawn = 1;

        private LastSpawned lastSpawned;
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
                    lastSpawned.Obj.SetActive(true);
                    lastSpawned.Obj.transform.position = lastSpawned.Temp.transform.position;
                    lastSpawned.Obj.transform.rotation = lastSpawned.Temp.transform.rotation;
                    lastSpawned.Obj.transform.localScale = lastSpawned.Temp.transform.localScale;
                    Destroy(lastSpawned.Temp);
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
                    lastSpawned = new LastSpawned();
                    lastSpawned.Obj = Instantiate(Item, center, Quaternion.identity) as GameObject;
                    lastSpawned.Obj.SetActive(false);
                    Block.Create(lastSpawned.Obj);

                    lastSpawned.Temp = Instantiate(TempItem, center, Quaternion.identity) as GameObject;
                    lastSpawn = 0;
                }

                if (lastSpawn < spawnRate && lastSpawned != null)
                {
                    lastSpawned.Temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (distance);
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

    public class LastSpawned
    {
        public GameObject Obj;
        public GameObject Temp;
    }
}
