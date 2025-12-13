using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXandY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXandY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;
    public float minimumVertical = -45.0f;
    public float maximumVertical = 45.0f;
    float _rotationX = 0;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
            body.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVertical, maximumVertical);

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
        else
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVertical, maximumVertical);

            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float rotationY = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }

    }

    private void OnGUI()
    {
        GameObject controllerObject = GameObject.Find("Controller");
        SceneController sc = controllerObject.GetComponent<SceneController>();
        GameObject playerObject = GameObject.Find("MyPlayer");
        PlayerCharacter player = playerObject.GetComponent<PlayerCharacter>();
        string currentScore = ""; 
        GameObject turretObject = GameObject.Find("Turret");
        if (turretObject == null)
        {
            // Turret is gone - only disaply player health
            currentScore = string.Format("Level: {0}\r\nPlayer Health: {1}\r\nEnemy Count {2}\r\nTurret Health 0", 
                sc.Level, player.Health, sc.LiveEnemyCount);
        }
        else
        {
            // Turret still alive - display player and turret health
            TurretReact turret = turretObject.GetComponent<TurretReact>();
            currentScore = string.Format("Level: {0}\r\nPlayer Health: {1}\r\nEnemy Count {2}\r\nTurret Health {3}", 
                sc.Level, player.Health, sc.LiveEnemyCount, turret.Health);
        }
        //Debug.Log(currentScore);
        GUI.Label(new Rect(75, 30, 250, 80), currentScore);
    }

}
