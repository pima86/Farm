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
        //New_GetKeyDown_UD();
        //New_GetKey_UD();
        //New_GetKeyUp_UD();
        /*
        GetKeyDown_UD();
        GetKey_UD();
        GetKeyUp_UD();
        */
    }

    /*
     * Update에서 호출하는 함수
     */

    #region New_GetKey_Script
    int move_x = 0;
    int move_y = 0;
    void New_GetKeyDown_UD()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            move_x -= 1;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            move_y += 1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            move_x += 1;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            move_y -= 1;
    }

    public void New_GetKey_UD(Vector2 pos)
    {
        if (pos.x == 0 && pos.y == 0)
        {
            Anim_Parameters_Bool("isWalking", false);
            Anim_Parameters_Bool("isRunning", false);
        }
        else
        {
            Anim_Parameters_Bool("isWalking", true);

            if (pos.x == 1)
                Anim_Parameters_Int("Looking at time", 3);
            else if (pos.x == -1)
                Anim_Parameters_Int("Looking at time", 9);
            else if (pos.y == 1)
                Anim_Parameters_Int("Looking at time", 12);
            else if (pos.y == -1)
                Anim_Parameters_Int("Looking at time", 6);

            if (Knowing_way(new Vector3(pos.x * 4f, pos.y, 0) * 0.01f))
                GetKey_Move((int)pos.x, (int)pos.y);
        }
    }

    void New_GetKeyUp_UD()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            move_x += 1;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            move_y -= 1;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            move_x -= 1;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            move_y += 1;

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.DownArrow))
            return;
        Anim_Parameters_Bool("isRunning", false);
        Anim_Parameters_Bool("isWalking", false);
    }

    #endregion
    #region GetKey_Script
    void GetKeyDown_UD()
    {
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
        if (animators[0].GetBool("isWalking") || animators[0].GetBool("isRunning"))
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
    }
    void GetKeyUp_UD()
    {
        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.DownArrow))
            return;

        Anim_Parameters_Bool("isRunning", false);
        Anim_Parameters_Bool("isWalking", false);
        /*
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Anim_Parameters_Bool("isRunning", false);
            Anim_Parameters_Bool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Anim_Parameters_Bool("isRunning", false);
            Anim_Parameters_Bool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Anim_Parameters_Bool("isRunning", false);
            Anim_Parameters_Bool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Anim_Parameters_Bool("isRunning", false);
            Anim_Parameters_Bool("isWalking", false);
        }
        */
    }
    #endregion

    /*
     * Update에서 키보드 값을 입력받아 호출하는 함수
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

    //애니메이션 변수 조정
    #region Anim_Parameters 
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
    #endregion

    //갈 수 있는 길인지
    #region Knowing_way
    bool Knowing_way(Vector3 pos)
    {
        LayerMask layer = LayerMask.GetMask("Tileset");
        RaycastHit2D hit = Physics2D.Raycast(transform.position + pos, transform.forward, 1f, layer);
        Debug.DrawRay(transform.position + pos, transform.forward, Color.green, 1f);
        if (hit)
        {
            if (hit.collider.name == "Layer Grass" || hit.collider.name == "Layer Load")
                return true;
            else
                return false;
        }
        return false;
    }
    #endregion

    //내가 어디 위를 걷고 있는지
    #region top_what
    float top_what()
    {
        LayerMask layer = LayerMask.GetMask("Tileset");
        RaycastHit2D hit= Physics2D.Raycast(transform.position, transform.forward, 10f, layer);
        if (hit)
        {
            if (hit.collider.name == "Layer Load")
            {
                Anim_Parameters_Bool("isRunning", true);
                return move_speed_load;
            }
            else
            {
                Anim_Parameters_Bool("isRunning", false);
                return move_speed;
            }
        }
        //Anim_Parameters_Bool("isRunning", false);
        return 0;
    }
    #endregion
}
