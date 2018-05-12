using System.Collections.Generic;
using UnityEngine;

public static class InternalForces
{

    public static void Compute_PressureForce(List<Vector3> l_pForce, List<float> l_density, List<float> l_pressure, List<Vector3> l_pos, List<List<int>> l_neighbors)
    {
        int n = FluidProperties.n_particles;
        Vector3 pForce = new Vector3();
        Vector3 vd = new Vector3();
        for (int i = 0; i < n; i++)
        {

            pForce.Set(0f, 0f, 0f);

            foreach (int j in l_neighbors[i])
            {
                if (i != j)
                {
                    vd = l_pos[i] - l_pos[j];
                    pForce += (l_pressure[i] + l_pressure[j]) / 2f * FluidProperties.mass / l_density[j] * Kernels.Spiky_kernel_gradient(ref vd);
                }
            }


            l_pForce[i] = -pForce;

        }

    }

    public static void Compute_ViscosityForce(List<Vector3> l_vForce, List<Vector3> l_pos, List<Vector3> l_velocity, List<float> l_density, float viscosity_coeficient, List<List<int>> l_neighbors)
    {
        int n = FluidProperties.n_particles;
        Vector3 vForce = new Vector3();
        Vector3 vd = new Vector3();
        for (int i = 0; i < n; i++)
        {
            vForce.Set(0f, 0f, 0f);
            float densityViscosity_factor = viscosity_coeficient / l_density[i];

            foreach (int j in l_neighbors[i])
            {
                vd = l_pos[i] - l_pos[j];
                vForce += (l_velocity[j] - l_velocity[i]) * FluidProperties.mass * Kernels.Viscosity_kernel_laplacian(ref vd);
            }

            l_vForce[i] = vForce * densityViscosity_factor / l_density[i];
        }
    }

}
