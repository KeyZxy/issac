using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    [Header("Animation")]
    public Animator playerMoveAnim;
    public Animator playerShotAnim;
    [Header("Sprite")]
    SpriteRenderer bodyRenderer;
    SpriteRenderer headRenderer;
    [Header("Transform")]
    public Transform head;
    public Transform body;
    [Header("Function")]
    private float lastshot;
    Vector2 moveInput;
    Vector2 shotInput;
    public float tearY;
    // Start is called before the first frame update
    void Start()
    {
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        headRenderer = head.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveAnim();
        ShotAnim();
    }
    void MoveAnim()
    {
        if (moveInput.x < 0) { bodyRenderer.flipX = true; headRenderer.flipX = true; }
        if (moveInput.x > 0) { bodyRenderer.flipX = false; headRenderer.flipX = false; }
        playerMoveAnim.SetFloat("Up&Down", Mathf.Abs(moveInput.y));
        playerMoveAnim.SetFloat("Left&Right", Mathf.Abs(moveInput.x));
        playerShotAnim.SetFloat("Left&RightLook", Mathf.Abs(moveInput.x));

        if (moveInput.y > 0)
        {
            playerShotAnim.SetBool("UpLook", true); // 위쪽 방향
        }
        else
        {
            playerShotAnim.SetBool("UpLook", false);
        }
        if (moveInput.y < 0)
        {
            playerShotAnim.SetBool("DownLook", true); 
        }
        else
        {
            playerShotAnim.SetBool("DownLook", false);
        }
    }
    void ShotAnim()
    {
        if (shotInput.x < 0) { headRenderer.flipX = true; }
        if (shotInput.x > 0) { headRenderer.flipX = false; }
        playerShotAnim.SetFloat("Left&RightShot", Mathf.Abs(shotInput.x));

        if (shotInput.y > 0)
        {
            playerShotAnim.SetBool("UpShot", true); 
        }
        else
        {
            playerShotAnim.SetBool("UpShot", false);
        }
        if (shotInput.y < 0)
        {
            playerShotAnim.SetBool("DownShot", true); 
        }
        else
        {
            playerShotAnim.SetBool("DownShot", false);
        }
    }
}
