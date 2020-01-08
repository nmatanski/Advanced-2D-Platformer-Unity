using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
	public class MovingPlatform : MonoBehaviour
	{
        [SerializeField]
        private GameObject platform;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private Transform[] points;

        [SerializeField]
        private int pointSelection = 1;

        private Transform currentPoint;


        // Start is called before the first frame update
        private void Start()
		{
            currentPoint = points[pointSelection];
		}

		// Update is called once per frame
		private void Update()
		{
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

            if (platform.transform.position == currentPoint.position)
            {
                pointSelection++;
                if (pointSelection == points.Length)
                {
                    pointSelection = 0;
                }
                currentPoint = points[pointSelection];
            }
		}
	}
}
