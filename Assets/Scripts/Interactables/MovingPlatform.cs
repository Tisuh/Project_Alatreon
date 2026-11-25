using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] platformPoints;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopTime = 2f;
    [SerializeField] private bool platformStops;

    private Rigidbody rb;
    private int currentPoint = 0;
    private bool isStopped;
    private Transform target;
    private Transform originalPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        target = platformPoints[currentPoint]; // Asignar el primer elemento del array como objetivo a seguir

        originalPoint = platformPoints[0];
        originalPoint.SetParent(null); // Desemparentar el original point para que no se mueva con la plataforma
    }

    private void FixedUpdate()
    {
        if (isStopped) return; // Si la plataforma esta detenida no se mueve ni cambia target

        MovePlatform();
        ChangeCurrentPoint();
    }

    void MovePlatform()
    {
        Vector3 newPosition = Vector3.MoveTowards(rb.position, platformPoints[currentPoint].position, speed * Time.fixedDeltaTime); // Calcula las posiciones a las que se movera la plataforma

        rb.MovePosition(newPosition); // Mueve la plataforma
    }

    void ChangeCurrentPoint()
    {
        if (Vector3.Distance(rb.position, target.position) < 0.01f)
        {
            if (currentPoint < platformPoints.Length - 1) // Sumar al indice del array y asignar nuevo target
            {
                currentPoint++;
                target = platformPoints[currentPoint];
            }
            else if (currentPoint == platformPoints.Length - 1) // Reiniciar valores del array al llegar al último elemento
            {
                currentPoint = 0;
                target = platformPoints[currentPoint];
            }

            if (platformStops) // Detiene la plataforma si se debe detener
            {
                isStopped = true;
                StartCoroutine(PlatformStopCrt());
            }
        }
    }

    IEnumerator PlatformStopCrt() // Detiene la plataforma durante el tiempo deseado
    {
        yield return new WaitForSeconds(stopTime);
        isStopped = false;
    }
}
