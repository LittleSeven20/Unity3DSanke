using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Purchasing;

public class SnakeHeads : MonoBehaviour
{
    public int step = 35;
    private int x;
    private int y;
    private Vector3 headPos;
    public float velocity = 0.35f;

    public AudioClip eatClip;
    public AudioClip dieClip;

    public GameObject dieEffect;
    public GameObject bodyPrefad;
    public List<Transform> bodyList = new List<Transform>();
    public Sprite[] bodySprites = new Sprite[2];
    public Transform canvas;
    public bool isDie = false;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        //Resources.Load("")加载资源，path的书写不需要文件扩展名
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh","sh02"));
        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
    }

    // Start is called before the first frame update
    void Start()
    {
        //按照velocity的时间间隔调用move方法
        InvokeRepeating("Move",0, velocity);
        x = 0; y = step;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false) {//按下空格
            //取消move方法的调用
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity - 0.2f);
        }


        if (Input.GetKeyUp(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {//松开空格
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }

        if (Input.GetKey(KeyCode.W) && y!= -step && MainUIController.Instance.isPause == false && isDie == false) {
            x = 0; y = step;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) && y!= step && MainUIController.Instance.isPause == false && isDie == false)
        {
            x = 0; y = -step;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);

        }
        if (Input.GetKey(KeyCode.A) && x!= step && MainUIController.Instance.isPause == false && isDie == false)
        {
            x = -step; y = 0;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetKey(KeyCode.D) && x != -step && MainUIController.Instance.isPause == false && isDie == false)
        {
            x = step; y = 0;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);

        }
    }

    void Move()
    {
        headPos = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(headPos.x + x, headPos.y + y);
        if (bodyList.Count > 0) {
            //bodyList.Last().localPosition = headPos;
            //bodyList.Insert(0, bodyList.Last());
            //bodyList.RemoveAt(bodyList.Count - 1);
            for (int i = bodyList.Count - 2; i >= 0 ; i--) {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;
            }
            bodyList.First().localPosition = headPos;
        }
        
    }

    void Grow()
    {
        AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;
        GameObject body = Instantiate(bodyPrefad, new Vector3(2000,2000,0),Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprites[index];
        //false 表示不保持世界坐标，以Parent为主
        body.transform.SetParent(canvas, false);
        bodyList.Add(body.transform);
    }

    void Die()
    {
        AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        CancelInvoke();
        isDie = true;
        headPos = gameObject.transform.localPosition;
        Instantiate(dieEffect,new Vector3(headPos.x,headPos.y,0),Quaternion.identity);
        PlayerPrefs.SetInt("lastl", MainUIController.Instance.length);
        PlayerPrefs.SetInt("lasts", MainUIController.Instance.score);
        if (PlayerPrefs.GetInt("bestl", 0) < MainUIController.Instance.length)
        {
            PlayerPrefs.SetInt("bestl", MainUIController.Instance.length);
            PlayerPrefs.SetInt("bests", MainUIController.Instance.score);
        }
        StartCoroutine(GameOver(1.5f));//调用延迟
    }

    IEnumerator GameOver(float t)
    {
        yield return new WaitForSeconds(t);//等待t
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.gameObject.CompareTag("Food")
        if (collision.tag == "Food")
        {
            Destroy(collision.gameObject);
            FoodsMake.instance.MakeFood(Random.Range(0, 100) < 20 ? true : false);
            MainUIController.Instance.UpdateUI();
            Grow();
        }
        else if (collision.tag == "Body")
        {
            Die();
            //print("Body Die");
        }
        else if (collision.tag == "Reward")
        {
            Destroy(collision.gameObject);
            Grow();
            MainUIController.Instance.UpdateUI(Random.Range(5, 15) * 10);

        }
        else if (MainUIController.Instance.hasBorder)
        {
            Die();
        }
        else
        {
            switch (collision.gameObject.name)
            {
                case "up":
                    transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                    break;
                case "down":
                    transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                    break;
                case "left":
                    transform.localPosition = new Vector3(-transform.localPosition.x + 6 * 30, transform.localPosition.y, transform.localPosition.z);
                    break;
                case "right":
                    transform.localPosition = new Vector3(-transform.localPosition.x + 8 * 30 + 15, transform.localPosition.y, transform.localPosition.z);
                    break;
            }

        }
    }
}
