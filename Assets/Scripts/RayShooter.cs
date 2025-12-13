using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{

    private Camera _camera;

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip hitWallSound;
    [SerializeField] private AudioClip hitEnemySound;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                TurretReact turretTarget = hitObject.GetComponent<TurretReact>();
                EnemyReact enemyTarget = hitObject.GetComponent<EnemyReact>();
                if (turretTarget != null)
                {
                    turretTarget.ReactToHit();
                    //soundSource.PlayOneShot(hitEnemySound);
                }
                else if (enemyTarget != null)
                {
                    enemyTarget.ReactToHit();
                    //soundSource.PlayOneShot(hitEnemySound);
                }
                else
                {
                    StartCoroutine(SphereIndicator(hit.point));
                    //soundSource.PlayOneShot(hitWallSound);
                }
            }
        }
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        // set the color
        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.SetColor("_Color", Color.yellow);

        yield return new WaitForSeconds(0.25f);

        Destroy(sphere);
    }

    private void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }
}
