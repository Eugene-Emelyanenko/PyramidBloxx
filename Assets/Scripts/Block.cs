using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Sprite blockPreviewSprite;

    [SerializeField] private GameObject shadow_0;
    [SerializeField] private GameObject shadow_180;
    [SerializeField] private GameObject shadow_90;
    [SerializeField] private GameObject shadow_270;

    private bool isLanded = false;
    private bool canLand = true;

    private Rigidbody2D rb = default;
    private GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        rb.gravityScale = 0f;
        shadow_0.SetActive(true);
        shadow_90.SetActive(false);
        shadow_180.SetActive(false);
        shadow_270.SetActive(false);
    }

    private void Update()
    {
        if (!isLanded)
        {
            transform.Translate(Vector3.down * gameManager.currentSpeed * Time.deltaTime, Space.World);
            UpdateShadows();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canLand)
        {
            isLanded = true;
            shadow_0.SetActive(false);
            shadow_90.SetActive(false);
            shadow_180.SetActive(false);
            shadow_270.SetActive(false);
            canLand = false;
            rb.gravityScale = 1f;
            StartCoroutine(Wait());
            gameManager.AfterLanding();
            gameManager.SpawnNextBlock();
        }
    }

    public void Move(Vector2 direction)
    {
        transform.Translate(direction, Space.World);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);

        while (!IsBlockStopped())
        {
            yield return new WaitForSeconds(0.1f);
        }

        rb.Sleep();
    }

    public Vector3 GetHighestPos()
    {
        Vector3 highestPos = Vector3.negativeInfinity;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Shadows"))
                continue;
            if (child.position.y > highestPos.y)
            {
                highestPos = child.position;
            }
        }
        return highestPos;
    }

    public bool IsBlockStopped()
    {
        return rb.velocity.sqrMagnitude < 0.1f && Mathf.Abs(rb.angularVelocity) < 0.1f;
    }

    private void UpdateShadows()
    {
        float zRotation = transform.rotation.eulerAngles.z;

        zRotation = Mathf.Round(zRotation);

        if (zRotation == 90f)
        {
            shadow_0.SetActive(false);
            shadow_90.SetActive(true);
            shadow_180.SetActive(false);
            shadow_270.SetActive(false);
        }
        else if (zRotation == 180f)
        {
            shadow_0.SetActive(false);
            shadow_90.SetActive(false);
            shadow_180.SetActive(true);
            shadow_270.SetActive(false);
        }
        else if (zRotation == 270f)
        {
            shadow_0.SetActive(false);
            shadow_90.SetActive(false);
            shadow_180.SetActive(false);
            shadow_270.SetActive(true);
        }
        else
        {
            shadow_0.SetActive(true);
            shadow_90.SetActive(false);
            shadow_180.SetActive(false);
            shadow_270.SetActive(false);
        }
    }
}
