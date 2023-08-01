using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    private float startYScale;  // 初期の高さ
    private float slideTimer;   // スライディング時間
    private Rigidbody rb;       // 物理
    private MovementManager pm; // MovementManagerを取得(元がPlayerMovveだったので変数はpm)

    [Header("Slideing")]
    [SerializeField] private float maxSlideTime;                // スライディング最長時間
    [SerializeField] private float slideForce;                  // スライディングの押し出す力
    [SerializeField] private float slideYScale;                 // スライディング時のObj高さ
    [SerializeField] private Transform playerObj;               // Obj取得

    [Header("Keybinding")]
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<MovementManager>();   // MovementManagerを取得

        startYScale = playerObj.localScale.y;   // Objの初期高さを取得
    }

    void Update()
    {
        //  キー入力した
        if (Input.GetKeyDown(slideKey) && (pm.x != 0 || pm.z != 0) && pm.isGround)
            StartSlide();

        // キー入力やめた
        if (Input.GetKeyUp(slideKey) && pm.sliding && pm.isGround)
            StopSlide();
    }

    private void FixedUpdate()
    {
        // ステータスと一致したらスライディング処理
        if (pm.sliding)
            SlidingMovement();
    }

    // スライディングを始める処理
    private void StartSlide()
    {
        pm.sliding = true;  // ステータスをスライディングにする

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);    // 高さをスライディングに変える
        rb.AddForce(Vector3.down * 5.0f, ForceMode.Impulse);                                                // スライディング : 押し出す

        slideTimer = maxSlideTime;  // 最長時間までスライディング
    }

    // スライディングを止める処理
    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);    // 初期の高さに戻す
    }

    // スライディング処理
    private void SlidingMovement()
    {
        Vector3 inputDirection = playerObj.forward; // Objの前方向

        // スライディング条件
        if(rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Impulse);

            slideTimer -= Time.deltaTime;
        }

        // 時間が来たら止める
        if (slideTimer <= 0)
            StopSlide();
    }
}
