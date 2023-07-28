using Photon.Pun;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(pv.IsMine)
            gameObject.SetActive(false);
    }


    private void Update()
    {
        if (cam == null)
            cam = FindObjectOfType<Camera>();

        if (cam == null)
            return;

        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
