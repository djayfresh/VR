using UnityEngine;
using System.Collections;
using Game.Manager;
using Game.Component.Leap;
using System;
using System.Collections.Generic;

namespace Game.Leap
{
    public class BlockGrab : MonoBehaviour
    {
        public HandGrabModel GrabbedObject;

        public List<GameObject> Watching;
        public float MaxGrabDistance = 0.5f;
        void Start ()
        {
            Watching = new List<GameObject>();
            HandManager.OnGrab += Grab;
            HandManager.ReleaseGrip += ReleaseGrip;
            BlockManager.OnCreated += NewObject;
            BlockManager.OnDestroyed += RemoveObject;
            PinchManager.OnPinch += RemoveObjectPinch;
        }

        void OnDestroy()
        {
            HandManager.OnGrab -= Grab;
            HandManager.ReleaseGrip -= ReleaseGrip;
            BlockManager.OnCreated -= NewObject;
            BlockManager.OnDestroyed -= RemoveObject;
        }

        void NewObject(GameObject obj)
        {
            Watching.Add(obj);
        }
        void RemoveObject(GameObject obj)
        {
            if(GrabbedObject != null && GrabbedObject.GrabbedObject == obj)
            {
                GrabbedObject = null;
            }
            Watching.Remove(obj);
        }

        void RemoveObjectPinch(PinchEvent evnt)
        {
            GrabbedObject = null;
        }

        void Grab(HandModel hand)
        {
            Vector3 palm = hand.GetPalmPosition();
            if (GrabbedObject == null)
            {
                GameObject close = GetNearbyObject(palm);
                if (close != null)
                {
                    Debug.Log("Hand Grab");
                    GrabbedObject = new HandGrabModel()
                    {
                        ActiveHand = hand,
                        GrabbedObject = close,
                        HandPositionLastFrame = palm
                    };
                }
            }
            else if(GrabbedObject.ActiveHand == hand)
            {
                GrabbedObject.GrabbedObject.transform.position += (palm - GrabbedObject.HandPositionLastFrame);
                Rigidbody body = GrabbedObject.GrabbedObject.GetComponent<Rigidbody>();
                body.angularVelocity = Vector3.zero;
                body.velocity = Vector3.zero;
                GrabbedObject.HandPositionLastFrame = palm;
            }
        }

        void ReleaseGrip(HandModel hand)
        {
            if (GrabbedObject != null && GrabbedObject.ActiveHand == hand)
            {
                Debug.Log("Release Grab");
                Rigidbody body = GrabbedObject.GrabbedObject.GetComponent<Rigidbody>();
                body.AddForce((GrabbedObject.ActiveHand.GetPalmPosition() - GrabbedObject.HandPositionLastFrame) * 20);
                GrabbedObject = null;
            }
        }

        private GameObject GetNearbyObject(Vector3 position)
        {
            GameObject returnObject = null;
            float lastDistance = MaxGrabDistance;
            foreach(var obj in Watching)
            {
                Collider collider = obj.GetComponent<Collider>();
                Vector3 postiionOnObj = collider.ClosestPointOnBounds(position);
                float distance = Vector3.Distance(postiionOnObj, position);

                if(distance < lastDistance)
                {
                    returnObject = obj;
                }
            }

            return returnObject;
        }
    }

    public class HandGrabModel
    {
        public GameObject GrabbedObject { get; set; }
        public HandModel ActiveHand { get; set; }
        public Vector3 HandPositionLastFrame { get; set; }
    }
}
