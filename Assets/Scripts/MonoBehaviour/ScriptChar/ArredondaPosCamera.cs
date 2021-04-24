using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ArredondaPosCamera : CinemachineExtension
{
    public float PixelsPerUnit = 32;                                        // Recebe a quantidade de pixels por unidade

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, 
        float deltaTime)
    {
        if(stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.FinalPosition;
            Vector3 pos2 = new Vector3(Round(pos.x),Round(pos.y),pos.z);
            state.PositionCorrection += pos2 - pos;
        }
    }

    // Funçao que arredonda um float de acordo com a quantidade de pixels por unidade
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}
