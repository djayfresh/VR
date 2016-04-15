using UnityEngine;
using System.Collections;

namespace Game.Manager
{
    public class Block : MonoBehaviour
    {
        public delegate void Created(GameObject obj);
        public static event Created OnCreated;

        public delegate void Destoryed(GameObject obj);
        public static event Destoryed OnDestory;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void Create(GameObject obj)
        {
            OnCreated(obj);
        }

        public static void Destory(GameObject obj)
        {
            OnDestory(obj);
        }

    }
}