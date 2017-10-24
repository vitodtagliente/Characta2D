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
        public bool top 
		{
			get { return !topInfo.Empty; }
		}
		public bool bottom
		{
			get { return !bottomInfo.Empty; }			
		}
        public bool left
		{
			get { return !leftInfo.Empty; }
		}
        public bool right
		{
			get { return !rightInfo.Empty; }
		}

		public Characta2D.HorizontalHitStateInfo topInfo = new HorizontalHitStateInfo();
		public Characta2D.HorizontalHitStateInfo bottomInfo = new HorizontalHitStateInfo();
		public Characta2D.VerticalHitStateInfo leftInfo = new VerticalHitStateInfo();
		public Characta2D.VerticalHitStateInfo rightInfo = new VerticalHitStateInfo();

        public Vector2 groundNormal = Vector2.zero;

        public bool isInvalid
        {
            get { return top && bottom && right && left; }
        }

		public void Clear()
		{
            groundNormal = Vector2.zero;
			topInfo.Clear ();
			bottomInfo.Clear ();
			leftInfo.Clear ();
			rightInfo.Clear ();
		}
    }

    [Serializable]
    public class HorizontalHitStateInfo
    {
        public bool left = false;
        public bool center = false;
        public bool right = false;
        
        public bool Empty
       	{
            get { return !left && !center && !right; }
        }

		public void Set(bool value){
			left = center = right = value;	
		}

        public void Clear(){
            left = center = right = false;
		}
	}

    [Serializable]
    public class VerticalHitStateInfo
    {
        public bool top = false;
        public bool center = false;
        public bool bottom = false;

		public bool Empty
        {
            get { return !bottom && !center && !top; }
        }

		public void Set(bool value){
			bottom = center = top = value;	
		}

		public void Clear(){
            top = center = bottom = false;
        }
    }
}
