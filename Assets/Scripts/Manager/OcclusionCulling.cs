using UnityEngine;

public class OcclusionCulling : MonoBehaviour
{
    [SerializeField] private LayerMask alwaysVisibleLayers; // El layer que deseas mantener siempre visible   

    private void Update()
    {
        // Obt�n todos los renderizadores del objeto actual y sus hijos
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Comprueba si el renderizador est� en el layer siempre visible
            if (renderer.gameObject.layer == alwaysVisibleLayers)
            {
                // Mant�n el renderizador siempre visible
                renderer.enabled = true;
                continue; // Salta al siguiente renderizador
            }

            // Comprueba si el renderizador est� visible por la c�mara

            switch (renderer.isVisible)
            {
                case true:
                    renderer.enabled = true;
                    break;

                case false:
                    renderer.enabled = false;
                    break;

            }
        }
    }
}
