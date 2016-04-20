using UnityEngine;
using System.Collections;

namespace Game.Manager
{
    public class BlockManager : MonoBehaviour
    {
        public delegate void Created(GameObject obj);
        public static event Created OnCreated;

        public delegate void Destroyed(GameObject obj);
        public static event Destroyed OnDestroyed;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void CreateBlock(GameObject obj)
        {
            if (OnCreated != null)
            {
                OnCreated(obj);
            }
        }

        public static void DestroyBlock(GameObject obj)
        {
            if (OnDestroyed != null)
            {
                OnDestroyed(obj);
            }
        }

    }
}