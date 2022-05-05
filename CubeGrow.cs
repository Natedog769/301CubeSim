using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     This is the primary script here, its on the cube prefab and and will be changed 
///     with the UI elements
///     User controlled variables are ui connecte3d
///     the process variables are used under the hood to help with the simulation
///     the bool variables of 'boost' are not used in the final product their implementation wasnt working right
///     
/// </summary>

public class CubeGrow : MonoBehaviour
{

    [Header("User Controlled Variables")]
    //if false this will destroy the older cube and replace it with the smaller one
    public bool eatSmaller = true;

    public bool boostTop = false;
    public bool boostBottom = false;
    public bool boostLeft = false;
    public bool boostRight = false;
    public bool boostBack = false;
    public bool boostFront= false;

    //if this cube has this many kids it dies
    public int deathMoreThan = 6;
    public int deathLessThan = 1;

    public int numOfSuccess = 0;
    public int boostAfterSuccess = 1;


    public float growReduction = 10; //when spawned its grow chance is reduced this much
    public float growBoost = 5; //when a small one colliders with a bigger one its grow chance grows
    public float minChanceForBoost = 20;


    [Header("Processing Variables")]
    public CubeGrow cubeLimb;//the reference to the prefab to spawn

    public Vector3 myStartColorRGB; //the RGB value of the cube

    public Renderer myRend;// this is the mesh renderer to change the color

    public Vector3 maxSize = new Vector3(1,1,1);// the max size of the cubes

    public List<Vector3> posAround; //this is a list of the 6 positions around it

    //out of 100
    public float growChance = 99;

    public int success = 0;

    // Start is called before the first frame update
    //when a cube is spawned it goes thru the spawning pprocess
    //it starts off with a 0 size and will grow to the max size
    void Start()
    {
        numOfSuccess = 0;
        transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(GrowCubes());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale != maxSize)
            transform.localScale = Vector3.Lerp(transform.localScale, maxSize, Time.deltaTime);

    }

    /// <summary>
    ///     This will iterate around the cube and roll dice to spawn another cube
    ///     when it grows one it will pass the current color with is change into the set color
    ///     when this is called it iterates all the positions around and will roll dice
    ///     the bigger the spawn chance the faster it will spawn
    /// </summary>
    /// <returns></returns>
    IEnumerator GrowCubes()
    {
        int post = 0;
        foreach (Vector3 pos in posAround)
        {
            if (growChance < 10)
            {
                switch (post)
                {
                    case 0:
                        {
                            if (boostTop)
                                growChance += growBoost;

                            break;
                        }
                    case 1:
                        {
                            if (boostBottom)
                                growChance += growBoost;
                            break;
                        }
                    case 2:
                        {
                            if (boostRight)
                                growChance += growBoost;

                            break;
                        }
                    case 3:
                        {
                            if (boostLeft)
                                growChance += growBoost;

                            break;
                        }
                    case 4:
                        {
                            if (boostFront)
                                growChance += growBoost;

                            break;
                        }
                    case 5:
                        {
                            if (boostBack)
                                growChance += growBoost;

                            break;
                        }
                }
            }
            yield return new WaitForSeconds(1 - (growChance / 100));

            //here is the magic
            if (RollDice())
            {
                //here it creates the new color
                float red = myRend.material.color.r + pos.y;
                float green = myRend.material.color.g + pos.x;
                float blue = myRend.material.color.b + pos.z;
                //here is gets the relative postion to put the new cube at
                Vector3 newPost = new Vector3(this.transform.position.x + pos.x, this.transform.position.y + pos.y, this.transform.position.z + pos.z);

                //print("Growing a cube at " + pos);
                //then create the new cube in that position with the prefab
                CubeGrow newC = Instantiate<CubeGrow>(cubeLimb, newPost, transform.rotation);
                //set its color to the new cube
                newC.SetColor(red, green, blue);
                //set its grow chance this chance minus the reduction rate set by the user
                newC.growChance = this.growChance - growReduction;

                //this passes on the boost but again not in final product.
                newC.boostFront = this.boostFront;
                newC.boostBack = this.boostBack;
                newC.boostTop = this.boostTop;
                newC.boostBottom = this.boostBottom;
                newC.boostRight = this.boostRight;
                newC.boostLeft = this.boostLeft;



                //then tally the spawn
                this.numOfSuccess++;
            }
            post++;

            //here we can add a check for the num of success like if only 1 child then boost the chance

        }

        //after we grow children we then will make a final check
        //if the num of suecces meets the death bumber it destroys the whole object otherwise it just removes this script
        if (numOfSuccess >= deathMoreThan && deathMoreThan > 0)
        {
            Destroy(this.gameObject);
        }
        else if (numOfSuccess < deathLessThan && deathLessThan > 0)
        {
            Destroy(this.gameObject);

        }
        else
        {
            //just remove the script
            Destroy(this);
        }
    }



    //these are the class methods that are used

    /// this will accept the rgb of a new color
    public void SetColor(float r, float g, float b)
    {
        if (r < 0)
            r = 0;
        else if (r > 255)
            r = 255;
        if (g < 0)
            g = 0;
        else if (g > 255)
            g = 255;
        if (b < 0)
            b = 0;
        else if (b > 255)
            b = 255;

        myRend.material.color = new Color(r, g, b);
    }

    public void SetChance(float parentSuccess)
    {
        if (parentSuccess == boostAfterSuccess)
        {
            growChance += growBoost;
        }
    }

    public bool RollDice()
    {
        int res = Random.Range(0, 100);
        //print("I rolled a "+res + "with a chance of " + growChance);
        

        if (res < growChance)
        {
            //print("returning a success roll");
            success++;
            return true;
        }
        else return false;

    }


    private void OnTriggerEnter(Collider other)
    {



        if (eatSmaller) //this will destroy the small
        {
            //if true this will destroy the smaller one or younger one
            if (this.transform.localScale.y > other.transform.localScale.y)
                Destroy(other.gameObject);
            else
                Destroy(this.gameObject);

            //help its randomness
            if (growChance < minChanceForBoost)
                growChance += growBoost;
        }
        else // 
        {
            //this will destroy the older one, here we could do something interesting
            if (this.transform.localScale.y > other.transform.localScale.y)
                Destroy(this.gameObject);
            else Destroy(other.gameObject);

            //we could help its grow rate

            //help its randomness
            if (growChance < minChanceForBoost)
                growChance += growBoost;


            //alter color oo alpha maybe
        }
    }

}
