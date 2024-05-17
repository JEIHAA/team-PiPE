using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObject:  ���� �纻�� �����Ǵ� ���� ����, ������� �ʴ� �����͸� ����ϴ� �������� ����� �� ����
//�޸𸮿� SCriptableObject�� ������ �纻�� ����. ���� ������Ʈ�� ������Ʈ�� ������ �� ����, �������� �����
public abstract class FlockBehavior : ScriptableObject 
{
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);
}
