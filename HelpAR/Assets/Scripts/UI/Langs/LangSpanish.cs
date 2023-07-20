using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LangSpanish : Lang
{
    public LangSpanish(): base()
    {
        code = "es";
        translations.Add(LANG.HOME, "Inicio");
        translations.Add(LANG.QUIT, "Dejar");
        translations.Add(LANG.BACK, "Volver");
        translations.Add(LANG.DONE, "Hecho");
        translations.Add(LANG.CANCEL, "Cancelar");
        translations.Add(LANG.CONNECT, "Conectar");
        translations.Add(LANG.CONNECTED, "Conectado");
        translations.Add(LANG.DISCONNECT, "Desconectar");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE, "Tu código EasyConnect");
        translations.Add(LANG.YOUR_EASYCONNECT_CODE_DESC, "Visita indico.lf2l.fr para iniciar sesión");
        translations.Add(LANG.OPTIONS, "Opciones");
        translations.Add(LANG.LANGUAGE, "Idioma");
        translations.Add(LANG.OTHER, "Mas");
        translations.Add(LANG.CLEAR_CACHE, "Eliminar caché");
        translations.Add(LANG.DISPLAY_CONSOLE, "Mostrar consola");
        translations.Add(LANG.HIDE_CONSOLE, "Ocultar consola");
        translations.Add(LANG.LOADING, "Cargando");
        translations.Add(LANG.LOADING_RESSOURCES, "Cargando recursos");
        translations.Add(LANG.CONFIRMATION, "Confirmación");
        translations.Add(LANG.START_ACTIVITY, "¿Iniciar actividad?");
        translations.Add(LANG.START_ACTIVITY_DESC, "Do you want to start the activity ?");
        translations.Add(LANG.START, "Start");
        translations.Add(LANG.ACTIVITIES, "Activities");
        translations.Add(LANG.ACTIVITY_DONE, "Activity finished");
        translations.Add(LANG.ACTIVITY_DONE_DESC, "La actividad ha terminado, puede volver a la pantalla de inicio");
        translations.Add(LANG.WORKPLACES, "Lugares de trabajo");
        translations.Add(LANG.ANCHOR_SCANNING, "Escanear anclas");
        translations.Add(LANG.ANCHOR_SCANNING_DESC, "Escanee las anclas solicitadas para continuar");
    }
}
