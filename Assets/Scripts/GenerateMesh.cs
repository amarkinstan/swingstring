using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public static class GenerateMesh {

 


    public static Vector3 AngleAndDistance(Vector3 origin, float length,float angle)
    {
        
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        return origin + q * Vector3.right * length;
        

    }

    public static List<Vector3> MakeShape(Vector3 origin, int numPoints, float minLength, float maxLength, float maxDeltaLength, float minDeltaLength)
    {
        float angleRange = 360f / (float)numPoints;

        List<Vector3> result = new List<Vector3>();

        float length = Random.Range(minLength,maxLength);

        float delta;
                
        for (int i = 0; i < numPoints; i++)
        {
            

            float currentAngle = angleRange * i;

           float angle = Random.Range(currentAngle - (angleRange / 2f), currentAngle + (angleRange / 2f));

           delta = Random.Range(minDeltaLength, maxDeltaLength);

           if (Random.value < 0.5f)
           {
               delta = -delta;
           }

           length += delta;

           if (length > maxLength)
           {
               length = Random.Range(maxLength-minDeltaLength, maxLength - maxDeltaLength);
           }
           if (length < minLength)
           {
               length = Random.Range(minLength + minDeltaLength, minLength + maxDeltaLength);
           }

            result.Add(GenerateMesh.AngleAndDistance(origin, length, angle));
                                       
        }

        return result;

    }

 

     public static Mesh CreateMesh(List<Vector3> shape, float width)
        {

            
         
            List<Vector3> vertexs =  ShapeToVerts(shape, width);

           
            List<int> triangles  = new List<int>();

            List<Vector3> normals = CreateNormals(vertexs.Count / 2);
            

            triangles.AddRange(CreateFaceTriangles(0, 1, shape.Count - 1, true));

            triangles.AddRange(CreateSideTriangles(1, shape.Count + 1, shape.Count - 1));
         
            triangles.AddRange(CreateFaceTriangles(shape.Count, shape.Count + 1, (shape.Count * 2) - 1, false));

            

            

            

            


            Mesh result = new Mesh();

            result.vertices = vertexs.ToArray();

            result.triangles = triangles.ToArray();

            result.normals = normals.ToArray();
            
            //result.RecalculateNormals();

            //result.RecalculateBounds();

            result.Optimize();

            return result;
        }

     public static List<Vector3> CreateNormals(int numPoints)
     {
         List<Vector3> normals = new List<Vector3>();

         for (int i = 0; i < numPoints; i++)
         {
             normals.Add(Vector3.forward);
         }

         for (int i = 0; i < numPoints; i++)
         {
             normals.Add(Vector3.back);
         }

         return normals;

     }

    

        public static List<int> CreateFaceTriangles (int centreIndex, int indexRangeStart, int indexRangeEnd, bool cw)
        {
            List<int> triangles = new List<int>();

            if (cw)
            {

                for (int index = indexRangeStart; index < indexRangeEnd; index++)
                {
                    triangles.Add(centreIndex);
                    triangles.Add(index);
                    triangles.Add(index + 1);

                }
                triangles.Add(centreIndex);
                triangles.Add(indexRangeEnd);
                triangles.Add(indexRangeStart);
            }
            else
            {
                for (int index = indexRangeStart; index < indexRangeEnd; index++)
                {
                    
                    
                    triangles.Add(index + 1);
                    triangles.Add(index);
                    triangles.Add(centreIndex);

                }
                
                
                triangles.Add(indexRangeStart);
                triangles.Add(indexRangeEnd);
                triangles.Add(centreIndex);
            }


            

            return triangles;

        }

        public static List<int> CreateSideTriangles(int startIndexFront, int startIndexBack, int faceVertCount)
        {
            List<int> triangles = new List<int>();
            List<int> frontFace = Enumerable.Range(startIndexFront, faceVertCount).ToList<int>();
            List<int> backFace = Enumerable.Range(startIndexBack, faceVertCount).ToList<int>();

            for (int index = 1; index < faceVertCount; index++)
            {
                triangles.Add(frontFace[index-1]);
                triangles.Add(backFace[index-1]);
                triangles.Add(frontFace[index]);

                triangles.Add(frontFace[index]);
                triangles.Add(backFace[index-1]);
                triangles.Add(backFace[index]);

            }

            triangles.Add(frontFace[faceVertCount - 1]);
            triangles.Add(backFace[faceVertCount - 1]);
            triangles.Add(frontFace[0]);

            triangles.Add(frontFace[0]);
            triangles.Add(backFace[faceVertCount - 1]);
            triangles.Add(backFace[0]);

            return triangles;
           
        }


        public static List<Vector3> ShapeToVerts(List<Vector3> shape, float width)
        {
            width = width/2f;

            shape.Insert(0, Vector3.zero);

            List<Vector3> frontList = GlobalStuff.DeepClone<List<Vector3>>(shape);

            for (int i = 0; i < frontList.Count; i++)
            {
                frontList[i] = new Vector3(frontList[i].x, frontList[i].y, width);
            }

            List<Vector3> backList = GlobalStuff.DeepClone<List<Vector3>>(shape);

            for (int i = 0; i < backList.Count; i++)
            {
                backList[i] = new Vector3(backList[i].x, backList[i].y, -width);
            }

            frontList.AddRange(backList);

           

            return frontList;

        }


        public static List<Vector3> RemoveConcave(List<Vector3> shape, Vector3 origin)
        {
            List<Vector3> result = GlobalStuff.DeepClone<List<Vector3>>(shape);

            float lowerPointDistance;

            float pointDistance;

            float higherPointDistance;

            for (int i = 0; i < shape.Count; i++)
            {
                pointDistance = Vector3.Distance(origin, shape[i]);

                if (i == 0)
                {
                    lowerPointDistance = Vector3.Distance(origin, shape[shape.Count - 1]);
                }
                else
                {
                    lowerPointDistance = Vector3.Distance(origin, shape[i - 1]);
                }

                if (i == shape.Count - 1)
                {
                    higherPointDistance = Vector3.Distance(origin, shape[0]);
                }
                else
                {
                    higherPointDistance = Vector3.Distance(origin, shape[i + 1]);
                }

                if (pointDistance < lowerPointDistance && pointDistance < higherPointDistance)
                {
                    result[i] = Vector3.zero;
                }

            }

            return result.Where(ele => ele != Vector3.zero).ToList();
            
        }
       


    }




