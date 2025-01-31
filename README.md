### **Introducción**

Maze_Runners es un juego desarrollado en el motor de juegos Unity, el cual consiste en una carrera a través de un laberinto entre dos jugadores, con el objetivo de llevar cada jugador todas sus fichas a la meta antes que el jugador rival. La generación de laberintos fue realizada de forma dinámica usando el algoritmo de Prim, del cual entraremos en profundidad un poco más adelante. La temática de este juego está basada en el anime *Fairy Tail*, vinculada a la carrera de las 24 horas del gremio, pero en este caso la carrera es dentro de un laberinto con trampas, teletransportadores y demás cositas...

### Desarrollo del Proyecto

Este proyecto lo tuve que dividir en varias fases para su desarrollo, así que empecé por la parte principal que es **El Laberinto**. En un primer momento, no tenía ni idea de cómo hacer un algoritmo que me generara laberintos de forma aleatoria, entonces me puse a investigar y existían varios métodos para conseguir este resultado, muchos de los cuales eran bastante turbios. Pero encontré uno que me encantó, especialmente porque era muy legible y usaba la técnica de recursividad conocida como *Backtracking* (este algoritmo es una variante del algoritmo de Prim). Lo que hace es abrir caminos de forma aleatoria, empezando por una celda en específico que está representada de forma matricial. Estos caminos se abren en cuatro direcciones posibles, cumpliendo ciertos requisitos para ello. Para esto se necesita una matriz y una forma de identificar cuándo es pared o no, y además, para manejar de mejor manera las direcciones, se utiliza una estructura para indicar las cuatro posibles direcciones. Con todo esto se llega al método que se muestra a continuación:
```csharp
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
De esta forma se consigue generar el laberinto de forma aleatoria, abriendo caminos de forma recursiva y sin dejar zonas inaccesibles. Como quería una meta, no sabía dónde la iba a colocar ni dónde sería más conveniente hacerlo, por el simple hecho de que el laberinto siempre iba a ser diferente. Por lo que se me ocurrió que podía hacerlo buscando el camino más largo desde donde empieza el laberinto, que en este caso es la posición [1,1] de la matriz. Es decir, buscar el lugar más lejano al que se puede llegar desde esa posición. Era consciente de que usando un DFS o el algoritmo de Lee sería algo sencillo; solo con investigar un poco y adaptar la idea conseguiría el resultado. Pero decidí hacerlo con lo que sabía, así que utilicé nuevamente *backtracking* para buscar de forma recursiva el camino más largo. La gracia me salió en acostarme a las 4 a.m. :| pero se logró el resultado y ya tenía el punto más lejano con estos métodos.
```csharp
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
Como tal, fue un poco de recursividad de pila, pasando la referencia del camino más largo que se consigue y actualizando el valor de la celda. Ya con todo esto, quedaría la generación de trampas, que fue hecha de forma aleatoria, recorriendo toda la matriz de celdas. Por cada celda que fuese un camino, tiene una posibilidad del 20% de que se genere una trampa en ese camino, cosa que fue bastante sencilla de implementar. Al ser marcada la celda como trampa, se llama a un método que decide de forma aleatoria qué tipo de trampa es la que se va a generar. Hay cuatro tipos de trampas diferentes que se encuentran implementadas: *Congelación*, *Somnífero*, *Teletransporte al inicio* y *Espinas*. Una vez generadas todas las trampas, se generan de igual manera las botellas de energía que incrementan las velocidades de las fichas. Por último, las zonas de teletransporte se generan en las zonas sin salida que no son ni la meta ni la salida. 

¿Y dónde se está guardando toda esta información, que si es trampa, zona de teletransporte, etc.? Para eso existe una de las clases más importantes de todo el proyecto, que es la clase Celda(*class Cell*), la cual se muestra a continuación con solamente sus propiedades:
```csharp
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
Con esta clase tenemos todas las características que tendrán los objetos de tipo celda, los cuales serán *ScriptableObject* para asignarles propiedades de forma más cómoda a los objetos en escena. Ahí se encuentran todos los parámetros que pueden llegar a tener una celda, así como otros elementos para el sonido, imágenes, entre otras cosas. 

Ya con todo esto hecho, era momento de generar el laberinto en escena. Para eso, usé otro método que se encargaba, en correspondencia con las propiedades de las celdas, de generar el laberinto. Esto se hacía instanciando unos prefabs predefinidos para las celdas, que se modifican según sus propiedades. Mientras se instanciaban todos estos prefabs en la escena, a su vez se guardaban las referencias de estos en una matriz estática para que en los otros apartados del juego fuese más cómodo trabajar con las celdas.

Todo esto que se ha mencionado está en la clase más densa del proyecto, y creo que ya deben tener una idea... Damas y caballeros, con ustedes:

```csharp
public class MazeGenerator : MonoBehaviour
```
Esta clase contiene todo lo necesario para generar el laberinto, tanto los métodos anteriores como otros que la suplementan. A continuación, se muestra un breve código de algunas de sus propiedades:
```csharp
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
Con la generación del laberinto completada y todo lo relacionado con ella terminado, ya en escena se tiene un laberinto con todo lo que se requiere para empezar a poner las fichas de los jugadores. Para eso, primeramente hay que manejar la lógica de selección de fichas. En este caso, tenemos cinco fichas para seleccionar por jugador. Para esto, se utilizó una clase que se encargaría de gestionar todo lo relacionado con la selección de personajes y que, después de seleccionar los personajes, guarda en una lista los que se seleccionaron por jugador. Esta lista es estática debido a que toda la información de los personajes seleccionados debe ser pasada a la clase *GameManager*, para que gestione la lógica de las fichas en escena. El código de la clase de la que hablo es el siguiente:
```csharp
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
Los métodos que tiene la clase se encargan de que, cuando se seleccione un personaje, se agregue de forma inmediata a la lista. Pero... ¿qué características tienen las fichas? :( Para saber eso, tenemos que adentrarnos en otra de las clases principales de este proyecto: la clase *Token*, que es la que guarda las características principales de las fichas en sus propiedades. A continuación, se muestra un fragmento de código de la misma:
```csharp
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
Es importante aclarar que en este fragmento no se encuentra todo lo que realmente tiene, pero sí lo más fundamental. Esta clase comparte la característica que tiene la clase *Cell* de heredar de **Scriptable Object**, ya que las fichas son objetos en escena que requieren de varias propiedades. Con todo el tema de las fichas controlado, es momento de crear los jugadores. Para ello, necesitamos tener las características de los jugadores, y de esto se encarga la clase *Player*, que además de sus propiedades, contiene varios métodos muy útiles para el control de las características y recursos que contiene cada jugador:
```csharp
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
En esta clase, como se puede ver, se almacenan las fichas que posee cada jugador actualmente, así como la referencia de las mismas en la lista *ObjectsInMaze*. Esta clase es uno de los complementos principales para la lógica del juego, y en ella se encuentra el método que permite que se instancien las fichas en escena, que, como el nombre lo indica, se llama *InstantiateTokens*.

## El desplazamiento

Las fichas se mueven utilizando las teclas W, A, S y D para moverse en el laberinto hacia arriba, izquierda, abajo y derecha, respectivamente. Para eso, lo fundamental es verificar si no hay una pared y, de ser así, moverse. También hay que tener en cuenta si la ficha tiene la energía (velocidad) suficiente para seguir moviéndose, entre otras cosas. Para conseguir todo esto, se le asigna el script de movimiento, que se va a mostrar a continuación, al prefab de las fichas para que tenga la capacidad de desplazarse por la matriz de objetos referenciada en la clase *Maze Generator*:
```csharp
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
Este script, además del movimiento, una vez que este es efectuado, verifica si el lugar al que se movió es una trampa, zona de teletransporte, botella de energía o incluso la meta, y maneja la lógica de estos.

## Las habilidades de las fichas y las trampas

Tanto en la clase *Token* como en la clase *Cell* se encuentra un delegado, el cual se encarga de almacenar un método estático que se corresponde con las habilidades de cada ficha y con la funcionalidad de cada celda, respectivamente. Al ser activada una habilidad o una trampa, se llamarán a estos métodos, por lo que con esa idea fue más fácil manejar la lógica de efectos en el juego. 

La clase estática *TokenSkill* es la que contiene los 7 métodos estáticos de las fichas que controlan sus habilidades, además de métodos auxiliares. La clase estática *TrapEffects* es la encargada de manejar la lógica de las 5 trampas; de ellas, el teletransporte fue agregado a esta clase, ya que me ahorraba muchas cosas, y la puse como si fuera un caso particular de trampa. Los delegados se encuentran en las clases *Token* y *Cell* mencionadas anteriormente.

## La clase GameManager

Con todos los pilares del juego listos, es momento de hablar de la clase encargada de gestionar la lógica del juego: la clase *GameManager*. Esta se divide en varias secciones según los momentos del juego. Al iniciar, lo primero sería seleccionar la ficha; para eso contamos con un botón que casualmente se llama *Seleccionar Ficha*. En la clase implementé un método que lo que hacía era activar un panel que estaba previamente creado y, dentro de él, instanciar las fichas para seleccionar una y empezar a moverla. Una vez seleccionada, se debe apretar el botón de confirmar y salir. Que fácil suena, pero me costó 3 días XD. 

Lo que hice fue inventarme unos botones que se iban a instanciar en el panel y a estos asignarles la lógica al presionarlos, algo extremadamente complejo y con un código relativamente extenso que pueden leer en el repositorio. Con esto se consigue que al apretar una ficha, esta se muestre en escena (cabe aclarar que desde que empieza el juego todas las fichas están instanciadas en la escena gracias a la clase *Player*), y además sea desplazable si la situación lo permite.

La otra parte sería la lógica de turnos, que no es muy complicada; simplemente activa el botón de seleccionar ficha, pero las fichas que salen son las del otro jugador y hace lo mismo. En esta clase, además, se maneja la lógica de todos los botones de la escena principal, del que podemos destacar el botón de la habilidad, el cual se encarga de llamar al método *ActivateSkillToken*, que permite activar la habilidad de la ficha si tiene cooldown disponible.

En el método `Start`, se asignan valores indispensables a propiedades de la clase para el correcto funcionamiento del juego, así como la instanciación inicial de todas las fichas de ambos jugadores, pero desactivadas. En las propiedades, directamente se le asigna a cada jugador lo que venía de la clase *TeamManager*. El método `Update` se encarga de comprobar la cantidad de fichas de cada jugador en la meta para, en caso de que estén todas, marcar al jugador como ganador y finalizar el juego. 

A continuación, se muestra un breve resumen del código de esta clase:
```csharp
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

### Conclusiones y Agradecimientos

Todo esto es el resultado de algunas noches sin dormir y unos cuantos días dedicándole el tiempo necesario, sin dejar de lado las 200 mil cosas que uno tiene siempre, como todos. Al final, no sé ni cómo, pero logré terminarlo bastante rápido y sin mucho trabajo, aunque eso no quita que fue un reto increíble. El hecho de instanciar botones, por poner un ejemplo, era algo que ni creía posible, y fue una lucha constante.

Las ideas me escaseaban, pero con los comentarios de algunos de mis compañeros pude mejorar algunas cosas y el resultado final es bastante decente. Siento que incluso pudo ser mejor, pero no puedo pedir más. Me gustaría agradecerle a *Manuel (El Tanke)*, *Elizabeth*, *Julio*, que me dieron ideas bastante útiles al hablar con ellos y explicarles muchas cosas. También a *Cristian*, que me dio ideas geniales y fue uno de los beta testers, y a la fan número uno de mi juego, que no le pude ganar ni una sola vez y que me ayudó a encontrar los bugs finales, así como las mejoras extras en el apartado audiovisual: mi novia *Laura* (me ganó 3 veces seguidas XD). Después de todos estos meses, el resultado en parte también se lo debo a todos ellos.

### Como Jugar

Una vez ejecutado el juego, aparecerá un menú sencillo en el que lo único que tendrán que hacer es apretar el botón de jugar. A continuación, saldrá la escena de selección de personajes. Una vez ahí, los jugadores podrán poner su nombre encima del panel de personajes que decidan. Cabe aclarar que cada jugador tendrá solamente las fichas asociadas a su panel disponibles, y que al seleccionarlas, el marco se pondrá en dorado. Para iniciar el juego, deberán apretar en confirmar teniendo la misma cantidad de fichas cada jugador.

Al entrar al juego, se encontrarán con el laberinto en el centro y dos botones en la parte superior izquierda de la pantalla, los cuales son los del cambio de turno y selección de ficha. Al apretar el botón de seleccionar ficha, saldrá un panel de selección con las fichas disponibles. Se le debe hacer clic a la ficha que se desea usar y, a continuación, apretar el botón de confirmar. Si la ficha no tiene cooldown, tendrá el botón de habilidad disponible y, al apretarlo, la ficha usará su habilidad.

Para moverse, deberán apretar las teclas W, A, S y D, como es utilizado comúnmente, y el objetivo será llegar a la celda roja, que es la meta. En el laberinto hay trampas esparcidas, pero no visibles, y hay un 20% de posibilidades de caer en una. También están las botellas de energía, que aumentan la estamina de la ficha en 3. En las zonas sin salidas se encuentran las zonas de teletransporte, que te envían a una zona de teletransporte aleatoria, incluyendo la misma en la que se entra.

Cuando todas las fichas del jugador logren llegar al final, el jugador será declarado ganador. En la parte inferior se muestra un contador de las fichas en la meta actualmente. Con todo esto, lo único que quedaría sería instalarlo.

### Instalación y Links de Descarga

Para instalar el juego se deben seguir los siguientes pasos una vez descargado el archivo .rar que aparece al final:

- Descomprimir el .rar.
- Ejecutar el ícono en rojo que tiene el nombre del proyecto.
- Por último... ¡a jugar! :)

Para descargar el archivo del juego (archivo .rar) deberá hacer clic [Aquí](https://github.com/Kaik0405/Maze_Runners/releases/download/v1.0/Maze_Runners_Version.1.0.rar).



