using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonDefinitions;
using static CommonLibrary.CommonMethods;
using static CommonLibrary.PlayerDefinitions;

public class PlayerThrow : MonoBehaviour
{
    public GameObject          _swordPrefab;
    public UnityEngine.UI.Text teleportWarningText;

    private GameObject       sword;
    private Animator         anim;
    private PlayerController controller;
    private PlayerSettings   settings;
    private AudioManager     audioManager;

    private float   timeStamp;
    private bool    canThrow;
    private Vector2 throwDirection;

    private void Start()
    {
        anim         = GetComponent<Animator>();
        controller   = GetComponent<PlayerController>();
        settings     = GameObject.Find("Player State").GetComponent<PlayerStateManager>().Settings;
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();

        timeStamp      = 0;
        canThrow       = true;
        throwDirection = Vector2.right;

        NewInput input = GetComponent<NewInput>();
            input.Player_onAim      += _input => { // ignore release of input
                if (_input != Vector2.zero)
                    throwDirection = _input;
                else
                    throwDirection = transform.rotation.y == 0 ? Vector2.right : Vector2.left;
            };
            input.Player_onMove     += _input => { if(_input != 0) throwDirection = new Vector2(_input, 0); }; // aim towards direction of movement
            input.Player_onThrow    += ThrowSword;
            input.Player_onTeleport += Teleport;
            input.Player_onRecall   += Recall;
            input.Hanging_onRecall  += Recall;

        GetComponent<PlayerHealth>().OnDeath += ResetSword;
    }

    private void ThrowSword()
    {
        if(!anim.GetBool("Attacking") && anim.GetBool("Has Sword") && timeStamp <= Time.time) // instead change to disabling throw controls when attacking
        {
            controller.SetHasSword(false);
            controller.SetThrowingState(throwDirection);

            //StartCoroutine(WaitForAnimationToFinish(anim.GetCurrentAnimatorStateInfo(0)));

            if(canThrow)
            {
                ThrowSword(throwDirection);
                canThrow = false;
            }
        }
    }

    private void Teleport()
    {
        if(!anim.GetBool("Has Sword"))
        {
            bool swordInCeiling = sword.GetComponent<SwordController>().CanHangOnSword;

            if(swordInCeiling) // hang
            {
                transform.position = sword.transform.position + new Vector3(0, -1, 0);

                controller.setHanging(sword.GetComponent<Rigidbody2D>());
            }
            else
            {
                TeleportToSword();
                canThrow = true;
            }
        }
    }

    private void Recall()
    {
        if(!anim.GetBool("Has Sword"))
        {
            controller.setHanging(null);
            AudioSource.PlayClipAtPoint(audioManager.SwordAudio.OnSwordRecall, transform.position, .5f);
            Destroy(sword);
            canThrow = true;
        }
    }

    /*private void Update()
    {
        if(!anim.GetBool("Attacking") && !controller.TEMP_INPUT_DISABLE_FLAG)
        {
	        if(anim.GetBool("Has Sword") && CheckInput(RIGHT_MOUSE_BUTTON, KeyCode.E) && timeStamp <= Time.time) // throw
            {
                Vector2 throwDirection = GetThrowDirection();

                controller.SetHasSword(false);
                controller.SetThrowingState(throwDirection);

                /*System.Predicate<AnimatorStateInfo> AnimStateIsThrow = ( animInfo ) => { 
                    return animInfo.IsName("Down") || animInfo.IsName("Side") || animInfo.IsName("Up");
                };

                if(AnimStateIsThrow(anim.GetCurrentAnimatorStateInfo(0)))*

                StartCoroutine(WaitForAnimationToFinish(anim.GetCurrentAnimatorStateInfo(0)));

                if(canThrow)
                {
                    ThrowSword(throwDirection);
                    canThrow = false;
                }
            }
            else if(!anim.GetBool("Has Sword") && (CheckInput(RIGHT_MOUSE_BUTTON, KeyCode.E) || CheckInput(MIDDLE_MOUSE_BUTTON, KeyCode.C))) // teleport & recall
            {
                bool swordInCeiling = sword.GetComponent<SwordController>().isInCeiling;

                if(CheckInput(RIGHT_MOUSE_BUTTON, KeyCode.E) && swordInCeiling == false) // teleport
                {
                    //if(CheckHeightOfLocation(sword.transform.position) < 2) // box collider around sword in shape of player: detects if player can fit
                    //    StartCoroutine(ShowTeleportWarning());
                    //else
                        TeleportToSword();
                }
                else if(CheckInput(RIGHT_MOUSE_BUTTON, KeyCode.E) && swordInCeiling == true) // hang
                {
                    transform.position = sword.transform.position + new Vector3(0,-1,0);
                    
                    GetComponent<SpringJoint2D>().connectedBody = sword.GetComponent<Rigidbody2D>();
                    GetComponent<SpringJoint2D>().enabled = true;
                    controller.playerHanging = true;
                }
                else // recall
                {
                    controller.playerHanging = false;
                    Destroy(sword);
                }

                    
                if(swordInCeiling == false && sword == null)
                {
                    controller.SetHasSword(true);
                    controller.SetThrowingState(Vector2.zero);
                }
            }
            else if(!anim.GetBool("Has Sword") && GameObject.Find("Sword(Clone)") == null) //Prevents instances where the sword is destroyed, but not returned to the player
                controller.SetHasSword(true);
        }
    }*/

    private void Update()
    {
        /*if(!sword.GetComponent<SwordController>().isInCeiling && sword == null)
        {
            controller.SetHasSword(true);
            controller.SetThrowingState(Vector2.zero);
        }*/

        if(!anim.GetBool("Has Sword") && GameObject.Find("Sword(Clone)") == null) //Prevents instances where the sword is destroyed, but not returned to the player
            controller.SetHasSword(true);
    }

    private IEnumerator WaitForAnimationToFinish(AnimatorStateInfo animInfo)
    {
        if(animInfo.IsName("Down") || animInfo.IsName("Up"))
            yield return new WaitForSeconds(1.67f);
        else if(animInfo.IsName("Side"))
            yield return new WaitForSeconds(.25f);

        canThrow = true;
    }

    private IEnumerator ShowTeleportWarning()
    {
        teleportWarningText.enabled = true;

        yield return new WaitForSeconds(2);

        teleportWarningText.enabled = false;
    }

    public void TeleportToSword()
    {
        transform.position = sword.transform.position + new Vector3(0, 0.6f, 0); // old offset: .495204f
        timeStamp = Time.time + settings.ThrowCooldownTime; //Tracks time since teleport

        Destroy(sword);
    }

    public void ThrowSword(Vector2 throwDirection)
    {
        AudioSource.PlayClipAtPoint(audioManager.SwordAudio.OnSwordThrow, transform.position);

        Vector3    playerPos      = transform.position;
        Quaternion playerRotation = transform.rotation;
 
        sword = Instantiate(_swordPrefab, playerPos, playerRotation);
        sword.GetComponent<SwordController>().Throw(throwDirection);
    }

    public void ResetSword()
    {
        Destroy(sword);
        controller.SetHasSword(true);
        canThrow = true;
    }

    public void HitEnemy()
    {
        ResetSword();
        timeStamp = Time.time + settings.ThrowCooldownTime; //Tracks time since enemy hit
    }

    public bool HasHit() => sword.GetComponent<SwordController>().HasHit();
}