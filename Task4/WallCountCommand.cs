using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    [Transaction(TransactionMode.Manual)]
    public class WallCountCommand : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiApp = commandData.Application;
            Application app = uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            
            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .OfType<Wall>()
                .ToList();

            if (walls.Count == 0)
            {
                TaskDialog.Show("Внимание", "В проекте не найдено ни одной стены.");
                return Result.Cancelled;
            }

            double maxWallLength = 0;
            double minWallLength = UnitUtils.ConvertFromInternalUnits(walls[0].get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble(), UnitTypeId.Millimeters);
            double allWallLength = 0;
            int maxIndex =0; int minIndex = 0;

            for (int i = 0; i < walls.Count; i++)
            {
                var wall = walls[i];
                Parameter length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                double lengthMM = UnitUtils.ConvertFromInternalUnits(length.AsDouble(), UnitTypeId.Millimeters);
                if (lengthMM > maxWallLength)
                {
                    maxWallLength = lengthMM;
                    maxIndex = i;
                }
                if (lengthMM < minWallLength)
                {
                    minWallLength = lengthMM;
                    minIndex = i;
                }
                allWallLength += lengthMM;
            }

                       double avgWallLength = allWallLength / walls.Count;

            using (Transaction t = new Transaction(doc, "Запись максимального и минимального параметра"))
            {
                t.Start();

                var maxWall = walls[maxIndex];
                var minWall = walls[minIndex];

                var commentParam = maxWall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                if (commentParam != null && !commentParam.IsReadOnly)
                    commentParam.Set("Самая длинная стена");

                commentParam = minWall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                if (commentParam != null && !commentParam.IsReadOnly)
                    commentParam.Set("Самая короткая стена");
                                
                t.Commit();
            }

            TaskDialog.Show("Количество стен", $"Общее кол-во стен: {walls.Count}" +
                $"\nСамая большая длина стены: {maxWallLength:F1}мм" +
                 $"\nСамая короткая длина стены: {minWallLength:F1}мм" +
                  $"\nСредняя длина стен: {avgWallLength:F1}мм"
                );

            return Result.Succeeded;

        }
    }
}
