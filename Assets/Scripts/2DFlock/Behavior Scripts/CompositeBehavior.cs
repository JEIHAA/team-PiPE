using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DFlock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    public FlockBehavior[] behaviors;
    public float[] weights;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //가중치 배열과 행동 배열 길이 불일치 처리
        if (weights.Length != behaviors.Length) {
            Debug.LogError("Data mismatch in "+ name, this);
            return Vector2.zero;
        }

        //움직임 설정
        Vector2 move = Vector2.zero;

        //행동 반복
        for (int i = 0; i < behaviors.Length; i++) {
            Vector2 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];

            if(partialMove != Vector2.zero)
            {
                if (partialMove != Vector2.zero) {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;             
    }
}
