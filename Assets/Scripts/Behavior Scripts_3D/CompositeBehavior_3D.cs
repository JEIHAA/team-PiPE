using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "3DFlock/Behavior/Composite")]
public class CompositeBehavior_3D : FlockBehavior_3D
{
    public FlockBehavior_3D[] behaviors;
    public float[] weights;

    public override Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock)
    {
        //가중치 배열과 행동 배열 길이 불일치 처리
        if (weights.Length != behaviors.Length) {
            Debug.LogError("Data mismatch in "+ name, this);
            return Vector3.zero;
        }

        //움직임 설정
        Vector3 move = Vector3.zero;

        //행동 반복
        for (int i = 0; i < behaviors.Length; i++) {
            Vector3 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];

            if(partialMove != Vector3.zero)
            {
                if (partialMove != Vector3.zero) {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;             
    }
}
