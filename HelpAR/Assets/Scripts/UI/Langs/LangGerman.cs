using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LangGerman : Lang
{
    public LangGerman(): base()
    {
        code = "de";
        translations.Add(LANG.HOME, "Hauptseite");
        translations.Add(LANG.QUIT, "Verlassen");
        translations.Add(LANG.BACK, "Zurück");
        translations.Add(LANG.DONE, "Erledigt");
        translations.Add(LANG.CANCEL, "Abbrechen");
        translations.Add(LANG.CONNECT, "Verbinden");
        translations.Add(LANG.CONNECTED, "Verbunden");
        translations.Add(LANG.DISCONNECT, "Trennen");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE, "Ihr EasyConnect-Code");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE_DESC, "Gehen Sie zu indico.lf2l.fr, um sich anzumelden");
        translations.Add(LANG.OPTIONS, "Optionen");
        translations.Add(LANG.LANGUAGE, "Sprache");
        translations.Add(LANG.OTHER, "Andere");
        translations.Add(LANG.CLEAR_CACHE, "Cache leeren");
        translations.Add(LANG.DISPLAY_CONSOLE, "Konsole anzeigen");
        translations.Add(LANG.HIDE_CONSOLE, "Konsole ausblenden");
        translations.Add(LANG.LOADING, "Laden");
        translations.Add(LANG.LOADING_RESSOURCES, "Laden der Ressourcen");
        translations.Add(LANG.CONFIRMATION, "Bestätigung");
        translations.Add(LANG.START_ACTIVITY, "Aktivität starten?");
        translations.Add(LANG.START_ACTIVITY_DESC, "Möchten Sie die Aktivität starten?");
        translations.Add(LANG.START, "Starten");
        translations.Add(LANG.ACTIVITIES, "Aktivitäten");
        translations.Add(LANG.ACTIVITY_DONE, "Aktivität abgeschlossen");
        translations.Add(LANG.ACTIVITY_DONE_DESC, "Die Aktivität ist beendet, du kannst zum Startbildschirm zurückkehren");
        translations.Add(LANG.WORKPLACES, "Arbeitsumgebungen");
        translations.Add(LANG.ANCHOR_SCANNING, "Anker-Scan");
        translations.Add(LANG.ARTIFACT_MARKER_SCANNING, "Scan artifact marker");
        translations.Add(LANG.ANCHOR_SCANNING_DESC, "Bitte den angeforderten Anker scannen");
    }
}
