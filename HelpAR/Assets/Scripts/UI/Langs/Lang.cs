using AimXRToolkit.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum LANG
{
    HOME,
    QUIT,
    BACK,
    DONE,
    CANCEL,

    CONNECT,
    CONNECTED,
    DISCONNECT,
    YOUR_EASYCONNECT_CODE,
    YOUR_EASYCONNECT_CODE_DESC,

    OPTIONS,
    LANGUAGE,
    OTHER,
    CLEAR_CACHE,
    DISPLAY_CONSOLE,
    HIDE_CONSOLE,

    LOADING,
    LOADING_RESSOURCES,

    CONFIRMATION,
    START_ACTIVITY,
    START_ACTIVITY_DESC,
    START,

    ACTIVITIES,
    ACTIVITY_DONE,
    ACTIVITY_DONE_DESC,

    WORKPLACES,

    ANCHOR_SCANNING,
    ANCHOR_SCANNING_DESC
}

public abstract class Lang
{
    public static readonly Dictionary<string, Lang> Langs = new Dictionary<string, Lang>
    {
        { "en", new LangEnglish() },
        { "fr", new LangFrench() },
        { "de", new LangGerman() },
        { "es", new LangSpanish() },
        { "it", new LangItalian() }
    };
    public static readonly Dictionary<string, string> Labels = new Dictionary<string, string>
    {
        { "en", "English" },
        { "fr", "Français" },
        { "de", "Deutsch" },
        { "es", "Español" },
        { "it", "Italiano" }
    };

    private static Lang _instance;
    public static Lang Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LangFrench();
            return _instance;
        }
        protected set
        {
            _instance = value;
        }
    }
    protected static List<Action> translationsCallbacks = new List<Action>();

    protected Dictionary<LANG, string> translations = new Dictionary<LANG, string>();
    protected string code = null;

    public void apply()
    {
        Instance = this;
        updateTranslations();
    }

    protected void updateTranslations()
    {
        if (AimXRManager.Instance.GetUser() != null)
            AimXRManager.Instance.GetUser().language = Instance.code;
        translationsCallbacks.ForEach(callback => callback());
    }

    public string getString(LANG key)
    {
        return translations.GetValueOrDefault(key, "- - - -");
    }

    public void registerTranslation(Action callback)
    {
        if (callback == null) return;
        translationsCallbacks.Add(callback);
        callback();
    }

    public string getCode()
    {
        return code;
    }
}
