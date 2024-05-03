using UnityEngine;
using UnityEngine.EventSystems;

public class TrackCameraController : MonoBehaviour
{
    [SerializeField] public GameObject track;

    private readonly float minOrthographicSize = 800f;
    private readonly int nbOfZoomSteps = 3;
    private new Camera camera;
    private float maxOrthographicSize = float.MaxValue;
    private int screenWidth;
    private Vector3 targetPosition;
    private LineRenderer trackLineRenderer;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        trackLineRenderer = track.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        FitCameraToTrack();
    }

    private void Update()
    {
        if (!enabled) return;
        if (Screen.width != screenWidth)
        {
            screenWidth = Screen.width;
            LeaveRoomToUI();
        }

        HandleZoom();
    }

    public void FitCameraToTrack()
    {
        if (!trackLineRenderer || trackLineRenderer.positionCount <= 0)
            return;

        Bounds bounds = new(trackLineRenderer.GetPosition(0), Vector3.zero);
        for (var i = 0; i < trackLineRenderer.positionCount; i++) bounds.Encapsulate(trackLineRenderer.GetPosition(i));

        var boundsSize = bounds.size;
        var aspectRatio = camera.aspect;

        // Determine the orthographic size
        float orthographicSize;
        if (aspectRatio >= 1.0f) // Wide aspect ratio
            // For wider screens, use half the bounds width, adjusted for aspect ratio
            orthographicSize = boundsSize.x / 2f / aspectRatio;
        else // Tall aspect ratio
            // For taller screens, use half the bounds height
            orthographicSize = boundsSize.y / 2f;

        // Set the orthographic size
        camera.orthographicSize = orthographicSize;
        maxOrthographicSize = orthographicSize;

        // Set the camera position
        transform.position = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);

        // Optionally, adjust the near and far clipping planes
        camera.nearClipPlane =
            -boundsSize.magnitude; // Using negative value as orthographic camera can view 'behind' its position
        camera.farClipPlane = boundsSize.magnitude;

        LeaveRoomToUI();
    }

    private void HandleZoom()
    {
        var scroll = 0;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            scroll = 1;
        if (Input.GetMouseButtonDown(1))
            scroll = -1;
        if (scroll == 0)
            return;

        camera.orthographicSize -= scroll * (maxOrthographicSize / nbOfZoomSteps);
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minOrthographicSize, maxOrthographicSize);

        switch (scroll)
        {
            case > 0:
            {
                Vector3 mousePos = new(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z);
                targetPosition = camera.ScreenToWorldPoint(mousePos);
                break;
            }
            case < 0:
                targetPosition = transform.position;
                break;
        }

        // Move the camera towards the target position
        if (camera.orthographicSize >= maxOrthographicSize)
            FitCameraToTrack();
        else
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    private void LeaveRoomToUI()
    {
        var rect = camera.rect;
        rect.x = GameObject.FindWithTag("LeftPanel").GetComponent<RectTransform>().sizeDelta.x / Screen.width;
        camera.rect = rect;
    }
}