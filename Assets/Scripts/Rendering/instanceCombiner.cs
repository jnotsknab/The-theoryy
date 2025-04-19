//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class instanceCombiner : MonoBehaviour
//{
//    [SerializeField] private List<MeshFilter> meshFiltersList = new List<MeshFilter>();
//    [SerializeField] private MeshFilter targetMesh;

//    [ContextMenu("Combine Meshes")]

//    private void CombineMesh()
//    {
//        var combine = new CombineInstance[meshFiltersList.Count];

//        for (int i = 0; i < meshFiltersList.Count; i++)
//        {
//            combine[i].mesh = meshFiltersList[i].sharedMesh;
//            combine[i].transform = meshFiltersList[i].transform.localToWorldMatrix;
//        }

//        var mesh = new Mesh();
//        mesh.CombineMeshes(combine);

//        targetMesh.mesh = mesh;

//        transform.localScale = new Vector3(1, 1, 1);
//        transform.rotation = Quaternion.identity;
//        transform.position = Vector3.zero;
        


//        //Save mesh here
//        SaveMesh(targetMesh.sharedMesh, gameObject.name, false, true);
//        print($"<color=#20E7B0>Combine Meshes was Successful!</color>");

//    }
    
//    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
//    {
//        string path = EditorUtility.SaveFilePanel("Save seperate mesh asset", "Assets/", name, "asset");
//        if (string.IsNullOrEmpty(path)) return;

//        path = FileUtil.GetProjectRelativePath(path);

//        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh: mesh;

//        if (optimizeMesh)
//        {
//            MeshUtility.Optimize(meshToSave);
//        }

//        AssetDatabase.CreateAsset(meshToSave, path);
//        AssetDatabase.SaveAssets();
//    }
//}
