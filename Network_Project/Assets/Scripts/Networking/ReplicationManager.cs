using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public int netId = 0;

}


public class ReplicationManager : MonoBehaviour
{
    List<NetworkObject> netObjects = new ();
    
    void Start()
    {
       
    }

    void Update()
    {
        
    }

    public void MakeNetObject(GameObject go)
    {
        NetworkObject no = go.AddComponent<NetworkObject>();
        netObjects.Add(no);
    }
}
