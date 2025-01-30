### **Introduccion**

Maze_Runners es un juego desarrollado en el motor de juegos unity el cual consiste en una carrera a traves de un laberinto entre dos jugadores con el objetivo de llevar cada jugador todas sus
fichas a la meta antes que el jugador rival. La generacion de laverintos fue realizada de forma dinamica usando el algoritmo de Prim del cual entraremos en profundidad un poco más adelante. La
temática de este juego esta basada en el anime Fairy Tail vinculada a la carrera de las 24 horas del gremio pero en este caso la carrera es dentro de un laberinto con trampas teletrnasportadores
y demás cositas...

### Desarrollo del Proyecto

Este proyecto lo tuve que dividir en varias faces para su desarrollo, asi que empece por la parte principal que es **El Laberinto** que en un primer momento no tenía ni idea de como hacer un
algoritmo que me generace laberintos de forma aleatoria, entonces me puse a investigar y existían varios métodos para conseguir este resultado, muchos de los cuales eran bastantes turbios pero
encontré uno que me encantó especialmente porque era muy legible y usaba la técnica de recursividad conocida como *Bactracking* (a este algoritmo es una variante del algoritmo de Prim), lo que
hace es abrir caminos de forma aleatria empezando por una celda en especifico que esta representada de forma matricial, estos caminos se abren en cuatro direcciones posible cumpliendo ciertos 
requisistos para ello para esto se necesita una matriz y una forma de identifar cuando es pared o no y ademas para manejar de mejor manera las direcciones se utiliza una estructura para indicar las 4 posibles direcciones. Con todo esto se llega al metodo que se muestra a coontinuacion:
```
void GenerateMaze(int x,int y) // Algoritmo recursivo para generar el laberinto
    {
        Maze[x, y].Obstacle = Path;

        Cells[] directions = new Cells[]
        {
            new Cells(0, 2), 
            new Cells(2, 0),  
            new Cells(0, -2),  
            new Cells(-2, 0)   
        };

        Shuffle(directions); // Mezcla las direcciones

        foreach (var direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (IsInBounds(newX, newY) && Maze[newX, newY].Obstacle == Wall) // Verifica si la dirección a la que se movió es valida y si es una pared
            {
                Maze[x + direction.x / 2, y + direction.y / 2].Obstacle = Path; // Elimina la pared del medio
                GenerateMaze(newX, newY);
            }
        }
    }
```
De esta forma se consigue generar el laberinto de forma aleatoria abriendo caminos de forma aleatoria, recursiva y sin dejar zonas inaccesibles. 
Como quería una meta no sabia donde la iba a colocar ni donde seria mas conveniente hacerlo por el simple hecho de que el laberinto siempre iba a ser diferenete por lo que se me ocurrio que 
podia hacerlo buscando el camino más largo desde donde empieza el laberinto que en este caso es la posicion [1,1] de la matriz, es decir buscar el lugar más lejano al que se puede llegar
desde esa posicion, era consiente de que usando un DFS o el algoritmo de Lee sería algo sencillo solo con investigar un poco y adaptar la idea conseguiria el resultado, pero decidi hacerlo
con lo que sabía, asi que utilicé nuevamente bactracking para buscar de forma recursiva el camino más largo, la gracia me salio en acostarme a las 4 am :| pero se logró el resultado y ya tenia el punto mas lejano con estos métodos:
```
Cells LongestPath(Cell[,] maze) // código recursivo para determinar el camino mas largo del laberinto
    {
        int max = 0;
        Cells cellF = new Cells(0,0);
        bool[,] mazeAux = new bool[Maze.GetLength(0), Maze.GetLength(1)];
        
        CopyValues(mazeAux);
        LongestPath(mazeAux,ref cellF,ref max);

        return cellF;
    } 
    private void LongestPath(bool[,] booleanMask,ref Cells cellF,ref int max,int currentMax = 0,int x = 1,int y = 1) 
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for(int s = 0;s < dx.Length;s++)
        {
            int sumX = x + dx[s];
            int sumY = y + dy[s];

            if(Verification(sumX,sumY,booleanMask.GetLength(0),booleanMask.GetLength(1)) && !booleanMask[sumX,sumY])
            {
                if(!booleanMask[sumX,sumY])
                {
                    booleanMask[sumX,sumY]=true;
                    LongestPath(booleanMask,ref cellF,ref max,currentMax + 1,sumX,sumY);
                    booleanMask[sumX,sumY]=false;
                }
            }
        }
        if(currentMax > max)
        {
            max = currentMax;
            cellF.x = x;
            cellF.y = y;
        }
    }
```
que como tal fue un poco de recursividad de pila pasando la referencia del camino mas largo que se consigue y actualizando el valor de la celda. Ya con todo esto quedaria la generacion de trampas que fue hecha de forma aleatoria recorriendo toda la matriz de celdas y por cada celda que fuese un camino tiene una posibilidad de un 20% de que se genere una trampa en ese camino cosa que fue bastante sencilla de implementar, al ser marcada la celda como trampa se llama a un metodo que decide de forma aleatoria que tipo de trampa es el que se va generar que son 4 tipos de trampas diferentes los que se encuentran implementados (*Congelacion*, *Sonmifero*, *Teletransporte al inicio* y *Espinas*), una vez generadas todas las trampas se generan de igual manera las botellas de energia que incrementan las velocidades de las fichas y por ultimo las zonas de teletransporte que se generan en las zonas sin salida que no son ni la meta ni la salida. Y donde se esta guardando toda esta informacion que si es trampa,zona de teletransporte ect... pues para eso existe una de las clases más importantes de todo el proyecto que es la clase celda la cual se muesta a continuacion con solamente sus propiedades:
```
public class Cell: ScriptableObject
{
    public bool Obstacle = true;
    public bool Tramp = false;
    public bool cellTeleport = false;
    public bool EnergyCell = false;
    public bool Start = false;
    public bool FinishLine = false;
    public int PosX = 0;
    public int PosY = 0;
    public delegate void TrampEffect(params object[] param);
    public TrampEffect trampEffect;
    public TrampType trampType = TrampType.Unknown;
    public AudioClip AudioTramp;
    public Sprite ImageTramp;
    public Sprite SpriteDefault = Resources.Load<Sprite>("X");
    public Cell(bool Obstacle)
    {
        this.Obstacle = Obstacle;  
    }
//hay mas metodos a continuacion..
}
```
Con esta clase tenemos todas las caracteristicas que tendran los objetos de tipo celda los cuales seran ScriptableObject para asignarle propiedades de forma mas comoda a los objetos en escena ahi se encuentran todos los parametros que pueden llegar a tener una celda y demas cositas para el sonido e imagenes entre otras cositas. Ya con todo esto hecho era momento de generar el laberinto en escena para eso use otro metodo que se encargaba de en correspondencia con las propiedades de las celdas generar el laberinto, esto se hacia instanciando unos prefab prefiajados para las celdas que se modifican segun sus propiedades y mientras se instanciaban todos estos prefab en la escena a su ves las referencias de estos se guardaban en una matriz estatica para que en los otros apartados del juego fuese mas comodo trabajar con las celdas. 
Todo esto que se ha mencionado esta en la clase mas densa del proyecto y creo que ya deben tener una idea.... damas y caballeros con ustedes ```public class MazeGenerator : MonoBehaviour``` esta contiene todo lo necesario para generar el laberinto tanto los metodos anteriores como otros que la suplementan, a continucion se muestra un breve codigo de algunas de sus propiedades:
```
public class MazeGenerator : MonoBehaviour
{
    public GameObject CellObjectScene;
    GameObject CellObject;
    Image CellImage;
    const int Width = 13;
    const int Height = 13;
    const bool Wall = true; //representa una pared
    const bool Path = false; //representa un camino
    public static Cell[,] Maze = new Cell[Height + 2, Width + 2];
    public static Cells cellEnd;
    public static GameObject[,] gameObjects = new GameObject[Maze.GetLength(0),Maze.GetLength(1)] ;
    public static List<GameObject> TeleportZones = new List<GameObject>();
//En las restantes lineas estan los metodos... 
}    
```
Con la generacion del laberinto completada y todo lo relacionado con ella terminado ya en escena se tiene un laberinto con todo lo que se requiere para empezar a poner las fichas de los jugadores, para eso primeramente hay que manejar la logica de seleccion de fichas que en este caso tenemos 5 fichas para seleccionar por jugador para esto se utilizo una clase la cual se encargaria de gestionar todo lo relacionado con la seleccion de personajes y que despues de seleccionar los personajes guarda en una lista los que se seleccionaron por jugador, esta lista es estatica debido a que toda la informacion de los personajes seleccionados debe ser pasada a la clase *GameManager* para que gestione la lógica de las fichas en escena el código de la clase de que hablo es el siguiente: 
```
public class TeamManager : MonoBehaviour
{
    //Scrip encargado de guardar y gestionar la información del menú de selección de personajes
    public static List<Token> TeamsPlayer1 = new List<Token>();
    public static List<Token> TeamsPlayer2 = new List<Token>();
    public static string NamePlayer1;
    public static string NamePlayer2;
    public TMP_Text player1Name;
    public TMP_Text player2Name;
    public GameObject Music;
    public GameObject SoundObject;
    bool[] presset = {false, false, false, false, false, false, false, false, false, false};
//
```
Los metodos que tiene la clase se encargan de que cuando se seleccione un personaje se agregue de forma inmediata a la lista. Pero... ¿que caracteristicas tiene las fichas :( ? pues para saber eso tenemos que adentrarnos en otra de las clases principales de este proyecto la clase *Token* que es la que guarda las caracteristicas principales de las fichas en sus propedades y de la cual se muestra un fragmento de codigo a continuacion:
```
public class Token : ScriptableObject
{
    //Clase encargadad de la informacion de la ficha
    public string Name;
    public string InfoHability;
    int Speed;
    int Cooldown;
    public bool Available = false;
    //Posiciones actual
    public int PosX = 1; 
    public int PosY = 1;
    //Posicion anterior
    public int PrePosX = 1; 
    public int PrePosY = 1;
    //Velocidad y Cooldown actual
    public int CurrentSpeed;
    public int CurrentCooldown;
```
es importante aclarar que en este fragmento no se encuentra todo lo que realmente tiene pero si lo mas fundamental, esta clase complarte la caracteristica que tiene la clase *Cell* de heredar de **Scriptable Object** ya que las fichas son objetos en escenas que requieren de varias propiedades. Con todo el tema de las fichas controlado es momento de crear los jugadores, y para ello necesitamos tener las caracteristicas de los jugadores, y de esto se encarga la clase *Player* que ademas de sus propiedades contiene varios metodos muy utiles para el control de las caracteristicas y recursos que contiene cada jugador :
```
public class Player : MonoBehaviour
{
    public string Name;
    public bool Turn = false;
    public int TokensInFinishLine = 0;
    public int NumToken;
    public List<Token> TokensList = new List<Token>(); // Guarda las fichas disponibles para el jugador
    public List<GameObject> ObjectsInMaze = new List<GameObject>(); // Almacena las referencias de las fichas en escena
    public Player(string name,bool turn,List<Token> tokens)
    {
        Name = name;
        Turn = turn;    
        TokensList = tokens;
        NumToken = tokens.Count;
    }
    public void InstantiateTokens() //Instancia las fichas en escena
    {
        GameObject TokenRef = GameManager.StaticTokenInScene;

        foreach (var item in TokensList)
        {
            GameObject tokenInScene = Instantiate(TokenRef,TokenRef.transform.position,TokenRef.transform.rotation);
            tokenInScene.GetComponent<TokenDisplay>().Token = item;
            if(Name == "Player1")
            {
                Image image = tokenInScene.GetComponent<Image>();
                image.color = Color.white;
            }
            else
            {
                Image image = tokenInScene.GetComponent<Image>();
                image.color = Color.white;
            }
            tokenInScene.GetComponent<TokenDisplay>().Asignate();
            ObjectsInMaze.Add(tokenInScene); 
        }
        foreach (var item in ObjectsInMaze)
            item.SetActive(false);
    }
// Resto de metodos a continuacion
}
```
en esta clase como se puede ver se almacenan las fichas que posee cada jugador actualmente y asi como la referencia de las mismas en la lista *ObjectsInMaze* esta clase es uno de los complementos principales para la logica del juego, y en ella se encuentra el metodo que permite que se instancien las fichas en escena que como el nombre lo indica se llama *InstantiateTokens*.

## El desplazamiento

Las fichas se mueven utilizando las teclas W A S D para moverse en el laberinto hacia arriba, izquierda, abajo y derecha respectivamente para eso lo fundamental es verificar si no hay una pared y de ser asi moverse aun tambien hay que tener en cuenta si la ficha tiene la energia(velocidad) suficiente para seguir moviendose entre otras cosas y para conseguir todo esto se le asigna el scrip de movimiento que se va a mostrar a continucion al prefab de las fichas para que tenga la capacidad de desplazarse por la matriz de objetos referenciada en la clase *Maze Generator* :
```
public class TokenMove : MonoBehaviour
{
    public GameObject CurrentToken;
    private bool value = false;    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Displace(-1,0);
        
        if (Input.GetKeyDown(KeyCode.S))
            Displace(1,0);

        if (Input.GetKeyDown(KeyCode.D))
            Displace(0,1);

        if (Input.GetKeyDown(KeyCode.A))
            Displace(0,-1);
    }
    private void Displace(int x,int y) //Metoddo para efectuar el desplazamiento segun la tecla presionada
    {

        int currentPosX = CurrentToken.GetComponent<TokenDisplay>().Token.PosX;
        int currentPosY = CurrentToken.GetComponent<TokenDisplay>().Token.PosY;
        
        int dX = currentPosX + x;
        int dY = currentPosY + y;
    // aqui se encuentra el condicional principal que contiene todas las condiciones para que la ficha se pueda desplazar
    }
}
```
este script es ademas de el movimiento una vez este es efectuado verifica si el lugar a donde se movio es una trampa, zona de teletransporte, botella de energia o incluso la meta y maneja la logica de estos.

## Las habilidades de las fichas y las trampas

Tanto en la clase *Token* como en la clase *Cell* se encuentra un delegado el cual se encarga de almacenar un metodo estatico que se corresponde con las habilidades de cada ficha y de la funcionalidad de cada celda respectivamente, al ser activada una habilidad o una trampa se llamaran a estos metodos por lo que con esa idea fue mas facil manejar la logica de efectos en el juego. La clase estatica *TokenSkill* es la que contiene los 7 metodos estaticos de las de las fichas que controlan sus habilidades ademas de metodos auxiliares y la clase estatica *TrampEffects* es la encargada de manejar la logica de las 5 trampas que de ellas el teletransporte fue agregada a esta clase ya que me ahorraba muchas cosas la puse como si fuera un caso particular de trampa. Los delegados se encuentran en las clases *Token* y *Cell* mencionadas anteriormente.

## La clase GameManager

Con todos los pilares del juego listo es momento de hablar de la clase encargada de gestionar la logica del juego la clase *GameManager* esta se divide en varias secciones segun los momentos del juego. Al iniciar lo primero seria seleccionar la ficha para eso contamos con un boton que casualmete se llama *Seleccionar Ficha* en la clase implemente un metodo que lo que hacia era activar un panel que estaba previamente creado y dentro de el instanciar las fichas para seleccionar una para empezar a moverla y una vez seleccionada apretar el boton de confirmar y salir, que facil suena, la gracia me costo 3 dias XD, pero lo que hice fue inventarme unos botones que fueron los que se iban a instanciar en el panel y a estos asignarles la logica al presionarlos algo extremadamente complejo y con un codigo relativamente extenso que pueden leer en el repositorio como tal pero con eso se consigue que al apretar una ficha esta se muestre en escena (cabe aclarar que desde que empieza el juego todas las fichas estan instanciadas en la escena cortecia de la clase *Player*), y ademas sea desplazable si la situacion lo permite. La otra parte seria la logica de turnos que no es muy complicada solamente activa el boton de seleccionar ficha pero las fichas que salen son las del otro jugador y de igual forma hace lo mismo, en esta clase ademas se maneja la logica de todos los botones de la escena principal del que podemos destacar el boton de la habilidad el cual se encarga de llamar al metodo *ActivateSkillToken* que es el que permite activar la habildad de la ficha si tiene cooldown disponible. En el metodo Start se asignan valores indispesables a propiedades de la clase para el correcto funcionamiento del juego asi como la instaciacion inicial de todas las fichas de ambos jugadores pero desactivadas, en las propiedades directamente se le asigna a cada jugador lo que venia de la clase *TeamManager*, el metodo *Update* se encarga de comprobar la cantidad de fichas de cada jugador en la meta para en caso de que esten todas marcar al jugador como ganador y finalizar el juego. A continuacion se muestra un breve resumen del codigo de esta clase:
```
public class GameManager : MonoBehaviour
{
    public GameObject TonkenInScene; // Referencia a el prefab de la ficha
    public static GameObject StaticTokenInScene; // Referencia a la ficha de la escena
    public GameObject PanelInScene; // Referencia a el panel de seleccion de fichas
    public GameObject ChangeBotton; // Referencia a el boton de cambio de fichas
    public static Player player1 = new Player(TeamManager.NamePlayer1,true,TeamManager.TeamsPlayer1); // Instanciacion del jugador1 
    public static Player player2 = new Player(TeamManager.NamePlayer2,false,TeamManager.TeamsPlayer2); // Instanciacion del jugador2 
    List<GameObject> BottonList = new List<GameObject>(); // Lista de botones que se generan en la escena
    public static Player currentPlayer; //Referencia al jugador actual por turno

    void Start()    
    {
        currentPlayer = new Player("",false,TeamManager.TeamsPlayer1); //Asignacion del jugador actual
        StaticTokenInScene = TonkenInScene;
        ActivateSkillButtonStatic = ActivateSkillButton;
        player1.InstantiateTokens();
        player2.InstantiateTokens();
        currentPlayer = player1;
        NameInScenePlayer1.text = player1.Name+":";
        NameInScenePlayer2.text = player2.Name+":";
        SoundEndAStatic = SoundEndA;
        SoundMoveStatic = SoundMove;
    }
    void Update()
    {
        //Logica de verificar la cantidad de fichas en la meta por jugador y actualizar los valores de los mismos
    }
    // lo otro son los metodos mencionados anteriormente asi como algunos metodos auxiliares
}
```
