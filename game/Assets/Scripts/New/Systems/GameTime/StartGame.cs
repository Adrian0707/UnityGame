using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

public class StartGame : MonoBehaviour
{
    public Signal2 startGameSignal;
    public BuildingPlacement buildingPlacement;
    public GameObject Fireplace;
    public GameObject Player;
    public GameObject[] objectsSpawnedAtStart;
    public StrategyInfoSystem infoSystem;
    public Scripts scpts;
    public GameObject[] activeGuiElements;
    void Start()
    {
       // Debug.LogError(GameObject.FindGameObjectWithTag("Fireplace"));
    }
    private void Awake()
    {

        // SaveSystem.LoadLootTables();
        SaveSystem.LoadCoins();
        SaveSystem.LoadEnemy();
        SaveSystem.LoadUpgrades();
        startGameSignal.Raise();
        StartCoroutine(StartCo());
        //SaveSystem.SaveScripts(scpts);
        SaveSystem.LoadScripts(scpts);
       // print(MoonSharpFactorial());
        
    }
    double MoonSharpFactorial()
    {
        string scriptCode = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end
        RaiseGameStart(1)
		return fact(5)";
        Script script = new Script();
         script.Globals["RaiseGameStart"] = (Action<int>)StartCorutnineStart;
        script.DoString(scriptCode);
       
        //print(script.ToString());
        DynValue res = script.Call(script.Globals["fact"], 3);
        
        //script.Call(script.Globals["RaiseGameStart"]);
        return res.Number;

    }
    public void StartCorutnineStart(int k)
    {
        StartCoroutine(StartCo());
    }
 
    // Update is called once per frame
    void Update()
    {
       /* if (!GameObject.FindGameObjectWithTag("Fireplace")) { 
            buildingPlacement.SetItem(basicObjects[0]);
        }
        else if (GameObject.FindGameObjectWithTag("Fireplace"))
        {
            buildingPlacement.SetItem(basicObjects[1]);
        }*/
        

    }
    IEnumerator StartCo()
    {
        yield return null;
        startGameSignal.Raise();
        buildingPlacement.PutItem(Fireplace);
        
        while (!buildingPlacement.hasPlaced)
            {
          //  startGameSignal.Raise();
            yield return null;
            }
        Fireplace.transform.position = Fireplace.transform.position - new Vector3(0, 0, 1);
        /* Fireplace.SetActive(false);
         Fireplace.SetActive(true);*/
        //yield return null;
        Fireplace.GetComponent<Fireplace>().makepath();
        //Debug.LogError(GameObject.FindGameObjectWithTag("Fireplace"));
        Player.gameObject.transform.position = Fireplace.transform.position+new Vector3(0,-2,0);
      //  int i = 0;
        foreach (GameObject item in objectsSpawnedAtStart)
        {
            GameObject.Instantiate(item, Fireplace.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
        foreach (GameObject item in activeGuiElements)
        {
            item.SetActive(true);
        }
        // Player.GetComponent<Rigidbody2D>().isKinematic = false;
        Player.SetActive(true);
        Fireplace.GetComponent<Fireplace>().enabled = true;
       // Player.SetActive(true);

        }
    }

