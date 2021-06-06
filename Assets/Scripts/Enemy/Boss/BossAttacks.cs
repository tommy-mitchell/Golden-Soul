using System.Collections;
using UnityEngine;

public class BossAttacks : BossControllerBase
{
    public float attackSpeed;
    public Transform teleport1;
    public Transform teleport2;
    public Transform hearts;

    private int rand = 0;
    private float originalAttackSpeed;
    private bool hasAttacked;
    private bool inLowSweepRange, inHighSweepRange;

    [SerializeField]
    private GameObject lowSweep;

    [SerializeField]
    private GameObject highSweep;

    [SerializeField]
    private GameObject fullSweep;

    private BossHealth health;

    public Canvas teleportFlash;

    protected override void Setup()
    {
        base.Setup();

        lowSweep.SetActive(false);
        highSweep.SetActive(false);
        fullSweep.SetActive(false);

        originalAttackSpeed = attackSpeed;

        health = GetComponent<BossHealth>();
    }

    protected override void BossUpdate()
    {
        base.BossUpdate();
        float bossHealthPercent = health.GetHealthPercentage();

        if(bossHealthPercent <= 66.66 && bossHealthPercent > 33.33 && attackSpeed != originalAttackSpeed + 2)
        {
            phase = 2;

            StartCoroutine(TeleportPlayer());

            attackCooldown -= 0.5f;
            attackSpeed += 2;
        }
        else if (bossHealthPercent <= 33.33 && attackSpeed != originalAttackSpeed + 4)
        {
            phase = 3;

            StartCoroutine(TeleportPlayer());

            attackCooldown -= 0.5f;
            attackSpeed += 2;
        }
    }

    protected override void Attack()
    {
        StartCoroutine("AttackCoroutine");
    }

    private IEnumerator TeleportPlayer()
    {
        anim.SetBool("Attacking", true);
        hasAttacked = true;

        yield return new WaitForSeconds(1.5f); // show boss using magic to teleport player

        System.Array.ForEach( new[] { hearts.GetChild(0), hearts.GetChild(1), hearts.GetChild(2) }, heart => heart.gameObject.SetActive(true));

        Vector3 teleportPosition = (phase == 2) ? teleport1.position : teleport2.position;

        teleportFlash.gameObject.SetActive(true);

        player.transform.position = teleportPosition;

        yield return new WaitForSeconds(.1f);

        teleportFlash.gameObject.SetActive(false);

        hasAttacked = false;
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        while (!hasAttacked)
        {
            if (rand > 3 && Random.value <= getChance(rand) && (inHighSweepRange == true || inLowSweepRange == true)) //Full Sweep
            {
                bool reached = false;
                Vector3 fullSweepEndPos = transform.Find("FullEndPoint").transform.position;
                Vector3 fullSweepStartPos = transform.Find("FullStartPoint").transform.position;
                Vector2 originalFullSweepPos = fullSweep.transform.position;
                fullSweep.SetActive(true);
                rand = 0;
                yield return new WaitForSeconds(1f);
                while (fullSweep.transform.position != fullSweepEndPos)
                {
                    while(fullSweep.transform.position != fullSweepStartPos && reached != true)
                    {
                        fullSweep.transform.position = Vector3.MoveTowards(fullSweep.transform.position, fullSweepStartPos, attackSpeed * Time.deltaTime*3);

                        if(fullSweep.transform.position == fullSweepStartPos)
                        {
                            fullSweep.transform.Rotate(new Vector3(0, 0, -49));
                            reached = true;
                        }
                        yield return null;
                    }

                    fullSweep.transform.position = Vector3.MoveTowards(fullSweep.transform.position, fullSweepEndPos, attackSpeed * Time.deltaTime*3);

                    yield return null;
                }

                yield return new WaitForSeconds(.5f);

                fullSweep.transform.position = originalFullSweepPos;
                fullSweep.transform.Rotate(new Vector3(0, 0, 49));
                fullSweep.SetActive(false);
            }

            else if(inLowSweepRange == true && inHighSweepRange == false) //Low Sweep
            {
                Vector3 lowSweepEndPos = transform.Find("LowEndPoint").transform.position;
                Vector2 originalLowSweepPos = lowSweep.transform.position;
                lowSweep.SetActive(true);
                rand++;
                yield return new WaitForSeconds(1f);

                while(lowSweep.transform.position != lowSweepEndPos)
                {
                    lowSweep.transform.position = Vector3.MoveTowards(lowSweep.transform.position, lowSweepEndPos, attackSpeed * Time.deltaTime);

                    yield return null;
                }

                yield return new WaitForSeconds(.5f);


                lowSweep.transform.position = originalLowSweepPos;
                lowSweep.SetActive(false);
            }
            else if (inHighSweepRange == true && inLowSweepRange == false) //High Sweep
            {
                Vector3 highSweepEndPos = transform.Find("HighEndPoint").transform.position;
                Vector2 originalHighSweepPos = highSweep.transform.position;
                highSweep.SetActive(true);
                rand++;
                yield return new WaitForSeconds(1f);

                while (highSweep.transform.position != highSweepEndPos)
                {
                    highSweep.transform.position = Vector3.MoveTowards(highSweep.transform.position, highSweepEndPos, attackSpeed * Time.deltaTime);

                    yield return null;
                }

                yield return new WaitForSeconds(.5f);


                highSweep.transform.position = originalHighSweepPos;
                highSweep.SetActive(false);
            }

            hasAttacked = true;
        }

        EndAttack();
    }

    void EndAttack()
    {
        attackTimeStamp = Time.time + attackCooldown;
        hasAttacked = false;
        isAttacking = false;
    }

    public void SetLowSweepState(bool boo)
    {
        inLowSweepRange = boo;
    }

    public void SetHighSweepState(bool boo)
    {
        inHighSweepRange = boo;
    }

    public double getChance(int num){
        double ret = 0;
        for(int i = 0; i < num-1; i++){
            ret += (50>>i);
        }
        return ret/100;
    }
}