using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObject:  값의 사본이 생성되는 것을 방지, 변경되지 않는 데이터를 사용하는 프리팹을 사용할 때 유용
//메모리에 SCriptableObject의 데이터 사본만 저장. 게임 오브젝트에 컴포넌트로 부착할 수 없고, 에셋으로 저장됨
public abstract class FlockBehavior_3D : ScriptableObject 
{
    public abstract Vector3 CalculateMove(FlockAgent_3D agent, List<Transform> context, Flock_3D flock, List<Transform> flags);
}
