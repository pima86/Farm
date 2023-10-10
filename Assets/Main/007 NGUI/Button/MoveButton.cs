using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour
{

    public enum DragEffect
    {
        None,
        Momentum,
        MomentumAndSpring,
    }

    public float joyStickPosX;
    public float joyStickPosY;
    private float posDivision;

    public Transform CenterObject;
    public float ClampRadius = 100.0f;


    private Vector3 scale = Vector3.one;
    public float scrollWheelFactor = 0f;
    public bool restrictWithinPanel = false;
    Plane mPlane;
    Vector3 mLastPos;
    UIPanel mPanel;
    bool mPressed = false;
    Vector3 mMomentum = Vector3.zero;
    float mScroll = 0f;
    Bounds mBounds;
    Coroutine routine;

    public UnityEvent<Vector2> onDragEvent = new UnityEvent<Vector2>();


    void Awake()
    {
        posDivision = 1 / ClampRadius;
    }

    void Update()
    {
        this.onDragEvent.Invoke(new Vector2(joyStickPosX, joyStickPosY));
    }

    void FindPanel()
    {
        mPanel = (CenterObject != null) ? UIPanel.Find(CenterObject.transform, false) : null;
        if (mPanel == null) restrictWithinPanel = false;
    }
    void OnPress(bool pressed)
    {
        if (enabled && NGUITools.GetActive(gameObject) && CenterObject != null)
        {
            mPressed = pressed;

            if (this.routine != null)
                this.StopCoroutine(this.routine);

            if (pressed)
            {
                if (restrictWithinPanel && mPanel == null) FindPanel();


                if (restrictWithinPanel) mBounds = NGUIMath.CalculateRelativeWidgetBounds(mPanel.cachedTransform, CenterObject);


                mMomentum = Vector3.zero;
                mScroll = 0f;


                SpringPosition sp = CenterObject.GetComponent<SpringPosition>();
                if (sp != null) sp.enabled = false;


                mLastPos = UICamera.lastHit.point;


                Transform trans = UICamera.currentCamera.transform;
                mPlane = new Plane((mPanel != null ? mPanel.cachedTransform.rotation : trans.rotation) * Vector3.back, mLastPos);
            }
            else if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None)
            {
                mPanel.ConstrainTargetToBounds(CenterObject, ref mBounds, false);
            }
            if (!pressed)
            {
                joyStickPosX = 0;
                joyStickPosY = 0;

                if (this.routine != null) StopCoroutine(this.routine);
                this.routine = StartCoroutine("SpringPositionUpdate");

            }
        }
    }


    void OnDrag(Vector2 delta)
    {
        if (enabled && NGUITools.GetActive(gameObject) && CenterObject != null)
        {
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

            Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
            float dist = 0f;

            if (mPlane.Raycast(ray, out dist))
            {
                Vector3 currentPos = ray.GetPoint(dist);
                mLastPos = currentPos;


                CenterObject.position = currentPos;

                CenterObject.transform.localPosition = new Vector3(Vector3.ClampMagnitude(CenterObject.transform.localPosition, ClampRadius).x, Vector3.ClampMagnitude(CenterObject.transform.localPosition, ClampRadius).y, 0);

                //joyStickPosX = CenterObject.transform.localPosition.x * posDivision;
                //joyStickPosY = CenterObject.transform.localPosition.y * posDivision;

                if (CenterObject.transform.localPosition.x > 10)
                    joyStickPosX = 1;
                else if (CenterObject.transform.localPosition.x < -10)
                    joyStickPosX = -1;
                else
                    joyStickPosX = 0;

                if (CenterObject.transform.localPosition.y > 10)
                    joyStickPosY = 1;
                else if (CenterObject.transform.localPosition.y < -10)
                    joyStickPosY = -1;
                else
                    joyStickPosY = 0;
            }
        }
    }

    void LateUpdate()
    {
        float delta = Time.deltaTime;
        if (CenterObject == null) return;

        if (mPressed)
        {

            SpringPosition sp = CenterObject.GetComponent<SpringPosition>();
            if (sp != null) sp.enabled = false;
            mScroll = 0f;
        }
        else
        {
            mMomentum += scale * (-mScroll * 0.05f);
            mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, delta);

            if (mMomentum.magnitude > 0.0001f)
            {

                if (mPanel == null) FindPanel();

                if (mPanel != null)
                {
                    CenterObject.position += NGUIMath.SpringDampen(ref mMomentum, 9f, delta);

                    if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None)
                    {
                        mBounds = NGUIMath.CalculateRelativeWidgetBounds(mPanel.cachedTransform, CenterObject);


                    }
                    return;
                }
            }
            else mScroll = 0f;
        }


        NGUIMath.SpringDampen(ref mMomentum, 9f, delta);
    }



    void OnScroll(float delta)
    {
        if (enabled && NGUITools.GetActive(gameObject))
        {
            if (Mathf.Sign(mScroll) != Mathf.Sign(delta)) mScroll = 0f;
            mScroll += delta * scrollWheelFactor;
        }
    }

    private Vector3 targetPosition = Vector3.zero;
    public float strength = 10f;
    float mThreshold = 0f;
    IEnumerator SpringPositionUpdate()
    {
        while (true)
        {
            float delta = Time.deltaTime;
            if (mThreshold == 0f) mThreshold = (targetPosition - CenterObject.localPosition).magnitude * 0.001f;
            CenterObject.localPosition = NGUIMath.SpringLerp(CenterObject.localPosition, targetPosition, strength, delta);

            if (mThreshold >= (targetPosition - CenterObject.localPosition).magnitude)
            {
                CenterObject.localPosition = targetPosition;
                Debug.Log("원점으로 돌아옴");
                break;
            }
            yield return 0;
        }
    }

}
