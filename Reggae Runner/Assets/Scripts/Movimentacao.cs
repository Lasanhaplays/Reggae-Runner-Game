using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour {

    /*
     * Propriedades(1a Configuracao):
     *
        * Velocidade            = 8.
        * Velocidade Max        = 20.
        * Velocidade Horizontal = 10.
        * Tamanho Pulo          = 5.
        * Altura Pulo           = 1.
        * Tamanho Deslize       = 8.
    *
    */

    /*Propriedades de Movimentacao.*/
    [SerializeField]
    float velocidade;
    [SerializeField]
    float velocidadeMaxima;
    [SerializeField]
    float velocidadeHorizontal;
    [SerializeField]
    float tamanhoPulo;
    [SerializeField]
    float alturaPulo;
    [SerializeField]
    float tamanhoDeslize;
    [SerializeField]
    GameObject MovimentoCamera;

    /*Indicadores de Movimento.*/
    bool andar = true;
    int linhaAtual = 1;
    float inicioPulo;
    bool taPulando = false;
    float inicioDeslize;
    bool taDeslizando = false;
    Vector3 alturaPosicao;
    Vector3 TamBoxCollider;

    /*Componentes Extras.*/
    int Score;
    public AbrirMenu Menus;
    public MusicaManager Audios;

    /*Componentes do Personagem.*/
    Rigidbody rb;
    Animator animacao;
    BoxCollider boxCollider;



    /*Funcao estaAndando():
    * Metodo responsavel por alterar a condicao de Andar
    * do Personagem (utilizado pelo Script dos Menus).*/
    public void estaAndando(bool VF)
    {

        andar = VF;
    }



    // Start is called before the first frame update
    void Start(){

        Score = 0;
        //andar = true;
        
        rb = GetComponent<Rigidbody>();
        animacao = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        TamBoxCollider = boxCollider.size;
    }



    // Update is called once per frame
    void Update(){

        if (!andar) return;

        /*Gerencia as acoes do Personagem movimentando-o de acordo com as teclas pressionadas.*/
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) MudarLinha(-1);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) MudarLinha(1);
        else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) Pular();
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.DownArrow)) Deslizar();


        /*Controla a posicao Vertical do Personagem, caso esteja pulando.*/
        if (taPulando) {

            float controle = (transform.position.z - inicioPulo) / tamanhoPulo;
            if (controle >= 1f) {

                taPulando = false;
                animacao.SetBool("Pulando", false);
            }
            else alturaPosicao.y = Mathf.Sin(controle * Mathf.PI) * alturaPulo;
        }
        else {
            alturaPosicao.y = Mathf.MoveTowards(alturaPosicao.y, 0, 5 * Time.deltaTime);
        }


        /*Controla o Deslizar do Personagem, caso esteja deslizando.*/
        if (taDeslizando) {

            float controle = (transform.position.z - inicioDeslize) / tamanhoDeslize;
            if(controle >= 1f) {

                taDeslizando = false;
                animacao.SetBool("Deslizando", false);
                boxCollider.size = TamBoxCollider;
            }
        }


        /*Atualiza a posicao do Personagem, de acordo com os comandos de movimento recebidos.*/
        Vector3 proximaPosicao = new Vector3(alturaPosicao.x, alturaPosicao.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, proximaPosicao, velocidadeHorizontal * Time.deltaTime);

        Menus.setPlacar(Score);   //Atualiza os pontos no menu de Placar.
    }



    private void FixedUpdate() {

        /*Verifica a condicao de Andar do Personagem e realiza
         * um movimento fixo para Frente no eixo Z.*/
        if (!andar) return;
        rb.velocity = Vector3.forward * velocidade;
    }



    /*Funcao MudarLinha():
    * Metodo responsavel por alterar a
    * posicao Horizontal do Personagem.*/
    void MudarLinha(int direcao) {

        int proximaLinha = linhaAtual + direcao;
        if (proximaLinha < 0 || proximaLinha > 2) return;

        linhaAtual = proximaLinha;
        alturaPosicao = new Vector3((linhaAtual - 1), 0, 0);

        Score += 1;   //Incrementa o Placar de pontos.
    }



    /*Funcao Pular():
     * Metodo responsavel por indicar o movimento de Pular
     * e animar o pulo do Personagem na Vertical.*/
    void Pular() {

        if (!taPulando) {

            inicioPulo = transform.position.z;
            animacao.SetFloat("VelocidadePulo", velocidade / tamanhoPulo);
            animacao.SetBool("Pulando", true);
            taPulando = true;

            Score += 1;   //Incrementa o Placar de pontos.
        }
    }



    /*Funcao Deslizar():
    * Metodo responsavel por indicar o movimento de
    * Deslizar e animar o deslize do Personagem.*/
    void Deslizar() {

        if(!taPulando && !taDeslizando) {

            inicioDeslize = transform.position.z;
            animacao.SetFloat("VelocidadePulo", velocidade / tamanhoDeslize);
            animacao.SetBool("Deslizando", true);
            Vector3 novoTamanho = boxCollider.size;
            novoTamanho.y = novoTamanho.y / 3;
            boxCollider.size = novoTamanho;
            taDeslizando = true;

            Score += 1;   //Incrementa o Placar de pontos.
        }
    }



    private void OnTriggerEnter(Collider other) {

        /*Verifica a colisao do Personagem com os Obstaculos.*/
        if (other.CompareTag("Obstaculos")) {

            //Exibe os menus de fim de jogo.
            Menus.LigarMenuMorte();

            /*Finaliza os movimentos do Personagem e da
             * Camera, e exibe a animacao de Morte.*/
            andar = false;
            Destroy(MovimentoCamera.GetComponent<MovimentoCamera>());
            rb.velocity = Vector3.zero;
            animacao.SetTrigger("Bateu");

            //Aciona os Audios de fim de jogo.
            Audios.iniciarAudioMorte();
        }
    }
}