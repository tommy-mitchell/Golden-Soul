using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonDefinitions;
using static CommonLibrary.CommonMethods;

public class PlayerAttack : MonoBehaviour
{
    //public AudioSource swordSwingSound;
    public Collider2D  attackTrigger;

    private float attackTimer;

    private Animator         anim;
    private PlayerController controller;
    private PlayerSettings   settings;
    private AudioManager     audioManager;

    void Start()
    {
        anim         = gameObject.GetComponent<Animator>();
        controller   = GetComponent<PlayerController>();
        settings     = GameObject.Find("Player State").GetComponent<PlayerStateManager>().Settings;
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();

        attackTrigger.enabled = false;

        GetComponent<NewInput>().Player_onAttack += () => StartCoroutine(Attack());
    }
    int debugTimer = 0;

    /*void mUpdate()
    {
        if(!controller.TEMP_INPUT_DISABLE_FLAG)
        {
            if(!anim.GetBool("Attacking") && anim.GetBool("Has Sword") && CheckInput(LEFT_MOUSE_BUTTON, KeyCode.F))
            {
                attackTimer = ATTACK_CD;
                attackTrigger.enabled = true;

                swordSwingSound.Play();
                controller.SetAttacking(true);

                debugTimer++;
            }
            else if(anim.GetBool("Attacking"))
            {
                if(attackTimer > 0) { 
                    attackTimer -= Time.unscaledDeltaTime;
                    debugTimer++;
                }
                else
                {
                    attackTimer = attackLength;
                    attackTrigger.enabled = false;

                    controller.SetAttacking(false);
                    debugTimer++;

                    Debug.Log("timer: " + debugTimer);

                    debugTimer = 0;
                }
            }
        }
    }*/
    int timer2 = 0;
    IEnumerator Attack()
    {
        if(anim.GetBool("Has Sword"))
        {
            attackTimer = settings.AttackCooldown;
            attackTrigger.enabled = true;

            AudioSource.PlayClipAtPoint(audioManager.SwordAudio.OnSwordSwing, transform.position, .25f);
            controller.SetAttacking(true);
            debugTimer++;
            yield return null;

            while(attackTimer > 0)
            {
                attackTimer -= Time.unscaledDeltaTime;
                debugTimer++;
                timer2++;
                yield return null;
            }

            yield return null;
            yield return null;
            //Debug.Log("attack");
            attackTimer = settings.AttackLengthTime;
            attackTrigger.enabled = false;

            controller.SetAttacking(false);
            debugTimer++;

            //Debug.Log("timer:  " + debugTimer);
            //Debug.Log("timer2: " + timer2);

            debugTimer = 0;
            timer2 = 0;
            yield return null;
        }
    }
}
