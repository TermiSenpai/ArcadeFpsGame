using UnityEngine;

public class OcclusionCulling : MonoBehaviour
{
    [SerializeField] private LayerMask alwaysVisibleLayers; // El layer que deseas mantener siempre visible   

    private void Update()
    {
        // Obtén todos los renderizadores del objeto actual y sus hijos
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Comprueba si el renderizador está en el layer siempre visible
            if (renderer.gameObject.layer == alwaysVisibleLayers)
            {
                // Mantén el renderizador siempre visible
                renderer.enabled = true;
                continue; // Salta al siguiente renderizador
            }

            // Comprueba si el renderizador está visible por la cámara

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
