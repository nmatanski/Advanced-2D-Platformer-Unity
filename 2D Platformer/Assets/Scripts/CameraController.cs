using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CameraController : MonoBehaviour
    {
        //cached
        private PlayerController playerController;

        //state

        private GameObject player;
#pragma warning disable CS0108 // 'CameraController.camera' hides inherited member 'Component.camera'. Use the new keyword if hiding was intended.
        private Transform camera;
#pragma warning restore CS0108 // 'CameraController.camera' hides inherited member 'Component.camera'. Use the new keyword if hiding was intended.

        private readonly float cameraZPos = -10f;

        //config

        [SerializeField]
        private float cameraXOffset = 5f;

        [SerializeField]
        private float cameraYOffset = 1f;

        [SerializeField]
        private float horizontalSpeed = 2f;

        [SerializeField]
        private float verticalSpeed = 2f;


        // Start is called before the first frame update
        private void Start()
        {
            if (!player)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }

            playerController = player.GetComponent<PlayerController>();

            camera = Camera.main.transform;
            camera.position = new Vector3(player.transform.position.x + cameraXOffset, player.transform.position.y + cameraYOffset, player.transform.position.z + cameraZPos);
        }

        // Update is called once per frame
        private void Update()
        {
            float newXPos = playerController.IsFacingRight ? cameraXOffset : -cameraXOffset;
            newXPos += player.transform.position.x;

            InterpolateCameraPosition(camera, new Vector2(newXPos, player.transform.position.y + cameraYOffset), cameraZPos, horizontalSpeed, verticalSpeed);
        }

        private void InterpolateCameraPosition(Transform camera, Vector2 newPositionXY, float cameraZPosition, float horizontalSpeed = 1f, float verticalSpeed = 1f)
        {
            camera.position = new Vector3(
                    Mathf.Lerp(camera.position.x, newPositionXY.x, horizontalSpeed * Time.deltaTime),
                    Mathf.Lerp(camera.position.y, newPositionXY.y, verticalSpeed * Time.deltaTime),
                    cameraZPosition);
        }
    }
}
