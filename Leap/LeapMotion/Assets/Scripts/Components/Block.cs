using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public class Block : MonoBehaviour
    {
        public delegate void ColliderEnter(ColliderEvent collider);
        public static event ColliderEnter OnColliderEnter;

        void OnCollisionEnter(Collision collision)
        {
            if (OnColliderEnter != null)
                OnColliderEnter(new ColliderEvent()
                {
                    HitCollision = collision,
                    HitObject = gameObject
                });
        }
    }

    public class ColliderEvent
    {
        public Collision HitCollision { get; set; }
        public GameObject HitObject { get; set; }
    }
}
