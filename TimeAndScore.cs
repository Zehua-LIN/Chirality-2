using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeAndScore : MonoBehaviour {

    [SerializeField] Text selectlvltext;
    [SerializeField] Text bscore;
    [SerializeField] Text timesPlayedtext;
    [SerializeField] Text bscorenumber;
    [SerializeField] Text timesPlayednumber;
    [SerializeField] Text slctdlvltext;
    [SerializeField] Button backBtn;
    [SerializeField] Canvas canvasselected;
    [SerializeField] Canvas canvasnotselected;
    [SerializeField] Sprite onemd;
    [SerializeField] Sprite twomd;
    [SerializeField] Sprite threemd;
    [SerializeField] Sprite fourmd;
    [SerializeField] Sprite fivemd;
    [SerializeField] Text lastgamescorenbr;
    [SerializeField] Canvas canvasnotselectedtwo;
    [SerializeField] List<GameObject> rows;
    [SerializeField] Canvas canvasr;


    public Canvas lvlbtncanvas;
    public Button[] lvlbtns;
    public Button button;
    private Text[] texts;

    void Start()
    {
        goToFirstPage();
    }

    public void noButtons()
    {
        
       // string slvname = lvname.text;

        Debug.Log("Clicked");
        

       // slctdlvltext.text = slvname;
       // Debug.Log("String:");
       // Debug.Log(slvname);
        //slctdlvltext.gameObject.SetActive(true);

        //timesPlayednumber.text = PlayerPrefs.GetInt("Level_5_Times_Played").ToString();
        
       // Debug.Log(hpercentage);

       // float lastgamepercentage = PlayerPrefs.GetFloat("Level_5_Last_Game_Score");
        //lastgamescorenbr.text = (lastgamepercentage * 100).ToString() + "%";

        
        
    }

    public void goToSecondPage()
    {

        canvasnotselected.gameObject.SetActive(false);
        canvasselected.gameObject.SetActive(false);
        canvasnotselectedtwo.gameObject.SetActive(true);
        texts = GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            float number = 0f;
            int whatText = 0;
            switch (texts[i].tag)
            {
                case "1":
                    number = PlayerPrefs.GetFloat("Level_4_High_Percentage_Stdd");
                    if (PlayerPrefs.GetFloat("Level_4_Time_Stdd") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;

                case "2":
                    number = PlayerPrefs.GetFloat("Level_4_High_Percentage_Time_Trial");
                    if (PlayerPrefs.GetFloat("Level_4_Time_Time_Trial") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;

                case "3":
                    number = PlayerPrefs.GetFloat("Level_5_High_Percentage");
                    if (PlayerPrefs.GetFloat("Level_5_Time") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;

                case "4":
                    number = PlayerPrefs.GetFloat("Level_6_High_Percentage");
                    if (PlayerPrefs.GetFloat("Level_6_Time") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;


                case "5":
                    number = PlayerPrefs.GetFloat("Level_4_Time_Stdd");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                case "6":
                    number = PlayerPrefs.GetFloat("Level_4_Time_Time_Trial");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                case "7":
                    number = PlayerPrefs.GetFloat("Level_5_Time");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                case "8":
                    number = PlayerPrefs.GetFloat("Level_6_Time");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                default:
                    break;
            }

            
            
            
        }

    }

    public void goToFirstPage()
    {
        texts = GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            float number = 0f;
            int whatText = 0;
            switch (texts[i].tag)
            {
                case "1":
                    number = PlayerPrefs.GetFloat("Level_1_High_Percentage");
                    if (PlayerPrefs.GetFloat("Level_1_Time") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;

                case "2":
                    number = PlayerPrefs.GetFloat("Level_2_High_Percentage_Stdd");
                    if (PlayerPrefs.GetFloat("Level_2_Time_Stdd") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;

                case "3":
                    number = PlayerPrefs.GetFloat("Level_2_High_Percentage_Time_Trial");
                    if (PlayerPrefs.GetFloat("Level_2_Time_Time_Trial") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;

                case "4":
                    number = PlayerPrefs.GetFloat("Level_2_High_Percentage_Ext");
                    if (PlayerPrefs.GetFloat("Level_2_Time_Ext") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;


                case "5":
                    number = PlayerPrefs.GetFloat("Level_3_High_Percentage");
                    if (PlayerPrefs.GetFloat("Level_3_Time") > 0f)
                    {
                        displayTextScoresAndTimes(2, number, texts[i]);
                    }
                    break;

                case "6":
                    number = PlayerPrefs.GetFloat("Level_1_Time");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                case "7":
                    number = PlayerPrefs.GetFloat("Level_2_Time_Stdd");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                case "8":
                    number = PlayerPrefs.GetFloat("Level_2_Time_Time_Trial");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                case "9":
                    number = PlayerPrefs.GetFloat("Level_2_Time_Ext");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;


                case "10":
                    number = PlayerPrefs.GetFloat("Level_3_Time");
                    if (number > 0f)
                    {
                        displayTextScoresAndTimes(1, number, texts[i]);
                    }
                    break;

                default:
                    break;
            }

        }
    }

    void displayTextScoresAndTimes(int n, float number, Text text)
    {
        switch (n)
        {
            case 1:
                int mnts = (int) (number / 60f);
                int scnds = (int) (number % 60f);
                string scndsw = scnds.ToString();
                if (scnds < 10)
                {
                    scndsw = "0" + scnds;
                }
                text.text = mnts + ":" + scndsw;
                break;
            case 2:
                 text.text = (number * 100).ToString() + "%";
                break;
            default:
                break;

        }
    }

    public void goToTimesAndScoresTable(int i)
    {
        canvasnotselectedtwo.gameObject.SetActive(false);
        int n = i;
        string scores = "";
        string times = "";
        
        switch(n)
        {
            case 1:
                scores = PlayerPrefs.GetString("Level_1_Percentages");
                times = PlayerPrefs.GetString("Level_1_Times");
                break;
            case 2:
                scores = PlayerPrefs.GetString("Level_2_Percentages");
                times = PlayerPrefs.GetString("Level_2_Times");
                break;
            case 3:
                scores = PlayerPrefs.GetString("Level_3_Percentages");
                times = PlayerPrefs.GetString("Level_3_Times");
                break;
            case 4:
                scores = PlayerPrefs.GetString("Level_4_Percentages");
                times = PlayerPrefs.GetString("Level_4_Times");
                break;
            case 5:
                scores = PlayerPrefs.GetString("Level_5_Scores");
                Debug.Log("scores:");
                Debug.Log(scores);
                //scores = PlayerPrefs.GetString("Level_5_Percentages");
                times = PlayerPrefs.GetString("Level_5_Times");
                break;
            case 6:
                scores = PlayerPrefs.GetString("Level_6_Percentages");
                times = PlayerPrefs.GetString("Level_6_Times");
                break;
            default:
                break;

        }

        Canvas[] arraycanvas = canvasr.GetComponentsInChildren<Canvas>();
        //Debug.Log("length:");
        //Debug.Log(arraycanvas.Length);

        char[] c = new char[1];
        c[0] = ',';
        string[] arrayScores = scores.Split(c);
        string score = "";
        int nmbtag = 1;

        string[] arrayTimes = times.Split(c);
        string sttime = "";
        Debug.Log(arrayTimes.Length);

        if (arrayTimes.Length > 19)
        {
            
            string[] arraynw = new string[19];
             
            for (int ii = 0; ii < 19; ii++)
            {
                arraynw[18 - ii] = arrayTimes[arrayTimes.Length - 1 - ii];
            }
            arrayTimes = new string[19];

            for(int iii = 0; iii<19; iii++)
            {
                arrayTimes[iii] = arraynw[iii];
            }
            
        }

        

        for(int j=1; j< arrayTimes.Length; j++)
        {
            score = arrayScores[arrayScores.Length - j - 1];
            sttime = arrayTimes[arrayTimes.Length - j - 1];

            int a = j;
            while (arraycanvas[a - 1].tag != nmbtag.ToString())
            {
                a++;
            }

            Sprite s;
            float b = float.Parse(score);

            if(b < 50)
            {
                s = fivemd;
            } else if(b >= 0.5f && b<= 0.69f)
            {
                s = fourmd;
            } else if(b >= 0.7f && b<=0.79f)
            {
                s = threemd;
            } else if(b >= 0.8f && b<=0.99f)
            {
                s = twomd;
            } else
            {
                s = onemd;
            }

            Canvas rcanvs = arraycanvas[a - 1];
            Debug.Log(arraycanvas[a - 1].tag);
            rcanvs.gameObject.SetActive(true);

            Text[] textrcanvs = rcanvs.GetComponentsInChildren<Text>();
            for (int e = 0; e < textrcanvs.Length; e++)
            {
                Text textrcanvst = textrcanvs[e];
                switch (textrcanvst.tag)
                {
                    case "1":
                        textrcanvst.text = "";
                        break;
                    case "2":
                        textrcanvst.text = "";
                        break;
                    case "3":
                        textrcanvst.text = (float.Parse(score) * 100).ToString() + "%";
                        break;
                    case "4":
                        int mnts = int.Parse(sttime) / 60;
                        int scnds = int.Parse(sttime) % 60;
                        string scndsw = scnds.ToString();
                        if (scnds < 10)
                        {
                            scndsw = "0" + scnds;
                        }
                        textrcanvst.text = mnts + ":" + scndsw;
                        break;
                    default:
                        break;
                }
            }

            Image img = rcanvs.GetComponentInChildren<Image>();
            img.sprite = s;

            nmbtag++;
        }
        
        canvasselected.gameObject.SetActive(true);
        Debug.Log("lgth");
        Debug.Log(arrayTimes.Length);
        for(int b = 1; b < 19; b++)
        {
            if (int.Parse(arraycanvas[b].tag) >= arrayTimes.Length)
            
            {

                Debug.Log("tag");
                Debug.Log(arraycanvas[b].tag);
                arraycanvas[b].gameObject.SetActive(false);
            } 
        }

        float prefrheight = Mathf.Round((arrayTimes.Length - 1) * 70);
        if (prefrheight < 528)
        {
            prefrheight = 500;
        }

        LayoutElement layoutelmnt = canvasr.GetComponent<LayoutElement>();
        layoutelmnt.preferredHeight = prefrheight;

    }

    public void backToSelectLevel()
    {
        SceneManager.LoadScene("Time_And_Score");
    } 

    public void backToHomeScreen()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void goToInfoScreen()
    {
        // SceneManager.LoadScene()
    }

}
