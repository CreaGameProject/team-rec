using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class TaskBasedEnemy : Enemy
{
    protected List<IEnemyTask> tasks;

    public float aliveTime;

    protected Animator animator;

    public IEnumerable<IEnemyTask> Tasks => tasks;

    public Animator Animator
    {
        get
        {
            if (this.animator == null)
                this.animator = GetComponent<Animator>();
            return this.animator;
        }
    }
    
    protected override void Kill(BulletObject bulletObject)
    {
        base.Kill(bulletObject);
    }

    void ThisDestroy()
    {
        Destroy(this.gameObject);
    }

    public void SetTasks(IEnumerable<IEnemyTask> tasks)
    {
        this.tasks = tasks.ToList();
    }

    IEnumerator RunTasks()
    {
        yield return new WaitWhile(()=> this.tasks == null);
        var tasks = this.tasks.ToList();
        aliveTime = 0;

        while (tasks.Any())
        {
            aliveTime += Time.deltaTime;
            
            //tasksはTimeでソートされていることを前提としている。
            foreach (var x in tasks.TakeWhile(x => x.Time < aliveTime))
            {
                // Debug.Log(aliveTime);
                x.Call(this);
            }
            
            tasks = tasks.SkipWhile(x => x.Time < aliveTime).ToList();
            yield return null;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunTasks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
