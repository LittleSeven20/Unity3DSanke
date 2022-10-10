using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodsMake : MonoBehaviour
{
    // Start is called before the first frame update

    private static FoodsMake _instance;
    public static FoodsMake instance
    {
        get {
            return _instance;
        }
    }
    public int xlimit = 21;
    public int ylimit = 11;
    public int xoffset = 7;
    public GameObject foodPrefad;
    public GameObject raward;
    public Sprite[] foodsPrites;//食物图片数组
    public Transform foodHolder;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        foodHolder = GameObject.Find("FoodHolder").transform;
        MakeFood(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeFood(bool isReword)
    {
        int index = Random.Range(0, foodsPrites.Length);
        GameObject food = Instantiate(foodPrefad);
        food.GetComponent<Image>().sprite = foodsPrites[index];
        food.transform.SetParent(foodHolder, false);
        int x = Random.Range(-xlimit + xoffset, xlimit);
        int y = Random.Range(-ylimit, ylimit);
        food.transform.localPosition = new Vector3(x * 25, y * 25, 0);
        if (isReword) {
            GameObject rawrd = Instantiate(raward);
            rawrd.transform.SetParent(foodHolder, false);
            x = Random.Range(-xlimit + xoffset, xlimit);
            y = Random.Range(-ylimit, ylimit);
            rawrd.transform.localPosition = new Vector3(x * 25, y * 25, 0);
        }
    }
}
