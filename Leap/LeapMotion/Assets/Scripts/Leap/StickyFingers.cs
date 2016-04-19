using UnityEngine;
using System.Collections;
using Leap.Unity;
using Game.Manager;
using System.Collections.Generic;
using Game.Components;
using BlockManager = Game.Manager.BlockManager;

public class StickyFingers : MonoBehaviour {

    public RigidFinger[] Fingers;

    public List<GameObject> Watching;
    public List<StickyFinger> Updating;

    public float StickyRadius = 0.5f;
	// Use this for initialization
	void Start () {
        BlockManager.OnCreated += AddBlock;
        BlockManager.OnDestory += RemoveBlock;
        Game.Components.Block.OnColliderEnter += Collision;
        Watching = new List<GameObject>();
        Updating = new List<StickyFinger>();
	}

    void Destory()
    {
        BlockManager.OnCreated -= AddBlock;
        BlockManager.OnDestory -= RemoveBlock;
        Game.Components.Block.OnColliderEnter -= Collision;
    }

    void AddBlock(GameObject obj)
    {
        Watching.Add(obj);
    }

    void RemoveBlock(GameObject obj)
    {
        Watching.Remove(obj);
        Updating.Remove(new StickyFinger()
        {
            Object = obj
        });
    }

    void Collision(ColliderEvent evt)
    {
        if(Watching.Contains(evt.HitObject))
        {
            foreach(var finger in Fingers)
            {
                Collider collider = finger.GetComponentInChildren<Collider>();
                if (collider != null && collider == evt.HitCollision.collider)
                {
                    var contactPoint = evt.HitCollision.contacts[0];
                    StickyFinger thisSet = new StickyFinger()
                    {
                        Finger = finger,
                        Object = evt.HitObject,
                        ContactPoint = contactPoint.point - evt.HitCollision.collider.transform.position
                    };
                    collider.enabled = false;
                    Updating.Add(thisSet);
                    Debug.Log("Found the Finger");
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        FindFingers();
        UpdateFingers();
	}

    void UpdateFingers()
    {
        foreach(var stickyFinger in Updating)
        {
            Rigidbody body = stickyFinger.Object.GetComponent<Rigidbody>();
            body.useGravity = true;
            stickyFinger.Object.transform.position = (stickyFinger.ContactPoint) + stickyFinger.Finger.GetTipPosition();
        }
    }
    
    void FindFingers()
    {
        foreach (var finger in Fingers)
        {
            foreach (var obj in Watching)
            {
                if (obj != null)
                {
                    StickyFinger thisSet = new StickyFinger()
                    {
                        Finger = finger,
                        Object = obj
                    };

                    if (Updating.Contains(thisSet))
                        continue;

                    Vector3 fingerPos = finger.GetTipPosition(); //finger.GetTipPosition();
                    //Debug.Log(string.Format("Finger Position: {0}", fingerPos));
                    //Debug.Log(string.Format("Object Position: {0}", obj.transform.position));

                    float dist = Vector3.Distance(fingerPos, obj.transform.position);
                    //Debug.Log("Distance: " + dist);
                    if (dist <= StickyRadius)
                    {
                        Updating.Add(thisSet);
                    }
                }
            }
        }
    }
}

public class StickyFinger
{
    public FingerModel Finger { get; set; }
    public GameObject Object { get; set; }
    public Vector3 ContactPoint { get; set; }

    public override bool Equals(object obj)
    {
        StickyFinger other = obj as StickyFinger;
        if (other == null)
            return false;

        return other.Object == this.Object;
    }
}
