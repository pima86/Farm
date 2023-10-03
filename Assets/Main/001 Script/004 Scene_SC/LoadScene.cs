using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSecene : MonoBehaviour
{
    [SerializeField] Image load_img;
    [SerializeField] TMP_Text load_txt;
    [SerializeField] GameObject press_to_start;
    float load_tm = 0;

    public void Load(string str)
    {
        press_to_start.SetActive(false);

        load_img.enabled = load_txt.enabled = true;
        StartCoroutine(Load_Co(str));
    }

    IEnumerator Load_Co(string str)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(str);

        while(!asyncOperation.isDone)
        {
            load_img.fillAmount = asyncOperation.progress;

            yield return null;
        }

        var a_NA = new NativeArray<float>(1, Allocator.TempJob);
        var Result = new NativeArray<bool>(2, Allocator.TempJob);

        Job job;
        job.a = load_tm;
        job.a_na = a_NA;
        job.result = Result;
        JobHandle Handle = job.Schedule();

        while (!Handle.IsCompleted)
        {
            yield return null;
        }
        Handle.Complete();

        load_tm = a_NA[0];
        if (Result[0])
            load_txt.text = load_Radnome_Txt();

        Result.Dispose();
    }

    string load_Radnome_Txt()
    {
        string[] ranmam_str = new string[5] {
            "작물이 움직이는 버그 수정 중..",
            "가출한 캐릭터 잡아오는 중..",
            "강아지랑 산책 중..",
            "고양이랑 눈싸움하는 중..",
            "수상한 유물 캐는 중.."};

        string str = "";
        do
            str = ranmam_str[Random.Range(0, ranmam_str.Length)];
        while (str == load_txt.text);

        return str;
    }

    struct Job : IJob
    {
        public float a;
        public NativeArray<float> a_na;
        public NativeArray<bool> result;

        public void Execute()
        {
            a_na[0] = a + Time.deltaTime;
            if (a_na[0] > 1f)
                result[0] = true;
            else
                result[0] = false;
        }
    }
}
