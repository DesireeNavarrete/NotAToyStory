using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


public class PlayerController : MonoBehaviour
{
    public GameObject[] camaras;
    //int que maneja la capa de display de las camaras
    public static int displayID;
    public static bool movimiento,ojo,activado, tocaSuelo;
    public GameObject postpo, postpo2, Jugador, pjCinem;
    public Vector2 minCamPos, maxCamPos;
    public float empuje, sacudida, xRot, yRot;
    public Image fade, wasd;
    [HideInInspector]
    public float hor;
    [HideInInspector]
    public float ver;
    public float time = 0.1f;
    float vMov = 0.5f, vRot= 100f, smoothing = 15f;


    void Start()
    {
        //si es true, el personaje puede saltar
        tocaSuelo = true;
        //si es true el personaje puede moverse
        movimiento = true;

        camaras = GameObject.FindGameObjectsWithTag("camara");
        displayID = 1;
        fade.GetComponent<Animator>();
        Jugador = GameObject.FindGameObjectWithTag("Player");
        MoverP.cogido = false;
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
    }

    void Update()
    {
        //cierra el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //valor que limita en el eje X
        float posX = Jugador.transform.position.x;
        //valor que limita en el eje y
        float posY = Jugador.transform.position.y+1;
        //limita la posicion de la camara en los ejes que le indiques para que solo pueda moverse dentrode un area delimitada
        camaras[0].transform.position = new Vector3(
            Mathf.Clamp(posX, minCamPos.x, maxCamPos.x),
            Mathf.Clamp(posY, minCamPos.y, maxCamPos.y),
            Jugador.transform.position.z-1);

        //animacion del cajon
        if (MoverP.llaveC == true)
        {
            Jugador.GetComponent<Animator>().SetBool("Llave", true);
        }
        else
        {
            Jugador.GetComponent<Animator>().SetBool("Llave", false);
        }

        //fade y cambio de camara con boton E
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            if (activado==false && cinematica.cinem==false)
            {
                activado = true;
                StartCoroutine(fadeIn());
                StopCoroutine(fadeOut());

            }else if (activado)
            {
                activado = false;
                StartCoroutine(fadeOut());
                StopCoroutine(fadeIn());
            }
        }

    
        //__________________MOV PJ
        //multiplicoo el valor que le indico al float vRot por el eje X
        //transform.Rotate(0, Input.GetAxis("Horizontal") * vRot, 0);
        if (movimiento)
        {
            pjCinem.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space) && saltoDetector.tocaSuelo == true)
            {
                Jump();
                //tocaSuelo = false;
            }
            if (saltoDetector.tocaSuelo == false)
            {
                Jugador.GetComponent<Animator>().SetBool("Saltar", false);
            }
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                Jugador.GetComponent<Animator>().SetBool("Andar", false);
                Jugador.GetComponent<Animator>().SetBool("Empujar", false);
                Jugador.GetComponent<Animator>().SetBool("Tirar", false);
            }
            else
            {
                Jugador.GetComponent<Animator>().SetBool("Andar", true);
            }
        }
        if (!movimiento)
        {
            Jugador.GetComponent<Animator>().SetBool("Andar", false);
            Jugador.GetComponent<Animator>().SetBool("Empujar", false);
            Jugador.GetComponent<Animator>().SetBool("Tirar", false);
        }

        
            
        
        if (MoverP.cogido)
        {
            if (Input.GetKey(KeyCode.S))
            {
                Jugador.GetComponent<Animator>().SetBool("Andar", false);
                Jugador.GetComponent<Animator>().SetBool("Empujar", false);
                Jugador.GetComponent<Animator>().SetBool("Tirar", true);
            }
            if (Input.GetKey(KeyCode.W))
            {
                Jugador.GetComponent<Animator>().SetBool("Empujar", true);
                Jugador.GetComponent<Animator>().SetBool("Andar", false);
                Jugador.GetComponent<Animator>().SetBool("Tirar", false);
            }

            //multiplico el valor que le indico al float vMot por el eje Z y el tiempo
            Jugador.transform.Translate(0, 0, Input.GetAxis("Vertical")* Time.deltaTime * vMov * 1);
            //rotacion a 0
            Jugador.transform.Rotate(0, 0, 0);
        }


        if (!MoverP.cogido && ojo == false)
        {
            //multiplico el valor que le indico al float vMot por el eje Z y el tiempo
            Jugador.transform.Translate(0, 0, Input.GetAxis("Vertical")* Time.deltaTime * vMov);

            //multiplico el valor que le indico al float vRot por el eje X y el tiempo
            Jugador.transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * vRot, 0);

        }else if(ojo)
        {
            //mov a cero para que no se mueva
            Jugador.transform.Translate(0, 0, 0);
            Jugador.transform.Rotate(0, 0, 0);
            StartCoroutine(cam());

        }
        
            
        


    }
    
    public void cambioCamara()
    {
        //camara ojo
        camaras[2].GetComponent<Camera>().targetDisplay = displayID - 1;
        
    }

    //fadeIn
    IEnumerator fadeIn()
    {
        movimiento = false;
        fade.GetComponent<Animator>().SetBool("fadeIn", true);
        yield return new WaitForSeconds(1);
        postpo.GetComponent<PostProcessVolume>().enabled = false;
        postpo2.GetComponent<PostProcessVolume>().enabled = true;
        camaras[2].GetComponent<Camera>().targetDisplay = displayID - 1;
        ojo = true;
        yield return new WaitForSeconds(1);
        fade.GetComponent<Animator>().SetBool("fadeIn", false);
        fade.GetComponent<Animator>().SetBool("fadeOut", true);
        yield return null;
    }

    //fadeOut
    IEnumerator fadeOut()
    {
        fade.GetComponent<Animator>().SetBool("fadeIn", true);
        yield return new WaitForSeconds(1);
        postpo.GetComponent<PostProcessVolume>().enabled = true;
        postpo2.GetComponent<PostProcessVolume>().enabled = false;
        camaras[2].GetComponent<Camera>().targetDisplay = displayID;
        ojo = false;
        yield return new WaitForSeconds(1);
        movimiento = true;
        fade.GetComponent<Animator>().SetBool("fadeIn", false);
        fade.GetComponent<Animator>().SetBool("fadeOut", true);
        yield return null;
    }
    
    //movimiento camara vision
    IEnumerator cam()
    {
        
        //suma la rotacion
        yRot += Input.GetAxis("Horizontal");
        xRot += Input.GetAxis("Vertical");

        //delimitamos la rotacion de la camara
        xRot = Mathf.Clamp(xRot, -8, 8);
        yRot = Mathf.Clamp(yRot, -20, 5);

        camaras[2].transform.rotation = Quaternion.Euler(xRot - 180, yRot , -180);      
        yield return null;

    }

    //SALTO
    public void Jump()
    {
        Jugador.GetComponent<Animator>().SetBool("Saltar", true);
        Jugador.GetComponent<Rigidbody>().AddForce(0, empuje, 0, ForceMode.Impulse);
    }

    //COLISIONES DE SALTO
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("suelo") || other.gameObject.CompareTag("coger"))
        {
            tocaSuelo = true;
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("suelo") || col.gameObject.CompareTag("coger"))
        {
            tocaSuelo = false;
        }
    }
}

