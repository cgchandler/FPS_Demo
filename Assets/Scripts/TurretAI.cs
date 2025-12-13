using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TurretAI : MonoBehaviour
{

    private float _spinSpeed = 1.0f;
    private int _spinDirection = -1;
    private bool _playerLockedOn = false;
    private float _speed = 6.0f;
    private float _detectionRange = 35.0f;
    private float _attackRange = 32.0f;

    [SerializeField]
    private GameObject fireballPrefab;
    private GameObject _fireball;

    private void Start()
    {
        // turn the spot light off
        GameObject spotObject = GameObject.Find("Spot Light");
        Light spotLight = spotObject.GetComponent<Light>();
        spotLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {

        GameObject player = GameObject.Find("MyPlayer");
        Transform detectionSourcePoint = GameObject.Find("TurretEye").transform;

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

                _playerLockedOn = true; // Enter locked on Mode

                // turn the spot light on
                GameObject spotObject = GameObject.Find("Spot Light");
                Light spotLight = spotObject.GetComponent<Light>();
                spotLight.intensity = 45;

                // Orient towards the player
                Vector3 lookPosition = player.transform.position;
                Vector3 lookDirection = (transform.position - lookPosition).normalized;
                if (lookDirection.sqrMagnitude != 0f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _speed);
                }

                // Determine if player is within attack range
                if (_fireball == null)
                {
                    //Debug.Log(string.Format("sqrDistance = {0}, sqrAttackRange = {1}", sqrDistance, sqrAttackRange));
                    if (sqrDistance < sqrAttackRange)
                    {
                        //Debug.Log("Attack");
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * -1.5f);
                        _fireball.transform.rotation = transform.rotation;
                    }
                }
            }

        }
        else
        {
            if (_playerLockedOn)
            {
                // turn the spot light off
                GameObject spotObject = GameObject.Find("Spot Light");
                Light spotLight = spotObject.GetComponent<Light>();
                spotLight.intensity = 0;

                _spinDirection *= -1; // Reverse the Spin Direction
                _playerLockedOn = false; // Leave locked on mode
            }
            // Just Spin
            transform.Rotate(0, _spinSpeed * _spinDirection, 0);
        }

    }

}
