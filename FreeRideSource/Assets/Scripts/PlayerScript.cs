using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject board;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float xSpeed = 1f;

    float currentSpeed = 0f;

    // [SerializeField] 
    float angleRotationSpeed = 100f;

    public float angle = 0f;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAngle();
        RotateBoard();
        Move();
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
        else if (Mathf.Abs(angle) > 45)
        {
            // do not change speed or increase slightly after stop
            if (currentSpeed == 0f)
                currentSpeed = 1f;
        }
        else
        {
            currentSpeed += Time.deltaTime * acceleration;
        }

        Vector3 transaltion = new Vector3(Sign(angle) * xSpeed, -currentSpeed, 0) * Time.deltaTime;
        transform.Translate(transaltion, Space.World);
    }
}
