using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class LoadingManager : SingletonMonoBehaviour<LoadingManager>
    {
        [SerializeField] private GameObject _loadingObj;

        public void SetActive(bool isActive)
        {
            _loadingObj.SetActive(isActive);
        }
    }
}
