using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHudManager : MonoBehaviour {

    private Text _lapTime_text;
    private Text _lastLapTime_text;
    private Text _lapDifference_text;
    private Text _lapCounter_text;

    private float _lapDifference;

    private Text _lapTime_textP2;
    private Text _lastLapTime_textP2;
    private Text _lapDifference_textP2;
    private Text _lapCounter_textP2; 

    private float _lapDifferenceP2;


    // Use this for initialization
    void Start ()
    {
        _lapTime_text = GameObject.Find("LapTime").GetComponent<Text>();
        _lastLapTime_text = GameObject.Find("LastLapTime").GetComponent<Text>();
        _lapDifference_text = GameObject.Find("LapDifference").GetComponent<Text>();
        _lapCounter_text = GameObject.Find("LapCount").GetComponent<Text>();

        //_lapTime_textP2 = GameObject.Find("LapTimeP2").GetComponent<Text>();
        //_lastLapTime_textP2 = GameObject.Find("LastLapTimeP2").GetComponent<Text>();
        //_lapDifference_textP2 = GameObject.Find("LapDifferenceP2").GetComponent<Text>();
        //_lapCounter_textP2 = GameObject.Find("LapCountP2").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		//P1
        CCar player = CGameManager.inst().GetPlayer();
   

        _lapTime_text.text = (System.Math.Round(player.GetLapTime(), 2)).ToString();
        _lastLapTime_text.text = (System.Math.Round(player.GetLastLapTime(), 2)).ToString();

       
        _lapDifference = player.GetLastLapTime() - player.GetLapTime();
		    


        if (_lapDifference > 0)
        {
            _lapDifference_text.color = Color.green;
            _lapDifference_text.text = "-" + ((int)_lapDifference).ToString();
        }
        else if (_lapDifference < 0)
        {
            _lapDifference_text.color = Color.red;
            _lapDifference_text.text = "+" + ((int)_lapDifference * -1).ToString();
        }
        else
        {
            _lapDifference_text.color = Color.green;
            _lapDifference_text.text =  ((int)_lapDifference).ToString();
        }

        _lapCounter_text.text = player.GetLapNumber().ToString() + "/" + CGameManager.inst().GetMaxNumberOfLaps().ToString();
      

		//P2

		//CCar player2 = CGameManager.inst().GetPlayer2();
		//_lapTime_textP2.text = (System.Math.Round(player2.GetLapTime(), 2)).ToString();
		//_lastLapTime_textP2.text = (System.Math.Round(player2.GetLastLapTime(), 2)).ToString();

		//_lapDifferenceP2 = player2.GetLastLapTime() - player2.GetLapTime();

		//if (_lapDifferenceP2 > 0)
		//{
		//	_lapDifference_textP2.color = Color.green;
		//	_lapDifference_textP2.text = "-" + ((int)_lapDifferenceP2).ToString();
		//}
		//else if (_lapDifferenceP2 < 0)
		//{
		//	_lapDifference_textP2.color = Color.red;
		//	_lapDifference_textP2.text = "+" + ((int)_lapDifferenceP2 * -1).ToString();
		//}
		//else
		//{
		//	_lapDifference_textP2.color = Color.green;
		//	_lapDifference_textP2.text =  ((int)_lapDifferenceP2).ToString();
		//}

		//_lapCounter_textP2.text = player2.GetLapNumber().ToString() + "/" + CGameManager.inst().GetMaxNumberOfLaps().ToString();

    }
}
