using System;
using UnityEngine;

/*
 * Store the collision state
 */

namespace Characta2D
{
    [Serializable]
    public class CollisionStateInfo
    {
        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;

        public bool IsInvalid
        {
            get { return up && down && right && left; }
        }

        public void Clear()
        {
            up = down = left = right = false;
        }
    }
}
