using UnityEngine;
using Photon.Pun;

public class CubeInteractionController : MonoBehaviourPun
{
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Debug.Log("Ray Fired");
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log("Ray Hit");
                // Check if the ray hit a cube
                if (hit.collider.gameObject.tag == "Cube") 
                {
                    Debug.Log("Cube destroyed");
                    // Destroy the cube for all players in the network
                    photonView.RPC("DestroyCubeRPC", RpcTarget.AllBuffered, hitObject.GetPhotonView().ViewID);
                }
            }
        }
    }

    [PunRPC]
    void DestroyCubeRPC(int cubeViewID)
    {
        PhotonView cubePhotonView = PhotonView.Find(cubeViewID);
        if (cubePhotonView != null)
        {
            // Destroy the cube for all clients
            PhotonNetwork.Destroy(cubePhotonView);
        }
    }
}
