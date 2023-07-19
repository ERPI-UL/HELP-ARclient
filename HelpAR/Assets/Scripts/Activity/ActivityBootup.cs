using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEditor.Experimental;

public class ActivityBootup : MonoBehaviour
{
    [SerializeField] private int defaultWorkplaceId;
    [SerializeField] private int defaultActivityId;
    [SerializeField] private ActivityManager activityManager;
    [SerializeField] private WorkPlaceManager workplaceManager;
    [SerializeField] private GameObject scene;
    [SerializeField] private GameObject qrcode;
    [SerializeField] private ActivityLoading loading;
    [SerializeField] private UnityEvent onBooted;

    void Start()
    {
        if (AimXRManager.Instance.GetUser() != null) init();
    }

    // Start is called before the first frame update
    public async void init()
    {
        if (AimXRManager.Instance.GetWorkplaceId() == 0)
        {
            AimXRManager.Instance.SetWorkplaceId(defaultWorkplaceId);
            AimXRManager.Instance.setWorkplacePose(new Pose(new Vector3(0f, 1.65f, 0f), Quaternion.Euler(0f, 0f, 0f)));
        }

        if (AimXRManager.Instance.GetActivityId() == 0)
            AimXRManager.Instance.SetActivityId(defaultActivityId);

        loading.setLoading(true);
        await loadWorkplace();
        await loadActivity();
        loading.setTitle("Chargement terminé");
        loading.setLabel("");
        loading.setLoading(false);
        await Task.Delay(2000);
        loading.gameObject.SetActive(false);
        onBooted.Invoke();
    }

    private async Task loadWorkplace()
    {
        loading.setLabel("Environnement");

        // load the artifacts
        Workplace workplace = await DataManager.GetInstance().GetWorkplaceAsync(AimXRManager.Instance.GetWorkplaceId());
        workplaceManager.SetWorkplace(workplace);

        GameObject obj_workplace = new GameObject();
        obj_workplace.transform.SetParent(scene.transform);
        obj_workplace.name = "Workplace";

        // add the qr code
        GameObject qr = Instantiate(qrcode, obj_workplace.transform);
        qr.transform.localPosition = Utils.toUnityCoord(workplace.GetAnchor().position);
        qr.transform.localRotation = Utils.toUnityAngle(workplace.GetAnchor().rotation);
        qr.name = "Anchor";
        qr.transform.SetParent(scene.transform);
        obj_workplace.transform.SetParent(qr.transform);

        // place the qr code at the scanned anchor position
        Pose anchor_pose = AimXRManager.Instance.getWorkplacePose();
        qr.transform.position = anchor_pose.position;
        qr.transform.rotation = anchor_pose.rotation;

        loading.setLabel("Artéfact");
        int index = 1;
        List<ArtifactInstance> artifacts = workplace.GetArtifacts();
        artifacts.ForEach(async instance =>
        {
            loading.setLabel("Artéfact " + index + "/" + artifacts.Count);

            // fetch and add model to the qr code
            GameObject obj_artifact = await workplaceManager.SpawnArtifact(instance);
            if (obj_artifact == null) return;
            ArtifactManager manager = obj_artifact.GetComponent<ArtifactManager>();

            GameObject obj_origin = new GameObject();
            obj_origin.transform.SetParent(obj_workplace.transform);
            obj_origin.transform.localPosition = Utils.toUnityCoord(instance.position);
            obj_origin.transform.localRotation = Utils.toUnityAngle(instance.rotation);
            obj_origin.name = "Instance " + index + " (" + manager.GetArtifact().GetName() + ")";

            // add the qr code
            GameObject qr = Instantiate(qrcode, obj_origin.transform);
            qr.transform.localPosition = Utils.toUnityCoord(manager.GetArtifact().GetAnchor().position);
            qr.transform.localRotation = Utils.toUnityAngle(manager.GetArtifact().GetAnchor().rotation);
            qr.name = "Anchor";

            obj_artifact.transform.SetParent(obj_origin.transform);
            obj_artifact.transform.localPosition = Vector3.zero;
            obj_artifact.transform.localRotation = Quaternion.identity;
            obj_artifact.transform.SetParent(qr.transform);

            Debug.Log("Registering instance " + instance.id + " for artifact " + manager.GetArtifact().GetName());
            workplaceManager.setArtifactInstance(instance.id, manager);
        });
    }

    private async Task loadActivity()
    {
        loading.setLabel("Activité");
        Activity activity = await DataManager.GetInstance().GetActivityAsync(AimXRManager.Instance.GetActivityId());
        activityManager.SetActivity(activity);

        loading.setLabel("Actions");
        List<Action> actions = new List<Action>();
        int actionId = activity.GetStart();
        int index = 1;
        while (actionId != 0)
        {
            loading.setLabel("Action " + index);
            Action a = await DataManager.GetInstance().GetActionAsync(actionId);
            actions.Add(a);
            actionId = a.GetNext();
        }
    }

    public void startActivity()
    {
        activityManager.NextAction();
    }

    public void NextAction()
    {
        activityManager.NextAction();
    }

    public void PreviousAction()
    {
        activityManager.PreviousAction();
    }
}
