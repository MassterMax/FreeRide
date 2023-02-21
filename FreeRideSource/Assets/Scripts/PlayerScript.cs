using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject board;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float xSpeed = 1f;

    //todo remove it!
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public float currentSpeed = 0f;
    float minSpeed = 0.1f;

    // [SerializeField] 
    float angleRotationSpeed = 100f;

    public float angle = 0f;

    float minOrthographicSize = 5f;
    float maxOrthographicSize = 25f;


    void Start()
    {
        virtualCamera.m_Lens.OrthographicSize = minOrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAngle();
        RotateBoard();
        Move();
        UpdateCamera();
    }

    void UpdateAngle()
    {
        float deltaAngle = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            deltaAngle -= Time.deltaTime * angleRotationSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            deltaAngle += Time.deltaTime * angleRotationSpeed;
        }
        angle += deltaAngle;
        if (angle < -90) angle = -90;
        if (angle > 90) angle = 90;
    }

    void RotateBoard()
    {
        board.transform.eulerAngles = new Vector3(0, 0, angle);
    }


    float Sign(float x)
    {
        if (Mathf.Abs(x) == 90 || x == 0) return 0;
        return Mathf.Sign(x);
    }

    void Move()
    {
        if (Mathf.Abs(angle) == 90)
        {
            currentSpeed = 0f;
        }
        else if (Mathf.Abs(angle) > 60)
        {
            // slow down
            // todo change to slow down depend on angle
            // float slowAcceleration = acceleration * 
            currentSpeed = Mathf.Max(currentSpeed - Time.deltaTime * acceleration, minSpeed);
        }
        else if (Mathf.Abs(angle) > 30)
        {
            // do not change speed
        }
        else
        {
            currentSpeed += Time.deltaTime * acceleration;
        }

        Vector3 transaltion = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle * 2) * currentSpeed, -currentSpeed, 0) * Time.deltaTime;
        transform.Translate(transaltion, Space.World);
    }

    void UpdateCamera()
    {
        //todo move to separate script
        virtualCamera.m_Lens.OrthographicSize = minOrthographicSize + currentSpeed / 4;
    }
}
