using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AreaCalc.Tools;

namespace AreaCalc.Commands
{
    [Transaction(TransactionMode.Manual)]
    class ModelLineToRoomSeparationLine : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;
			Document doc = activeUIDocument.Document;
			Selection selection = activeUIDocument.Selection;

			IList<Reference> pickedObjs = selection.PickObjects(ObjectType.Element, "Select Model lines");
			List<ElementId> ids = (from Reference r in pickedObjs select r.ElementId).ToList();
			FilteredElementCollector basePointsActive =new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_SketchLines);
			

			using (Transaction t = new Transaction(doc))
			{
				
				t.Start("transaction");
				if (pickedObjs != null && pickedObjs.Count > 0)
				{



					foreach (ElementId id in ids)
					{
						Element e = doc.GetElement(id);
						ModelCurve l = e as ModelCurve;
						LocationCurve loc = l.Location as LocationCurve;
						
						XYZ stPoint = new XYZ(loc.Curve.GetEndPoint(0).X,loc.Curve.GetEndPoint(0).Y,loc.Curve.GetEndPoint(0).Z); // Arch Model to Active Model Coordinates (= Transform Attribute)
						XYZ enPoint = new XYZ(loc.Curve.GetEndPoint(1).X,loc.Curve.GetEndPoint(1).Y,loc.Curve.GetEndPoint(1).Z );




						Level level = doc.ActiveView.GenLevel;

						Line line = Line.CreateBound(stPoint, enPoint);
						CurveArray cArray = new CurveArray();
						//cArray.Append(line);

						cArray.Append(loc.Curve);

						SketchPlane skP = SketchPlane.Create(doc, level.Id);
						doc.Create.NewRoomBoundaryLines(skP, cArray, doc.ActiveView);


					}


				}
				t.Commit();
			}

				return Result.Succeeded;
        }

		

	}
}
