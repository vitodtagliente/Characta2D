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

        void LateUpdate()
        {
            if ((character.collision.left || character.collision.right) && character.isJumping == false)
            {
                var h = Input.GetAxis("Horizontal");
                // Esegui lo slide solamente se l'utente direziona il personaggio vero la aprete di scivolo
                if ((h == -1f && character.collision.left) || (h == 1f && character.collision.right))
                {
                    character.velocity.y *= slideSpeed;
                    // L'oggetto ha iniziato ora a scivolare sul muro
                    wasSliding = true;
                }
                else wasSliding = false;
            }
            else wasSliding = false;
        }
    }
}
