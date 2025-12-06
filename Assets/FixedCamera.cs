using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedCamera : MonoBehaviour
{
    public float referenceOrthographicSize = 5f;   // el tamaño que usas ahora en 16:9
    public float referenceAspect = 16f / 9f;       // aspecto base con el que has diseñado

    private Camera cam;
    private float lastAspect;

    void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateCameraSize();
    }

    void Update()
    {
        float aspect = (float)Screen.width / Screen.height;
        if (Mathf.Abs(aspect - lastAspect) > 0.01f)
        {
            UpdateCameraSize();
        }
    }

    void UpdateCameraSize()
    {
        float aspect = (float)Screen.width / Screen.height;
        lastAspect = aspect;

        // ancho de mundo que quieres mantener (el que ves en el editor en 16:9)
        float worldWidthAtReference = 2f * referenceOrthographicSize * referenceAspect;

        // calculamos el nuevo orthographicSize para este aspect
        cam.orthographicSize = worldWidthAtReference / (2f * aspect);
    }
}
