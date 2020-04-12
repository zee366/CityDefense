using UnityEngine;

namespace Utils {
    public class SphereDebug : MonoBehaviour {

        private SphereCollider _col;

        private void OnDrawGizmos() {
            _col = GetComponent<SphereCollider>();
            if ( _col != null )
                Gizmos.DrawWireSphere(transform.position, _col.radius);

            Gizmos.DrawSphere(transform.position, 0.2f);
        }

    }
}