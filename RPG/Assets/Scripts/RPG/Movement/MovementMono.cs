using UnityEngine;

namespace RPG.Movement
{
    public partial class MovementMono : MonoBehaviour
    {
        public Vector3 Grivity { get; set; } = new Vector3(0, -9.81f, 0);
        public Vector3 Velocity { get; set; } = Vector3.zero;
        public Quaternion RotationVelocity { get; set; } = Quaternion.identity;
        public bool IsGrounded { get; set; } = false;
        public Vector3 Forward => transform.forward;
        public Quaternion TargetRotation { get; set; } = Quaternion.identity;
        private void Update()
        {
            UpdateGround();

            ApplyGravity();
            ApplyMovement();
        }

        private void UpdateGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
            {
                IsGrounded = true;
                if (Velocity.y < 0)
                {
                    Velocity = new Vector3(Velocity.x, 0, Velocity.z);
                }
            }
            else
            {
                IsGrounded = false;
            }
        }

        private void ApplyGravity()
        {
            Velocity += Grivity * Time.deltaTime;
        }

        private void ApplyMovement()
        {
            transform.position += Velocity * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, RotationVelocity, Time.deltaTime * 5f);
        }
    }
}