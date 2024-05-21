using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LangItalian : Lang
{
    public LangItalian(): base()
    {
        code = "it";
        translations.Add(LANG.HOME, "Home");
        translations.Add(LANG.QUIT, "Esci");
        translations.Add(LANG.BACK, "Indietro");
        translations.Add(LANG.DONE, "Fatto");
        translations.Add(LANG.CANCEL, "Annulla");
        translations.Add(LANG.CONNECT, "Connetti");
        translations.Add(LANG.CONNECTED, "Connesso");
        translations.Add(LANG.DISCONNECT, "Disconnetti");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE, "Codice EasyConnect");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE_DESC, "Vai su indico.lf2l.fr per loggare");
        translations.Add(LANG.OPTIONS, "Opzioni");
        translations.Add(LANG.LANGUAGE, "Lingua");
        translations.Add(LANG.OTHER, "Altro");
        translations.Add(LANG.CLEAR_CACHE, "Pulisci cache");
        translations.Add(LANG.DISPLAY_CONSOLE, "Mostra console");
        translations.Add(LANG.HIDE_CONSOLE, "Nascondi console");
        translations.Add(LANG.LOADING, "Caricamento");
        translations.Add(LANG.LOADING_RESSOURCES, "Caricamento risorse");
        translations.Add(LANG.CONFIRMATION, "Conferma");
        translations.Add(LANG.START_ACTIVITY, "avviare l'attività ?");
        translations.Add(LANG.START_ACTIVITY_DESC, "Vuoi iniziare l'attività ?");
        translations.Add(LANG.START, "Inizia");
        translations.Add(LANG.ACTIVITIES, "Attività");
        translations.Add(LANG.ACTIVITY_DONE, "Attività completata");
        translations.Add(LANG.ACTIVITY_DONE_DESC, "L'attività è ora finita, puoi tornare alla schermata principale");
        translations.Add(LANG.WORKPLACES, "Luoghi di lavoro");
        translations.Add(LANG.ANCHOR_SCANNING, "Scansione ancore");
        translations.Add(LANG.ARTIFACT_MARKER_SCANNING, "Scan artifact marker");
        translations.Add(LANG.ANCHOR_SCANNING_DESC, "Scansiona le ancore richieste per continuare");
    }
}
