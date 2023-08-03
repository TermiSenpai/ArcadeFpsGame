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
    [SerializeField] float controllerSensMultiplier;
    PlayerWeapons player;

    float camCurXRot;
    private Vector2 inputDelta;
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
        camCurXRot += inputDelta.y * currentSens;

        // Limita el valor de camCurXRot dentro del rango definido por config.minY y config.maxY
        // asegurando que la rotación vertical de la cámara esté dentro de los límites establecidos
        camCurXRot = Mathf.Clamp(camCurXRot, config.minY, config.maxY);

        // Actualiza la rotación local de cameraHolder en el eje X con el valor negativo de camCurXRot
        // controlando la inclinación vertical de la cámara
        cameraHolder.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // Actualiza la rotación del jugador en el eje Y
        // utilizando la entrada horizontal del ratón multiplicada por la sensibilidad
        // Esto permite que el jugador rote horizontalmente        
        transform.eulerAngles += new Vector3(0, inputDelta.x * currentSens, 0);
    }
    #endregion

    #region input

    public void OnLookInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;
        if (settings.GetState() == State.paused)
        {
            inputDelta = Vector2.zero;
            return;
        }
        InputDevice curDevice = context.control.device;

        if (curDevice is Gamepad && curDevice.displayName.Contains("Wireless Controller"))
            inputDelta = context.ReadValue<Vector2>() * controllerSensMultiplier;
        // Ajuste de sensibilidad para los mandos
        else
            inputDelta = context.ReadValue<Vector2>();

    }
    #endregion
}
