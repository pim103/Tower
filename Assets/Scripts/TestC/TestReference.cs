using System.Collections;
using System.Collections.Generic;
using Games.Global;
using UnityEngine;

public class FakeEntity
{
    public Dictionary<int, int> dic = new Dictionary<int, int>();
    public BasicStruct basic;
}

public struct BasicStruct
{
    public int aloa;
}

public class TestReference : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FakeEntity fake1 = new FakeEntity();
        fake1.basic = new BasicStruct {aloa = 0};
        
        FillDic(fake1);

        StartCoroutine(CoroutineModifyDic(fake1));
        StartCoroutine(CoroutineModifyDic2(fake1));
    }

    void FillDic(FakeEntity fake)
    {
        fake.dic.Add(0, 1);
        fake.dic.Add(1, 2);
        fake.dic.Add(2, 3);
        fake.dic.Add(3, 4);
    }

    public IEnumerator CoroutineModifyDic(FakeEntity fake)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            
            Debug.Log("Coroutine 1");
            Debug.Log(fake.basic.aloa);
        }
    }
    
    public IEnumerator CoroutineModifyDic2(FakeEntity fake)
    {
        yield return new WaitForSeconds(4);
        fake.basic.aloa = 15;
    }
}
