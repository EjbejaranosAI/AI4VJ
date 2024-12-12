using UnityEngine;

public class Slots : MonoBehaviour
{
    public int melee;
    public GameObject meleePrefab;
    public int missile;
    public GameObject missilePrefab;
    public int magic;
    public GameObject magicPrefab;
    public GameObject ghost;

    void Start()
    {
        int front = 2 * melee / 3;
        int rear = melee - front;
        createRow(front, -2f, meleePrefab);
        createRow(missile, -4f, missilePrefab);
        createRow(magic, -6f, magicPrefab);
        createRow(rear, -8f, meleePrefab);
    }

    void createRow(int num, float z, GameObject pf)
    {
        float pos = 1 - num;
        for (int i = 0; i < num; ++i) {
            Vector3 position = ghost.transform.TransformPoint(new Vector3 (pos,0f,z));
            GameObject temp = (GameObject)Instantiate(pf, position, ghost.transform.rotation); 
            temp.AddComponent<Formation>();
            temp.GetComponent<Formation>().pos = new Vector3 (pos,0,z);
            temp.GetComponent<Formation>().target = ghost;
            pos += 2f;
        }
    }
}
