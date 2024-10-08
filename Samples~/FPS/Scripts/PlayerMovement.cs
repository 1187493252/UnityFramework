using UnityEngine;

namespace FPS
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 12f;
        public float gravity = -10f;
        public float jumpHeight = 2f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        private Vector3 velocity;
        private bool isGrounded;

        private void Update()
        {
            float x, y, z;
            bool jumpPressed = false;

            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Verticaly");
            z = Input.GetAxis("Vertical");
            jumpPressed = Input.GetButtonDown("Jump");

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            Vector3 move = Vector3.zero;

            if (gravity.Equals(0))
            {
                move = transform.right * x + transform.up * y + transform.forward * z;
            }
            else
            {
                move = transform.right * x + transform.forward * z;
            }

            controller.Move(move * speed * Time.deltaTime);

            if (jumpPressed && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
