# Clase 1 - Introducción

La idea es tener para el día jueves 4 un servicio REST pensado o investigado, o si lo tienen armado mucho mejor para ir tocando e integrando a lo que se vaya viendo durante el curso. Se pretende que sea muy sencillo.

El mismo intercambiaría un dato del tipo "Cliente" con el siguiente formato JSon.

```
{
  "id": 1,               (entero)
  "nombre": "Juan", (string)
  "apellido": "Perez", (string)
  "cuil": "20123456789", (string)
  "tipoDocumento": "DNI", (string)
  "nroDocumento": 12345678, (entero)
  "esEmpleadoBNA": true, (booleano)
  "paisOrigen": "ARGENTINA" (string)
}
```

Y debería tener mínimamente los siguientes métodos:

Método Get:  /clientes
Descripción: Devuelve todos los clientes presentes en la base de datos

Método Get:  /clientes/{id}  
Descripción: Obtiene el cliente del id especificado de la base de datos

Método Put:  /clientes
Descripción: Hace un update en la base de datos del objeto Cliente pasado en formato Json por el body

Método Post:  /clientes
Descripción: Inserta en la base de datos el cliente pasado en formato Json por el body.


La idea es hacerlo en NET 6 (equipos 1,2 ,3 y 5) o Java 8 (equipo 4). En principio la persistencia seria en memoria, pero si llegan a tiempo, estaría bueno que sea en SQL Server
Para acelerar el desarrollo se recomienda usar ORMs como Entity Framework Core o Hibernate. Dado que es un servicio muy chico, los equipos de NET, pueden considerar usar Minimal APIs para quienes manejen esta modalidad.

# Clase 2 - Serilog

para la siguiente siguiente sesión, se va a requerir lo siguiente:

 Sobre el servicio de la clase pasada, se debe agregar Serilog en el caso de las aplicaciones .NET. En las aplicaciones JAVA, alguna biblioteca de log estructurado como SLF4J o similar. En ambos casos se recomienda que la configuración de logueo se deje en el archivo de configuración (Serilog soporta esto de manera nativa). Minimamente, el log se debe generar en archivo o por consola.
Sería deseable también agregar el sink de serilog para loguear por http, a fin de enviarlo mas adelante al producto ELK.

Crear un servicio muy simple de transferencias intrabancarias (solo entre cuentas del BNA), muy similar al de la sesión anterior. El mismo deberá constar con los siguientes métodos:

Método POST:  /transferencias

Descripción: Inserta en la base de datos el objeto transferencia pasado por JSon en el body y devolverá el objeto RespuestaTransferencia


**Objeto Transferencia**
```
{
  "cuilOriginante": "20111111117",
  "cuilDestinatario": "27222222223",
  "cbuOrigen": "1111111111111111111111",
  "cbuDestino": "2222222222222222222222",
  "importe": 1000.00,
  "concepto": "VARIOS",
  "descripcion": "Pago de cuota"
}
```

**Objeto RespuestaTransferencia**
```
{
  "id": 0,
  "resultado": "FINALIZADA",
  "importe": 1000.00,
  "cuentaOrigen": "1111111111111",
  "cuentaDestino": "2222222222222"
}
```

Por el momento lo único que debe realizar este método es consumir al servicio de Clientes de la semana pasada via http  para validar que los CUILs existan, y de ser posible, persistir la Transferencia en una base de datos (recordar las recomendaciones de aislación de datos entre diferentes microservicios vistos en la sesión anterior).
En caso de que las validaciones estén ok, deberá devolver un código 201-Created con un objeto RespuestaTransferencia, en caso de que algun CUIL no se encuentre, deberá devolver un código 400-Bad Request, con el msje del error correspondiente.


Método Get:  /transferencias/{cbu}
Descripción: Lista todas las transferencias realizadas para el CBU pasado por URL, es decir, devuelve una lista de objetos Transferencia

Sería deseable que este servicio también loguee con Serilog.

# Clase 3 - Kafka

Paso en limpio lo pedido para la próxima sesión:
Realizar un microservicio, para el caso puede tratarse de una simple aplicación de consola, que esté escuchando en un tópico de kafka (CONSUMIDOR) a los msjes producidos por el microservicio de Clientes (PRODUCTOR), el cual deberá modificarse para que escriba en dicho tópico. Al recibir el msje, el programa CONSUMIDOR, puede imprimir por pantalla el msje. recibido o si quieren realizar algo mas como guardar en DB, como si fuera un sistema de auditoría.

La implementacion quedara a libre decisión, un posible uso sería que cuando se consuma algún método del servicio de Clientes (get, post o put) el mismo escriba un msje en la cola de kafka como para que el programa de consola lo tome

## Apache Kafka con docker compose

[Apaches Kafka con docker compose (producer y consumer)](https://www.youtube.com/watch?v=rhUuD0eA-EQ&list=PLqItYvLE1B78RmAJxvEHk3reDVEk2MM1l&index=2)

## Prueba desde terminal

1. **Creo los contenedores de zookeeper y kafka-broker-1**

```
cd Practicas/C#/Curso_Microservicios/Kafka

docker-compose up
```

Para ver los contenedores que se estan corriendo

```
docker ps
```

2. **Abro 2 terminales para simular un productor y un consumidor**

- En ambas terminales, me conecto al contenedor broker de kafka

```
docker exec -it kafka-broker-1 bash
```

### Terminal Productor

- Creo un topico con el nombre curso-test

```
kafka-topics --bootstrap-server kafka-broker-1:9092 --create --topic curso-test
```

- Para empezar a escribir mensajes dentro del topico curso-test

```
kafka-console-producer --bootstrap-server kafka-broker-1:9092 --topic curso-test
```

- Empiezo a escribir mensajes

### Terminal Consumidor

- Creacion del consumidor

```
kafka-console-consumer --bootstrap-server kafka-broker-1:9092 --topic curso-test --from-beginning
```

## Ejemplo Con Python

1. **Creo los contenedores de zookeeper y kafka-broker-1**

```
cd Practicas/C#/Curso_Microservicios/Kafka

docker-compose up
```

2. **Ejecuto el script del consumidor para que comienze a escuchar**

3. **Ejecuto el script del productor n veces para ver que el consumidor recibe el mensaje**

# Clase 4 - Opentelemetry

Implementar Instrumentación Automática con Opentelemetry en los servicios ya realizados

[OpenTelemetry 🔭 para la MONITORIZACIÓN y OBSERVABILIDAD en los sistemas distribuidos](https://www.youtube.com/watch?v=V8TvJK2hU54)
