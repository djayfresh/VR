using UnityEngine;
using System.Collections.Generic;

namespace Game.Manager
{
    public class DistanceDestoryer : MonoBehaviour
    {
        public Transform SpawnPoint;
        public float despawnDistance;
        public List<GameObject> Watching;

        // Use this for initialization
        void Start()
        {
            BlockManager.OnCreated += NewObject;
            Watching = new List<GameObject>();
        }

        void Destory()
        {
            BlockManager.OnCreated -= NewObject;
        }
                
        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < Watching.Count; i++)
            {
                GameObject obj = Watching[i];
                if (obj != null)
                {
                    float distance = Vector3.Distance(obj.transform.position, SpawnPoint.position);
                    if (distance >= despawnDistance)
                    {
                        Destroy(obj);
                        Watching[i] = null;
                    }
                }
            }
        }

        void NewObject(GameObject obj)
        {
            Watching.Add(obj);
        }
    }
}