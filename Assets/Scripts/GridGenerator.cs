using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject gridSquare;
    [SerializeField]
    private GameObject panel;

    public float gridSpaceSize;
    public static GridGenerator singleton;

    private float width;
    private float height;
    private float area;

    private List<GameObject> grid;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        grid = new List<GameObject>();
        //gridSpaceSize /= 10;
        width = GetComponent<RectTransform>().rect.width / gridSpaceSize;
        height = GetComponent<RectTransform>().rect.height / gridSpaceSize;

        area = (width * height);

        panel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(gridSpaceSize, gridSpaceSize);

        for (float i = 0; i < area; i++)
        {
            grid.Add(Instantiate(gridSquare, panel.transform));
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject nodeAt(Vector3 pos)
    {
        for (int i = 0; i < grid.Count; i++)
        {
            if (grid[i].GetComponent<BoxCollider>().bounds.Contains(pos))
            {
                return grid[i];
            }
        }

        return null;
    }
}
