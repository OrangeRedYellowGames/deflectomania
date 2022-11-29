using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts {
    public class RandomizeColor : MonoBehaviour {
        public Color[] colors;
        private SpriteRenderer _spriteRenderer;
        private PlayerInput _playerInput;

        private void Awake() {
            _spriteRenderer = gameObject.GetComponentInChildren(typeof(SpriteRenderer)) as SpriteRenderer;
            _playerInput = gameObject.GetComponent<PlayerInput>();
        }

        private void Start() {
            _spriteRenderer.color = colors[_playerInput.playerIndex];
        }
    }
}