using UnityEngine;

#if WINDOWS_UWP
    using Windows.Perception.Spatial;
#endif

/// <summary>
/// Used to retreive the QRCode position from the MixedReality SDK and convert it to Unity's world coordinates
/// </summary>
public class SpatialGraphCoordinateSystem
{
#if WINDOWS_UWP
    private SpatialCoordinateSystem CoordinateSystem = null;
#endif
    private System.Guid id;
    public System.Guid Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
            createCoordinates();
        }
    }

    private Pose pose;
    public Pose CurrentPose
    {
        get { return pose; }
    }

    private void createCoordinates()
    {
#if WINDOWS_UWP
        CoordinateSystem = Windows.Perception.Spatial.Preview.SpatialGraphInteropPreview.CreateCoordinateSystemForNode(id);
#endif
    }

    /// <summary>
    /// All this class code can be found et Microsoft's github, the only lines I changed are lines 58/59 replaced by line 60
    /// (This is the new correct way to get the QRCode's position from the SDK)
    /// </summary>
    private void UpdateLocation()
    {
        // #define WINDOWS_UWP
#if WINDOWS_UWP
        if (CoordinateSystem == null) this.createCoordinates();

        if (CoordinateSystem != null)
        {
            Quaternion rotation = Quaternion.identity;
            Vector3 translation = new Vector3(0.0f, 0.0f, 0.0f);
            
            // OLD // System.IntPtr rootCoordnateSystemPtr = UnityEngine.XR.WindowsMR.WindowsMREnvironment.OriginSpatialCoordinateSystem;
            // OLD // SpatialCoordinateSystem rootSpatialCoordinateSystem = (SpatialCoordinateSystem)System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(rootCoordnateSystemPtr);
            SpatialCoordinateSystem rootSpatialCoordinateSystem = SpatialLocator.GetDefault().CreateStationaryFrameOfReferenceAtCurrentLocation().CoordinateSystem;

            // Get the relative transform from the unity origin
            System.Numerics.Matrix4x4? relativePose = CoordinateSystem.TryGetTransformTo(rootSpatialCoordinateSystem);

            if (relativePose != null)
            {
                System.Numerics.Vector3 scale;
                System.Numerics.Quaternion rotation1;
                System.Numerics.Vector3 translation1;
       
                System.Numerics.Matrix4x4 newMatrix = relativePose.Value;

                // Platform coordinates are all right handed and unity uses left handed matrices. so we convert the matrix
                // from rhs-rhs to lhs-lhs 
                // Convert from right to left coordinate system
                newMatrix.M13 = -newMatrix.M13;
                newMatrix.M23 = -newMatrix.M23;
                newMatrix.M43 = -newMatrix.M43;

                newMatrix.M31 = -newMatrix.M31;
                newMatrix.M32 = -newMatrix.M32;
                newMatrix.M34 = -newMatrix.M34;

                System.Numerics.Matrix4x4.Decompose(newMatrix, out scale, out rotation1, out translation1);
                translation = new Vector3(translation1.X, translation1.Y, translation1.Z);
                rotation = new Quaternion(rotation1.X, rotation1.Y, rotation1.Z, rotation1.W);

                pose = new Pose(translation, rotation);

                // If there is a parent to the camera that means we are using teleport and we should not apply the teleport
                // to these objects so apply the inverse

                // THE QR CODE'S POSITION WAS SHIFTED, SO I SHIFT IT BACK TO THE ORIGINAL POSITION
                float shiftDistanceY = -0.071f;
                float shiftDistanceZ = -0.095f;

                Pose camPose = new Pose(
                    Camera.main.transform.position + Camera.main.transform.forward*shiftDistanceZ + Camera.main.transform.up*shiftDistanceY,
                    Camera.main.transform.rotation
                );
                camPose.rotation = Quaternion.Euler(0, camPose.rotation.eulerAngles.y, 0);
                pose = pose.GetTransformedBy(camPose);
            }
        }
        else
        {
            pose = new Pose(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        }
#endif
    }
    // Update is called once per frame
    public void calculate()
    {
        UpdateLocation();
    }
}
