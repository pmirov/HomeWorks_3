using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Task5
{
    internal class FamilyInstanceFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is FamilyInstance;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
