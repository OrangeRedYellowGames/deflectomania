using Mirror;

namespace Player.Scripts {
    /// <summary>
    /// Adapted from https://forum.unity.com/threads/the-new-input-system-package-doesnt-seem-to-work-with-mirror-networking-api.951107/
    /// </summary>
    public class PlayerInputActivator : NetworkBehaviour {
        public override void OnStartAuthority() {
            base.OnStartAuthority();

            UnityEngine.InputSystem.PlayerInput playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            playerInput.enabled = true;
        }
    }
}