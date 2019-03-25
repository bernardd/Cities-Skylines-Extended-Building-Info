using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace ExtendedBuildings
{
    public class ExtendedLoading : LoadingExtensionBase
    {
        static GameObject buildingWindowGameObject, serviceWindowGameObject;
        BuildingInfoWindow5 buildingWindow;
        ServiceInfoWindow2 serviceWindow;
        private LoadMode _mode;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            _mode = mode;

            buildingWindowGameObject = new GameObject("buildingWindowObject");
            this.buildingWindow = buildingWindowGameObject.AddComponent<BuildingInfoWindow5>();
            UIPanel info = attachWindow(
                this.buildingWindow,
                new PropertyChangedEventHandler<bool>(buildingInfo_eventVisibilityChanged),
                "(Library) ZonedBuildingWorldInfoPanel");
            this.buildingWindow.baseBuildingWindow = info.gameObject.transform.GetComponentInChildren<ZonedBuildingWorldInfoPanel>();

            serviceWindowGameObject = new GameObject("serviceWindowObject");
            this.serviceWindow = serviceWindowGameObject.AddComponent<ServiceInfoWindow2>();
            info = attachWindow(
                 this.serviceWindow,
                 new PropertyChangedEventHandler<bool>(serviceBuildingInfo_eventVisibilityChanged),
                 "(Library) CityServiceWorldInfoPanel");
            this.serviceWindow.baseBuildingWindow = info.gameObject.transform.GetComponentInChildren<CityServiceWorldInfoPanel>();
        }

        private UIPanel attachWindow(UIPanel p, PropertyChangedEventHandler<bool> vc, String parent)
        {
            var info = UIView.Find<UIPanel>(parent);
            p.transform.parent = info.transform;
            p.size = new Vector3(info.size.x, info.size.y);
            p.position = new Vector3(0, 12);
            info.eventVisibilityChanged += vc;

            return info;
        }

        public override void OnLevelUnloading()
        {
            if (_mode != LoadMode.LoadGame && _mode != LoadMode.NewGame)
                return;

            if (buildingWindow != null)
            {
                if (this.buildingWindow.parent != null)
                {
                    this.buildingWindow.parent.eventVisibilityChanged -= buildingInfo_eventVisibilityChanged;
                }
            }

            if (buildingWindowGameObject != null)
            {
                GameObject.Destroy(buildingWindowGameObject);
            }

            if (serviceWindowGameObject != null)
            {
                GameObject.Destroy(serviceWindowGameObject);
            }
        }

        void buildingInfo_eventVisibilityChanged(UIComponent component, bool value)
        {
            this.buildingWindow.isEnabled = value;
            if (value)
            {
                this.buildingWindow.Show();
            }
            else
            {
                this.buildingWindow.Hide();
            }
        }

        private void serviceBuildingInfo_eventVisibilityChanged(UIComponent component, bool value)
        {
            this.serviceWindow.isEnabled = value;
            if (value)
            {
                this.serviceWindow.Show();
            }
            else
            {
                this.serviceWindow.Hide();
            }
        }
    }
}
