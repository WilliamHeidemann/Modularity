using UnityEngine;

namespace Runtime.Components
{
    public class FollowCursor : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private void Update()
        {
            var mousePos = Input.mousePosition;
            var direction = new Vector2(
                mousePos.x - transform.position.x,
                mousePos.y - transform.position.y
            );

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _speed * Time.deltaTime);
        }
    }
}
