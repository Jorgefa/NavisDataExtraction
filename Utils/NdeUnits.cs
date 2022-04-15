using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.NavisUtils
{
    class NdeUnits
    {
        public static float ConvertUnitsToMeters(float dim)
        {
            // Get current active document.
            Document doc = Application.ActiveDocument;

            // Get units of the document
            Units units = doc.Units;

            // Return converted value to meters
            switch (units)
            {
                case (Units.Centimeters): return dim*0.01f;
                case (Units.Feet): return dim * 0.3048f;
                case (Units.Inches): return dim * 0.0254f;
                case (Units.Kilometers): return dim * 1000f;
                case (Units.Meters): return dim * 1f;
                case (Units.Microinches): return dim * 0.0000000254f;
                case (Units.Micrometers): return dim * 0.000001f;
                case (Units.Miles): return dim * 1609.43f;
                case (Units.Millimeters): return dim * 0.001f;
                case (Units.Mils): return dim * 0.0000254f;
                case (Units.Yards): return dim * 0.9144f;
                default: return dim * 1;
            }
        }
    }
}
