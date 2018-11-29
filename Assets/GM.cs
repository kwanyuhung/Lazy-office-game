using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{

    public int workmoney = 0;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject menuUI;
    public TextMeshProUGUI workmoney_text;

    public List<GameObject> table;
    public List<GameObject> ITDog;


    public int outcome = 20;
    public float outcometimer = 0;
    public int maxoutcomtime = 5;

    bool gameEnd = false;

    public void Update()
    {
        outcometimer += Time.deltaTime;
        if(outcometimer > maxoutcomtime)
        {
            outcometimer =0;
            Removemoney(outcome);
        }
    }

    public void Start()
    {
        ITDog = new List<GameObject>(GameObject.FindGameObjectsWithTag("staff"));
        table = new List<GameObject>(GameObject.FindGameObjectsWithTag("table"));
        UpdateUI();
    }

    public void Updatemoney(int i)
    {
        workmoney += i;
        UpdateUI();

        if(workmoney >= 1000){
            workmoney_text.gameObject.SetActive(false);
            winUI.SetActive(true);
        }
    }

    public void Removemoney(int i){
        workmoney -= i;
        UpdateUI();
    }

    private void UpdateUI()
    {
        workmoney_text.text = "working  =   $" + workmoney;
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

    public GameObject Findfreetable(){
        foreach (GameObject tableT in table)
        {
            if(tableT.GetComponent<tablefull>().isfull == false){
                return tableT;
            }
        }
        return null;
    }



    public void YouWin()
    {
        gameEnd = true;
        winUI.SetActive(true);
    }

    public void YouLose()
    {
        gameEnd = true;
        loseUI.SetActive(true);
    }

    public void Nextstage(){

    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        if (Time.timeScale != 0)
        {
            menuUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            menuUI.SetActive(false);
        }
    }
}
