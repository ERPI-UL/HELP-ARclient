using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LangEnglish : Lang
{
    public LangEnglish(): base()
    {
        code = "en";
        translations.Add(LANG.HOME, "Home");
        translations.Add(LANG.QUIT, "Quit");
        translations.Add(LANG.BACK, "Back");
        translations.Add(LANG.DONE, "Done");
        translations.Add(LANG.CANCEL, "Cancel");
        translations.Add(LANG.CONNECT, "Connect");
        translations.Add(LANG.CONNECTED, "Connected");
        translations.Add(LANG.DISCONNECT, "Disconnect");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE, "Your EasyConnect code");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE_DESC, "Go to indico.lf2l.fr to log in");
        translations.Add(LANG.OPTIONS, "Options");
        translations.Add(LANG.LANGUAGE, "Language");
        translations.Add(LANG.OTHER, "More");
        translations.Add(LANG.CLEAR_CACHE, "Clear cache");
        translations.Add(LANG.DISPLAY_CONSOLE, "Display console");
        translations.Add(LANG.HIDE_CONSOLE, "Hide console");
        translations.Add(LANG.LOADING, "Loading");
        translations.Add(LANG.LOADING_RESSOURCES, "Loading ressources");
        translations.Add(LANG.CONFIRMATION, "Confirmation");
        translations.Add(LANG.START_ACTIVITY, "start activity ?");
        translations.Add(LANG.START_ACTIVITY_DESC, "Do you want to start the activity ?");
        translations.Add(LANG.START, "Start");
        translations.Add(LANG.ACTIVITIES, "Activities");
        translations.Add(LANG.ACTIVITY_DONE, "Activity finished");
        translations.Add(LANG.ACTIVITY_DONE_DESC, "The activity is now finished, you can go back to the home screen");
        translations.Add(LANG.WORKPLACES, "Workplaces");
        translations.Add(LANG.ANCHOR_SCANNING, "Scan markers");
        translations.Add(LANG.ARTIFACT_MARKER_SCANNING, "Scan artifact marker");
        translations.Add(LANG.ANCHOR_SCANNING_DESC, "Scan the asked markers to continue");
    }
}
