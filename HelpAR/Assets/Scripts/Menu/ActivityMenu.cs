using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivityMenu : MonoBehaviour
{
    [SerializeField] GameObject list;
    [SerializeField] GameObject loading;
    [SerializeField] GameObject activityCard;
    [SerializeField] TMPro.TextMeshPro activityTitle;
    [SerializeField] TMPro.TextMeshPro activityDesc;
    [SerializeField] private Pagination pagination;

    private ActivityPagination actPagination;
    public UnityEvent onSelected;
    public UnityEvent onLoaded;

    // Start is called before the first frame update
    public void init()
    {
        actPagination = new ActivityPagination(3, AimXRManager.Instance.GetWorkplaceId());
        nextPage();
    }

    public async void nextPage()
    {
        bool exists = await actPagination.LoadNextPage();
        if (exists) updateActivities();
        updatePagination();
    }

    public async void prevPage()
    {
        bool exists = await actPagination.LoadPreviousPage();
        if (exists) updateActivities();
        updatePagination();
    }

    public void updatePagination()
    {
        pagination.setPageTotal(actPagination.GetCurrentPage().GetPages());
        pagination.setPageNumber(actPagination.GetCurrentPage().GetPage());
    }

    void updateActivities()
    {
        // clear the shown workplaces
        for (int i = 0; i < list.transform.childCount; i++)
        {
            Destroy(list.transform.GetChild(i).gameObject);
        }

        const float CARD_SIZE = 0.05f;
        const float CARD_MARGIN = 0.01f;
        float shift = 0f;
        Page<ActivityShort> page = actPagination.GetCurrentPage();
        page.GetItems().ForEach(activity =>
        {
            GameObject obj = Instantiate(activityCard, list.transform);
            obj.transform.localPosition = new Vector3(0f, shift, 0f);
            TextButton btn = obj.GetComponent<TextButton>();
            if (btn)
            {
                btn.setLabel(activity.GetName());
                btn.onRelease.AddListener(() =>
                {
                    selectActivity(activity.GetId());
                });
            }
            shift -= CARD_SIZE + CARD_MARGIN;
        });
    }

    public async void selectActivity(int id)
    {
        onSelected.Invoke();
        activityTitle.text = "Chargement";
        activityDesc.text = "Chargement de l'activité en cours, veuillez patienter";

        // display loading
        loading.SetActive(true);

        // fetch informations
        try
        {
            Activity act = await DataManager.GetInstance().GetActivityAsync(id);

            // set informations
            AimXRManager.Instance.SetActivityId(id);
            activityTitle.text = act.GetName();
            activityDesc.text = act.GetDescription();
            onLoaded.Invoke();
        }
        catch (WorkplaceNotFoundException)
        {
            activityTitle.text = "Activité inconnue";
            activityDesc.text = "Cette activité n'existe pas :(";
        }
        catch (TimeoutException)
        {
            activityTitle.text = "Déconnecté";
            activityDesc.text = "Vous n'avez plus internet !";
        }

        loading.SetActive(false);
    }

    public void cancelActivity()
    {
        AimXRManager.Instance.SetActivityId(-1);
    }
}
