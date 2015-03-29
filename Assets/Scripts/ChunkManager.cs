using UnityEngine;
using System.Collections;
using System.Linq;

using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    //load up the various types of chunk
    public GameObject sparseSmall;
    public GameObject sparseBig;
    public GameObject denseSmall;
    public GameObject denseBig;
    public GameObject startChunk;

    //use starting chunk?
    public bool useStart;

    //the player gameObject
    public GameObject Player;

    //how far the player has to be from a chunk before it unloads. Should be about 200
    public float maxDistance;

    //seed for generating random things
    private float seed;

    //a list of all chunks stored by their co-ordinates
    private Dictionary<string, Chunk> allChunks = new Dictionary<string, Chunk>();

    //childvar
    private Transform child;

    // Use this for initialization
    void Start()
    {
        //get seed from global seed
        seed = GlobalStore.Seed;

        //make first 9 chunbks around the player
        if (useStart)
        {
            allChunks.Add("0,0", new Chunk(0, 0, seed, startChunk,startChunk,startChunk,startChunk));
        }
        else
        {
            allChunks.Add("0,0", new Chunk(0, 0, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        }
        
        allChunks.Add("1,0", new Chunk(1, 0, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        allChunks.Add("0,1", new Chunk(0, 1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        allChunks.Add("1,1", new Chunk(1, 1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        allChunks.Add("0,-1", new Chunk(0, -1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        allChunks.Add("-1,0", new Chunk(-1, 0, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        allChunks.Add("1,-1", new Chunk(1, -1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        allChunks.Add("-1,1", new Chunk(-1, 1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
        allChunks.Add("-1,-1", new Chunk(-1, -1, seed, sparseSmall, sparseBig, denseSmall, denseBig));




    }
    //Chunk - COntainer for information about the block field in a 100 by 100 area
    private class Chunk : Object
    {
        //position in index space
        int xIndex;
        int yIndex;

        //The chunk we create that contains the blocks
        public GameObject field;

        //Is the chunk is process of being deleted?
        public bool deleting;

        //new chunk
        public Chunk(int xCoord, int yCoord, float seed, GameObject sparseSmall, GameObject sparseBig, GameObject denseSmall, GameObject denseBig)
        {


            //use the perlin noise function to determine what the denisty (blocks per chunk) and size (scale mulktipler) of the chunk should be
            float density = GlobalStuff.getDensity(IndexToWorld(xCoord), IndexToWorld(yCoord), seed);
            //Mathf.PerlinNoise (IndexToWorld (xCoord) / 1000f + seed, IndexToWorld (yCoord) / 1000f + seed);
            float size = GlobalStuff.getSize(IndexToWorld(xCoord), IndexToWorld(yCoord), seed);
            //Mathf.PerlinNoise (IndexToWorld (xCoord) / 1000f + (seed * 2f), IndexToWorld (yCoord) / 1000f + (seed * 2f));

            //print("desnity: " + density);
            //print("size: " + size);

            this.xIndex = xCoord;
            this.yIndex = yCoord;

            this.deleting = false;

            

                //combinations of size and density give different chunk types
                if (size < 0.5f & density < 0.5f)
                {

                    this.field = (GameObject)Instantiate(sparseSmall, new Vector3(IndexToWorld(xCoord), IndexToWorld(yCoord), 0f), Quaternion.identity);
                }
                if (size > 0.5f & density > 0.5f)
                {

                    this.field = (GameObject)Instantiate(denseBig, new Vector3(IndexToWorld(xCoord), IndexToWorld(yCoord), 0f), Quaternion.identity);
                }
                if (size < 0.5f & density > 0.5f)
                {

                    this.field = (GameObject)Instantiate(denseSmall, new Vector3(IndexToWorld(xCoord), IndexToWorld(yCoord), 0f), Quaternion.identity);
                }
                if (size > 0.5f & density < 0.5f)
                {

                    this.field = (GameObject)Instantiate(sparseBig, new Vector3(IndexToWorld(xCoord), IndexToWorld(yCoord), 0f), Quaternion.identity);
                }

            




        }



    }

    //indoex space in where a chunk takes up an area of size 1 x 1

    //Transform from index space to world space
    public static float IndexToWorld(int index)
    {

        return ((float)index) * 100f;

    }

    //Transform from world space to index space
    public static int WorldToIndex(float world)
    {

        float top = Mathf.Ceil(world / 100f);
        float bottom = Mathf.Floor(world / 100f);

        if (Mathf.Repeat(world, 100f) > 50f)
        {
            return (int)top;
        }
        else
        {
            return (int)bottom;
        }
        //return (int)(world / 100f);
    }



    // Update is called once per frame
    void Update()
    {

        //check if the 3 x 3 chunks around the player and load  ones that are missing

        int currentX = WorldToIndex(Player.transform.position.x);
        int currentY = WorldToIndex(Player.transform.position.y);
        for (int q = -1; q <= 1; q++)
        {
            for (int p = -1; p <= 1; p++)
            {
                string tempX = (currentX + q).ToString();
                string tempY = (currentY + p).ToString();
                
                if (allChunks.ContainsKey(tempX + "," + tempY) == false)
                {
                    if ((currentX +q == 0) && (currentY +p == 0) && useStart)
                    {
                        allChunks.Add(tempX + "," + tempY, new Chunk(currentX + q, currentY + p, seed, startChunk, startChunk, startChunk, startChunk));
                    }
                    else
                    {
                        allChunks.Add(tempX + "," + tempY, new Chunk(currentX + q, currentY + p, seed, sparseSmall, sparseBig, denseSmall, denseBig));
                        //print("MAKE1");
                    }

                }
            }
        }
        List<string> list = allChunks.Select(x => x.Key).ToList();


        //delete chunks that are too far away from teh player
        foreach (string entry in list)
        {
            if (Vector3.Distance(Player.transform.position, allChunks[entry].field.transform.position) > maxDistance && allChunks[entry].deleting == false)
            {

                allChunks[entry].deleting = true;
                //DestroyObject(allChunks[entry].field);
                
                //allChunks.Remove(entry);

            }
            if (allChunks[entry].deleting == true && allChunks[entry].field.transform.childCount != 0)
            {
                //print("smalldelete");
                child = allChunks[entry].field.transform.GetChild(0);
                Destroy(child.gameObject);
                
            }
            if (allChunks[entry].deleting == true && allChunks[entry].field.transform.childCount == 0)
            {
                //print("BIGdelete");
                Destroy(allChunks[entry].field);

                allChunks[entry].deleting = false;

                allChunks.Remove(entry);




            }
        }




    }
}