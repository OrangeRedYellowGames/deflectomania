using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Environment.MovingPlatforms {
    public class GenerateTiles : MonoBehaviour {
        private Tilemap _t;
        public TileBase tile;
        public int width = 1;
        public int length = 1;

        // Start is called before the first frame update
        private void Awake() {
            Assert.IsNotNull(tile);
            Assert.IsNotNull(_t);
            Assert.IsTrue(width >= 0);
            Assert.IsTrue(length >= 0);
        }

        private void Start() {
            ChangePlatformSize();
        }
        
        /// <summary>
        /// This Function is called to Change platform size to the given width and length provided in the class.
        /// </summary>
        private void ChangePlatformSize() {
            _t = GetComponent<Tilemap>();
            _t.ClearAllTiles();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < length; j++) {
                    _t.SetTile(new Vector3Int(i, j, 0), tile);
                }
            }
        }
// Reference to suppress warning for "SendMessage cannot be called during Awake, CheckConsistency, or OnValidate"
// https://forum.unity.com/threads/sendmessage-cannot-be-called-during-awake-checkconsistency-or-onvalidate-can-we-suppress.537265/#post-5560519
#if UNITY_EDITOR
        /// <summary>
        /// Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector. 
        /// </summary>
        private void OnValidate() {
            UnityEditor.EditorApplication.delayCall += _OnValidate;
        }
#endif
        /// <summary>
        /// This function is called by the OnValidate to apply actions to respond to changes in the editor.
        /// </summary>
        private void _OnValidate() {
            if(width < 0 || length < 0 )
                return;
            ChangePlatformSize();
        }
    }
}
