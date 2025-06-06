using UnityEngine;
using TMPro;  // Importa el espacio de nombres para TextMeshPro
using System.Collections.Generic;

public class TriggerVelocidad : MonoBehaviour
{
    public TextMeshPro velocidadTexto;


    public TextMeshProUGUI[] alturaTexts;
    public TextMeshProUGUI[] diametroTexts;
    public TextMeshProUGUI[] tiempoTexts;
    public TextMeshProUGUI[] rangoTexts;

    public float conversionFactor;

    public GameObject panelQuitar;
    public GameObject panelPoner;

    // Variables privadas

    public int currentStep = 0;
    private const float ALTURA_CONSTANTE = 0.92f; // Altura de la mesa en metros
    private const float DIAMETRO_CONSTANTE = 0.03f;
    private int pasoExperimento;
    private float tiempoEntrada;
    private float tiempoSalida;
    private float rango;
    private Vector3 sitioInicialBola;
    private Vector3 exitTriggerPosition;
    private Renderer ballRenderer;
    private Material ballMaterial;
    private Color currentBallColor;
    private List<GameObject> impactCircles = new List<GameObject>();
    private int impactCount = 0;
    private int colorIndex = 0;
    private LineRenderer lineRenderer;
    private List<GameObject> rulerMarks = new List<GameObject>();
    public Color[] colores = { Color.blue, Color.yellow, Color.green, new Color(1f, 0.647f, 0f) };
    private int maxTiros = 4;
    private int tirosRealizados = 0;

    // Start se encarga de la inicialización de componentes
    void Start()
    {
        InitializeVariables();
    }

    // Inicializa las variables y los componentes
    private void InitializeVariables()
    {
        pasoExperimento = 0;
        tiempoEntrada = 0f;
        ballRenderer = GetComponent<Renderer>();
        ballMaterial = ballRenderer.material;
        sitioInicialBola = transform.position;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material.color = currentBallColor;
        lineRenderer.enabled = false;
    }

    // Detecta la entrada al trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("fotocelula"))
        {
            tiempoEntrada = Time.time * 1000f; // Convertimos a milisegundos
        }
    }

    // Detecta la salida del trigger y calcula el tiempo de vuelo
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("fotocelula"))
        {
            exitTriggerPosition = transform.position;
            tiempoSalida = Time.time * 1000f;
            float tiempoTotal = tiempoSalida - tiempoEntrada;

            velocidadTexto.text = tiempoTotal.ToString("000") + " ms";
        }
    }

    // Detecta la colisión con el suelo y calcula el rango
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (tirosRealizados >= maxTiros)
            {
                return;
            }
            ContactPoint contact = collision.contacts[0];
            Vector3 collisionPoint = contact.point;

            if (exitTriggerPosition == Vector3.zero)
            {
                UnityEngine.Debug.LogError("Error: exitTriggerPosition no se guardó correctamente.");
                return;
            }
            tirosRealizados++;
            CalculateRange(collisionPoint);
            if (tirosRealizados >= maxTiros)
            {
                ballRenderer.enabled = false; // Ocultar la bola después del último tiro
                panelFinal();
            }
        }
    }

    private void panelFinal()
    {
        panelQuitar.SetActive(false);
        panelPoner.SetActive(true);
    }

    // Calcula el rango y crea los elementos visuales
    private void CalculateRange(Vector3 collisionPoint)
    {
        float distanceX = Mathf.Abs(collisionPoint.x - exitTriggerPosition.x);
        float ballScale = 0.0521115f;
        distanceX *= ballScale;
        distanceX *= conversionFactor;

        rango = distanceX;
        CreateCircle(collisionPoint, ballRenderer.bounds.size.x * 0.5f);
        ballRenderer.enabled = false;

        if (tirosRealizados < maxTiros)
        {
            ReturnBallToStartPosition();
        }
        DrawRulerLine(exitTriggerPosition, collisionPoint, distanceX, collisionPoint.y);
        UpdateCanvas();
    }

    // Crea un círculo en el impacto
    void CreateCircle(Vector3 position, float radius)
    {
        SetBallColor();
        int segments = 36;
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;
        float angleStep = 360.0f / segments;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            vertices[i] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 2 > segments) ? 1 : i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GameObject circle = new GameObject("Circle", typeof(MeshFilter), typeof(MeshRenderer));

        float circleHeight = position.y + 0.01f;
        circle.transform.position = new Vector3(position.x, circleHeight, position.z);
        circle.transform.rotation = Quaternion.Euler(0, 0, 180);
        circle.GetComponent<MeshFilter>().mesh = mesh;

        Material circleMaterial = new Material(Shader.Find("Unlit/Color"));
        circleMaterial.color = currentBallColor;
        circle.GetComponent<MeshRenderer>().material = circleMaterial;


        Rigidbody rb = circle.GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);

        Collider collider = circle.GetComponent<Collider>();
        if (collider != null) Destroy(collider);

        impactCircles.Add(circle);
        impactCount++;

        // Crear y colocar el texto de la distancia en verde encima del círculo
        CreateDistanceText(position, rango);
    }

    // Crea el texto que muestra la distancia del impacto
    void CreateDistanceText(Vector3 position, float distance)
    {
        GameObject distanceTextObject = new GameObject("DistanceText");
        TextMeshPro textMeshPro = distanceTextObject.AddComponent<TextMeshPro>();

        textMeshPro.text = $"{distance:F2} m";  // Mostrar la distancia con 2 decimales
        textMeshPro.color = Color.green;  // Establecer el color del texto a verde

        textMeshPro.fontSize = 1.0f;  // Ajusta el tamaño de la fuente
        textMeshPro.alignment = TextAlignmentOptions.Center;

        distanceTextObject.transform.position = position + Vector3.up * 0.2f;

        Canvas canvas = distanceTextObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 1;

        RectTransform rectTransform = distanceTextObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);
    }

    // Devuelve la bola a su posición inicial
    void ReturnBallToStartPosition()
    {
        transform.position = sitioInicialBola;
        ballRenderer.enabled = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // Establece el color de la bola de acuerdo con el índice
    void SetBallColor()
    {
        currentBallColor = colores[colorIndex % colores.Length];
        colorIndex++;
    }

    // Dibuja una línea que representa la regla entre dos puntos
    private void DrawRulerLine(Vector3 startPoint, Vector3 endPoint, float totalDistance, float lineHeight)
    {
        lineRenderer.enabled = true;


        Material markMaterial = new Material(Shader.Find("Unlit/Color"));
        markMaterial.color = currentBallColor;
        lineRenderer.material = markMaterial;


        lineRenderer.material.color = currentBallColor;

        // Aumentar el grosor de la línea
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        Vector3 startLinePosition = new Vector3(startPoint.x, lineHeight, startPoint.z);
        Vector3 endLinePosition = new Vector3(endPoint.x, lineHeight, endPoint.z);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startLinePosition);
        lineRenderer.SetPosition(1, endLinePosition);

        float markInterval = 0.1f;
        int numberOfMarks = Mathf.FloorToInt(totalDistance / markInterval);

        for (int i = 0; i <= numberOfMarks; i++)
        {
            float t = i / (float)numberOfMarks;
            Vector3 markPosition = Vector3.Lerp(startLinePosition, endLinePosition, t);
            CreateRulerMark(markPosition, currentBallColor);
        }
    }


    // Crea una marca en la regla
    private void CreateRulerMark(Vector3 position, Color color)
    {
        GameObject mark = new GameObject("RulerMark");
        LineRenderer markRenderer = mark.AddComponent<LineRenderer>();

        markRenderer.startWidth = 0.01f;
        markRenderer.endWidth = 0.01f;

        Material markMaterial = new Material(Shader.Find("Unlit/Color"));
        markMaterial.color = currentBallColor;  // Asegurar que las marcas tengan el mismo color
        markRenderer.material = markMaterial;


        float markSize = 0.05f;

        Vector3 start = position + Vector3.up * markSize * 0.5f;
        Vector3 end = position - Vector3.up * markSize * 0.5f;

        markRenderer.positionCount = 2;
        markRenderer.SetPosition(0, start);
        markRenderer.SetPosition(1, end);

        rulerMarks.Add(mark);
    }

    public void UpdateCanvas()
    {
        if (currentStep < alturaTexts.Length && currentStep < diametroTexts.Length &&
            currentStep < tiempoTexts.Length && currentStep < rangoTexts.Length)
        {
            // Usar las constantes para altura y diámetro sin unidades
            string formattedAltura = ALTURA_CONSTANTE.ToString("F4");
            string formattedDiametro = DIAMETRO_CONSTANTE.ToString("F4");

            // Convertir tiempo a int antes de usar D2, ya que es en milisegundos
            int tiempoInt = Mathf.RoundToInt(tiempoSalida - tiempoEntrada);
            string formattedTiempo = tiempoInt.ToString("D2");

            // Formatear rango con 3 decimales usando F3 y sin unidades
            string formattedRango = rango.ToString("F4");

            // Actualizar los textos en el Canvas sin unidades
            alturaTexts[currentStep].text = formattedAltura;
            diametroTexts[currentStep].text = formattedDiametro;
            tiempoTexts[currentStep].text = formattedTiempo;
            rangoTexts[currentStep].text = formattedRango;

            currentStep++;
        }
        else
        {
            Debug.LogWarning("Current step is out of bounds.");
        }
    }
}