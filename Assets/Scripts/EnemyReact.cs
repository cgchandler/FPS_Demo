using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReact : MonoBehaviour
{
    public void ReactToHit()
    {
        EnemyAI behavior = GetComponent<EnemyAI>();
        if (behavior != null)
            behavior.SetAlive(false);

        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        Renderer targetRenderer = this.gameObject.GetComponent<Renderer>();
        targetRenderer.material.SetColor("_Color", Color.red);
        
        this.transform.Rotate(-75, 0, 0);

        yield return new WaitForSeconds(1.0f);

        Destroy(this.gameObject);
    }

}
