using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCam : MonoBehaviour
{
    #region variables
    [SerializeField] PlayerIngameSettings settings;
    [SerializeField] Transform cameraHolder;
    [SerializeField] PlayerSensConfig config;
    PlayerWeapons player;

    float camCurXRot;

    private Vector2 mouseDelta;
    PhotonView pv;
    #endregion

    #region Unity

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        player = GetComponent<PlayerWeapons>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!pv.IsMine)
        {
            Camera[] cameras = cameraHolder.GetComponentsInChildren<Camera>();
            foreach (Camera c in cameras)
            {
                Destroy(c.gameObject);
            }
            CinemachineVirtualCamera cam = cameraHolder.GetComponentInChildren<CinemachineVirtualCamera>();
            Destroy(cam);
        }
    }

    private void LateUpdate()
    {
        if (pv.IsMine)
            PlayerLook();
    }
    #endregion

    #region Custom Methods
    void PlayerLook()
    {
        float currentSens = player.isAiming ? config.aimSens : config.sensitivity;
        // Aumenta el valor actual de camCurXRot por la entrada vertical del ratón multiplicada por un factor de sensibilidad
        // Esto controla la rotación vertical de la cámara
        camCurXRot += mouseDelta.y * currentSens;

        // Limita el valor de camCurXRot dentro del rango definido por config.minY y config.maxY
        // asegurando que la rotación vertical de la cámara esté dentro de los límites establecidos
        camCurXRot = Mathf.Clamp(camCurXRot, config.minY, config.maxY);

        // Actualiza la rotación local de cameraHolder en el eje X con el valor negativo de camCurXRot
        // controlando la inclinación vertical de la cámara
        cameraHolder.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // Actualiza la rotación del jugador en el eje Y
        // utilizando la entrada horizontal del ratón multiplicada por la sensibilidad
        // Esto permite que el jugador rote horizontalmente        
        transform.eulerAngles += new Vector3(0, mouseDelta.x * currentSens, 0);
    }
    #endregion

    #region input

    public void OnLookInput(InputAction.CallbackContext context)
    {
        if (settings.GetState() == State.paused)
        {
            mouseDelta = Vector2.zero;
            return;
        }

        mouseDelta = context.ReadValue<Vector2>();
    }
    #endregion
}
