using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject startGame;
    bool loading;
    public TextMeshProUGUI text;
    int time;
    void Start()
    {
        loading = true;
        StartCoroutine(LoadingCo());
    }

    IEnumerator LoadingCo()
    {
        while (loading)
        {
            if (time == 1)
            {
                text.text = "Loading .";
            }
            else if (time == 2)
            {
                text.text = "Loading . .";
            }
            else if(time ==3)
            {
                text.text = "Loading . . .";
            }
             else
            {
                text.text = "Loading ";
            }
            time++;
            time = time % 4;

            yield return new WaitForSeconds(0.1f);
        }
    }
    public void EndOfLoading()
    {
        loading = false;
        startGame.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
