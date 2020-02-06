using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MoverP : MonoBehaviour
{
    public GameObject [] objCoger;
    public static bool cogido, llaveC;
    int num;
    public GameObject llave, llaveBuena, particula,cajon1,fin;
    public Image fade, e;



    void Start()
    {
        cogido = false;
        objCoger = GameObject.FindGameObjectsWithTag("coger");
        fin.SetActive(false);
        llaveBuena.SetActive(false);
    
    }


    void Update()
    {

        if(llaveC == true)
        {
            cajon1.GetComponent<Animator>().SetBool("abrir", true);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag== "llave")
        {
            StartCoroutine(llavero());
        }

        if (other.transform.tag == "boton")
        {
            print("botoon");
            //fadein
            fade.GetComponent<Animator>().SetBool("fadeIn", true);
            StartCoroutine(fadeOutFin());


        }

      
        if (other.transform.tag == "canvas")
        {
           e.GetComponent<Animator>().SetBool("e", true);

        }
        
    }
    //se activa la llave de la mano
    IEnumerator llavero()
    {
        llaveC = true;
        yield return new WaitForSeconds(1f);
        llave.SetActive(false);
        llaveBuena.SetActive(true);
        particula.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        llaveC = false;
        yield return null;
    }

    //coger y soltar
    private void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "coger")
        {
            if (num <= objCoger.Length)
            {
                num = System.Int32.Parse(col.gameObject.name);

                for (int i = 0; i < objCoger.Length; i++)
                {
                    if (Input.GetKeyDown(KeyCode.E) && !cogido)
                    {
                        if (num == i)
                        {
                            objCoger[i].transform.SetParent(transform);
                            print("Tocado caja " + num);
                            cogido = true;
                            print(i);
                            
                        }
                        
                    }
                    else if (Input.GetKeyDown(KeyCode.E) && cogido)
                    {
                        if (num == i)
                        {
                            objCoger[i].transform.SetParent(null);
                            cogido = false;
                       
                        }

                        
                  }


                }
            }

        }




       

    }

   //pantalla de fin
    IEnumerator fadeOutFin()
    {
        fin.SetActive(false);

        yield return new WaitForSeconds(1);

        fade.GetComponent<Animator>().SetBool("fadeIn", true);
        yield return new WaitForSeconds(1f);
        fade.GetComponent<Animator>().SetBool("fadeIn", false);
        fade.GetComponent<Animator>().SetBool("fadeOut", true);
        yield return new WaitForSeconds(.1f);
        fin.SetActive(true);
        yield return new WaitForSeconds(1f);
        fade.GetComponent<Animator>().SetBool("fadeIn", true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
