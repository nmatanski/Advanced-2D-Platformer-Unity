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
        private Transform camera;

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
            if (playerController.IsFacingRight)
            {
                camera.position = new Vector3(
                    Mathf.Lerp(camera.position.x, player.transform.position.x + cameraXOffset, horizontalSpeed * Time.deltaTime),
                    Mathf.Lerp(camera.position.y, player.transform.position.y + cameraYOffset, verticalSpeed * Time.deltaTime),
                    cameraZPos);
            }
            else
            {
                camera.position = new Vector3(
                    Mathf.Lerp(camera.position.x, player.transform.position.x - cameraXOffset, horizontalSpeed * Time.deltaTime),
                    Mathf.Lerp(camera.position.y, player.transform.position.y + cameraYOffset, verticalSpeed * Time.deltaTime),
                    cameraZPos);
            }
        }
    }
}
