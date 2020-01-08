using System.Collections;
using UnityEngine;

namespace Platformer
{
    public class CollapsablePlatform : MonoBehaviour
    {
        public void CollapsePlatform(float delay)
        {
            StartCoroutine(Collapse(GetComponent<Rigidbody2D>(), delay));
        }

        private IEnumerator Collapse(Rigidbody2D rigidbody, float delay)
        {
            yield return new WaitForSeconds(delay);
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.mass = 2;
            rigidbody.gravityScale = 3f;
        }
    }
}
