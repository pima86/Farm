using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class Farming_Player_Main : MonoBehaviour
{
    #region Static_Inst
    public static Farming_Player_Main Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }
    #endregion
    [Header("이동속도")]
    [SerializeField] float move_speed = 0.01f;

    [HideInInspector] public Transform[] transforms;
    private TransformAccessArray _transformAccessArray;
    private void Start()
    {
        _transformAccessArray = new TransformAccessArray(transforms);
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            GetKey_Move(-1, 0);
        if (Input.GetKey(KeyCode.UpArrow))
            GetKey_Move(0, 1);
        if (Input.GetKey(KeyCode.RightArrow))
            GetKey_Move(1, 0);
        if (Input.GetKey(KeyCode.DownArrow))
            GetKey_Move(0, -1);
    }

    /*
     * Update 또는 FixedUpdate에서 키보드 값을 입력받아 호출하는 함수
     */
    #region GetKey
    void GetKey_Move(int x = 0, int y = 0)
    {
        var job = new MoveJob();
        job.x = x;
        job.y = y;
        job.speed = move_speed;
        job.Schedule(_transformAccessArray);
    }
    #endregion

    /*
     * 플레이어 캐릭터의 이동을 멀티 스레드로 수행
     * x, y => 방향
     * speed => 속도
     */
    #region Move Job
    public struct MoveJob : IJobParallelForTransform
    {
        public float x;
        public float y;
        public float speed;
        public void Execute(int index, TransformAccess transform)
        {
            Vector3 position = transform.position;
            position.x += x * speed;
            position.y += y * speed;

            transform.position = position;
        }
    }
    #endregion
}
