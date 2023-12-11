using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ClickPool : MonoBehaviour
{
    public ClickFeedback ClickFeedbackPrefab;
    public Transform ParentContainer;
    public int Min = 10;
    public int Max = 30;

    public IObjectPool<ClickFeedback> Pool;
    public List<ClickFeedback> ClicksPool;

    public void InitPool()
    {
        Pool = new ObjectPool<ClickFeedback>
            (CreateFunc,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyClick,
            false, Min, Max);

        List<ClickFeedback> tmp = new List<ClickFeedback>();
        int howMany = Max;
        for (int i = 0; i < howMany; ++i)
        {
            tmp.Add(Pool.Get());

        }
        for (int i = 0; i < howMany; ++i)
        {
            Pool.Release(tmp[i]);
        }
    }

    public void SpawnClickFeedback(int amount, Vector3 initPos)
    {
        var click = Pool.Get();
        click.PoolReference = this;
        click.InitClick(amount, initPos);
    }

    public ClickFeedback CreateFunc()
    {
        var ClickGO = Instantiate(ClickFeedbackPrefab, ParentContainer);
        
        return ClickGO;
    }

    public void OnTakeFromPool(ClickFeedback click) => click.TakeFromPool();

    public void OnReturnToPool(ClickFeedback click) => click.ReturnToPool();

    public void OnDestroyClick(ClickFeedback click) => Destroy(click);

    private void Start()
    {
        InitPool();
        GameManager.OnClick += SpawnClickFeedback;
    }

    private void OnDestroy()
    {
        GameManager.OnClick -= SpawnClickFeedback;
    }
}
