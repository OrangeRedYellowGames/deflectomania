using System.Collections.Generic;
using NLog;
using UnityEngine;
using Logger = NLog.Logger;

// Reference: https://www.raywenderlich.com/847-object-pooling-in-unity
namespace Entities.ObjectPooler {
    public class ObjectPooler : MonoBehaviour {
        public static ObjectPooler SharedInstance;
        public List<GameObject> pooledObjects;
        public List<ObjectPoolItem> itemsToPool;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private void Awake() {
            SharedInstance = this;
        }

        private void Start() {
            // Create a list of game objects and instantiate them all and set them to inactive.
            pooledObjects = new List<GameObject>();
            foreach (ObjectPoolItem item in itemsToPool) {
                for (int i = 0; i < item.amountToPool; i++) {
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
            Logger.Debug("Instantiated all pooled objects!");
        }
    
        public GameObject GetPooledObject(string objectTag) {
            // Check if any object is available and return it.
            foreach (var pooledObject in pooledObjects) {
                if (!pooledObject.activeInHierarchy && pooledObject.CompareTag(objectTag)) {
                    Logger.Trace("Returning object!");
                    return pooledObject;
                }
            }
            // If cannot find anything, expand.
            foreach (ObjectPoolItem item in itemsToPool) {
                if (item.objectToPool.CompareTag(objectTag)) {
                    if (item.shouldExpand) {
                        Logger.Debug("Expanding " + objectTag +" pool of objects!");
                        GameObject obj = (GameObject)Instantiate(item.objectToPool);
                        obj.SetActive(false);
                        pooledObjects.Add(obj);
                        return obj;
                    }
                }
            }
            return null;
        }
    
    }
}
