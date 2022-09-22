using UnityEngine;

namespace Level.Generation.Buildings
{
    public class ReferenceBuilding : MonoBehaviour
    {
        [SerializeField] private Vector3 size;
        public Vector3 Size => size;

        public float MaxZ => transform.localPosition.z + Size.z;
        
        [ContextMenu("Update size")]
        private void ComputeSize()
        {
            var meshFilter = GetComponentInChildren<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogWarning($"[ReferenceBuilding] MeshFilter not found for {name}, size won't be calculated properly");
            }
            else
            {
                var meshTransform = meshFilter.transform;
                size = Vector3.Scale(meshFilter.sharedMesh.bounds.size, meshTransform.localScale);
                
                if (Mathf.Approximately(meshTransform.eulerAngles.y, 90f) 
                    || Mathf.Approximately(meshTransform.eulerAngles.y, 270f))
                {
                    size = new Vector3(size.z, size.y, size.x);
                }
            }
        }

        [ContextMenu("Center object in parent")]
        private void CenterObjectInParent()
        {
            var meshFilter = GetComponentInChildren<MeshFilter>();
            if (meshFilter != null)
            {
                var meshGo = meshFilter.gameObject;
                var meshScale = meshFilter.transform.localScale;
                var meshSize = meshFilter.sharedMesh.bounds.size;
                meshGo.transform.localPosition = Vector3.up * meshSize.y * meshScale.y * 0.5f + 
                                                 Vector3.forward * meshSize.x * meshScale.x * 0.5f;
            }
        }
    }
}
