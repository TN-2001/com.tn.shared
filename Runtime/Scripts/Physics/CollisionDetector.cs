using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Library.Physics
{
    [RequireComponent(typeof(Collider))]
    public class CollisionDetector : MonoBehaviour
    {
        public string tagName = null; // 当たり判定の対象のタグ

        // 引数にColliderを持ったUnityEvent
        [HideInInspector] public UnityEvent<Collider> onTriggerEnter = null;
        [HideInInspector] public UnityEvent<Collider> onTriggerStay = null;
        [HideInInspector] public UnityEvent<Collider> onTriggerExit = null;

        [SerializeField, ReadOnly] private List<Collider> colliders = new();
        public List<Collider> Colliders => colliders;


        private void OnTriggerEnter(Collider other)
        {
            if (tagName != "")
            {
                if (other.gameObject.CompareTag(tagName))
                {
                    if (!colliders.Contains(other))
                    {
                        colliders.Add(other);
                        onTriggerEnter?.Invoke(other);
                    }
                }
            }
            else
            {
                if (!colliders.Contains(other))
                {
                    colliders.Add(other);
                    onTriggerEnter?.Invoke(other);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (tagName != "")
            {
                if (other.gameObject.CompareTag(tagName))
                {
                    onTriggerStay?.Invoke(other);
                }
            }
            else
            {
                onTriggerStay?.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (tagName != "")
            {
                if (other.gameObject.CompareTag(tagName))
                {
                    if (colliders.Contains(other))
                    {
                        colliders.Remove(other);
                        onTriggerExit?.Invoke(other);
                    }
                }
            }
            else
            {
                if (colliders.Contains(other))
                {
                    colliders.Remove(other);
                    onTriggerExit?.Invoke(other);
                }
            }
        }
    }
}
