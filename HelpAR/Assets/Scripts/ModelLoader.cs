using System;
using GLTFast;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class ModelLoader : AimXRToolkit.LoaderSource
{
    [SerializeField] private Material black_material;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override async Task<GameObject> LoadGlb(string uri)
    {
        Debug.Log("ModelLoader > Loading GLB from " + uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        TaskCompletionSource<UnityWebRequest> tcs = new TaskCompletionSource<UnityWebRequest>();
        www.SendWebRequest().completed += operation =>
        {
            tcs.SetResult(www);
        };
        Debug.Log("ModelLoader > Waiting for response ...");
        await tcs.Task;
        // get bytes
        var bytes = www.downloadHandler.data;
        ImportSettings importSettings = new ImportSettings()
        {
            NodeNameMethod = NameImportMethod.OriginalUnique
        };

        Debug.Log("ModelLoader > Loading binary ...");
        var gltf = new GltfImport();
        var success = await gltf.LoadGltfBinary(bytes, importSettings: importSettings);

        if (!success)
        {
            Debug.Log("ModelLoader > GLB loading failed !");
            return null;
        }
        else Debug.Log("ModelLoader > GLB loading success !");

        var o = new GameObject("Artifact");
        o.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        await gltf.InstantiateMainSceneAsync(o.transform);

        // on remonte dans la machine tout ce qui est dans la scene

        var scene = o.transform.GetChild(0);
        for (int i = scene.childCount - 1; i >= 0; i--)
        {
            var child = scene.GetChild(i);
            child.SetParent(o.transform);
        }

        // on remplace tous les shaders des objets par du noir
        setAllBlack(o.transform);

        // on supprime la scene
        Destroy(scene.gameObject);

        Debug.Log("ModelLoader > Loaded finished !");
        return o;
    }

    public void setAllBlack(Transform t)
    {
        if (t == null) return;

        MeshRenderer mrend = t.GetComponent<MeshRenderer>();
        if (mrend != null)
        {
            mrend.material = black_material;
        }

        for (int i = 0; i < t.childCount; i++)
        {
            var child = t.GetChild(i);
            setAllBlack(child);
        }
    }
}