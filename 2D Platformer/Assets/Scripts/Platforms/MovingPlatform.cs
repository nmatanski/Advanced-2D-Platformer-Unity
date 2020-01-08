using UnityEngine;

namespace Platformer
{
    public class MovingPlatform : MonoBehaviour
    {
        public Vector3 Difference { get; private set; }


        [SerializeField]
        private GameObject platform;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private Transform[] points;

        [SerializeField]
        private int pointSelection = 1;

        private Transform currentPoint;
        private Vector3 lastPosition;


        // Start is called before the first frame update
        private void Start()
        {
            currentPoint = points[pointSelection];
            lastPosition = transform.GetChild(0).position;
        }

        // Update is called once per frame
        private void Update()
        {
            Difference = platform.transform.position - lastPosition;
            Difference /= Time.deltaTime;
            lastPosition = platform.transform.position;

            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

            if (platform.transform.position == currentPoint.position)
            {
                pointSelection++;
                if (pointSelection == points.Length)
                    pointSelection = 0;
                currentPoint = points[pointSelection];
            }
        }
    }
}
