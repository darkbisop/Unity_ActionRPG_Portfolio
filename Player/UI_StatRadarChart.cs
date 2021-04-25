using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatRadarChart : MonoBehaviour
{
    private Stats stats;
    private CanvasRenderer canvasRenderer;
    private GameObject canvasRender;

    [SerializeField] private Material material;

    private void Awake()
    {
        canvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
        canvasRender = GameObject.Find("StatRadarChart").transform.Find("radarMesh").gameObject;
    }

    public void Setstats(Stats stats)
    {
        this.stats = stats;
        stats.OnStatChanged += Stats_OnStatChanged;
        UpdateStatVisual();
    }

    private void Stats_OnStatChanged(object sender, System.EventArgs e)
    {
        UpdateStatVisual();
    }

    private void UpdateStatVisual()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertice = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangle = new int[15];

        float angleIncrement = 360 / 5;

        float raderCharSize = 150f;
        Vector3 attackVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * raderCharSize * stats.GetStatAmountNormalize(Stats.Type.ATTACK);
        int attackIndex = 1;

        Vector3 defecneVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * raderCharSize * stats.GetStatAmountNormalize(Stats.Type.DEFENCE);
        int defenceIndex = 2;

        Vector3 healthVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * raderCharSize * stats.GetStatAmountNormalize(Stats.Type.HEALTH);
        int healthIndex = 3;

        Vector3 criVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * raderCharSize * stats.GetStatAmountNormalize(Stats.Type.CRI);
        int criIndex = 4;

        Vector3 speedVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * raderCharSize * stats.GetStatAmountNormalize(Stats.Type.STAMINA);
        int speedIndex = 5;

        vertice[0] = Vector3.zero;
        vertice[attackIndex] = attackVertex;
        vertice[defenceIndex] = defecneVertex;
        vertice[healthIndex] = healthVertex;
        vertice[criIndex] = criVertex;
        vertice[speedIndex] = speedVertex;

        triangle[0] = 0;
        triangle[1] = attackIndex;
        triangle[2] = defenceIndex;

        triangle[3] = 0;
        triangle[4] = defenceIndex;
        triangle[5] = healthIndex;

        triangle[6] = 0;
        triangle[7] = healthIndex;
        triangle[8] = criIndex;

        triangle[9] = 0;
        triangle[10] = criIndex;
        triangle[11] = speedIndex;

        triangle[12] = 0;
        triangle[13] = speedIndex;
        triangle[14] = attackIndex;

        mesh.vertices = vertice;
        mesh.uv = uv;
        mesh.triangles = triangle;

        canvasRender.GetComponent<CanvasRenderer>().SetMesh(mesh);
        canvasRender.GetComponent<CanvasRenderer>().SetMaterial(material, null);
    }
}
