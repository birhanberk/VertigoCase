using UnityEngine;

namespace Fail
{
    public class FailUIController : MonoBehaviour
    {
        [SerializeField] private VignetteController vignetteController;

        private void OnEnable()
        {
            vignetteController.PlayVignette();
        }

        private void OnDisable()
        {
            vignetteController.StopVignette();
        }
    }
}
