using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class Player_Move : MonoBehaviour
{
    [Header("카메라")]
    [SerializeField] Camera cam;

    [Header("애니메이션")]
    [SerializeField] Animator[] animators;

    [Header("이동속도")]
    [SerializeField] float move_speed = 0.01f;
    [SerializeField] float move_speed_load = 0.01f;
    public Transform[] transforms;

    private TransformAccessArray _transformAccessArray;
    private void Start()
    {
        _transformAccessArray = new TransformAccessArray(transforms);
    }

    void Update()
    {
        GetKeyDown_UD();
        GetKey_UD();
        GetKeyUp_UD();
    }

    void GetKeyDown_UD()
    {
        if (animators[0].GetBool("isWalking") || animators[0].GetBool("isRunning"))
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Anim_Parameters_Int("Looking at time", 9);
            Anim_Parameters_Bool("isWalking", true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Anim_Parameters_Int("Looking at time", 12);
            Anim_Parameters_Bool("isWalking", true);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Anim_Parameters_Int("Looking at time", 3);
            Anim_Parameters_Bool("isWalking", true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Anim_Parameters_Int("Looking at time", 6);
            Anim_Parameters_Bool("isWalking", true);
        }
    }
    void GetKey_UD()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Knowing_way(Vector3.left * 0.1f))
                GetKey_Move(-1, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Knowing_way(Vector3.up * 0.1f))
                GetKey_Move(0, 1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Knowing_way(Vector3.right * 0.1f))
                GetKey_Move(1, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Knowing_way(Vector3.down * 0.1f))
                GetKey_Move(0, -1);
        }
    }
    void GetKeyUp_UD()
    {
        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.DownArrow))
            return;

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Anim_Parameters_Bool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Anim_Parameters_Bool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Anim_Parameters_Bool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Anim_Parameters_Bool("isWalking", false);
        }
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
        job.speed = top_what();

        if(job.speed != 0)
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

    void Anim_Parameters_Int(string str, int tm)
    {
        for(int i = 0; i < animators.Length; i++)
            animators[i].SetInteger(str, tm);
    }

    void Anim_Parameters_Bool(string str, bool bo)
    {
        for (int i = 0; i < animators.Length; i++)
            animators[i].SetBool(str, bo);
    }

    bool Knowing_way(Vector3 pos)
    {
        LayerMask layer = LayerMask.GetMask("Tileset");
        RaycastHit2D hit = Physics2D.Raycast(transform.position + pos, transform.forward, 1f, layer);
        if (hit)
        {
            if (hit.collider.name == "Layer Water")
                return false;
            else
                return true;
        }
        return false;
    }

    float top_what()
    {
        LayerMask layer = LayerMask.GetMask("Tileset");
        RaycastHit2D hit= Physics2D.Raycast(transform.position + new Vector3(0, -0.05f, 0), transform.forward, 10f, layer);
        if(hit)
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.name == "Layer Load")
                return move_speed_load;
            else
                return move_speed;
        }
        return 0;
    }
}
