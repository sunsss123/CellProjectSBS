using System.Collections;
using UnityEngine;

public class SwitchingCamera : MonoBehaviour
{
    private bool isTransitioning;

    public Camera Camera2D;
    public Camera Camera3D;

    private Camera ActiveCamera;
    [Header("2D 카메라 회전")]
    public Vector3 camrot2D;

    [Header("2D 카메라 위치")]
    public Vector3 camPos2D;
    [Header("2D 카메라 orthographic 사이즈")]
    public float orthographicSize2D = 5f;
    [Header("2D 카메라 near/far clipping planes")]
    public float nearClipPlane2D = -10f;
    public float farClipPlane2D = 10f;

    [Header("3D 카메라 회전")]
    public Vector3 camrot3D;
    [Header("3D 카메라 위치")]
    public Vector3 camPos3D;
    [Header("3D 카메라 field of view")]
    public float fieldOfView3D = 60f;
    [Header("3D 카메라 near/far clipping planes")]
    public float nearClipPlane3D = 0.1f;
    public float farClipPlane3D = 1000f;

    [Header("2D할지 3D할지 결정하는거 (Play mode 중에는 건들지 말기)")]
    public bool is2D;

    private Vector3 camPos;
    private Transform target;
    [Header("카메라 추격 시간")]
    public float CameraTrackingTime;
    [Header("카메라 2D/3D 전환 시간")]
    public float CameraChangeDuration = 1.0f;

    private bool ZPin;

    private float cameraspeed;
    private Vector3 CalculateVector;

    void Start()
    {
        Apply2DSettings();
        Apply3DSettings();
        Camera3D.transform.rotation=Camera2D.transform.rotation;
        SwapActiveCamera();
        PlayerStat.instance.Trans3D = !is2D;
        PlayerHandler.instance.RegisterChange3DEvent(StartChangeCameraCorutine);
    }
    void calculateCameraVector()
    {
        CameraTrackingTime = ProjectSetting.instance.CameraTrackingTime;
        float cameraVector = ((target.position + camPos) - ActiveCamera.transform.position).magnitude;
        cameraspeed = cameraVector / CameraTrackingTime;

        camPos = is2D ? camPos2D : camPos3D;

        CalculateVector = !ZPin ? target.position + camPos : (Vector3)((Vector2)target.position + (Vector2)camPos) + Vector3.forward * ActiveCamera.transform.position.z;
    }
    private void FixedUpdate()
    {
        Apply2DSettings();
        Apply3DSettings();
        if (PlayerHandler.instance.CurrentPlayer != null)
        {
            target = PlayerHandler.instance.CurrentPlayer.transform;
        }
        if (target == null || isTransitioning)
            return;
        if (!is2D)
            Camera2D.transform.position = Camera3D.transform.position;
        else
        {
            Camera3D.transform.position = Camera2D.transform.position;
        }
        calculateCameraVector();

        ActiveCamera.transform.position = Vector3.Lerp(ActiveCamera.transform.position, CalculateVector, Time.deltaTime * cameraspeed);
        //RotateCameraTowardsPlayerDirection();
    }

    public void ActiveZPin(float f)
    {
        ZPin = true;
        ActiveCamera.transform.position = new Vector3(ActiveCamera.transform.position.x, ActiveCamera.transform.position.y, f);
    }

    public void DeactiveZPin()
    {
        ZPin = false;
    }
    public void StartChangeCameraCorutine()
    {
        if ( !isTransitioning)
        {
            StartCoroutine(SwitchCameraMode());
        }
    }
  

    IEnumerator SwitchCameraMode()
    {
        
        calculateCameraVector();

        ActiveCamera.transform.position = CalculateVector;

        if (!is2D)
            Camera2D.transform.position = Camera3D.transform.position;
        else
        {
            Camera3D.transform.position = Camera2D.transform.position;
        }
        isTransitioning = true;
        Time.timeScale = 0;
        is2D = !is2D;
        

        if (is2D)
        {
            // 3D에서 2D로 전환
            yield return StartCoroutine(TransitionCamera(camPos2D, camrot2D, true));
           
        }
        else
        {
            // 2D에서 3D로 전환
            yield return StartCoroutine(TransitionCamera(camPos3D, camrot3D, false));
        
        }

        Time.timeScale = 1.0f;
        isTransitioning = false;
    }

    void SwapActiveCamera()
    {
        if (is2D)
        {
            Camera2D.transform.position = Camera3D.transform.position;
            Camera3D.enabled = false;
            Camera2D.enabled = true;
          
            ActiveCamera = Camera2D;
        }
        else
        {
            Camera3D.transform.position = Camera2D.transform.position;
            Camera2D.enabled = false;
            Camera3D.enabled = true;
            ActiveCamera = Camera3D;
        }
        PlayerStat.instance.Trans3D = !is2D;
    }

    IEnumerator TransitionCamera(Vector3 newPos, Vector3 newRot, bool isOrtho)
    {
        if(!is2D)
            SwapActiveCamera();

        float elapsed = 0.0f;

        Vector3 startingPos = ActiveCamera.transform.position;
        Quaternion startingRot = ActiveCamera.transform.rotation;

        Vector3 targetPosition = target.position + newPos;

        while (elapsed < CameraChangeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / CameraChangeDuration);

            ActiveCamera.transform.position = Vector3.Lerp(startingPos, targetPosition, t);
            ActiveCamera.transform.rotation = Quaternion.Lerp(startingRot, Quaternion.Euler(newRot), t);

            yield return null;
        }

        ZPin = isOrtho;
        if(is2D)
            SwapActiveCamera();
        if (isOrtho)
        {
            Vector3 finalPos = ActiveCamera.transform.position;
            finalPos.z = target.position.z + camPos2D.z;
            ActiveCamera.transform.position = finalPos;
        }
    }

    private void Apply2DSettings()
    {
        //Camera2D.orthographicSize = orthographicSize2D;
        //Camera2D.nearClipPlane = nearClipPlane2D;
        //Camera2D.farClipPlane = farClipPlane2D;
        //Camera2D.orthographic = true;
        Camera2D.transform.rotation = Quaternion.Euler(camrot2D);
    }

    private void Apply3DSettings()
    {
        Camera3D.fieldOfView = fieldOfView3D;
        Camera3D.nearClipPlane = nearClipPlane3D;
        Camera3D.farClipPlane = farClipPlane3D;
        Camera3D.orthographic = false;
        if(!is2D)
        Camera3D.transform.rotation = Quaternion.Euler(camrot3D);
    }
    private void RotateCameraTowardsPlayerDirection()
    {
        if (!is2D)
        {
            float hori = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");

   
            if (hori != 0 || vert != 0)
            {
               
                Vector3 direction = new Vector3(hori, 0, vert);
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                ActiveCamera.transform.rotation = Quaternion.Lerp(ActiveCamera.transform.rotation, targetRotation, Time.deltaTime * cameraspeed);
            }
            else
            {

                ActiveCamera.transform.rotation = Quaternion.Lerp(
                    ActiveCamera.transform.rotation,
                    Quaternion.Euler(camrot3D),
                    Time.deltaTime * cameraspeed);
            }
        }
    }
}