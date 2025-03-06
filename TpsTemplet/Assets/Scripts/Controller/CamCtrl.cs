using UnityEngine;

[System.Serializable]


//구면 좌표계를 활용한 카메라 제어
// https://srdeveloper.tistory.com/7#google_vignette
public class SphericalCoordinates
{
    private float radius, azimuth, elevation;

    public float Azimuth
    {
        get
        {
            return azimuth;
        }
        private set
        {
            azimuth = Mathf.Repeat(value, maxAzimuth_Rad - minAzimuth_Rad);
        }
    }

    public float Elevation
    {
        get { return elevation; }
        private set
        {
            elevation = Mathf.Clamp(value, minElevation_Rad, maxElevation_Rad);
        }
    }

    //Azimuth range
    public float minAzimuth_Deg = 0f;
    private float minAzimuth_Rad;

    public float maxAzimuth_Deg = 360f;
    private float maxAzimuth_Rad;

    //Elevation rages
    public float minElevation_Deg = -20f;
    private float minElevation_Rad;

    public float maxElevation_Deg = 40f;
    private float maxElevation_Rad;

    public SphericalCoordinates(Vector3 _camCoordinate, float _radius)
    {
        //방위각 라디안 값(최대, 최소)을 구한다.
        minAzimuth_Rad = Mathf.Deg2Rad * minAzimuth_Deg;
        maxAzimuth_Rad = Mathf.Deg2Rad * maxAzimuth_Deg;
        //앙각 라디안 값(최대, 최소)을 구한다.
        minElevation_Rad = Mathf.Deg2Rad * minElevation_Deg;
        maxElevation_Rad = Mathf.Deg2Rad * maxElevation_Deg;

        radius = _radius;
        //역함수로 방위각과 앙각을 구한다.
        Azimuth = Mathf.Atan2(_camCoordinate.z, _camCoordinate.x);
        Elevation = Mathf.Asin(_camCoordinate.y / radius);
    }

    public Vector3 toCartesian
    {
        get
        {
            //camera position = (r cosΦ cosθ, r sinΦ, r cosΦ sinθ)
            float t = radius * Mathf.Cos(Elevation);
            return new Vector3(t * Mathf.Cos(Azimuth),
                radius * Mathf.Sin(Elevation), t * Mathf.Sin(Azimuth));
        }
    }

    public SphericalCoordinates Rotate(float newAzimuth, float newElevation)
    {
        Azimuth += newAzimuth;
        Elevation += newElevation;
        return this;
    }
}

public class CamCtrl : MonoBehaviour
{
    private Vector3 lookPosition;
    public Vector3 targetCamPos = new Vector3(0, 0.5f, -4);


    public Transform Player;
    public SphericalCoordinates sphericalCoordinates;

    void Start()
    {
        //카메라 위치 계산을 위해 x, y, z좌표와 반지름 r값을 넘겨준다.
        sphericalCoordinates = new SphericalCoordinates(targetCamPos, Mathf.Abs(targetCamPos.z));
        transform.position = sphericalCoordinates.toCartesian + Player.position;

        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        Cursor.visible = false;                   //커서 안보이게
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * -1;
        float vertical = Input.GetAxis("Mouse Y") * -1;

        //플레이어 위치에서 조금더 오른쪽 위로 자리잡게 만든다.
        lookPosition = new Vector3(Player.position.x + 0.5f, Player.position.y + targetCamPos.y, Player.position.z);

        //플레이어 중심으로 구한 구면좌표를 카메라 위치에 적용
        transform.position = sphericalCoordinates.Rotate
            (horizontal * Time.deltaTime, vertical * Time.deltaTime).toCartesian + lookPosition;

        //목표지점으로 카메라를 보게함
        transform.LookAt(lookPosition);


        RaycastHit hit;
        Vector3 dir = transform.position - Player.transform.position;
        Debug.DrawRay(Player.transform.position, dir.normalized * dir.magnitude, Color.red);


    }
}
