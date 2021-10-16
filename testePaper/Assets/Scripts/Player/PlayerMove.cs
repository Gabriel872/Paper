using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMove : MonoBehaviour
{
    //private CharacterController characterControl;
    public Transform cam;
    public Renderer rend;
    public Color defaultColor;
    public Color currentColor;
    public string currentColorName;
    public float speed = 5f;
    private Rigidbody rb;
    public CinemachineFreeLook CineMach;
    public bool coroutineON = false;

    [Range(0.1f, 8f)]
    public float zoomSpeed = 2f;
    public float turnSmooth = 0.1f;
    float smoothVelocity;
    float zoom = 40;

    public static PlayerMove intance;

    void Start()
    {
        intance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //characterControl = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        currentColor = defaultColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (zoom >= 16f && zoom <= 52)
        {
            zoom = CineMach.m_Lens.FieldOfView -= Input.mouseScrollDelta.y * 2f;
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //characterControl.Move(moveDir.normalized * speed * Time.deltaTime);
            Vector3 finaleMove = (moveDir.normalized * speed * Time.deltaTime);
            finaleMove.y = 0f;
            rb.velocity += finaleMove;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        if (go.tag.Equals("Color"))
        {
            if (coroutineON)
            {
                StopAllCoroutines();
                Debug.Log("Stop");
            }

            ChangeColor(go.GetComponent<Renderer>().material.color, go.GetComponent<PisoCor>().color);
        }

        if (go.tag.Equals("Ambiente") && (currentColor != defaultColor))
        {
            Debug.Log("Ativou");
            StartCoroutine(ColorTime());
        }
    }

    IEnumerator ColorTime()
    {
        coroutineON = true;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSecondsRealtime(.4f);
            rend.material.color = defaultColor;
            yield return new WaitForSecondsRealtime(.4f);
            rend.material.color = currentColor;
        }
        yield return new WaitForSecondsRealtime(.4f);
        coroutineON = false;
        ChangeColor(defaultColor, "roxo");
        Debug.Log("2s");
    }

    void ChangeColor(Color color, string name)
    {
        currentColorName = name;
        currentColor = color;
        rend.material.color = color;
    }
}