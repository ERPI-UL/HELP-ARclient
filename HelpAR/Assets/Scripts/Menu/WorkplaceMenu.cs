using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using System;
using UnityEngine;
using UnityEngine.Events;

enum WorkplaceMenuType
{
    SCAN_QR,
    SELECT_CARD
}

public class WorkplaceMenu : MonoBehaviour
{
    [SerializeField] GameObject list;
    [SerializeField] GameObject loading;
    [SerializeField] GameObject workplaceCard;
    [SerializeField] TMPro.TextMeshPro workplaceTitle;
    [SerializeField] TMPro.TextMeshPro workplaceDesc;
    [SerializeField] WorkplaceMenuType menuType;
    [SerializeField] private Pagination pagination;

    private WorkplacePagination workPagination;
    public UnityEvent onSelected;
    public UnityEvent onLoaded;

    // Start is called before the first frame update
    public async void init()
    {
        if (menuType == WorkplaceMenuType.SCAN_QR)
        {
            Debug.Log("AR workplace selected, showing panel 2");
            PanelManager pm = GetComponent<PanelManager>();
            if (!pm) Debug.Log("PanelManager cannot be found for workplace QR scan");
            else pm.showPanel(2);
        } else
        {
            workPagination = new WorkplacePagination(3);
            nextPage();
        }
    }

    public async void nextPage()
    {
        bool exists = await workPagination.LoadNextPage();
        if (exists) updateWorkplaces();
        updatePagination();
    }

    public async void prevPage()
    {
        bool exists = await workPagination.LoadPreviousPage();
        if (exists) updateWorkplaces();
        updatePagination();
    }

    public void updatePagination()
    {
        pagination.setPageTotal(workPagination.GetCurrentPage().GetPages());
        pagination.setPageNumber(workPagination.GetCurrentPage().GetPage());
    }

    void updateWorkplaces()
    {
        // clear the shown workplaces
        for (int i = 0; i < list.transform.childCount; i++)
        {
            Destroy(list.transform.GetChild(i).gameObject);
        }

        const float CARD_SIZE = 0.05f;
        const float CARD_MARGIN = 0.01f;
        float shift = 0f;
        Page<WorkplaceShort> page = workPagination.GetCurrentPage();
        page.GetItems().ForEach(workplace =>
        {
            GameObject obj = Instantiate(workplaceCard, list.transform);
            obj.transform.localPosition = new Vector3(0f, shift, 0f);
            TextButton btn = obj.GetComponent<TextButton>();
            if (btn)
            {
                btn.setLabel(workplace.GetName());
                btn.onRelease.AddListener(() =>
                {
                    selectWorkplace(workplace.GetId(), new Pose());
                });
            }
            shift -= CARD_SIZE + CARD_MARGIN;
        });
    }

    async public void selectWorkplace(int id, Pose p)
    {
        onSelected.Invoke();
        workplaceTitle.text = "Chargement";
        workplaceDesc.text = "Chargement du workplace en cours, veuillez patienter";

        // display loading
        loading.SetActive(true);
        
        // fetch informations
        try
        {
            Workplace wp = await DataManager.GetInstance().GetWorkplaceAsync(id);
            
            // set informations
            AimXRManager.Instance.SetWorkplaceId(id);
            AimXRManager.Instance.setWorkplacePose(p);
            workplaceTitle.text = wp.GetName();
            workplaceDesc.text = wp.GetDescription();
            onLoaded.Invoke();
        } catch (WorkplaceNotFoundException)
        {
            workplaceTitle.text = "Workplace inconnu";
            workplaceDesc.text = "Ce workplace n'existe pas :(";
        } catch (TimeoutException)
        {
            workplaceTitle.text = "Déconnecté";
            workplaceDesc.text = "Vous n'avez plus internet !";
        }

        loading.SetActive(false);
    }

    public void cancelWorkplace()
    {
        AimXRManager.Instance.SetWorkplaceId(-1);
        PanelManager pm = GetComponent<PanelManager>();
        if (pm != null) pm.showPanel(menuType == WorkplaceMenuType.SCAN_QR ? 2 : 0);
    }

    public void cancelScan()
    {
        IQRScanProvider provider = QRScanProvider.getProvider();
        if (provider == null)
        {
            Debug.LogError("Error : QR Scan Provider is null for [cancelScan]");
            return;
        }

        provider.cancelScan();
    }

    public void launchWorkplaceQRScan()
    {
        IQRScanProvider provider = QRScanProvider.getProvider();
        if (provider == null)
        {
            Debug.LogError("Error : QR Scan Provider is null for [launchWorkplaceQRScan]");
            return;
        }

        provider.beginUnknownQRScan(QRCodeType.Workplace, (infos) =>
        {
            selectWorkplace(infos.id, new Pose(infos.position, infos.rotation));
        });
    }
}
