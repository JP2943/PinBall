using UnityEngine;
using UnityEngine.InputSystem;

public class FripperController : MonoBehaviour
{
    //HingeJointコンポーネントを入れる
    private HingeJoint myHingeJoint;

    //初期の傾き
    private float defaultAngle = 20;
    //弾いた時の傾き
    private float flickAngle = -20;

    // タッチの保持状態（左右同時タップ対応）
    // Began/Moved/Stationary を「押下中」、Ended/Canceled を「離した」とみなす
    private bool leftTouchHeld = false;
    private bool rightTouchHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        //HingeJointコンポーネント取得
        this.myHingeJoint = GetComponent<HingeJoint>();

        //フリッパーの傾きを設定
        SetAngle(this.defaultAngle);
    }

    // Update is called once per frame
    void Update()
    {

        //左矢印キー(もしくはAキー)を押した時左フリッパーを動かす
        if ((Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame) && this.gameObject.CompareTag("LeftFripperTag"))
        {
            SetAngle(this.flickAngle);
        }
        //右矢印キー(もしくはDキー)を押した時右フリッパーを動かす
        if ((Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame) && this.gameObject.CompareTag("RightFripperTag"))
        {
            SetAngle(this.flickAngle);
        }
        //下矢印キー(もしくはSキー)を押した時左右フリッパーを動かす
        if ((Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame) && (this.gameObject.CompareTag("RightFripperTag") || (this.gameObject.CompareTag("LeftFripperTag"))))
        {
            SetAngle(this.flickAngle);
        }

        //矢印キー離された時フリッパーを元に戻す
        if ((Keyboard.current.leftArrowKey.wasReleasedThisFrame || Keyboard.current.aKey.wasReleasedThisFrame) && this.gameObject.CompareTag("LeftFripperTag"))
        {
            SetAngle(this.defaultAngle);
        }
        if ((Keyboard.current.rightArrowKey.wasReleasedThisFrame || Keyboard.current.dKey.wasReleasedThisFrame) && this.gameObject.CompareTag("RightFripperTag"))
        {
            SetAngle(this.defaultAngle);
        }
        if ((Keyboard.current.downArrowKey.wasReleasedThisFrame || Keyboard.current.sKey.wasReleasedThisFrame) && (this.gameObject.CompareTag("RightFripperTag") || (this.gameObject.CompareTag("LeftFripperTag"))))
        {
            SetAngle(this.defaultAngle);
        }
        // マルチタッチ対応
        HandleTouches();
    }

    //フリッパーの傾きを設定
    public void SetAngle(float angle)
    {
        JointSpring jointSpr = this.myHingeJoint.spring;
        jointSpr.targetPosition = angle;
        this.myHingeJoint.spring = jointSpr;
    }
    
    // タッチ入力処理（左右同時タップOK / 指が動いている間は押下継続）
    private void HandleTouches()
    {
        int leftActiveCount = 0;
        int rightActiveCount = 0;

        // 画面中央X
        float halfW = Screen.width * 0.5f;

        // すべてのタッチを走査
        for (int i = 0; i < Input.touchCount; i++)
        {
            UnityEngine.Touch t = Input.touches[i];

            // 押下中かどうか（phaseで判定）※ TouchPhase も UnityEngine を明示
            bool isActive =
                t.phase == UnityEngine.TouchPhase.Began ||
                t.phase == UnityEngine.TouchPhase.Moved ||
                t.phase == UnityEngine.TouchPhase.Stationary;

            bool isLeftSide = t.position.x < halfW;

            if (isActive)
            {
                if (isLeftSide) leftActiveCount++;
                else rightActiveCount++;
            }
            // Ended/Canceled は押下として数えない
        }

        // このフレームの左右押下状態
        bool leftNowHeld = leftActiveCount > 0;
        bool rightNowHeld = rightActiveCount > 0;

        // 左フリッパー：押下開始/解除のエッジで動かす
        if (this.gameObject.CompareTag("LeftFripperTag"))
        {
            if (!leftTouchHeld && leftNowHeld)  SetAngle(this.flickAngle);   // 押下開始
            if (leftTouchHeld && !leftNowHeld)  SetAngle(this.defaultAngle); // 押下解除
        }

        // 右フリッパー：押下開始/解除のエッジで動かす
        if (this.gameObject.CompareTag("RightFripperTag"))
        {
            if (!rightTouchHeld && rightNowHeld)  SetAngle(this.flickAngle);
            if (rightTouchHeld && !rightNowHeld)  SetAngle(this.defaultAngle);
        }

        // 次フレーム比較用に状態を更新
        leftTouchHeld = leftNowHeld;
        rightTouchHeld = rightNowHeld;
    }
}