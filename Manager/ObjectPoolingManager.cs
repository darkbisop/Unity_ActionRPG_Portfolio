using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager instance;

    public static ObjectPoolingManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPoolingManager>();
            }

            return instance;
        }
    }

    //void Awake() { st = this; }
    //// 게임종료 후 메모리 날려버림.
    //void OnDestroy()
    //{
    //    MemoryDelete();
    //}

    public GameObject[] Origin;        
    [SerializeField] private List<GameObject> Manager;
    public Transform pos;
    void Start()
    {
        SetObject("SKill_Destroy", 1);
        //int Count = Origin.Length; 
        //string Name = "NoData";
        //for (int i = 0; i < Count; i++)
        //{
        //    switch (i)
        //    {
        //        case 0: Name = "Slash1"; break;
        //        case 1: Name = "Slash2"; break;
        //        case 2: Name = "Slash3"; break;
        //        case 3: Name = "SlashEx"; break;
        //        case 4: Name = "Potion"; break;
        //    }

        //    // 오브젝트 생성.
        //    SetObject(Origin[i], 1, Name);
        //}
    }

    // 오브젝트를 받아 생성.
    public void SetObject(GameObject _Obj, int _Count, string _Name, Transform pos)
    {
        for (int i = 0; i < _Count; i++)
        {
            GameObject obj = Instantiate(_Obj) as GameObject;
            obj.transform.name = _Name;                    
            obj.transform.parent = pos.transform;
            obj.SetActive(false);                           
            //obj.transform.parent = transform;              
            Manager.Add(obj);                            
        }
    }

    public void SetObject(GameObject _Obj, int _Count, string _Name)
    {
        for (int i = 0; i < _Count; i++)
        {
            GameObject obj = Instantiate(_Obj) as GameObject;
            obj.transform.name = _Name;
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
            obj.transform.parent = transform;              
            Manager.Add(obj);
        }
    }

    public void SetObject(string _Name, int _Count, Transform pos)
    {
        GameObject obj = null;
        int Count = Origin.Length;
        for (int i = 0; i < Count; i++)
        {
            if (Origin[i].name == _Name)
            {
                obj = Origin[i];
                obj.transform.position = pos.transform.position;
            }
        }

        SetObject(obj, _Count, _Name, pos);
    }

    public void SetObject(string _Name, int _Count)
    {
        GameObject obj = null;
        int Count = Origin.Length;
        for (int i = 0; i < Count; i++)
        {
            if (Origin[i].name == _Name)
            {
                obj = Origin[i];
                obj.transform.position = pos.transform.position;
            }
        }

        SetObject(obj, _Count, _Name);
    }

    // 필요한 오브젝트를 찾아 반환.
    public GameObject GetObject(string _Name, Transform Pos)
    {
        // 리스트가 비어있으면 종료.
        if (Manager == null)
            return null;

        int Count = Manager.Count;
        for (int i = 0; i < Count; i++)
        {
            // 이름이 같지 않으면.
            if (_Name != Manager[i].name)
                continue;

            GameObject Obj = Manager[i];
            
            // 활성화가 되어있다면.
            if (Obj.activeSelf == false)
            {
                Obj.transform.position = Pos.transform.position;
                Obj.SetActive(true);
                return Manager[i];
            }
        }
        return null;
    }

    public GameObject GetObject(string _Name, Vector3 Pos)
    {
        // 리스트가 비어있으면 종료.
        if (Manager == null)
            return null;

        int Count = Manager.Count;
        for (int i = 0; i < Count; i++)
        {
            // 이름이 같지 않으면.
            if (_Name != Manager[i].name)
                continue;

            GameObject Obj = Manager[i];

            // 활성화가 되어있다면.
            if (Obj.activeSelf == false)
            {
                Obj.transform.position = Pos;
                Obj.SetActive(true);
                return Manager[i];
            }
        }
        return null;
    }

    public GameObject GetObject(string _Name)
    {
        // 리스트가 비어있으면 종료.
        if (Manager == null)
            return null;

        int Count = Manager.Count;
        for (int i = 0; i < Count; i++)
        {
            // 이름이 같지 않으면.
            if (_Name != Manager[i].name)
                continue;

            GameObject Obj = Manager[i];

            // 활성화가 되어있다면.
            if (Obj.activeSelf == false)
            {
                //Obj.SetActive(true);
                return Manager[i];
            }
        }
        return null;
    }

    public GameObject OffObject(string _Name)
    {
        // 리스트가 비어있으면 종료.
        if (Manager == null)
            return null;

        int Count = Manager.Count;
        for (int i = 0; i < Count; i++)
        {
            // 이름이 같지 않으면.
            if (_Name != Manager[i].name)
                continue;

            GameObject Obj = Manager[i];

            // 활성화가 되어있다면.
            if (Obj.activeSelf == true)
            {
                Obj.SetActive(false);
                return Manager[i];
            }
        }
        return null;
    }

    //// 메모리 삭제.
    //public void MemoryDelete()
    //{
    //    if (Manager == null)
    //        return;

    //    int Count = Manager.Count;

    //    for (int i = 0; i < Count; i++)
    //    {
    //        GameObject obj = Manager[i];
    //        GameObject.Destroy(obj);
    //    }
    //    Manager = null;
    //}

    // 원하는 오브젝트를 만든다.
    //public void CreateObject(string _Name, int _Count)
    //{
    //    if (Manager == null)
    //        Manager = new List<GameObject>();

    //    int Count = Origin.Length;
    //    for (int i = 0; i < Count; i++)
    //    {
    //        GameObject obj = Origin[i];
    //        if (obj.name == _Name)
    //        {
    //            SetObject(obj, _Count, _Name);   // 총알을 생성.
    //            break;
    //        }
    //    }
    //}
}
