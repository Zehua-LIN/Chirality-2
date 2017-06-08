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
    [SerializeField] Canvas canvasselected;
    [SerializeField] Canvas canvasnotselected;
    [SerializeField] Sprite onemd;
    [SerializeField] Sprite twomd;
    [SerializeField] Sprite threemd;
    [SerializeField] Sprite fourmd;
    [SerializeField] Sprite fivemd;
    [SerializeField] Text lastgamescorenbr;
    [SerializeField] List<GameObject> rows;
    [SerializeField] Canvas canvasr;
    [SerializeField] Sprite sn;

    public Canvas lvlbtncanvas;
    public Button[] lvlbtns;
    public Button button;
    private Text[] texts;
    private Image[] imgs;
    private Dictionary<string, Image> imgsmds;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        Debug.Log("6: " + PlayerPrefs.GetInt("Level_6_Times_Pl"));
        goToFirstPage();
    }

    public void goToFirstPage()
    {
        texts = GetComponentsInChildren<Text>();
        imgs = GetComponentsInChildren<Image>();

        for (int j = 0; j < imgs.Length; j++) {
            switch (imgs[j].tag)
            {
                case "10":
                    if (PlayerPrefs.HasKey("Level_1_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_1_High_Percentage"), 1);
                        imgs[j].color = new Color(255, 255, 255, 255);

                    }
                    break;
                
                case "11":
                    if (PlayerPrefs.HasKey("Level_2_Standard_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_2_Standard_High_Percentage"), 2);
                        imgs[j].color = new Color(255, 255, 255, 255);
                    }
                    break;
                case "12":
                      if (PlayerPrefs.HasKey("Level_2_Trial_High_Percentage"))
                      {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_2_Trial_High_Percentage"), 3);
                        imgs[j].color = new Color(255, 255, 255, 255);
                      }
                    break;
                case "13":
                    if (PlayerPrefs.HasKey("Level_2_Extreme_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_2_Extreme_High_Percentage"), 4);
                        imgs[j].color = new Color(255, 255, 255, 255);
                    }
                    break;
                case "14":
                    if (PlayerPrefs.HasKey("Level_3_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_3_High_Percentage"), 5);
                        imgs[j].color = new Color(255, 255, 255, 255);
                    }
                    break;
                case "15":
                    if (PlayerPrefs.HasKey("Level_4_Standard_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_4_Standard_High_Percentage"), 6);
                        imgs[j].color = new Color(255, 255, 255, 255);
                    }
                    break;
                case "16":
                    if (PlayerPrefs.HasKey("Level_4_Extreme_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_4_Extreme_High_Percentage"), 7);
                        imgs[j].color = new Color(255, 255, 255, 255);
                    }
                    break;
                case "17":
                    if (PlayerPrefs.HasKey("Level_5_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_5_High_Percentage"), 8);
                        imgs[j].color = new Color(255, 255, 255, 255);
                    }
                    break;
                case "18":
                    if (PlayerPrefs.HasKey("Level_6_High_Percentage"))
                    {
                        imgs[j].sprite = displayCorrespondingMdImage(PlayerPrefs.GetFloat("Level_6_High_Percentage"), 9);
                        imgs[j].color = new Color(255, 255, 255, 255);
                    }
                    break;
                default:
                    break;

            }

            imgs[j].gameObject.SetActive(true);
        }

        for (int i = 0; i < texts.Length; i++)
        {
            float number = 0f;
            int whatText = 0;
            switch (texts[i].tag)
            {
                case "1":
                    if (PlayerPrefs.GetInt("Level_1_Already_Played") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_1_High_Percentage");
                        displayTextScoresAndTimes(2, number, texts[i], 1);
                    }
                    break; 

                case "2":
                    if (PlayerPrefs.GetInt("Level_1_Already_Played") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_1_Time");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }
                    break;   

                case "3":
                    if (PlayerPrefs.GetInt("Level_2_Already_Played_Stdd") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_2_Standard_High_Percentage");
                        displayTextScoresAndTimes(1, number, texts[i], 2);
                    }
                    break;

                case "4":
                    if (PlayerPrefs.GetInt("Level_2_Already_Played_Stdd") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_2_Standard_High_Percentage");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                       
                    }

                    break;
                case "5":
                    if (PlayerPrefs.GetInt("Level_2_Already_Played_Time_Trial") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_2_Trial_High_Percentage");
                        displayTextScoresAndTimes(1, number, texts[i], 3);
                    } 
                    break;

                case "6":
                    if (PlayerPrefs.GetInt("Level_2_Already_Played_Time_Trial") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_2_Trial_High_Percentage");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }
                        
                    break;

                case "7":
                    if (PlayerPrefs.GetInt("Level_2_Already_Played_Ext") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_2_Extreme_High_Percentage");
                        displayTextScoresAndTimes(1, number, texts[i], 4);
                    }
                        
                    break;

                case "8":
                    if (PlayerPrefs.GetInt("Level_2_Already_Played_Ext") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_2_Extreme_High_Percentage");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }
                        
                    break;

                case "9":
                    if (PlayerPrefs.GetInt("Level_3_Already_Played") == 1) {
                        number = PlayerPrefs.GetFloat("Level_3_High_Percentage");
                        displayTextScoresAndTimes(2, number, texts[i], 5);
                    }
                    
                    break;

                case "10":
                    if (PlayerPrefs.GetInt("Level_3_Already_Played") == 1) {
                        number = PlayerPrefs.GetFloat("Level_3_Time");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }  
                    break;

                case "11":
                    if (PlayerPrefs.GetInt("Level_4_Already_Played_Stdd") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_4_Standard_High_Percentage");
                        displayTextScoresAndTimes(2, number, texts[i], 6);
                    }
                    break;

                case "12":
                    if (PlayerPrefs.GetInt("Level_4_Already_Played_Stdd") == 1) {
                        number = PlayerPrefs.GetFloat("Level_4_Time_Stdd");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }
                    break;

                case "13":
                    if (PlayerPrefs.GetInt("Level_4_Already_Played_Ext") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_4_Standard_High_Percentage");
                        displayTextScoresAndTimes(1, number, texts[i], 7);
                    }
                    break;

                case "14":
                    if (PlayerPrefs.GetInt("Level_4_Already_Played_Ext") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_4_Time_Ext");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }
                    break;
                case "15":
                    if (PlayerPrefs.GetInt("Level_5_Already_Played") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_5_High_Percentage");
                        displayTextScoresAndTimes(2, number, texts[i], 8);
                    }
                    break;

                case "16":
                    if (PlayerPrefs.GetInt("Level_5_Already_Played") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_5_Time");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }
                    break;

                case "17":
                    if (PlayerPrefs.GetInt("Level_6_Already_Played") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_6_High_Percentage");
                        displayTextScoresAndTimes(2, number, texts[i], 9);
                    }
                    break;

                case "18":
                    if (PlayerPrefs.GetInt("Level_6_Already_Played") == 1)
                    {
                        number = PlayerPrefs.GetFloat("Level_6_Time");
                        displayTextScoresAndTimes(1, number, texts[i], 0);
                    }
                    break;

                default:
                    break;
            }

        }

    }

    void displayTextScoresAndTimes(int n, float number, Text text, int which)
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
        canvasnotselected.gameObject.SetActive(false);

        int n = i;
        string scores = "";
        string times = "";
        string days = "";
        string modes = "";
        
        switch(n)
        {
            case 1:
                scores = PlayerPrefs.GetString("Level_1_Percentages");
                times = PlayerPrefs.GetString("Level_1_Times");
                days = PlayerPrefs.GetString("Level_1_Days");
                slctdlvltext.text = "1: Functional Groups";
                break;
            case 2:
                scores = PlayerPrefs.GetString("Level_2_Percentages_Stdd");
                times = PlayerPrefs.GetString("Level_2_Times_Stdd");
                days = PlayerPrefs.GetString("Level_2_Days_Stdd");
                slctdlvltext.text = "2: Structure Classification - Standard";
                break;
            case 3:
                scores = PlayerPrefs.GetString("Level_2_Percentages_Time_Trial");
                times = PlayerPrefs.GetString("Level_2_Times_Time_Trial");
                days = PlayerPrefs.GetString("Level_2_Days_Time_Trial");
                slctdlvltext.text = "2: Structure Classification - Time Trial";

                
                break;

            case 4:
                scores = PlayerPrefs.GetString("Level_2_Percentages_Ext");
                times = PlayerPrefs.GetString("Level_2_Times_Ext");
                days = PlayerPrefs.GetString("Level_2_Days_Ext");
                slctdlvltext.text = "2: Structure Classification - Extreme"; 
                break;
            case 5:

                scores = PlayerPrefs.GetString("Level_3_Percentages");
                times = PlayerPrefs.GetString("Level_3_Times");
                days = PlayerPrefs.GetString("Level_3_Days");
                slctdlvltext.text = "3: Intermolecular Forces";
                break;

            case 6:
                scores = PlayerPrefs.GetString("Level_4_Percentages_Stdd");
                times = PlayerPrefs.GetString("Level_4_Times_Stdd");
                days = PlayerPrefs.GetString("Level_4_Days_Stdd");
                slctdlvltext.text = "4: Isomers - Standard";

                break;

            case 7:
                scores = PlayerPrefs.GetString("Level_4_Percentages_Ext");
                times = PlayerPrefs.GetString("Level_4_Times_Ext");
                days = PlayerPrefs.GetString("Level_4_Days_Ext");
                slctdlvltext.text = "4: Isomers - Extreme";
                break;

            case 8:
                scores = PlayerPrefs.GetString("Level_5_Percentages");
                times = PlayerPrefs.GetString("Level_5_Times");
                days = PlayerPrefs.GetString("Level_5_Days");
                slctdlvltext.text = "5: Chiral Carbons";
                break;
            case 9:
                scores = PlayerPrefs.GetString("Level_6_Percentages");
                times = PlayerPrefs.GetString("Level_6_Times");
                days = PlayerPrefs.GetString("Level_6_Days");
                slctdlvltext.text = "6: Naming Molecules";
                break;

            default:
                break;

        }

        Canvas[] arraycanvas = canvasr.GetComponentsInChildren<Canvas>();
        Dictionary<string, Canvas > rowsCanvas = new Dictionary<string, Canvas>();
        for (int ii =0; ii < arraycanvas.Length; ii++)
        {
            rowsCanvas.Add(arraycanvas[ii].tag, arraycanvas[ii]);
        }

        char[] c = new char[1];
        c[0] = ',';
        string[] arrayScores = scores.Split(c);
        string score = "";
        int nmbtag = 0;

        string[] arrayTimes = times.Split(c);
        string sttime = "";

        string[] arrayDays = days.Split(c);
        string day = "";

        for (int j=1; j< arrayTimes.Length; j++)
        {
            Debug.Log("r loop " + j);
            score = arrayScores[arrayScores.Length - j - 1];
            sttime = arrayTimes[arrayTimes.Length - j - 1];
            day = arrayDays[arrayDays.Length - j - 1];

            Debug.Log("score:" + score);
            Debug.Log("time:" + sttime);
            Debug.Log("day:" + day);

            Sprite s = sn;

            float b = 0f;
            if (!score.StartsWith("Game"))
            {
                b = float.Parse(score);
            

            if (n != 2 && n !=3 && n !=4)
            {
                if (n == 7)
                {
                    if ((int)b >= 40)
                    {
                        s = onemd;
                    }
                    else if ((int)b >= 30)
                    {
                        s = twomd;
                    }
                    else if ((int)b >= 20)
                    {
                        s = threemd;
                    }
                    else if ((int)b >= 10)
                    {
                        s = fourmd;
                    }
                    else
                    {
                        s = fivemd;
                    }

                } else
                {
                    
                    if (b < 0.5f)
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

                }
                
            } else if (!score.StartsWith("Game"))
            {
                switch (n)
                {
                    case 2: // Level 2 Stdd
                        if ((int) b <= 80)
                        {
                            s = onemd;
                        } else if ((int) b <= 120)
                        {
                            s = twomd;
                        } else if ((int) b <= 180)
                        {
                            s = threemd;
                        } else if ((int) b <= 240)
                        {
                            s = fourmd;
                        } else if ((int) b > 240)
                        {
                            s = fivemd;
                        }
                        break;
                    case 4: // Level 2 Ext
                        if ((int)b <= 70)
                        {
                            s = fivemd;
                        }
                        else if ((int)b <= 120)
                        {
                            s = fourmd;
                        }
                        else if ((int)b <= 180)
                        {
                            s = threemd;
                        }
                        else if ((int)b <= 240)
                        {
                            s = twomd;
                        }
                        else if ((int) b >= 240)
                        {
                            s = onemd;
                        }
                        break;
                    case 3: // Level 2 Time Trial
                        if ((int)b <= 20)
                        {
                            s = fivemd;
                        }
                        else if ((int)b <= 35)
                        {
                            s = fourmd;
                        }
                        else if ((int)b <= 50)
                        {
                            s = threemd;
                        }
                        else if ((int)b < 65)
                        {
                            s = twomd;
                        }
                        else if ((int)b >= 65)
                        {
                            s = onemd;
                        }
                        break;

                }
            }

            }


            Canvas rcanvs = rowsCanvas[j.ToString()];
            Debug.Log("r canvas tag: " + rcanvs.tag + " j: " + j.ToString());
            rcanvs.gameObject.SetActive(true);

            Text[] textrcanvs = rcanvs.GetComponentsInChildren<Text>();
            for (int e = 0; e < textrcanvs.Length; e++)
            {
                Text textrcanvst = textrcanvs[e];
                switch (textrcanvst.tag)
                {
                    case "1":
                        if (n == 2 || n == 4)
                        {
                        }
                        
                        break;
                    case "2":
                        textrcanvst.text = day;
                        break;
                    case "3":
                        if (n == 2 || n==3 || n == 4 || n== 7)
                        {
                            Debug.Log("score:");
                            Debug.Log(score);
                            if (!score.StartsWith("Game"))
                            {
                                int mnts = (int)float.Parse(score) / 60;
                                int scnds = (int)float.Parse(score) % 60;
                                string scndsw = scnds.ToString();
                                if (scnds < 10)
                                {
                                    scndsw = "0" + scnds;
                                }
                                textrcanvst.text = mnts + ":" + scndsw;
                            } else
                            {
                                textrcanvst.text = score;
                            }

                            
                        } else
                        {
                            textrcanvst.text = (float.Parse(score) * 100).ToString() + "%";
                        }
                        break;

                    case "4":

                        if (!sttime.StartsWith("Game"))
                        {
                            int mntst = (int)float.Parse(sttime) / 60;
                            int scndst = (int)float.Parse(sttime) % 60;
                            string scndswt = scndst.ToString();
                            if (scndst < 10)
                            {
                                scndswt = "0" + scndst;
                            }
                            textrcanvst.text = mntst + ":" + scndswt;
                        } else
                        {
                            textrcanvst.text = sttime;
                        }  

                        break;
                    default:
                        break;
                }
            }

            Image[] imgs = rcanvs.GetComponentsInChildren<Image>();

            for (int a = 0; a < imgs.Length; a++)
            {
                if (imgs[a].tag == "md")
                {
                    Image img = imgs[a];

                    if (!score.StartsWith("Game"))
                    {
                        img.sprite = s;
                        img.color = new Color(255, 255, 255, 255);
                    }
                }
            }
            nmbtag++;
        }
        
        canvasselected.gameObject.SetActive(true);

        
        for(int b = arrayTimes.Length; b < 51; b++)
        {
          rowsCanvas[b.ToString()].gameObject.SetActive(false);
           
        }

        float prefrheight = Mathf.Round((arrayTimes.Length - 1) * 66);
        if (prefrheight < 528)
        {
            prefrheight = 528;
        }

        LayoutElement layoutelmnt = canvasr.GetComponent<LayoutElement>();
        layoutelmnt.preferredHeight = prefrheight;

      
    }

    public Sprite displayCorrespondingMdImage(float b, int i)
    {
        Sprite s = sn;
            if (i == 7) // Level 4 Extreme
            {
                if ((int)b >= 40)
                {
                    s = onemd;
                }
                else if ((int)b >= 30)
                {
                    s = twomd;
                }
                else if ((int)b >= 20)
                {
                    s = threemd;
                }
                else if ((int)b >= 10)
                {
                    s = fourmd;
                }
                else
                {
                    s = fivemd;
                }

            } else if (i == 1 || i == 5 || i == 6 || i == 8 || i == 9) // Levels 1, 3, 4 Standard, 5 and 6
            {
                if (b < 0.5f)
                {
                    s = fivemd;
                }
                else if (b >= 0.5f && b <= 0.69f)
                {
                    s = fourmd;
                }
                else if (b >= 0.7f && b <= 0.79f)
                {
                    s = threemd;
                }
                else if (b >= 0.8f && b <= 0.99f)
                {
                    s = twomd;
                }
                else
                {
                    s = onemd;
                }

            } else
        {
            switch(i)
            {

                case 2: // Level 2 Standard
                    if ((int)b <= 80)
                    {
                        s = onemd;
                    }
                    else if ((int)b <= 120)
                    {
                        s = twomd;
                    }
                    else if ((int)b <= 180)
                    {
                        s = threemd;
                    }
                    else if ((int)b <= 240)
                    {
                        s = fourmd;
                    }
                    else if ((int)b > 240)
                    {
                        s = fivemd;
                    }
                    break;
                case 4: // Level 2 Extreme
                    if ((int)b <= 70)
                    {
                        s = fivemd;
                    }
                    else if ((int)b <= 120)
                    {
                        s = fourmd;
                    }
                    else if ((int)b <= 180)
                    {
                        s = threemd;
                    }
                    else if ((int)b <= 240)
                    {
                        s = twomd;
                    }
                    else if ((int)b >= 240)
                    {
                        s = onemd;
                    }
                    break;
                case 3: // Level 2 Time Trial
                    if ((int)b <= 20)
                    {
                        s = fivemd;
                    }
                    else if ((int)b <= 35)
                    {
                        s = fourmd;
                    }
                    else if ((int)b <= 50)
                    {
                        s = threemd;
                    }
                    else if ((int)b < 65)
                    {
                        s = twomd;
                    }
                    else if ((int)b >= 65)
                    {
                        s = onemd;
                    }
                    break;

            }

        }
        return s;
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
