using UnityEngine;

/*
 * Simple Characta2D Player Controller
 * 
 */

namespace Characta2D
{
    [AddComponentMenu("Characta2D/PlayerController")]
    [RequireComponent(typeof(Characta2D.CharactaSprite))]
	public class CharactaPlayerController : MonoBehaviour
    {
        Characta2D.CharactaSprite _sprite;
        public Characta2D.CharactaSprite sprite
        {
            get
            {
                if(_sprite == null)
                    _sprite = GetComponent<Characta2D.CharactaSprite>();
                return _sprite;
            }
        }

        // The movement speed
        public float speed = 7f;
		public string horizontalInputAxis = "Horizontal";

        // Get the user input
        public virtual void Update()
        {
            sprite.ApplyInput(
                Input.GetAxis(horizontalInputAxis) * speed,
                0.0f
            );
        }

    }
}
