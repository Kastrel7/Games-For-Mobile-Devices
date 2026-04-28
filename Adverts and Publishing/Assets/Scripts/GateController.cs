using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateController : MonoBehaviour
{
    public float speed = 1.5f;
    public Material positive;
    public Material negative;

    private Renderer gateRenderer;
    private GameObject[] poles;
    private TextMeshPro numberText;
    private bool hasChanged = false;

    private int num;

    void Start()
    {
        gateRenderer = GameObject.FindWithTag("Gate").GetComponent<Renderer>();
        poles = GameObject.FindGameObjectsWithTag("Pole");
        numberText = GameObject.FindWithTag("Text").GetComponent<TextMeshPro>();
    }

    void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * speed);
        ChangeGate();
    }

    void ChangeGate()
    {
        if (transform.position.z <= 15 && !hasChanged)
        {
            hasChanged = true;
            num = Random.Range(-10, 5);
            ChangeMaterialAndText(num);

        }
    }

    void ChangeMaterialAndText(int number)
    {
        Material mat = null;
        if (number > 0)
            mat = positive;
        else if (number <= 0)
            mat = negative;

        Color transparent = mat.color;
        transparent.a = 100f / 255f;

        foreach (GameObject pole in poles)
        {
            pole.GetComponent<Renderer>().material = mat;
        }
        gateRenderer.material = mat;
        gateRenderer.material.color = transparent;

        numberText.text = number.ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && hasChanged)
        {
            num++;
            ChangeMaterialAndText(num);
        }
        if (other.gameObject.tag == "Soldier")
        {
            SoldierGridManager.Instance.AddSoldiers(num);
            Destroy(gameObject);
        }
    }
}
