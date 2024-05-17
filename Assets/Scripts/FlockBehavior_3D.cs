using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObject:  ���� �纻�� �����Ǵ� ���� ����, ������� �ʴ� �����͸� ����ϴ� �������� ����� �� ����
//�޸𸮿� SCriptableObject�� ������ �纻�� ����. ���� ������Ʈ�� ������Ʈ�� ������ �� ����, �������� �����
public abstract class FlockBehavior_3D : ScriptableObject 
{
    public abstract Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock);
}
