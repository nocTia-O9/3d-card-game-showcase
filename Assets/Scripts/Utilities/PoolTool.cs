using UnityEngine;

//注意命名空间是UnityEngine.Pool
//引用了就可以使用Unity自带的各种类型的对象池了
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]//值越小越先执行，保证对象池较先执行
public class PoolTool : MonoBehaviour
{
    //需要生成的prefab
    public GameObject objPrefab;
    //对象池
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        //初始化对象池
        //参数主要就是往对象池的委托中装函数，以及容量等参数
        pool=new ObjectPool<GameObject> (
            createFunc:()=>Instantiate(objPrefab,transform),
            actionOnGet:(obj)=>obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),            
            actionOnDestroy:(obj)=>Destroy(obj),
            collectionCheck:false,
            defaultCapacity:10,
            maxSize:100
        );

        //预先填充11个对象
        PreFillPool(10);
    }

    /// <summary>
    /// 预先填充对象池，提前从对象池里拿出来一些对象，然后将它们回收
    /// 实际效果就是提前生成一些对象，然后让它们失活
    /// 这样需要使用时会更加方便
    /// </summary>
    /// <param name="count"></param>
    private void PreFillPool(int count)
    {
        var preFillArray=new GameObject[count];
        
        for(int i = 0; i < count; i++)
        {
            preFillArray[i]=pool.Get();
        }

        foreach(var item in preFillArray)
        {
            pool.Release(item);
        }
    }

    //为外部提供获取和释放对象的方法
    //获取对象
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }
    //释放对象
    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
