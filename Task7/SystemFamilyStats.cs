using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task7
{
    [Transaction(TransactionMode.Manual)]
    public class SystemFamilyStats : IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var el = doc.GetElement(uiDoc.Selection.PickObject(ObjectType.Element));

            if (el is FamilyInstance)
            {
                TaskDialog.Show(
                    "Ошибка",
                    "Выбранный элемент не является системным семейством!"
                );
                return Result.Cancelled;
            }

            Options options = new Options
            {

                DetailLevel = ViewDetailLevel.Fine,
                IncludeNonVisibleObjects = true

            };
            var geo = el.get_Geometry(options);
            List<Solid> solids = new List<Solid>();


            //var solids = el.get_Geometry(options)
            //.Where(g => g is Solid)
            //.OfType<Solid>()
            //.ToList();

            bool subGeo = false;

            foreach (GeometryObject obj in geo)
            {
                Solid solid = obj as Solid;
                if (solid != null && solid.Volume > 0)
                {
                    solids.Add(solid);
                    continue;
                }

                GeometryInstance instance = obj as GeometryInstance;
                if (instance != null)
                {
                    subGeo = true;
                    foreach (GeometryObject geometryObject in instance.SymbolGeometry)
                    {
                        Solid instSolid = geometryObject as Solid;
                        if (instSolid != null && instSolid.Volume > 0)
                        {
                            solids.Add(instSolid);
                        }
                    }
                }
            }

            if (subGeo)
            {
                TaskDialog.Show("Внимание!", "Выбранное системное семейство содержит вложенную геометрию!");
            }

            /*            var solids = geometryInstance
                    .GetInstanceGeometry()
                    // .GetSymbolGeometry()
                    .Where(g => g is Solid)
                    .OfType<Solid>()
                    .Where(g => g.Volume > 0.0001)
                    .ToList();*/
            double volume = 0;
            double area = 0;
            int faceCount = 0;

            int edgeCount = 0;
            double length = 0;

            foreach (var solid in solids)
            {
                volume += solid.Volume;
                area += solid.SurfaceArea;
                FaceArray faces = solid.Faces;
                faceCount += faces.Size;
                EdgeArray edges = solid.Edges;
                edgeCount += edges.Size;
                foreach (Edge edge in solid.Edges)
                {
                    Curve curve = edge.AsCurve();
                    length += curve.Length;

                }
            }

            double cubMetersVolume = UnitUtils.ConvertFromInternalUnits(volume, UnitTypeId.CubicMeters);
            double squareMetersArea = UnitUtils.ConvertFromInternalUnits(area, UnitTypeId.SquareMeters);
            double metersLength = UnitUtils.ConvertFromInternalUnits(length, UnitTypeId.Meters);

            string text =
                $"Количество солидов: {solids.Count}шт.\n" +
                $"Объём солидов: {cubMetersVolume:F3}м³\n" +
                $"Площадь элемента: {squareMetersArea:F3}м²\n" +
                $"Количество граней: {faceCount}шт.\n" +
                $"Количество рёбер: {edgeCount}шт.\n" +
                $"Длина всех рёбер: {metersLength:F2}м";


            TaskDialog.Show("Информация", text);

            return Result.Succeeded;
        }
    }
}
