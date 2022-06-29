using UnityEngine;

namespace Level.Generation
{
    [RequireComponent(typeof(Building))]
    public class BuildingTestUtil : MonoBehaviour
    {
        [SerializeField] private Vector3 size;
        [SerializeField] private float startingZPosition;


        [ContextMenu("Set size")]
        private void SetSize()
        {
            var building = gameObject.GetComponent<Building>();
            building.Size = size;
        }
        
        [ContextMenu("Set starting z position")]
        private void SetStartingZPosition()
        {
            var building = gameObject.GetComponent<Building>();
            building.StartingZPosition = startingZPosition;
        }

        [ContextMenu("Print finish z position")]
        private void GetFinishZPosition()
        {
            var building = gameObject.GetComponent<Building>();
            Debug.Log($"Finish z position: {building.FinishZPosition}");
        }
    }
}