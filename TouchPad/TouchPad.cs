using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPad : MonoBehaviour
{
    // 터치패드 오브젝트를 연결
    private RectTransform touchPad;

    // 터치 입력중에 방향 컨트롤러 영역 안에 있는 입력을 구분하기 위한 아이디
    private int touchId = -1;

    // 입력이 시작되는 좌표
    private Vector3 startPos = Vector3.zero;

    // 방향 컨트롤러가 원으로 움직이는 반지름
    public float dragRad = 60f;

    // 방향키가 변경되면 플레이어에게 신호를 보내기 위해 
    // 플레이어의 움직임을 관리하는 PlayerMovement와 연결
    public PlayerMovement player;

    // 버튼이 눌렸는지 체크하는 불 변수
    private bool isPressed = false;

    void Start()
    {
        // 터치패드의 RectTransform 오브젝트를 가져옴
        touchPad = GetComponent<RectTransform>();

        // 터치패드의 좌표를 가져옴
        startPos = touchPad.position;
    }

    public void ButtonDown()
    {
        // 버튼이 눌렸는지 확인
        isPressed = true;
    }

    public void ButtonUp()
    {
        isPressed = false;

        // 버튼이 떼어졌을때 패드의 좌표를 원래대로
        HandleInput(startPos);
    }

    private void FixedUpdate()
    {
        HandleTouchInput();

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
        HandleInput(Input.mousePosition);
#endif
    }

    private void HandleTouchInput()
    {
        // 터치 아이디를 매기기 위한 번호
        int i = 0;

        // 터치 입력은 한번에 여러개가 들어올수 있기 때문에 터치가 하나 이상 입력되면 실행
        if (Input.touchCount > 0)
        {
            // 각각의 터치 입력을 하나씩 조회
            foreach (Touch touch in Input.touches)
            {
                // 터치 아이디를 매기기 위한 번호를 1 증가
                i++;

                // 현재 터치 입력의 x,y 좌표를 구함
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);

                // 터치 입력이 시작되었다면 혹은 TouchPhase.Began이라면
                if (touch.phase == TouchPhase.Began)
                {
                    // 터치의 좌표가 현재 방향기 범위 내에 있다면
                    if (touch.position.x <= (startPos.x + dragRad))
                    {
                        // 터치 아이디를 기준으로 방향 컨트롤러를 조작
                        touchId = i;
                    }
                }

                // 터치 입력이 움직였거나 가만히 있다면
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    // 터치 아이디로 지정된 경우에만
                    if (touchId == i)
                    {
                        // 좌표 입력을 받아들인다
                        HandleInput(touchPos);
                    }
                }

                // 터치 입력이 끝났는데
                if (touch.phase == TouchPhase.Ended)
                {
                    // 입력 받고자 했던 터치 아이디라면
                    if (touchId == i)
                    {
                        // 터치 아이디를 해제
                        touchId = -1;
                    }
                }
            }
        }
    }

    private void HandleInput(Vector3 input)
    {
        // 버튼이 눌려진 상황이라면
        if (isPressed)
        {
            // 방향 컨트롤러 기준 좌표로부터 입력받은 좌표가 얼마나 떨어져있는지 확인
            Vector3 diffVector = (input - startPos);

            // 입력 지점과 기준좌표의 거리를 비교해서 최대치보다 크다면
            if (diffVector.sqrMagnitude > dragRad * dragRad)
            {
                // 방향 벡터의 거리를 1로 만듬
                diffVector.Normalize();

                // 그리고 방향컨트롤러는 최대치만큼만 움직이게 함
                touchPad.position = startPos + diffVector * dragRad;
            }
            // 입력지점과 기준좌표가 최대치보다 크지 않다면
            else  
            {
                // 현재 입력 좌표에 방향키를 이동
                touchPad.position = input;
            }
        }
        else
        {
            // 버튼에서 손이 떼어지면 방향키를 원래대로
            touchPad.position = startPos;
        }

        // 방향키와 기준 지점의 차이를 구함
        Vector3 diff = touchPad.position - startPos;

        // 방향키의 방향을 유지한채 거리를 나누어 방향을 구함
        Vector2 norDiff = new Vector3(diff.x / dragRad, diff.y / dragRad);
        
        if (player != null)
        {
            // 플레이어가 연결되어 있다면 플레이어에게 변경된 좌표 전달
            player.OnstickChanged(norDiff);
        }
    }
}
