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
        //����ġ �迭�� �ൿ �迭 ���� ����ġ ó��
        if (weights.Length != behaviors.Length) {
            Debug.LogError("Data mismatch in "+ name, this);
            return Vector3.zero;
        }

        //������ ����
        Vector3 move = Vector3.zero;

        //�ൿ �ݺ�
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
