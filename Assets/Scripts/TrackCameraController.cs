using UnityEngine;
using UnityEngine.EventSystems;

public class TrackCameraController : MonoBehaviour
{
    // Add "padding" to the camera area, so the track is not hugging borders
    private const float MaxOrthographicSizeOffset = 100f;
    private const float MinOrthographicSize = 800f;
    private const int NbOfZoomSteps = 3;
    [SerializeField] public GameObject track;

    private Camera _camera;
    private float _maxOrthographicSize = float.MaxValue;
    private int _screenWidth;
    private Vector3 _targetPosition;
    private LineRenderer _trackLineRenderer;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _trackLineRenderer = track.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        FitCameraToTrack();
    }

    private void Update()
    {
        if (!enabled) return;
        if (Screen.width != _screenWidth)
        {
            _screenWidth = Screen.width;
            LeaveRoomToUI();
        }

        HandleZoom();
    }

    public void FitCameraToTrack()
    {
        if (!_trackLineRenderer || _trackLineRenderer.positionCount <= 0)
            return;

        Bounds bounds = new(_trackLineRenderer.GetPosition(0), Vector3.zero);
        for (var i = 0; i < _trackLineRenderer.positionCount; i++)
            bounds.Encapsulate(_trackLineRenderer.GetPosition(i));

        var boundsSize = bounds.size;
        var aspectRatio = _camera.aspect;

        // Determine the orthographic size
        float orthographicSize;
        if (aspectRatio >= 1.0f) // Wide aspect ratio
            // For wider screens, use half the bounds width, adjusted for aspect ratio
            orthographicSize = boundsSize.x / 2f / aspectRatio;
        else // Tall aspect ratio
            // For taller screens, use half the bounds height
            orthographicSize = boundsSize.y / 2f;

        orthographicSize += MaxOrthographicSizeOffset;

        // Set the orthographic size
        _camera.orthographicSize = orthographicSize;
        _maxOrthographicSize = orthographicSize;

        // Set the camera position
        transform.position = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);

        // Optionally, adjust the near and far clipping planes
        _camera.nearClipPlane =
            -boundsSize.magnitude; // Using negative value as orthographic camera can view 'behind' its position
        _camera.farClipPlane = boundsSize.magnitude;

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

        _camera.orthographicSize -= scroll * (_maxOrthographicSize / NbOfZoomSteps);
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, MinOrthographicSize, _maxOrthographicSize);

        switch (scroll)
        {
            case > 0:
            {
                Vector3 mousePos = new(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z);
                _targetPosition = _camera.ScreenToWorldPoint(mousePos);
                break;
            }
            case < 0:
                _targetPosition = transform.position;
                break;
        }

        // Move the camera towards the target position
        if (_camera.orthographicSize >= _maxOrthographicSize)
            FitCameraToTrack();
        else
            transform.position = new Vector3(_targetPosition.x, _targetPosition.y, transform.position.z);
    }

    private void LeaveRoomToUI()
    {
        var rect = _camera.rect;
        rect.x = GameObject.FindWithTag("LeftPanel").GetComponent<RectTransform>().sizeDelta.x / Screen.width;
        _camera.rect = rect;
    }
}