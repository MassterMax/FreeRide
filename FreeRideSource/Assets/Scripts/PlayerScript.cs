using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject board;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float stopAcceleration = 10f;
    [SerializeField] float xSpeed = 1f;

    //todo remove it!
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public float currentSpeed = 0f;
    float prevSpeed = 0f;
    float minSpeed = 0f;

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
        float abs_angle = Mathf.Abs(angle);
        prevSpeed = currentSpeed;

        // if (abs_angle == 90)
        // {
        //     // currentSpeed = 0f;
        // }
        if (abs_angle > 75)
        {
            // slow down
            // todo change to slow down depend on angle
            // float slowAcceleration = acceleration * 
            currentSpeed -= Time.deltaTime * Mathf.Sin(Mathf.Deg2Rad * abs_angle) * stopAcceleration;
            currentSpeed = Mathf.Max(currentSpeed, minSpeed);
        }
        else if (abs_angle > 30)
        {
            // do not change speed
        }
        else
        {
            currentSpeed += Time.deltaTime * acceleration;
        }

        // todo на самом деле направление движения по оси х не определятся углом (так как в реальности ты вряд ли поедешь под 90 на склоне)
        Vector3 transaltion = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle * 2) * currentSpeed, -currentSpeed, 0) * Time.deltaTime;
        transform.Translate(transaltion, Space.World);
    }

    void UpdateCamera()
    {
        //todo move to separate script
        var targetSize = minOrthographicSize + Mathf.Max(currentSpeed - minOrthographicSize, 0f);
        float sizeChangeSpeed;
        if (currentSpeed == prevSpeed)
            sizeChangeSpeed = 0f;
        else
            sizeChangeSpeed = Mathf.Abs(currentSpeed - prevSpeed) / (currentSpeed + prevSpeed);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetSize, 0.2f);
    }
}
