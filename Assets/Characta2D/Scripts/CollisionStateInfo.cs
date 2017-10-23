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
        public bool top = false;
        public bool bottom = false;
        public bool left = false;
        public bool right = false;

        public Vector2 groundNormal = Vector2.zero;

        public bool isInvalid
        {
            get { return top && bottom && right && left; }
        }

		public void Clear()
		{
			top = bottom = left = right = false;
            groundNormal = Vector2.zero;
		}
    }
}
