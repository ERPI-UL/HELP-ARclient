using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.QR;
using UnityEngine;
using System.Threading.Tasks;
using LitJson;
using UnityEngine.Events;
using Unity.VisualScripting;

/// <summary>
/// Represents a QRCodeEvent (to process it in the Update function instead if the Event's callbacks)
/// </summary>
public struct QRCodeEvent
{
    /// <summary>
    /// Represents the different possible types of events
    /// </summary>
    public enum TYPE { ADDED, UPDATED, REMOVED }

    /// <summary>
    /// Event's type (see QRCodeEvent.TYPE for more)
    /// </summary>
    public TYPE type;
    /// <summary>
    /// Event's target QRCode object
    /// </summary>
    public QRCode code;
}

public class QRCodeDetection
{
    private static int DetectorCounter = 1;
    public UnityEvent<QRInfos> onQRAdded = new UnityEvent<QRInfos>();
    public UnityEvent<QRInfos> onQRUpdated = new UnityEvent<QRInfos>();
    public UnityEvent<QRInfos> onQRRemoved = new UnityEvent<QRInfos>();
    private List<QRInfos> qrCodes = new List<QRInfos>();
    private bool _shouldDestroy = false;
    public int id = 0;


    public QRCodeDetection()
    {
        id = DetectorCounter++;
    }

    public bool containsQR(string content)
    {
        return qrCodes.FindIndex(qr => qr.content == content) >= 0;
    }

    public void addQR(QRInfos infos)
    {
        qrCodes.Add(infos);
        onQRAdded.Invoke(infos);
    }

    public void removeQR(QRInfos infos)
    {
        int index = qrCodes.FindIndex(qr => qr.content == infos.content);
        if (index < 0) return;
        QRInfos qr = qrCodes[index];
        qrCodes.RemoveAt(index);
        onQRRemoved.Invoke(qr);
    }

    public void updateQR(QRInfos infos)
    {
        Debug.Log("Update QR of id " + infos.id + " on detection of id " + id + " (shouldDestroy=" + _shouldDestroy + ")");
        int index = qrCodes.FindIndex(qr => qr.content == infos.content);
        if (index < 0)
        {
            addQR(infos);
            index = qrCodes.FindIndex(qr => qr.content == infos.content);
        }

        qrCodes[index] = infos;
        onQRUpdated.Invoke(infos);
    }

    public void end()
    {
        _shouldDestroy = true;
    }

    public bool shouldDestroy()
    {
        return _shouldDestroy;
    }
}

/// <summary>
/// Used to take care of QRCode detection and event handling
/// </summary>
public class QRCodesManager : MonoBehaviour
{
    public static QRCodesManager Instance { get; private set; }

    /// <summary>
    /// QR Code tracker (from Microsoft's MixedReality SDK)
    /// </summary>
    private QRCodeWatcher qrTracker = null;
    /// <summary>
    /// Is the QR Code detection ready to operate
    /// </summary>
    private bool ready = false;
    /// <summary>
    /// Has the QR Code detection started to intitialize
    /// </summary>
    private bool started = false;
    /// <summary>
    /// Had the QR Code starting process an error
    /// </summary>
    private bool error = false;

    /// <summary>
    /// Events to process, sent by the qrTracker
    /// </summary>
    public Queue<QRCodeEvent> events = new Queue<QRCodeEvent>();

    /// <summary>
    /// Number of QRCode positions to retreive before calling the onDetected event
    /// </summary>
    public int QR_POSITIONS_COUNT = 4;
    
    /// <summary>
    /// List of positions of the detected QRCode (to do an average and be more precise)
    /// </summary>
    private List<Pose> QRPositions = new List<Pose>(); // TODO: use it in tracking

    /// <summary>
    /// List of detection objects for QRCode events
    /// </summary>
    private List<QRCodeDetection> detections = new List<QRCodeDetection>();

    /// <summary>
    /// Shows a debug message on the UI using the OnScreen script
    /// </summary>
    /// <param name="str"></param> message to show
    private void debugLog(string str)
    {
        Debug.Log(str);
    }

    /// <summary>
    /// Request to the HoloLens the ability to track QR Codes (Asks for camera access)
    /// </summary>
    /// <returns></returns>
    private async Task requestTracking()
    {
        debugLog("Requesting QR Code tracking ...");
        QRCodeWatcherAccessStatus status = await QRCodeWatcher.RequestAccessAsync();
        ready = status == QRCodeWatcherAccessStatus.Allowed;
        if (!ready) error = true;
        debugLog("Requesting QR Code tracking ... " + (status == QRCodeWatcherAccessStatus.Allowed? "Granted" : "Refused"));
    }

    /// <summary>
    /// Created the QRTracker object and registers the Added/Updated/Removed events to it
    /// </summary>
    private void SetupQRTracking()
    {
        debugLog("Doing QR Tracking setup ...");
        try
        {
            started = false;
            qrTracker = new QRCodeWatcher();
            qrTracker.Added += QRCodeWatcher_Added;
            qrTracker.Updated += QRCodeWatcher_Updated;
            qrTracker.Removed += QRCodeWatcher_Removed;
            qrTracker.EnumerationCompleted += QRCodeWatcher_EnumerationCompleted;
        }
        catch (Exception e)
        {
            error = true;
            debugLog("Error during Setup QR Tracking : " + e.ToString());
        }
        debugLog("Doing QR Tracking setup ... DONE");
    }

    /// <summary>
    /// If the QRTracker is not created, creates the QRTracker and if teh QRTracker isn't started yet, starts it.
    /// </summary>
    public void StartQRTracking()
    {
        debugLog("Starting QR Tracking ...");
        if (qrTracker == null)
        {
            SetupQRTracking();
            return;
        }
        if (!started)
        {
            try
            {
                qrTracker.Start();
                started = true;
            }
            catch (Exception e)
            {
                error = true;
                debugLog("Error during Start QR Tracking : " + e.ToString());
            }
        }
        debugLog("Starting QR Tracking ... DONE");
    }

    /// <summary>
    /// Asks the QRTracker to stop tracking
    /// </summary>
    public void StopQRTracking()
    {
        debugLog("Stopping QR Tracking ...");
        if (started)
        {
            started = false;
            if (qrTracker != null)
            {
                qrTracker.Stop();
            }
        }
        debugLog("Stopping QR Tracking ... DONE");
    }

    /// <summary>
    /// Event listener for QRCode Removed's event
    /// </summary>
    /// <param name="sender">event sender</param>
    /// <param name="args">event arguments (such as the targeted qr code)</param>
    private void QRCodeWatcher_Removed(object sender, QRCodeRemovedEventArgs args)
    {
        events.Enqueue(new QRCodeEvent() { code = args.Code, type = QRCodeEvent.TYPE.REMOVED });
    }

    /// <summary>
    /// Event listener for QRCode Updated's event
    /// </summary>
    /// <param name="sender">event sender</param>
    /// <param name="args">event arguments (such as the targeted qr code)</param>
    private void QRCodeWatcher_Updated(object sender, QRCodeUpdatedEventArgs args)
    {
        events.Enqueue(new QRCodeEvent() { code = args.Code, type = QRCodeEvent.TYPE.UPDATED});
    }

    /// <summary>
    /// Event listener for QRCode Added's event
    /// </summary>
    /// <param name="sender">event sender</param>
    /// <param name="args">event arguments (such as the targeted qr code)</param>
    private void QRCodeWatcher_Added(object sender, QRCodeAddedEventArgs args)
    {
        events.Enqueue(new QRCodeEvent() { code = args.Code, type = QRCodeEvent.TYPE.ADDED});
    }

    /// <summary>
    /// Event listener for QRCode EnumerationCompleted's event
    /// </summary>
    /// <param name="sender">event sender</param>
    /// <param name="e">Honestly I don't know what this parameter is ¯\_(ツ)_/¯</param>
    private void QRCodeWatcher_EnumerationCompleted(object sender, object e) { }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    /// <summary>
    /// Check if QRCode tracking is supported, and if so it calls the requestTracking method
    /// to start the QRCode tracker setup
    /// </summary>
    private async void Start()
    {
        // quick reminder : flags WINDOWS_UWP and ENABLE_WINMD_SUPPORT enabled when compiling on the HoloLens
        debugLog("Start called, checking for QRCodeWatcher support ...");
        try {
            bool supported = QRCodeWatcher.IsSupported();
            if (!supported)
            {
                debugLog("Start called, checking for QRCodeWatcher support ... NOT SUPPORTED");
                return;
            }
            debugLog("Start called, checking for QRCodeWatcher support ... SUPPORTED");
            await requestTracking();
        } catch { error = true; }
    }

    /// <summary>
    /// Triggers StartQRTracking function if the QRCode Tracking isn't setup yet.
    /// If it's all setup, it processes the events stored in [events]
    /// </summary>
    void Update()
    {
        if (error) return;
        if (!ready) return;
        if (!started) StartQRTracking();

        while (events.Count > 0)
        {
            destroyDeletedDetections();
            QRCodeEvent ev = events.Dequeue();
            switch (ev.type)
            {
                case QRCodeEvent.TYPE.UPDATED:
                    debugLog("New event detected ["+ ev.code.Data +"] : UPDATED");
                    updateQRCode(ev.code);
                    break;
                case QRCodeEvent.TYPE.ADDED:
                    debugLog("New event detected ["+ ev.code.Data +"] : ADDED");
                    addQRCode(ev.code);
                    break;
                case QRCodeEvent.TYPE.REMOVED:
                    debugLog("New event detected ["+ ev.code.Data +"] : REMOVED");
                    removeQRCode(ev.code);
                    break;
                default: break;
            }
        }
    }

    void destroyDeletedDetections()
    {
        Debug.Log("Checking for detection deletion");
        for (int i = detections.Count - 1;  i >= 0; i--)
        {
            QRCodeDetection detect = detections[i];
            Debug.Log("Trying id=" + detect.id);
            if (detect != null)
            {
                Debug.Log("Not null ! shouldDestroy=" + detect.shouldDestroy());
                if (detect.shouldDestroy())
                {
                    Debug.Log("Deleting !");
                    detections.RemoveAt(i);
                }
            }
        }
        Debug.Log("Done");
    }

    void updateQRCode(QRCode code)
    {
        debugLog("UpdateQRCode with data [" + code.Data + "]");

        QRInfos infos = QRInfos.FromQRObject(code);
        if (infos == null) return; // Not an Indico QRCode 

        detections.ForEach(detect =>
        {
            if (!detect.containsQR(infos.content))
                detect.addQR(infos);
            else detect.updateQR(infos);
        });
    }

    void addQRCode(QRCode code)
    {
        debugLog("addQRCode with data [" + code.Data + "]");

        QRInfos infos = QRInfos.FromQRObject(code);
        if (infos == null) return; // Not an Indico QRCode 

        detections.ForEach(detect =>
        {
            if (!detect.containsQR(infos.content))
                detect.addQR(infos);
        });
    }

    void removeQRCode(QRCode code)
    {
        debugLog("removeQRCode with data [" + code.Data + "]");

        QRInfos infos = QRInfos.FromQRObject(code);
        if (infos == null) return; // Not an Indico QRCode 

        detections.ForEach(detect =>
        {
            if (detect.containsQR(infos.content))
                detect.removeQR(infos);
        });
    }

    public void triggerFakeDetection(QRInfos infos)
    {
        destroyDeletedDetections();
        detections.ForEach(detect =>
        {
            if (!detect.containsQR(infos.content))
                detect.addQR(infos);
            detect.updateQR(infos);
        });
    }
    
    public QRCodeDetection scanForQR()
    {
        QRCodeDetection detection = new QRCodeDetection();
        detections.Add(detection);
        return detection;
    }
}
