using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Text;

namespace Task5
{
    [Transaction(TransactionMode.Manual)]
    public class SelectionStats : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Application application = uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                IList<Autodesk.Revit.DB.Reference> pickedRefs = uiDoc.Selection.PickObjects(ObjectType.Element, new FamilyInstanceFilter(), "Выберите элементы");
                Dictionary<string, int> categories = new Dictionary<string, int>();
                List<FamilyInstance> instances = new List<FamilyInstance>();
                StringBuilder sb = new StringBuilder();
                foreach (var pickedRef in pickedRefs)
                {
                    Element element = doc.GetElement(pickedRef);

                    if (element is FamilyInstance)
                    {
                        instances.Add(element as FamilyInstance);
                        if (categories.ContainsKey(element.Category.Name))
                            categories[element.Category.Name]++;
                        else
                            categories.Add(element.Category.Name, 1);
                    }


                }
                foreach (var cat in categories)
                {
                    sb.Append($"{cat.Key}: {cat.Value}\n");
                }
                TaskDialog.Show("Инфо", $"Общее количество элементов: {instances.Count}\n{sb}");
            }
            catch
            {
                TaskDialog.Show("Внимание!", "Элементы не выбраны");
            }
            return Result.Succeeded;
        }
    }
}
