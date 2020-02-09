using System.Collections;
using UnityEngine;

namespace Platformer
{
    public class ModifyTime : MonoBehaviour
    {
        [SerializeField]
        private float newTime = 0.05f;

        [SerializeField]
        private int restoreSpeed = 10;

        [SerializeField]
        private float delay = 0.1f;

        [SerializeField]
        private GameObject impactEffect;

        [SerializeField]
        private Animator animator;

        private float speed;
        private bool isTimeRestored;


        // Start is called before the first frame update
        private void Start()
        {
            isTimeRestored = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (isTimeRestored)
            {
                if (Time.timeScale < 1f)
                {
                    Time.timeScale += Time.deltaTime * speed;
                }
                else
                {
                    Time.timeScale = 1f;
                    isTimeRestored = false;

                    ///TODO: add when having particle system or another FX
                    //animator.SetBool("IsTimeStopped", false);
                }
            }
        }

        public void StopTime()
        {
            speed = restoreSpeed;

            if (delay > 0)
            {
                StopCoroutine(StartTime(delay));
                StartCoroutine(StartTime(delay));
            }
            else
            {
                isTimeRestored = true;
            }

            ///TODO: add when having particle system or another FX
            //Instantiate(impactEffect, transform.position, Quaternion.identity);
            //animator.SetBool("IsTimeStopped", true);

            Time.timeScale = newTime;
        }

        private IEnumerator StartTime(float delay)
        {
            isTimeRestored = true;
            yield return new WaitForSecondsRealtime(delay);
        }
    }
}
