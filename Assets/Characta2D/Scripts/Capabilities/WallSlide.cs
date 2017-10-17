using UnityEngine;
using UnityEngine.Events;

// TODO: AI

namespace Characta2D
{
    [AddComponentMenu("Characta2D/Capability/WallSlide")]
    public class WallSlide : Characta2D.CharactaCapability
    {
		// slide speed
		public float slideSpeed = .8f;
		public string horizontalInputAxis = "Horizontal";
		// activation events
        public UnityEvent OnSlideStart = new UnityEvent();
        public UnityEvent OnSlideStop = new UnityEvent();
        // keep the slide state
        bool _wasSliding = false;
        bool wasSliding
        {
            get { return _wasSliding; }
            set
            {
                if(_wasSliding != value)
                {
                    if (value)
                        OnSlideStart.Invoke();
                    else OnSlideStop.Invoke();
                }
                _wasSliding = value;
            }
        }
		// AI input
		[HideInInspector]
		public float wallSlideDirection = 0f;

        void LateUpdate()
        {
			wallSlideDirection = 0f;

            if ((character.collision.left || character.collision.right) && character.isJumping == false)
            {
				float h = (isPlayer) ? Input.GetAxis(horizontalInputAxis) : wallSlideDirection;
				// slide only if the input look at the place in which there is the wall
                if ((h == -1f && character.collision.left) || (h == 1f && character.collision.right))
                {
                    character.velocity.y *= slideSpeed;
                    // the character started to slide on the wall
                    wasSliding = true;
                }
                else wasSliding = false;
            }
            else wasSliding = false;
        }
    }
}
