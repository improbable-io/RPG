using UnityEngine;

namespace Assets.Gamelogic.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        float distance = 20.0f;
        float actualDistance = 20.0f;
        float rotation = 0.0f;

        public Transform cameraRootTransform;
        public Transform cameraTransform;

        public float distMin;
        public float distMax;
        public float angleMin;
        public float angleMax;
        public float speed = 1.0f;

        private void OnEnable()
        {
            UpdateDistance(distance);
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
                UpdateRotation(rotation + Input.GetAxis("Mouse X") * 5);

            UpdateDistance(distance - Input.GetAxis("Mouse ScrollWheel") * distance * speed);
        }

        private void UpdateRotation(float r)
        {
            rotation = r;
            cameraRootTransform.rotation = Quaternion.AngleAxis(r, new Vector3(0.0f, 1.0f, 0.0f));
        }

        private void UpdateDistance(float d)
        {
            distance = Mathf.Clamp(d, distMin, distMax);
            actualDistance = dtLerp(actualDistance, distance, 0.9f, 0.25f);
            float angle = (actualDistance - distMin) / (distMax - distMin) * (angleMax - angleMin) + angleMin;
            cameraTransform.localPosition = new Vector3(0.0f, actualDistance * Mathf.Sin(Mathf.Deg2Rad * angle), -actualDistance * Mathf.Cos(Mathf.Deg2Rad * angle));
            cameraTransform.localRotation = Quaternion.AngleAxis(angle, new Vector3(1.0f, 0.0f, 0.0f));
        }

        // This lerp should complete p% in t seconds
        private float dtLerp(float a, float b, float p, float t)
        {
            return Mathf.Lerp(a, b, 1.0f - Mathf.Pow(1.0f - p, Time.deltaTime / t));
        }
    }
}