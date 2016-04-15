﻿using UnityEngine;
using System.Collections.Generic;
using Game.Leap;

namespace Game.Manager
{
    public class Gravity : MonoBehaviour
    {
        public static int COOLDOWN = 2000;

        public bool GravityEnabled = false;
        public List<Rigidbody> Watching;

        public HandModel[] Hands;

        public Vector3 UP = Vector3.up;
        public float FacingOffset = 0.8f;
        public int HandsMovingLastFrameCount;

        private int Cooldown = 0;
        private Vector3[] InitalHandPositions;
        private Vector3[] LastFrameHandPositions;
        // Use this for initialization
        void Start()
        {
            Block.OnCreated += NewObject;
            InitalHandPositions = new Vector3[Hands.Length];
            LastFrameHandPositions = new Vector3[Hands.Length];
        }

        void Destory()
        {
            Block.OnCreated -= NewObject;
        }

        void NewObject(GameObject obj)
        {
            Rigidbody body = obj.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.useGravity = GravityEnabled;
                Watching.Add(body);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                ModifyGravity(!GravityEnabled);
            }
            HandGravityCheck();
        }
        

        void ModifyGravity(bool gravity)
        {
            GravityEnabled = gravity;
            Debug.Log(string.Format("Gravity: {0}", GravityEnabled));
            foreach (var obj in Watching)
            {
                if (obj != null)
                {
                    obj.useGravity = GravityEnabled;
                }
            }
        }

        void HandGravityCheck()
        {
            bool bothFacingUpThisFrame = true;
            bool bothFacingDownThisFrame = true;
            for(int h = 0; h < Hands.Length; h++)
            {
                HandModel hand = Hands[h];
                if (hand != null && hand.IsTracked)
                {     
                    if(hand.HandState == HandState.Down)
                    {
                        bothFacingUpThisFrame = false;
                    }
                    if(hand.HandState == HandState.Up)
                    {
                        bothFacingDownThisFrame = false;
                    }
                    if(hand.HandState == HandState.NotTracking)
                    {
                        bothFacingDownThisFrame = false;
                        bothFacingUpThisFrame = false;
                    }

                    if (hand.HandState == HandState.Down || hand.HandState == HandState.Up)
                    {
                        if(InitalHandPositions == null)
                        {
                            InitalHandPositions = new Vector3[Hands.Length];
                            InitalHandPositions[h] = hand.GetPalmPosition();
                        }
                        if (LastFrameHandPositions == null)
                        {
                            LastFrameHandPositions = new Vector3[Hands.Length];
                        }

                        if (InitalHandPositions[h] == null)
                        {
                            InitalHandPositions[h] = hand.GetPalmPosition();
                        }
                        else
                        {
                            LastFrameHandPositions[h] = hand.GetPalmPosition();
                        }
                    }
                }
                else
                {
                    bothFacingDownThisFrame = false;
                    bothFacingUpThisFrame = false;
                }
            }

            //Debug.Log(string.Format("Up: {0} | Down: {1}", bothFacingUpThisFrame, bothFacingDownThisFrame));

            if(bothFacingUpThisFrame)
            {
                //Debug.Log(string.Format("Up: {0}", bothFacingUpThisFrame));
                bool changeLastFrame = MovingCheck();
                ComputeChange(changeLastFrame, false);
            }
            else if(bothFacingDownThisFrame)
            {
                //Debug.Log(string.Format("Down: {0}", bothFacingDownThisFrame));
                bool changeLastFrame = MovingCheck(false);
                ComputeChange(changeLastFrame, true);
            }
            else
            {
                InitalHandPositions = null;
                LastFrameHandPositions = null;
                HandsMovingLastFrameCount = 0;
                Cooldown = 0;
            }
        }

        void ComputeChange(bool handsMovingThisFrame, bool gravityState)
        {
            if (handsMovingThisFrame && HandsMovingLastFrameCount == 0)
            {
                Cooldown--;
            }

            if (handsMovingThisFrame && Cooldown <= 0)
            {
                HandsMovingLastFrameCount++;
            }
            if (HandsMovingLastFrameCount > 150 && handsMovingThisFrame)
            {
                ModifyGravity(gravityState);
                HandsMovingLastFrameCount = 0;
                Cooldown = COOLDOWN;
            }
        }

        bool MovingCheck(bool checkMovingUp = true)
        {
            bool handsMovingThisFrame = false;
            for(int i = 0; i < InitalHandPositions.Length; i++)
            {
                Vector3 initalPos = InitalHandPositions[i];
                if(initalPos != null && LastFrameHandPositions != null)
                {
                    Vector3 lastPos = LastFrameHandPositions[i];
                    if (lastPos != null)
                    {
                        Vector3 direction = lastPos - initalPos;
                        float directionDot = Vector3.Dot(direction, UP);
                        if(checkMovingUp)
                        {
                            if (directionDot > 0)
                            {
                                handsMovingThisFrame = true;
                            }
                        }
                        else
                        {
                            if (directionDot < 0)
                            {
                                handsMovingThisFrame = true;
                            }
                        }
                    }
                }
            }

            return handsMovingThisFrame;
        }
    }
}