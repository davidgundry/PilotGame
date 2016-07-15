using UnityEngine;
using System.Collections;

public class AngleControlledPlanePhysics : MonoBehaviour
{

    Rigidbody2D body;
    Transform trans;
    public float flyPower;
    private float flyPower_P;
    public float rotatePowerUp, rotatePowerDown;
    private float rotatePowerUp_P, rotatePowerDown_P;
    public float rotateUpLimit, rotateDownLimit;
    public float speed;
    private float speed_P;
    public float maxSpeed;
    Vector3 euler;
    public float angular;
    bool pressed;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        trans = transform;
        euler = trans.eulerAngles;
        //
        resetVariables();
    }

    void resetVariables()
    {
        rotatePowerUp_P = rotatePowerUp;
        rotatePowerDown_P = rotatePowerDown;
        flyPower_P = flyPower;
        speed_P = speed;
    }

    void rotateDown()
    {

        euler = trans.eulerAngles;
        if (euler.z > 120 && euler.z < 280)
        {
            if (angular > -rotateDownLimit)
            {
                rotatePowerDown_P += 200 * Time.deltaTime;
                angular += rotatePowerDown_P * Time.deltaTime;
            }
            body.angularVelocity = angular;
        }
        else if (euler.z > 280 && angular > 20)
        {
            angular = 10;

        }
        else if (euler.z < 120)
        {
            if (angular > -rotateDownLimit)
            {

                rotatePowerDown_P += 200 * Time.deltaTime;
                angular -= rotatePowerDown_P * Time.deltaTime;
            }
            body.angularVelocity = angular;
        }

    }

    void rotateUp()
    {
        euler = trans.eulerAngles;

        if (angular < rotateUpLimit)
        {
            if (angular < 0)
            {
                rotatePowerUp_P += 300 * Time.deltaTime;
            }
            else
            {
                rotatePowerUp_P += 100 * Time.deltaTime;
            }
            angular += rotatePowerUp_P * Time.deltaTime;
        }
        body.angularVelocity = angular;
    }

    void flyUp()
    {
        body.AddForce(new Vector2(0, flyPower_P));
        if (flyPower_P > 0)
        {
            flyPower_P -= 10 * Time.deltaTime;
        }
        else
        {
            flyPower_P = 0;
        }
    }

    void moveForward()
    {
        speed_P += 100 * Time.deltaTime;
        body.AddRelativeForce(Vector2.right * speed_P);
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //    
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            rotateUp();
            flyUp();
            moveForward();
        }
        else
        {
            if (pressed)
            {
                rotateDown();
            }
        }


        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            pressed = true;
            resetVariables();
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        angular = 0;
        body.angularVelocity = angular;
        pressed = false;
    }
}