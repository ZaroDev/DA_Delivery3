using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTracker : MonoBehaviour
{
    private const int invalidID = -1;
    public int id = invalidID;

    private PlayerData playerData;

    public void Start()
    {
        // We heavily rely on Unity not screwing up the scene here
        if(id == invalidID)
        {
            RequestID();
        }
    }

    private void RequestID()
    {
        playerData = new PlayerData(0, gameObject.name);
        playerData.callback += (uint id) => this.id = (int)id;

        DataSender.Instance.PostPlayerData(playerData);
    }

    
}
