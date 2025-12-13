using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    private bool _alive;
    private float _speed = 6.0f;
    private float _detectionRange = 35.0f;
    private float _attackRange = 32.0f;

    [SerializeField]
    private GameObject fireballPrefab;
    private GameObject _fireball;

    private void Start()
    {
        _alive = true;
    }

    void Update()
    {
        if (_alive)
        {

            GameObject player = GameObject.Find("MyPlayer");
            Transform detectionSourcePoint = this.gameObject.transform;

            // Check for obstructions
            RaycastHit[] hits = Physics.RaycastAll(detectionSourcePoint.position, (player.transform.position - detectionSourcePoint.position).normalized, _detectionRange, -1, QueryTriggerInteraction.Ignore);
            RaycastHit closestValidHit = new RaycastHit();
            closestValidHit.distance = Mathf.Infinity;
            Collider turretCollider = GetComponent<Collider>();
            Collider playerCollider = player.GetComponent<Collider>();
            Collider hitCollider = null;
            bool foundValidHit = false;
            bool playerDetected = false;
            int hitCount = 0;
            //string debuglog = "";
            foreach (var hit in hits)
            {
                hitCount += 1;
                if ((hit.collider != turretCollider) && hit.distance < closestValidHit.distance)
                {
                    hitCollider = hit.collider;
                    closestValidHit = hit;
                    foundValidHit = true;
                }
                if (foundValidHit)
                {
                    if (hitCollider == playerCollider)
                    {
                        playerDetected = true;
                    }
                }
            }

            if (playerDetected)
            {
                // Determine if player is within detection range
                float sqrDetectionRange = _detectionRange * _detectionRange;
                float sqrAttackRange = _attackRange * _attackRange;
                float closestSqrDistance = Mathf.Infinity;
                float sqrDistance = (player.transform.position - detectionSourcePoint.position).sqrMagnitude;
                if (sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
                {

                    // Orient towards the player
                    Vector3 lookPosition = player.transform.position;
                    Vector3 lookDirection = (transform.position - lookPosition).normalized;
                    if (lookDirection.sqrMagnitude != 0f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _speed);
                    }

                    // Move forward
                    transform.Translate(0, 0, -1 * speed * Time.deltaTime);

                    // Determine if player is within attack range
                    if (_fireball == null)
                    {
                        //Debug.Log(string.Format("sqrDistance = {0}, sqrAttackRange = {1}", sqrDistance, sqrAttackRange));
                        if (sqrDistance < sqrAttackRange)
                        {
                            //Debug.Log("Attack");
                            _fireball = Instantiate(fireballPrefab) as GameObject;
                            _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                            _fireball.transform.rotation = transform.rotation;
                        }
                    }
                }

            }
            else
            {
                // In "Wandering" Mode - Just Move forward
                transform.Translate(0, 0, speed * Time.deltaTime);

                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(ray, 0.75f, out hit))
                {
                    if (hit.distance < obstacleRange)
                    {
                        // hit an obstacle - turn
                        float angle = Random.Range(-110, 110);
                        transform.Rotate(0, angle, 0);
                    }
                }
            }

        }
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }
}
