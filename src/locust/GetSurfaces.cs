﻿using System;
using System.Collections.Generic;
using compute.geometry;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Newtonsoft.Json;
using Grasshopper.Kernel.Types;

namespace locust
{
    public class GetSurfaces : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GetSurfaces()
          : base("surfaces", "Get Surfaces",
              "Get Surface Geometry from compute.rhino3d",
              "Locust", "Get")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("serialized", "serialized", "response from the server", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surfaces", "Surfaces", "Deserialized Surfaces", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string input = string.Empty;
            DA.GetData<string>(0, ref input);

            List<Surface> Surfaces = new List<Surface>();

            GrasshopperOutput objectList = JsonConvert.DeserializeObject<GrasshopperOutput>(input);
            List<GrasshopperOutputItem> items = objectList.Items;
            if (items != null)
            {
                foreach (GrasshopperOutputItem output in items)
                {
                    switch (output.TypeHint)
                    {
                        case "surface":
                            Surface surface = JsonConvert.DeserializeObject<Surface>(output.Data);
                            Surfaces.Add(surface); break;
                    }
                }
            }
            DA.SetDataList(0, Surfaces);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("95126A7F-DB08-46FC-B7EC-56F17F64DDF6"); }
        }
    }
}