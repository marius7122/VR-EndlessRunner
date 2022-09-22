using UnityEngine;

namespace Level.Generation.Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private Vector3 _size = Vector3.one;
        public Vector3 Size
        {
            get => _size;
            set 
            {
                _size = value;
                transform.localScale = _size;
                meshRenderer.material.mainTextureScale = new Vector2(_size.x, _size.z);
            }
        }
        
        public float StartingZPosition
        {
            get => transform.position.z;
            set
            {
                var currentPosition = transform.position;
                currentPosition.z = value;
                transform.position = currentPosition;
            }
        }

        public float FinishZPosition => transform.position.z + _size.z;

            
        private void Awake()
        {
            _size = transform.localScale;
        }
    }
}
