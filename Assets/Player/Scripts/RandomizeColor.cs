using Mirror;
using UnityEngine;

namespace Player.Scripts {
    public class RandomizeColor : NetworkBehaviour {
        [SyncVar] private Color _currentColor;
        public Color[] colors;
        private SpriteRenderer _spriteRenderer;

        private void Awake() {
            _spriteRenderer = gameObject.GetComponentInChildren(typeof(SpriteRenderer)) as SpriteRenderer;
        }

        private void Start() {
            if (!isLocalPlayer) {
                return;
            }

            GetColor();
        }

        private void Update() {
            if (_spriteRenderer.color != _currentColor) {
                _spriteRenderer.color = _currentColor;
            }
        }

        // Should probably figure out a better way to assign player colors
        [Command]
        private void GetColor() {
            var color = colors[(netIdentity.netId - 1) % colors.Length];
            _currentColor = color;
        }
    }
}