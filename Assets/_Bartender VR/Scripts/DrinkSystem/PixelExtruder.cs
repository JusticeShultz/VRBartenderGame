using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PixelExtruder : MonoBehaviour
{
    public Material pixelMat;

    [ReadOnlyField]
    public Texture pixelTex;

    public float height;

    public float scale;

   
    private MeshFilter meshFilter;

    private Mesh myMesh;

    [ReadOnlyField]
    public Vector2 texScale;

    


    private List<Vector3> myVerts = new List<Vector3>();
    private List<int> myTris = new List<int>();



    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        myMesh = new Mesh();
        myMesh.name = "shitMesh";
        meshFilter.mesh = myMesh;


        pixelTex = pixelMat.mainTexture;
        texScale = new Vector2(pixelTex.width, pixelTex.height);

        InitializeMesh();

        MeshToFile("Assets/TestShit.obj");
    }






    void Update()
    {
        
    }

    void InitializeMesh()
    {
        myVerts.Clear();
        myTris.Clear();

        transform.position = new Vector3(-texScale.y * scale / 2.0f, 0, -texScale.y * scale / 2.0f);

        for (int h = 0; h < texScale.y + 1; h++)
        {
            for (int w = 0; w < texScale.y + 1; w++)
            {
                myVerts.Add(new Vector3(w * scale, 0, h * scale));
            }
        }

        for (int h = 0; h < texScale.y; h++)
        {
            for (int w = 0; w < texScale.y; w++)
            {
                myTris.Add(w + (h * ((int)texScale.y + 1)));
                myTris.Add(w + (h * ((int)texScale.y + 1)) + (int)texScale.y + 2);
                myTris.Add(w + (h * ((int)texScale.y + 1)) + 1);

                myTris.Add(w + (h * ((int)texScale.y + 1)));
                myTris.Add(w + (h * ((int)texScale.y + 1)) + (int)texScale.y + 1);
                myTris.Add(w + (h * ((int)texScale.y + 1)) + (int)texScale.y + 2);
            }
        }

        myMesh.Clear();
        myMesh.vertices = myVerts.ToArray();
        myMesh.triangles = myTris.ToArray();

        myMesh.RecalculateNormals();
    }


    public  string MeshToString()
    {
        Mesh m = myMesh;
        Material[] mats = new Material[] { pixelMat };

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(myMesh.name).Append("\n");
        foreach (Vector3 v in m.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
           sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }
        return sb.ToString();
    }

    public  void MeshToFile(string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(MeshToString());
        }
    }


}
