using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIController : MonoBehaviour
{
    public TMP_Text lastText;
    public TMP_Text bestText;
    public Toggle blue;
    public Toggle yellow;
    public Toggle border;
    public Toggle noBorder;

    void Awake()
    {
        lastText.text = "上次长度" + PlayerPrefs.GetInt("lastl", 0) + "\n上次分数" + PlayerPrefs.GetInt("lasts", 0);
        bestText.text = "最好长度" + PlayerPrefs.GetInt("bestl", 0) + "\n最好分数" + PlayerPrefs.GetInt("bests", 0);
    }

    void Start()
    {
        if (PlayerPrefs.GetString("sh", "sh01") == "sh01")
        {
            blue.isOn = true;
            //PlayerPrefs.SetString("sh", "sh01");
            //PlayerPrefs.SetString("sb01", "sb0101");
            //PlayerPrefs.SetString("sb02", "sb0102");
        }
        else
        {
            yellow.isOn = true;
            //PlayerPrefs.SetString("sh", "sh02");
            //PlayerPrefs.SetString("sb01", "sb0201");
            //PlayerPrefs.SetString("sb02", "sb0202");
        }
        if (PlayerPrefs.GetInt("border", 1) == 1)
        {
            border.isOn = true;
            //PlayerPrefs.SetInt("border", 1);
        }
        else
        {
            noBorder.isOn = true;
            //PlayerPrefs.SetInt("border", 0);
        }
    }

    public void BlueSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
            print(PlayerPrefs.GetString("sb02"));
        }
    }

    public void YellowSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
            print(PlayerPrefs.GetString("sb02"));
        }
    }

    public void BorderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 1);
        }
    }

    public void NoBorderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
