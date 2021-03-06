﻿using System.Collections.Generic;
using UnityEngine;

public static class DensityNpressure {

	public static void Compute_massDensity(List<Vector3> l_pos, List<float> l_density, List<List<int>> l_neighbors)
    {

        for (int i = 0; i < l_pos.Count; i++)

            foreach (int j in l_neighbors[i])
            {
                Vector3 vd = l_pos[i] - l_pos[j];
                l_density[i] = FluidProperties.mass * Kernels.Standard_kernel_value(ref vd);
            }

    }

    public static void Compute_Pressure(List<float> l_density, List<float> l_pressure, float ref_density, float fluid_stiffness)
    {
        for (int i = 0; i < l_density.Count; i++)
            l_pressure[i] = fluid_stiffness * (l_density[i] - ref_density);
    }

}
