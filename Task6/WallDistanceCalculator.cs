using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6
{
    [Transaction(TransactionMode.Manual)]
    public class CommandClass : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            IList<Reference> elems = null;

            try
            {
                elems = uiDoc.Selection.PickObjects(ObjectType.Element, "Выберите 2 стены");

            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {

                return Result.Cancelled;
            }

            var selectedIds = elems.Select(r => r.ElementId).ToList();

            if (selectedIds.Count != 2)
            {
                TaskDialog.Show("Ошибка", "Выберите 2 стены");
                return Result.Failed;
            }

            Wall wall1 = uiDoc.Document.GetElement(selectedIds.First()) as Wall;
            Wall wall2 = uiDoc.Document.GetElement(selectedIds.Last()) as Wall;

            if (wall1 == null || wall2 == null)
            {
                TaskDialog.Show("Ошибка", "Выберите именно стены!");
                return Result.Failed;
            }

            XYZ direction1 = GetWallNormal(wall1);
            XYZ direction2 = GetWallNormal(wall2);

            if (!AreVectorParallel(direction1, direction2))
            {
                TaskDialog.Show("Результат", "Стены не параллельны!");
                return Result.Failed;
            }

            XYZ point1 = GetMiddleOfWall(wall1);
            XYZ point2 = GetMiddleOfWall(wall2);

            var vectorBetweenWall = point2 - point1;
            XYZ normVector = vectorBetweenWall.Normalize();

            double result = vectorBetweenWall.DotProduct(normVector);
            TaskDialog.Show("Итог", $"Расстояние между стенами {UnitUtils.ConvertFromInternalUnits(result, UnitTypeId.Millimeters):F2} мм");

            return Result.Succeeded;

        }

        public XYZ GetWallNormal(Wall wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            Curve curve = location.Curve;
            XYZ wallDirection = (curve.GetEndPoint(1) - curve.GetEndPoint(0)).Normalize();
            XYZ up = XYZ.BasisZ;

            return wallDirection.CrossProduct(up).Normalize();
        }

        public bool AreVectorParallel(XYZ a, XYZ b, double tolerance = 1e-10)
        {
            double dotProduct = Math.Abs(a.DotProduct(b));
            return Math.Abs(dotProduct - 1.0) < tolerance;
        }

        public XYZ GetMiddleOfWall(Wall wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            Curve curve = location.Curve;
            XYZ middleOfWall = (curve.GetEndPoint(1) + curve.GetEndPoint(0)) / 2;
            return middleOfWall;
        }


    }
}
