using UnityEngine;

public class OcclusionCulling : MonoBehaviour
{
    [SerializeField]
    private LayerMask alwaysVisibleLayers; // Los layers que deseas mantener siempre visibles

    private Camera mainCamera;
    private Bounds cullingBounds;

    private void Start()
    {
        mainCamera = Camera.main;

        // Calcular los l�mites de culling basados en el frustum de la c�mara
        CalculateCullingBounds();
    }

    private void Update()
    {
        // Realizar culling de objetos
        PerformObjectCulling();
    }

    private void CalculateCullingBounds()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Calcular los l�mites de culling en funci�n del frustum de la c�mara
        cullingBounds = new Bounds(mainCamera.transform.position, Vector3.zero);
        foreach (Plane plane in frustumPlanes)
        {
            var planeRay = new Ray(plane.normal * plane.distance, plane.normal);
            if (planeRay.direction != Vector3.zero)
            {
                cullingBounds.Encapsulate(planeRay.GetPoint(-planeRay.origin.magnitude / planeRay.direction.magnitude));
                cullingBounds.Encapsulate(planeRay.GetPoint(planeRay.origin.magnitude / planeRay.direction.magnitude));
            }
        }
    }

    private void PerformObjectCulling()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Comprobar si el renderizador est� en los layers siempre visibles
            if ((alwaysVisibleLayers.value & (1 << renderer.gameObject.layer)) != 0)
            {
                // Si est� en los layers siempre visibles, mostrar el renderizador
                renderer.enabled = true;
                continue; // Saltar al siguiente renderizador
            }

            // Comprobar si el renderizador est� dentro de los l�mites de culling
            if (cullingBounds.Intersects(renderer.bounds))
            {
                // Si est� dentro de los l�mites, mostrar el renderizador
                renderer.enabled = true;
            }
            else
            {
                // Si est� fuera de los l�mites, ocultar el renderizador
                renderer.enabled = false;
            }
        }
    }
}
