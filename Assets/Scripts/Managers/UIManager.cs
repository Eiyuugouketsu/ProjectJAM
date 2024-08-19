using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ReticleUI reticleUI;
    public ScalePowerUI scalePowerUI;

    public void PlayerSpawn()
    {
        reticleUI.SubscribeToPlayerEvents();
        scalePowerUI.SubscribeToPlayerEvents();
    }
}
