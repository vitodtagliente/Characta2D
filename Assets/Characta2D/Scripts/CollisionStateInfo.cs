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

        public Characta2D.HorizontalHitStateInfo topInfo;
        public Characta2D.HorizontalHitStateInfo bottomInfo;
        public Characta2D.VerticalHitStateInfo leftInfo;
        public Characta2D.VerticalHitStateInfo rightInfo;

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

    [Serializable]
    public class HorizontalHitStateInfo
    {
        public bool left = false;
        public bool center = false;
        public bool right = false;
        
        public bool isInCollision
        {
            get { return left || center || right; }
        }

        public bool isNull
        {
            get { return !left && !center && !right; }
        }

        public void Clear()
        {
            left = center = right = false;
        }
    }

    [Serializable]
    public class VerticalHitStateInfo
    {
        public bool top = false;
        public bool center = false;
        public bool bottom = false;

        public bool isInCollision
        {
            get { return bottom || center || top; }
        }

        public bool isNull
        {
            get { return !bottom && !center && !top; }
        }

        public void Clear()
        {
            top = center = bottom = false;
        }
    }
}
