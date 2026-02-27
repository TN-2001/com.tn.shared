using UnityEngine;

namespace Library.Animation
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private Rigidbody pelvis;
        [SerializeField] private Rigidbody[] rigidbodies;

        private Animator animator;


        private void Start()
        {
            animator = GetComponent<Animator>();
            SetRagdoll(false);
        }


        [ContextMenu("Set Rigidbodies")]
        private void SetRigidbodies()
        {
            rigidbodies = pelvis.GetComponentsInChildren<Rigidbody>();
        }

        [ContextMenu("Set Ragdoll True")]
        private void EnableRagdoll()
        {
            SetRagdoll(true);
        }

        [ContextMenu("Set Ragdoll False")]
        private void DisableRagdoll()
        {
            SetRagdoll(false);
        }

        public void SetRagdoll(bool newValue)
        {
            // ボーン上のRigidbodyのOn/Offを切り替え
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = !newValue;
                rb.detectCollisions = newValue;
            }

            // AnimatorのOn/Offを切り替え
            if (animator != null)
            {
                animator.enabled = !newValue;
            }
        }
    }
}
