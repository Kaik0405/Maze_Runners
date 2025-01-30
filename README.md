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
requisistos para ello para esto se necesita una matriz y una forma de identifar cuando es pared o no y ademas para manejar de mejor manera las direcciones se utiliza una estructura para indicar
las 4 posibles direcciones. Con todo esto se llega al metodo que se muestra a coontinuacion:

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
