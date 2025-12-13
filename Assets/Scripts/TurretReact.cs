using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretReact : MonoBehaviour
{

    private int _health = 10;

    public int Health
    {
        get { return _health; }
    }

    public void ReactToHit()
    {
        StartCoroutine(TargetHit());
    }
    
    private IEnumerator TargetHit()
    {

        Renderer targetRenderer = this.gameObject.GetComponent<Renderer>();
        targetRenderer.material.SetColor("_Color", Color.red);

        yield return new WaitForSeconds(0.25f);
        _health -= 1;
        //Debug.Log(string.Format("Target Hit: Health = {0}", _health));
        if (_health == 0)
        {
            GameObject gunTower = GameObject.Find("GunTower");
            Destroy(gunTower);
        }
        else
        {
            targetRenderer.material.SetColor("_Color", Color.white);
        }

    }
}
