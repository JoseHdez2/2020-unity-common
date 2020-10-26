using UnityEngine;
using ExtensionMethods;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    Vector3 origin = Vector3.zero;
    public float fovDegs = 90f;
    public int rayCount = 50;
    public float viewDistance = 50f;

    private float startingAngle;

    private Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update(){
        float angleIncrease = fovDegs / rayCount;
        float angle = startingAngle;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++){
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, VectorExtensions.Vector3FromAngle(angle), viewDistance, layerMask);
            if(raycastHit2D.collider == null){
                vertex = origin + VectorExtensions.Vector3FromAngle(angle) * viewDistance;
            } else {
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0){
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease; // we want CW and not CCW.
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin){
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection){
        startingAngle = aimDirection.ToAngle() - fovDegs / 2f;
    }
}
