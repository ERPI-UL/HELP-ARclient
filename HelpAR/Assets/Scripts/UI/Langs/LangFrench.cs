using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LangFrench : Lang
{
    public LangFrench(): base()
    {
        code = "fr";
        translations.Add(LANG.HOME, "Accueil");
        translations.Add(LANG.QUIT, "Quitter");
        translations.Add(LANG.BACK, "Retour");
        translations.Add(LANG.DONE, "Termin�");
        translations.Add(LANG.CANCEL, "Annuler");
        translations.Add(LANG.CONNECT, "Se connecter");
        translations.Add(LANG.CONNECTED, "Connect�");
        translations.Add(LANG.DISCONNECT, "D�connexion");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE, "Votre code EasyConnect");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE_DESC, "Allez sur indico.lf2l.fr pour vous connecter");
        translations.Add(LANG.OPTIONS, "Options");
        translations.Add(LANG.LANGUAGE, "Langue");
        translations.Add(LANG.OTHER, "Autre");
        translations.Add(LANG.CLEAR_CACHE, "Nettoyer le cache");
        translations.Add(LANG.DISPLAY_CONSOLE, "Afficher la console");
        translations.Add(LANG.HIDE_CONSOLE, "Masquer la console");
        translations.Add(LANG.LOADING, "Chargement");
        translations.Add(LANG.LOADING_RESSOURCES, "Chargement des ressources");
        translations.Add(LANG.CONFIRMATION, "Confirmation");
        translations.Add(LANG.START_ACTIVITY, "Commencer l'activit� ?");
        translations.Add(LANG.START_ACTIVITY_DESC, "Voulez-vous commencer l'activit� ?");
        translations.Add(LANG.START, "Commencer");
        translations.Add(LANG.ACTIVITIES, "Activit�s");
        translations.Add(LANG.ACTIVITY_DONE, "Activit� termin�e");
        translations.Add(LANG.ACTIVITY_DONE_DESC, "L'activit� est termin�e, vous pouvez retourner � l'accueil");
        translations.Add(LANG.WORKPLACES, "Environnements");
        translations.Add(LANG.ANCHOR_SCANNING, "Scan des anches");
        translations.Add(LANG.ANCHOR_SCANNING_DESC, "Veuillez scanner l'ancre demand�e");
    }
}
