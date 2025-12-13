using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public float speed = -50.0f;
    public int damage = 1;
    //private float _maxDistance = 100.0f;

    // Update is called once per frame
    void Update()
    {
        // Move the fireball
        transform.Translate(0, 0, speed * Time.deltaTime);

        // This was orginally necessary because I had issue with Turret firing above walls, 
        // then the fireball would travel to infinity
        //GameObject turretEye = GameObject.Find("TurretEye");
        //// destroy fireball if the turret is destroyed
        //if (turretEye == null)
        //    Destroy(this.gameObject);
        //else
        //{ 
        //    // destroy fireball if it travels further than max distance
        //    float sqrMaxDistance = _maxDistance * _maxDistance;
        //    float sqrDistanceTraveled = (this.gameObject.transform.position - turretEye.transform.position).sqrMagnitude;
        //    if (sqrDistanceTraveled > sqrMaxDistance)
        //        Destroy(this.gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collide");
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
            player.Hurt(damage);

        Destroy(this.gameObject);
    }
}