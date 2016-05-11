using UnityEngine;

//[RequireComponent(typeof(Light))]
namespace Assets.Scripts
{
    public class LightEffect : MonoBehaviour
    {
        public enum FlickerinLightStyles { CampFire = 0, Fluorescent = 1 };
        public FlickerinLightStyles FlickeringLightStyle = FlickerinLightStyles.CampFire;

        // Campfire Methods
        public enum CampfireMethods { Intensity = 0, Range = 1, Both = 2 };
        public CampfireMethods CampfireMethod = CampfireMethods.Intensity;

        // Intensity Styles
        public enum CampfireIntesityStyles { Sine = 0, Random = 1 };
        public CampfireIntesityStyles CampfireIntesityStyle = CampfireIntesityStyles.Random;

        // Range Styles
        public enum CampfireRangeStyles { Sine = 0, Random = 1 };
        public CampfireRangeStyles CampfireRangeStyle = CampfireRangeStyles.Random;

        // Base Intensity Value
        public float CampfireIntensityBaseValue = 0.5f;
        // Intensity Flickering Power
        public float CampfireIntensityFlickerValue = 0.1f;

        // Base Range Value
        public float CampfireRangeBaseValue = 10.0f;
        // Range Flickering Power
        public float CampfireRangeFlickerValue = 2.0f;

        // If Style is Sine
        private float _campfireSineCycleIntensity = 0.0f;
        private float _campfireSineCycleRange = 0.0f;

        // "Glow" Speeds
        public float CampfireSineCycleIntensitySpeed = 5.0f;
        public float CampfireSineCycleRangeSpeed = 5.0f;

        public float FluorescentFlickerMin = 0.4f;
        public float FluorescentFlickerMax = 0.5f;
        public float FluorescentFlicerPercent = 0.95f;

        public new Light light;
        // ------------------------
        void Start()
        {
            light = GetComponent<Light>();
        }
        // Update is called once per frame
        void Update()
        {

            switch (FlickeringLightStyle)
            {
                // If Flickering Style is Campfire
                case FlickerinLightStyles.CampFire:

                    // If campfire method is Intesity OR Both
                    if (CampfireMethod == CampfireMethods.Intensity || CampfireMethod == CampfireMethods.Both)
                    {
                        // If Intensity style is Sine
                        if (CampfireIntesityStyle == CampfireIntesityStyles.Sine)
                        {
                            // Cycle the Campfire angle
                            _campfireSineCycleIntensity += CampfireSineCycleIntensitySpeed;
                            if (_campfireSineCycleIntensity > 360.0f) _campfireSineCycleIntensity = 0.0f;

                            // Base + Values
                            light.intensity = CampfireIntensityBaseValue + ((Mathf.Sin(_campfireSineCycleIntensity * Mathf.Deg2Rad) * (CampfireIntensityFlickerValue / 2.0f)) + (CampfireIntensityFlickerValue / 2.0f));
                        }
                        else light.intensity = CampfireIntensityBaseValue + Random.Range(0.0f, CampfireIntensityFlickerValue);
                    }

                    // If campfire method is Range OR Both
                    if (CampfireMethod == CampfireMethods.Range || CampfireMethod == CampfireMethods.Both)
                    {
                        // If Range style is Sine
                        if (CampfireRangeStyle == CampfireRangeStyles.Sine)
                        {
                            // Cycle the Campfire angle
                            _campfireSineCycleRange += CampfireSineCycleRangeSpeed;
                            if (_campfireSineCycleRange > 360.0f) _campfireSineCycleRange = 0.0f;

                            // Base + Values
                            light.range = CampfireRangeBaseValue + ((Mathf.Sin(_campfireSineCycleRange * Mathf.Deg2Rad) * (_campfireSineCycleRange / 2.0f)) + (_campfireSineCycleRange / 2.0f));
                        }
                        else light.range = CampfireRangeBaseValue + Random.Range(0.0f, CampfireRangeFlickerValue);
                    }
                    break;

                // If Flickering Style is Fluorescent
                case FlickerinLightStyles.Fluorescent:
                    if (Random.Range(0.0f, 1.0f) > FluorescentFlicerPercent)
                    {
                        light.intensity = FluorescentFlickerMin;
                    }
                    else light.intensity = FluorescentFlickerMax;
                    break;

                default:
                    break;
            }
        }
    }
}
