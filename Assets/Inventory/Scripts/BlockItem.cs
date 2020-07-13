using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    public BlockType type;
    public ItemAsset itemAsset;
    [SerializeField] private bool isValid = true;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float force = 10f;
    [SerializeField] private float pickCD = 1f;
    private float timer = 0;
    private Rigidbody rBody;
    private GameObject target;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (rBody == null)
            rBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        rBody.MoveRotation(rBody.rotation * Quaternion.Euler(new Vector3(0, rotateSpeed, 0) * Time.deltaTime));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isValid || timer > 0)
            return;

        if(other.tag == "PlayerPickRange")
        {
            target = other.gameObject;
            isValid = false;
            StartCoroutine("BePicked");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isValid || timer > 0)
            return;

        if (other.tag == "PlayerPickRange")
        {
            target = other.gameObject;
            isValid = false;
            StartCoroutine("BePicked");
        }
    }
    public void InitBlockItem()
    {
        Block block = Block.blockDict[type];
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        //top
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        //bottom
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        //north
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        //south
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        //west
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        //east
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));

        for (int i = 0; i < 6; ++i)
        {
            triangles.AddRange(new int[] { i * 4, i * 4 + 1, i * 4 + 2, i * 4, i * 4 + 2, i * 4 + 3 });
        }

        uvs.AddRange(block.top.textureData.uvs);
        uvs.AddRange(block.bottom.textureData.uvs);
        uvs.AddRange(block.north.textureData.uvs);
        uvs.AddRange(block.south.textureData.uvs);
        uvs.AddRange(block.west.textureData.uvs);
        uvs.AddRange(block.east.textureData.uvs);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.mainTexture = WorldTexture.textureAtlas;
    }
    public void AddForce()
    {
        rBody.AddForce(Vector3.up * force);
    }
    private IEnumerator BePicked()
    {
        Vector3 direction = (target.transform.position - rBody.position).normalized;
        float distance = (target.transform.position - rBody.position).magnitude;
        while (distance > 0.5)
        {
            rBody.MovePosition(rBody.position + direction * moveSpeed * Time.deltaTime);
            direction = (target.transform.position - rBody.position).normalized;
            distance = (target.transform.position - rBody.position).magnitude;
            yield return 0;
        }

        bool result = false;
        foreach (KeyValuePair<InventoryID, InventoryManager> iManager in GameManager.instance.inventoryDict)
        {
            result = iManager.Value.AddItem(itemAsset);
            if (result)
                break;
        }
        if (result)
            Destroy(gameObject);
        else
        {
            timer = pickCD;
            isValid = true;
            StartCoroutine("WaitPickCD");
        }
    }
    private IEnumerator WaitPickCD()
    {
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return 0;
        }
        timer = 0;
    }
}
