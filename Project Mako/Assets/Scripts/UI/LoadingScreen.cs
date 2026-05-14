using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mako
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _loadingCircle;
        [SerializeField] private TMP_Text _messageText;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Hide();
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public void ShowMessage(string text) => _messageText.text = text;

        private void Update()
        {
            _loadingCircle.transform.Rotate(Vector3.forward * Time.deltaTime * 100, Space.World);
        }
    }
}
