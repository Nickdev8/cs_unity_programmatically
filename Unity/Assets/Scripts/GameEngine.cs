using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameEngine : MonoBehaviour
{
    public GameObject pigModel;
    public GameObject tileModel;
    public GameObject towerModel;

    private Tower tower;
    private Enemy enemy;

    //maak hier private class variable, LETOP dit wordt een ARRAY van GameObject(en):
    //1 (acces=private, Type= GameObject[], name=path)
    //gebruik nog geen new
    private GameObject[] path; 

    void Start()
    {
        int x = 0;
        int z = 0;
        int size = 2;

        //maak hier een lokale variable,
        //1 (Type= RelAdd[], name=pathplus)
        //maak deze meteen ook met new aan, 10 groot graag
        RelAdd[] pathplus = new RelAdd[] {

            new RelAdd() {x=0,z=0 },
            new RelAdd() {x=0,z=1 },
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=0,z=-1 },
            new RelAdd() {x=0,z=-1 },
            new RelAdd() {x=0,z=-1 },
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
        };
        
        //geef path (class variable) hier een waarde met new, even groot als pathplus! 
        //- gebruik pathplus.Length voor de grote!
        path = new GameObject[pathplus.Length];
        
        //nu maken we een for loop over pathplus 
        //zet binnen de forloop deze code neer:
        //- loop tot pathplus.Length!
        for (int i = 0; i < pathplus.Length; i++)
        {
            RelAdd step = pathplus[i];
            
            x += step.x * size;
            z += step.z * size;
            
            path[i] = Instantiate(tileModel, new Vector3(x, 0, z), Quaternion.identity);
            //path[i] = Instantiate(tileModel, new Vector3(i*2, 0, 0), Quaternion.identity);
            
        }
	
	
        //pak hier het eerste GameObject uit path
        GameObject enemyStart = path[0];
        GameObject enemyObj = Instantiate(pigModel,enemyStart.transform.position, Quaternion.identity);
        enemy = new Enemy(enemyObj);//deze werkt nog niet ga naar Enemy.cs en los de ??? op
        enemy.from = 0;
        enemy.to = 1;

        //pak hier het 5de GameObject uit path
        GameObject towerPlace = path[4];
        GameObject onTile = Instantiate(tileModel,   towerPlace.transform.position+new Vector3(0, 0, 2), Quaternion.identity);
        GameObject towerObj = Instantiate(towerModel, onTile.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        tower = new Tower(towerObj, 5, onTile);//deze werkt nog niet ga naar Tower.cs en los de ??? op
    }



    void Update()
    {
        MoveEnemy(enemy);
    }

    double GetDist(GameObject a, GameObject b)
    {
        float dx = a.transform.position.x - b.transform.position.x;
        float dy = a.transform.position.y - b.transform.position.y;
        float dz = a.transform.position.z - b.transform.position.z;


        float powered = (dx * dx) + (dy * dy) + (dz * dz);
        double dist = Math.Sqrt(powered);
        //Debug.Log(dist);
        return dist;
    }

    void MoveEnemy(Enemy enemy)
    {
        if (enemy.to >= path.Length)
        {
            return;
        }
        
        GameObject from = path[enemy.from];
        GameObject to = path[enemy.to];
        
        double dist = GetDist(enemy.obj, to);
        Debug.Log(dist);
        
        if (dist <= 0.03)
        {
            enemy.obj.transform.position = to.transform.position;
            enemy.from++;
            enemy.to++;
            Debug.Log("goto next!" + this.enemy.from);
        }
        
        float dx = to.transform.position.x - from.transform.position.x;
        float dy = to.transform.position.y - from.transform.position.y;
        float dz = to.transform.position.z - from.transform.position.z;

      //  Debug.Log(dx + " " + dy + " " + dz);
        enemy.obj.transform.position += new Vector3(dx, dy, dz) * Time.deltaTime;
    }
}