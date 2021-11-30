using UnityEngine;
using UnityEngine.XR.ARFoundation;      // AR Foundation API
using UnityEngine.XR.ARSubsystems;      // AR Subsystems API
using System.Collections.Generic;       // 系統 集合 (清單 List)

// RequireComponent 要求元件，在第一次套用此腳本時會同時添加指定元件
// ARRaycastManager Foundation 提供的 AR 射線碰撞管理器
[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlaceObject : MonoBehaviour
{
    #region 欄位
    [Header("要放置的物件")]
    public GameObject goTarget;

    /// <summary>
    /// AR 射線碰撞管理元件
    /// </summary>
    private ARRaycastManager arManager;

    /// <summary>
    /// 射線碰撞到的物件清單
    /// </summary>
    private List<ARRaycastHit> arHit = new List<ARRaycastHit>();

    /// <summary>
    /// 點擊座標
    /// </summary>
    private Vector2 posClick;
    #endregion

    #region 事件
    private void Start()
    {
        arManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        Tap();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 點擊：判斷是否觸控、並取得觸控座標進行射線碰撞偵測最後生成物件
    /// </summary>
    private void Tap()
    {
        // 如果 玩家按下左鍵 (手機上會轉為觸控)
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 儲存觸控座標
            posClick = Input.mousePosition;
            // AR 管理器.射線碰撞(座標，碰撞清單，碰撞類型)
            arManager.Raycast(posClick, arHit, TrackableType.PlaneWithinPolygon);

            // 取得觸控座標並生成物件在此座標上
            Vector3 pos = arHit[0].pose.position;
            GameObject temp = Instantiate(goTarget, pos, Quaternion.identity);

            // 面向攝影機並將 X 與 Z 軸 歸零
            Vector3 angle = temp.transform.eulerAngles;
            temp.transform.LookAt(transform.position);
            angle.x = 0;
            angle.z = 0;
            temp.transform.eulerAngles = angle;
        }
    }
    #endregion
}
