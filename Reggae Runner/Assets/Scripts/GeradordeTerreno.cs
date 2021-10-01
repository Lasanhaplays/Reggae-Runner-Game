using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradordeTerreno : MonoBehaviour{

    public GameObject[] terrenos;
    public int posicao = 137;   //Tamanho do primeiro terreno.
    public bool terrenoCriado = false;
    public int terrenoEscolhido;



    // Update is called once per frame
    void Update(){

        if(terrenoCriado == false) {

            terrenoCriado = true;
            StartCoroutine(GerarTerreno());
        }
    }



    /*Corrotina para instanciar um novo Terreno aleatorio no Jogo.*/
    IEnumerator GerarTerreno() {

        terrenoEscolhido = Random.Range(0, 3);
        transform.position = new Vector3(0, 0, transform.position.z + 137);
        Instantiate(terrenos[terrenoEscolhido], new Vector3(19.5f, 25, posicao), Quaternion.Euler(0, 0, 0));
        posicao += 115;
        yield return new WaitForSeconds(5);
        terrenoCriado = false;
    }
}