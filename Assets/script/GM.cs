using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{

    public int workmoney = 0;

    public GameObject worker;

    public GameObject outcomeUI;
    public GameObject menuUI;
    public GameObject gameEndUI;

    public GameObject outcomeanimation;
    public GameObject nextlevel;
    public TextMeshProUGUI workmoney_text;


    public List<GameObject> table;
    public List<GameObject> ITDog;

    public List<GameObject> totalITworker;
    public int workercount = 1;

    public List<GameObject> totalTable;
    public int tablecount = 1;

    public int workermoney = 1;
    public int outcome = 20;
    public float outcometimer = 0;
    public int maxoutcomtime = 5;

    bool gameEnd = false;

    
    public int maxgoal = 1000;
    public float goaltimer = 0;
    public int maxgoaltime = 15;

    public int highermoney = 0;

    public int[] ScoreBoard = new int[9];
    public int issave = 0;

    public TextMeshProUGUI Socrelist;

    public void Update()
    {
        outcometimer += Time.deltaTime;
        goaltimer += Time.deltaTime;
        if (outcometimer > maxoutcomtime)
        {
            outcometimer = 0;
            Removemoney(outcome);
            outcomeanimation.SetActive(true);
            outcomeanimation.GetComponent<TextMeshProUGUI>().text = "(-$" + outcome + ")";
            outcomeanimation.GetComponent<Animation>().Play();
        }

        if (goaltimer >= maxgoaltime)
        {
            goaltimer = 0;
            Nextstage();
        }
    }

    public void FristTimeSave()
    {
        if (PlayerPrefs.GetInt("Save ed", issave) == 0)
        {
            Debug.Log("Not Save Yet");
            for (int i = 0; i < ScoreBoard.Length; i++)
            {
                ScoreBoard[i] = 200;
                PlayerPrefs.SetInt("socore" + i, ScoreBoard[i]);
            }
            issave = 1;
            PlayerPrefs.SetInt("Save ed", issave);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("save ed");
            for (int i = 0; i < ScoreBoard.Length; i++)
            {
                ScoreBoard[i] = PlayerPrefs.GetInt("socore" + i, ScoreBoard[i]);
            }
        }
    }

    public void Start()
    {
        
        FristTimeSave();
        ITDog = new List<GameObject>(GameObject.FindGameObjectsWithTag("staff"));
        table = new List<GameObject>(GameObject.FindGameObjectsWithTag("table"));
        UpdateUI();
    }

    public void Updatemoney(int i)
    {
        workmoney += i;
        if (workmoney > highermoney)
        {
            highermoney = workmoney;
        }
        UpdateUI();

    }

    public void Removemoney(int i)
    {
        workmoney -= i;
        UpdateUI();
    }

    private void UpdateUI()
    {
        workmoney_text.text = "working = $" + workmoney;
        if (workmoney < -200)
        {//lose
            Endgame();
        }
    }

    public void Checktable(GameObject T, GameObject staff)
    {
        foreach (GameObject tableT in table)
        {
            if (tableT.GetComponent<tablefull>().isfull == false && tableT == T)
            {
                GameObject it = ITDog.Find(x => x == staff);
                it.gameObject.GetComponent<StaffAI>().Needtowork(tableT);
                it.gameObject.GetComponent<StaffAI>().table = tableT;
                it.gameObject.GetComponent<StaffAI>().working = true;
                tableT.GetComponent<tablefull>().isfull = true;
                tableT.GetComponent<tablefull>().staff = staff;
            }
        }
    }

    public void Leavetable(GameObject staff)
    {
        foreach (GameObject tableT in table)
        {
            if (tableT.GetComponent<tablefull>().staff == staff)
            {
                tableT.GetComponent<tablefull>().staff = null;
                tableT.GetComponent<tablefull>().isfull = false;
            }
        }
    }

    public GameObject Findfreetable()
    {
        foreach (GameObject tableT in table)
        {
            if (tableT.GetComponent<tablefull>().isfull == false)
            {
                return tableT;
            }
        }
        return null;
    }



    public void Endgame()
    {
        gameEnd = true;
        gameEndUI.SetActive(true);
        gameEndUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Your max Score:" + highermoney;
        SaveRecord(highermoney);

        for(int i = 0;i < ScoreBoard.Length; i++)
        {
            Socrelist.text += (i + 1) + ")  " + ScoreBoard[i] +"\n";
        }

        Time.timeScale = 0;
    }


    public void Nextstage()
    {
        maxgoal *= 2;
        outcome *= 2;
        nextlevel.SetActive(true);
        nextlevel.GetComponent<Animation>().Play();
        outcomeUI.GetComponent<TextMeshProUGUI>().text = "Outcome:" + outcome + "$(5sec)";
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        if (Time.timeScale != 0)
        {
            menuUI.SetActive(true);
            //menuUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "goal money:$" + maxgoal;
            menuUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "worker =" + workermoney + "($)";
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            menuUI.SetActive(false);
        }
    }

    public void SaveRecord(int higherscore)
    {
        for (int i = 0; i < ScoreBoard.Length; i++)
        {
            if (higherscore >= ScoreBoard[i])
            {
                Debug.Log("move down");
                MoveDown(i, higherscore);
                SaveData();
                return;
            }
        }
    }

    void SaveData()
    {
        for (int i = 0; i < ScoreBoard.Length; i++)
        {
            PlayerPrefs.SetInt("socore" + i, ScoreBoard[i]);
            PlayerPrefs.Save();
        }
    }

    void MoveDown(int numberofscore, int score)
    {
        for (int i = ScoreBoard.Length-1; i >= 1; i--)
        {
            ScoreBoard[i] = ScoreBoard[i-1];
        }
        ScoreBoard[numberofscore] = score;
    }

    public void Upgrade()
    {
        if (workmoney >= 500)
        {
            Removemoney(500);
            workermoney += 1;
            menuUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "worker =" + workermoney + "($)";
        }
    }

    public void buyworker()
    {
        if (workercount < 4)
        {
            if (workmoney >= 45)
            {
                Removemoney(45);
                foreach (GameObject worker in totalITworker)
                {
                    if (worker.activeSelf == false)
                    {
                        workercount += 1;
                        worker.SetActive(true);
                        ITDog.Add(worker);
                        return;
                    }
                }
            }
        }
    }

    public void buytable()
    {
        if (tablecount < 4)
        {
            if (workmoney >= 100)
            {
                Removemoney(100);
                foreach (GameObject Table in totalTable)
                {
                    if (Table.activeSelf == false)
                    {
                        tablecount += 1;
                        Table.SetActive(true);
                        table.Add(Table);
                        return;
                    }
                }
            }
        }
    }
}
