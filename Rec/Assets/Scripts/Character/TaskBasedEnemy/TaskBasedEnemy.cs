using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    public class TaskBasedEnemy : global::Enemy
    {
        protected List<IEnemyTask> tasks;

        protected Animator animator;

        public IEnumerable<IEnemyTask> Tasks
        {
            get { return tasks; }
        }

        public Animator Animator
        {
            get
            {
                if (this.animator == null)
                    this.animator = GetComponent<Animator>();
                return this.animator;
            }
        }

        public virtual void TriggerAnimation(string name)
        {

        }
        
        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                BulletObject bulletObject =  other.gameObject.GetComponent<BulletObject>();
                Force colForce = bulletObject.Force;
                if (colForce == Force.Player)
                {
                    Debug.Log("攻撃力" + bulletObject.bulletclass.AttackPoint + "のPlayerの弾が当たりました");
                    Damage(bulletObject.bulletclass.AttackPoint, bulletObject);
                }
            }
        }

        protected override void Kill(BulletObject bulletObject)
        {
            base.Kill(bulletObject);
            // TODO: ゴールイベントの実装する
        }

        public void SetTasks(IEnumerable<IEnemyTask> tasks)
        {
            this.tasks = tasks.ToList();
        }

        IEnumerator RunTasks()
        {
            yield return new WaitWhile(() => this.tasks == null);

            foreach (var task in tasks)
            {
                yield return task.Call(this);
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

}