using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace C2D
{
    [Serializable]
    public class CollisionStateInfo
    {
        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;

        public bool isInvalid
        {
            get { return up && down && right && left; }
        }

        public void Clear()
        {
            up = down = left = right = false;
        }
    }
}