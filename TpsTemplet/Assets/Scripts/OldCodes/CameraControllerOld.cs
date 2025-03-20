using UnityEditor.Overlays;
using UnityEngine;


//ī�޶� ��Ʈ�ѷ� ������
public class CameraControllerOld : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.5f, -4);// ī�޶� ��ġ ����
    public float cameraSpeed = 5f; // ī�޶� �̵� �ӵ�
    public Transform player; // �÷��̾� ĳ����
    public float sensitivity = 2.0f; // ���콺 ����
    public float zoomFOV = 10f; // �� �� FOV
    public float normalFOV = 20f; // �⺻ FOV

    private float pitch = 0f; // ���Ʒ� ȸ��
    private float yaw = 0f; // �¿� ȸ��
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; // ���콺 ���
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //// ��ǥ ��ġ ���
        Vector3 desiredPosition = player.position + offset;

        //// �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraSpeed);

        //// �׻� ĳ���͸� �ٶ󺸰� ���� 
        //���� ���Ը� �ؾ� �Ǽ� �켱 �ּ� ó��
        transform.LookAt(player);

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -30f, 60f);          // ���Ʒ� ���� ����

        // ī�޶� ��ġ �� ȸ�� ����
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position);

        //// ��Ŭ�� �� �� (FOV ����)
        //float targetFOV = Input.GetMouseButton(1) ? zoomFOV : normalFOV;
        //cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFOV, Time.deltaTime * 100f); // �� ������ �� ����


        //2025 03 10 - ī�޶�2 �ڵ� �Űܵ�
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //yaw += mouseX;
        //pitch += mouseY;
        ////�� �Ʒ� ���� ����
        //pitch = Mathf.Clamp(pitch, -30f, 45f);
        ////Vector3 direction = Player.position + CameraOffset;
        //Vector3 direction = PlayerLookObj.position + CameraOffset;

        ////�ε巴�� �����̱�
        ////transform.position = Vector3.SmoothDamp(transform.position, direction, ref mouseSensitivity, 0.3f);


        ////�÷��̾� ��ġ���� ���ݴ� ������ ���� �ڸ���� �����.
        ////lookPosition = new Vector3(Player.position.x + 0.5f, Player.position.y + CameraOffset.y, Player.position.z);
        //lookPosition = new Vector3(PlayerLookObj.position.x, PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);


        ////PlayerLookObj
        ////transform.position = Player.position  + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;
        //transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;


        //transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //transform.LookAt(lookPosition);

        ////������ �ð�ȭ �ϱ� ����
        ////Vector3 rayDirection = transform.position - Player.transform.position;
        ////Debug.DrawRay(Player.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);

        //Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
        //Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);


        //2025-03-10  22:02 - smoothDemp ���
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //yaw += mouseX;
        //pitch += mouseY;
        ////�� �Ʒ� ���� ����
        //pitch = Mathf.Clamp(pitch, -30f, 45f);
        //Vector3 direction = PlayerLookObj.position + CameraOffset;

        //float smoothPitch = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.x, pitch, ref pitchVelocity, smoothTime);
        //float smoothYaw = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, yaw, ref yawVelocity, smoothTime);
        //transform.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0);


        ////�÷��̾� ��ġ���� ���ݴ� ������ ���� �ڸ���� �����.
        ////lookPosition = new Vector3(PlayerLookObj.position.x , PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
        //transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;
        //lookPosition = new Vector3(PlayerLookObj.position.x, PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
        //transform.LookAt(lookPosition);


        ////transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        ////transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);



        ////transform.LookAt(lookPosition);

        ////������ �ð�ȭ �ϱ� ����
        //Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
        //Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);


    }
}
