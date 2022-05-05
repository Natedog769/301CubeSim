using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
///     This script will be the user interactions and controls
///     it has all the references to the ui inputs and will spawn a cube and will set its varible on spawn
/// </summary>
public class UserSpawn : MonoBehaviour
{
    //here we will have parallel variables that will be tied to UI elements so the user can
    //alter the cube when spawning it

    public CubeGrow cubePrefab;//here is the prefab on spawn we will set the variables
    public Camera cam;
    public Transform spawnPoint;

    [Header("UI Elements")]
    public Slider startGrowChance;
    public Slider growReduction;
    public Slider growBoost;

    public Slider deathGreaterThanSuccess;
    public Slider deathLessThanSuccess;

    public Slider boostAfterSuccess;

    public Toggle top;
    public Toggle bottom;
    public Toggle right;
    public Toggle left;
    public Toggle back;
    public Toggle front;

    public TextMeshProUGUI sliderOut;
    public TextMeshProUGUI sliderOut2;
    public TextMeshProUGUI sliderOut3;
    public TextMeshProUGUI sliderOut4;
    public TextMeshProUGUI sliderOut5;
    public TextMeshProUGUI sliderOut6;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderOut.text = ""+ startGrowChance.value;
        sliderOut2.text = "" + growReduction.value;
        sliderOut3.text = "" + growBoost.value;
        sliderOut4.text = "" + deathGreaterThanSuccess.value;
        sliderOut5.text = "" + boostAfterSuccess.value;
        sliderOut6.text = "" + deathLessThanSuccess.value;





        if (Input.GetMouseButtonDown(1))
        {
            
            //spawn a cube
            CubeGrow spawn = Instantiate<CubeGrow>(cubePrefab, spawnPoint.position, Quaternion.identity);

            spawn.growChance = startGrowChance.value;

            spawn.deathMoreThan = (int)deathGreaterThanSuccess.value;
            spawn.boostAfterSuccess = (int)boostAfterSuccess.value;
            spawn.deathLessThan = (int)deathLessThanSuccess.value;

            spawn.growBoost = growBoost.value;
            spawn.growReduction = growReduction.value;

            spawn.boostFront = front.isOn;
            spawn.boostBack = back.isOn;
            spawn.boostLeft = left.isOn;
            spawn.boostRight = right.isOn;
            spawn.boostBottom = bottom.isOn;
            spawn.boostTop = top.isOn;
            


        }
    }


    public void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
