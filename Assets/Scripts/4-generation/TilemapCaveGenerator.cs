using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;


/**
 * This class demonstrates the CaveGenerator on a Tilemap.
 * 
 * By: Erel Segal-Halevi
 * Since: 2020-12
 */

public class TilemapCaveGenerator: MonoBehaviour {

    [SerializeField] SpriteRenderer Player = null;

    [SerializeField] Tilemap tilemap = null;

    [Tooltip("The tile that represents a wall (an impassable block)")]
    [SerializeField] TileBase wallTile = null;

    [Tooltip("The tile that represents a floor (a passable block)")]
    [SerializeField] TileBase floorTile = null;

    [SerializeField] TileBase exitFloor = null;

    [Tooltip("The percent of walls in the initial random map")]
    [Range(0, 1)]
    [SerializeField] float randomFillPercent = 0.5f;

    [Tooltip("Length and height of the grid")]
    [SerializeField] int gridSize = 50;

    [Tooltip("how precent to grow")]
    [SerializeField] int growMap = 10;





    private CaveGenerator caveGenerator;

    void Start()  {
        //To get the same random numbers each time we run the script
        Random.InitState(100);

        caveGenerator = new CaveGenerator(randomFillPercent, gridSize);
        caveGenerator.RandomizeMap();
                
        //For testing that init is working
        GenerateAndDisplayTexture(caveGenerator.GetMap());
            
        //Start the simulation
       
    }

    private void Update()
    {
        if(Player.transform.position.x>gridSize-1 && Player.transform.position.y > gridSize - 1)
        {
            
            float addgrid = (float)gridSize * ((float)growMap / 100);
            gridSize+=(int)addgrid;
            //Debug.Log("addgrid size is:   " + addgrid);
           // Debug.Log("grid size is:   "+gridSize);
            caveGenerator = new CaveGenerator(randomFillPercent, gridSize);
            //Calculate the new values
            caveGenerator.RandomizeMap();
            caveGenerator.SmoothMap(gridSize);

            //Generate texture and display it on the plane
            GenerateAndDisplayTexture(caveGenerator.GetMap());
            
            Player.transform.position = new Vector3(2.16f, 1.47f, 0);

        }
    }


    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateCavePattern()  {
        for (int i = 0; i < 21; i++)   {
            yield return new WaitForSeconds(1);

            //Calculate the new values
            caveGenerator.SmoothMap(gridSize);

            //Generate texture and display it on the plane
            GenerateAndDisplayTexture(caveGenerator.GetMap());
        }
        Debug.Log("Simulation completed!");
    }



    //Generate a black or white texture depending on if the pixel is cave or wall
    //Display the texture on a plane
    private void GenerateAndDisplayTexture(int[,] data) {
        for (int y = 0; y < gridSize; y++) {
            for (int x = 0; x < gridSize; x++) {
                var position = new Vector3Int(x, y, 0);
                var tile = data[x, y] == 1 ? wallTile: floorTile;
                tilemap.SetTile(position, tile);
            }
        }
        var endPos = new Vector3Int(gridSize - 1, gridSize - 1, 0);
        tilemap.SetTile(endPos, exitFloor);
    }
}
