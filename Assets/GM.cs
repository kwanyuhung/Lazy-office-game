using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{

    public int workmoney = 0;

    public GameObject outcomeUI;
    public GameObject menuUI;

    public GameObject outcomeanimation;
    public GameObject nextlevel;
    public TextMeshProUGUI workmoney_text;
    

    public List<GameObject> table;
    public List<GameObject> ITDog;

    public int workermoney = 1;
    public int outcome = 20;
    public float outcometimer = 0;
    public int maxoutcomtime = 5;

    bool gameEnd = false;

    public int maxgoal = 1000;

    public int highermoney = 0;

    public void Update()
    {
        outcometimer += Time.deltaTime;
        if(outcometimer > maxoutcomtime)
        {
            outcometimer =0;
            Removemoney(outcome);
            outcomeanimation.SetActive(true);
            outcomeanimation.GetComponent<TextMeshProUGUI>().text = "(-$" + outcome + ")";
            outcomeanimation.GetComponent<Animation>().Play();
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
        if(workmoney > highermoney)
        {
            highermoney = workmoney;
        }
        UpdateUI();

        if(workmoney >= maxgoal){
            Nextstage();
        }
    }

    public void Removemoney(int i){
        workmoney -= i;
        UpdateUI();
    }

    private void UpdateUI()
    {
        workmoney_text.text = "working = $" + workmoney;

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



    public void Endgame()
    {
        gameEnd = true;
    }


    public void Nextstage(){
        maxgoal += 1500;
        outcome *= 2;
        nextlevel.SetActive(true);
        nextlevel.GetComponent<Animation>().Play();
        outcomeUI.GetComponent<TextMeshProUGUI>().text = "Outcome:" + outcome + "$(sec)";
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        if (Time.timeScale != 0)
        {
            menuUI.SetActive(true);
            menuUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "goal money:$"+maxgoal;
            menuUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "worker =" + workermoney+"($)";
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            menuUI.SetActive(false);
        }
    }
}
