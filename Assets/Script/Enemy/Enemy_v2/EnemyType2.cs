using UnityEngine;

public class EnemyType2 : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        moveDir = Vector3.back;
        isHeartBarActive = true;
        heartBar.SetMaxHealth(blood);
    }

    // Update is called once per frame
    void Update()
    {
        DespawnEnemy();

        if (!IsClose())
        {
            Move();
        }
        else
        {
            animator.SetTrigger("Attack");
        }
    }
}
