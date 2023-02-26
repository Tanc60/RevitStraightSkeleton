using AreaCalc.Tools;
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
using CGAL.Wrapper;
using System.Numerics;


namespace AreaCalc.Commands
{

    [Transaction(TransactionMode.Manual)]
    internal class StraightSkeleton : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;
            Document doc = activeUIDocument.Document;
            Selection selection = activeUIDocument.Selection;

            IList<Reference> pickedObjs = selection.PickObjects(ObjectType.Element, "Select Model lines");
            List<ElementId> ids = (from Reference r in pickedObjs select r.ElementId).ToList();
            FilteredElementCollector basePointsActive = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_SketchLines);
            selection.Dispose();
            using (Transaction t = new Transaction(doc))
            {
                t.Start("transaction");
                if (pickedObjs != null && pickedObjs.Count > 0)
                {
                    HashSet<KeyValuePair<XYZ, XYZ>> edgePairs = new HashSet<KeyValuePair<XYZ, XYZ>>();

                    foreach (ElementId id in ids)
                    {
                        Element e = doc.GetElement(id);
                        ModelCurve l = e as ModelCurve;
                        LocationCurve loc = l.Location as LocationCurve;

                        XYZ stPoint = new XYZ(loc.Curve.GetEndPoint(0).X, loc.Curve.GetEndPoint(0).Y, loc.Curve.GetEndPoint(0).Z); // Arch Model to Active Model Coordinates (= Transform Attribute)
                        XYZ enPoint = new XYZ(loc.Curve.GetEndPoint(1).X, loc.Curve.GetEndPoint(1).Y, loc.Curve.GetEndPoint(1).Z);

                        //conversion
                        KeyValuePair<XYZ, XYZ> edge = new KeyValuePair<XYZ, XYZ>(stPoint, enPoint);
                        edgePairs.Add(edge);

 

                    }

                    //extract the z value
                    double z = edgePairs.First().Key.Z;

                    //conversion
                    var polygons = CommonFunction.GetMultiPolygonsFromRevit(edgePairs);

                    
                    //get the outer polygon and inside holes
                    var vec2_polygons = CommonFunction.ConvertXYZtoVector2List(polygons);
                    //outers
                    var outer = vec2_polygons[CommonFunction.GetOuterPolygon(vec2_polygons)];
                    //holes
                    vec2_polygons.RemoveAt(CommonFunction.GetOuterPolygon(vec2_polygons));

                    //CGAL wrapper
                    var ssk = CGAL.Wrapper.StraightSkeleton.Generate(outer, vec2_polygons);



                    Level level = doc.ActiveView.GenLevel;
                    SketchPlane skP = SketchPlane.Create(doc, level.Id);

                    foreach (var edge in ssk.Skeleton)
                    {
                        var stPoint = new XYZ(edge.Start.Position.X, edge.Start.Position.Y, z);
                        var enPoint = new XYZ(edge.End.Position.X, edge.End.Position.Y, z);

                        if (!stPoint.IsAlmostEqualTo(enPoint,0.0001))
                        {
                            Line line = Line.CreateBound(stPoint, enPoint);
                            CurveArray cArray = new CurveArray();
                            cArray.Append(line);
                            doc.Create.NewRoomBoundaryLines(skP, cArray, doc.ActiveView);
                        }


                    }
                    
                    string msg = "";
                    foreach (var polygon in polygons)
                    {
                        foreach (var pt in polygon)
                        {
                            msg += "(" + pt.X.ToString() + "," + pt.Y.ToString() + ")\n";
                        }
                        msg += "\n";
                    }                       
                    MessageBox.Show(msg, "Point List");
                    

                }
                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}