using UnityEngine;
using System.Collections;

public class StaffAI : MonoBehaviour {


	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]


	bool working = false;
    public bool tableworking= false;

    public GameObject table;
	public Vector2 move;
	public Vector2 stay;
	public int range = 0;
	public int range2 = 0;
	public float number = 0;
	public float speed  = 1.0f;

	public float time_i = 0.0f;
	public float time_c = 3.0f;

	public float time_a = 0.0f;
	public float time_b;

	private Animator anim;

    public GM GM;

    float moneytimer = 0;
    int earnmoney = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


	void Update () {

		if (working == false) {
			if (Time.time > time_i) {
				anim.Play ("character01_move");
				time_i = Time.time + time_c;
				range = Random.Range (-3, 3);
				range2 = Random.Range (-3, 3);
			}
			AImove ();
        }else{//working
            if (ChecktimerScale())
            {
                moneytimer += Time.deltaTime;
                GM.Updatemoney(earnmoney);
            }
        }
		if (Time.time > time_a) {
			time_a = Time.time+time_b;
			working =false;
            if(tableworking == true){
                print("leave");
                tableworking = false;
                GM.Leavetable(this.gameObject);
            }
		}
	}

	void AImove (){
			move = new Vector2 ((range) * Time.deltaTime, (range2) * Time.deltaTime);
			transform.Translate (move);

	}

	void OnMouseDown(){
        if (ChecktimerScale())
        {
            GM.Checktable(GM.Findfreetable(), this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            range = range * -1;
            range2 = range2 * -1;
            AImove();
            print("ground");
        }
        if (collision.gameObject.tag == "table")
        {
            GM.Checktable(collision.gameObject, this.gameObject);
            working = true;
        }
    }
	
	public void Needtowork(GameObject table){ //call by GM  // working
		float position = table.transform.position.x+1.5f;
		float position2 = table.transform.position.y+1;

		stay = new Vector2 (position,position2);
        this.transform.position =stay;

        anim.Play ("character 01");
        tableworking = true;
		callrandomshouldiworking();

	}

	void callrandomshouldiworking(){
		time_b= Random.Range (1.0f, 6.0f);
		time_a = Time.time + time_b;
	}

	void FixedUpdate () {
		float h = range;
		if (h < 0 && !facingRight)
			flip ();
		else if (h > 0 && facingRight)
			flip ();
	}

	void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public bool ChecktimerScale()
    {
        if(Time.timeScale != 0) {
            return true;
        }
        else
        {
            return false;
        }
    }
}
