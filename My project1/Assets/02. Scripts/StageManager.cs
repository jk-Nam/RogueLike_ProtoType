using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public List<string> stage1List;
    public List<string> stage2List;
    public List<string> stage3List;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);        
    }

    private void Start()
    {
        GameManager.Instance.isPlay = true;
        InitializeStageList();
        SelectRandomStage();
    }

    

    public void InitializeStageList()
    {
        stage1List = new List<string> { "02. Stage1-1", "02. Stage1-2", "02. Stage1-3" };
        stage2List = new List<string> { "03. Stage2-1", "03. Stage2-2", "03. Stage2-3" };
        stage3List = new List<string> { "04. Stage3-1", "04. Stage3-2", "04. Stage3-3" };
    }

    public void SelectRandomStage()
    {
        Shuffle(stage1List);
        Shuffle(stage2List);
        Shuffle(stage3List);

        foreach (string stage in stage1List)
        {
            Debug.Log(stage);
        }
    }

    void Shuffle(List<string> stageList)
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            string temp = stageList[i];
            int rNum = Random.Range(i, stageList.Count);
            stageList[i] = stageList[rNum];
            stageList[rNum] = temp;
        }
    }

    


}
