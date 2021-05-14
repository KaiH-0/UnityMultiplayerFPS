using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public Camera cam;
    public Animator anim;
    //public Animator gunanim;
    public GameObject gun;
    public GameObject gunaimpos;
    public GameObject gunpos;
    public PlayerManager manager;
    public ParticleSystem effect;
    public ParticleSystem effect2;
    public ParticleSystem effect3;
    public float lightDuration = 0.2f;
    //public Light light;
    public int timer;
    public int timeramount;
    public bool down;
    //public GameObject[] myObjects;
    //public int prevweapon;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= 1;
        }
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    cam.fieldOfView = 110f;
        //}

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            manager.up = 1;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            manager.up = 2;
        }

        timeramount = manager.myObjects[manager.weapon].GetComponent<GunHand>().delay;

        if (Input.GetKeyDown(KeyCode.Mouse0) && timer <= 0)
        {
            //HERE HERE HERE
            //manager.UpdatePlayers(manager.id);
            for (int i = 0; i < manager.playerinfo.Count; i++)
            {
                Debug.Log(manager.playerinfo[i].Username);
            }

            ClientSend.PlayerShoot(camTransform.forward);
            manager.gunanim.Play("Shoot");
            effect.Play();
            timer = timeramount;
            //effect2.Play();
            //StartCoroutine(MuzzleFlashLight());
            //effect3.Play();
        }

        if (down == true)
        {
            manager.myObjects[0].transform.position = Vector3.Lerp(gun.transform.position, gunaimpos.transform.position, 10 * Time.deltaTime);
        }
        else
        {
            manager.myObjects[0].transform.position = Vector3.Lerp(gun.transform.position, gunpos.transform.position, 10 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            down = true;
            //gun.transform.position = Vector3.Lerp(gun.transform.position, gunaimpos.transform.position, 1 * Time.deltaTime);
            //ClientSend.PlayerThrowItem(camTransform.forward);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            down = false;
            //gun.transform.position = Vector3.Lerp(gun.transform.position, gunpos.transform.position, 1 * Time.deltaTime);
            //ClientSend.PlayerThrowItem(camTransform.forward);
        }
    }

    private IEnumerator MuzzleFlashLight()
    {
        GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(lightDuration);
        GetComponent<Light>().enabled = false;
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
            Input.GetKey(KeyCode.LeftShift),
            Input.GetKeyDown(KeyCode.LeftShift),
            Input.GetKeyUp(KeyCode.LeftShift)
        };

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            manager.isWalking = true;
            anim.SetBool("isWalking", true);
        }
        else
        {
            manager.isWalking = false;
            anim.SetBool("isWalking", false);
        }

        ClientSend.PlayerMovement(_inputs);
    }
}
