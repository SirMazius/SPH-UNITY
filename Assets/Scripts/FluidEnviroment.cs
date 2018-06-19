using System.Collections.Generic;
using UnityEngine;

public class FluidEnviroment : MonoBehaviour
{

    public GameObject fluid_particle;
    public Vector3 sphere_center;
    public float sphere_radius;
    public int xAxis_particles, yAxis_particles, zAxis_particles;
    public int kernel_particles;

    public List<GameObject> l_spheres;
    private List<Vector3> l_pos;
    private List<List<int>> l_neighbors;
    private List<float> l_density;
    private List<float> l_pressure;

    // Use this for initialization
    private void Start()
    {
        int number_of_particles = xAxis_particles * yAxis_particles * zAxis_particles;


        FluidProperties.Initialize_fluid_properties(number_of_particles, kernel_particles, Time.fixedDeltaTime);

        Initialize_lists();

        Kernels.Init_kernel(FluidProperties.support_radius);
        HashTable.Initialize();

        Integrator.Initialize_integrator();



        Build_fluidCube();

        sphere_center = Get_center();
        HashTable.Insert(l_pos);
        HashTable.Search_neighbors(l_pos, l_neighbors);
    }

    // private void FixedUpdate()
    // {
    //     HashTable.Insert(l_pos);
    //     HashTable.Search_neighbors(l_pos, l_neighbors);
    //     //DensityNpressure.Compute_massDensity(l_pos, l_density, l_neighbors);
    // }

    private void Build_fluidCube()
    {

        //Debug.Log(FluidProperties.support_radius);
        Vector3 pos = new Vector3();
        for (int i = 0; i < xAxis_particles; i++)
            for (int j = 0; j < yAxis_particles; j++)
                for (int k = 0; k < zAxis_particles; k++)
                {
                    pos.Set(FluidProperties.support_radius * i + 5, FluidProperties.support_radius * j + 5, FluidProperties.support_radius * k + 5);
                    GameObject gO = Instantiate(fluid_particle, pos, Quaternion.identity);
                    l_spheres.Add(gO);
                    l_pos.Add(gO.transform.position); 
                }
    }

    private Vector3 Get_center()
    {
        Vector3 auxPos = new Vector3();
        foreach (Vector3 pos in l_pos)
            auxPos += pos;

        return auxPos / FluidProperties.n_particles;
    }

    private void Check_collision()
    {
        Vector3 vd = new Vector3();
        int n = FluidProperties.n_particles;
        for (int i = 0; i < n; i++)
        {
            vd = l_pos[i] - sphere_center;
            if (vd.magnitude > sphere_radius)
                l_pos[i] = vd.normalized * sphere_radius;
        }
    }

    private void Initialize_lists()
    {
        l_pos = new List<Vector3>();
        l_density = new List<float>();
        l_pressure = new List<float>();
        l_neighbors = new List<List<int>>();
        l_spheres = new List<GameObject>();

        for (int i = 0; i < FluidProperties.n_particles; i++)
        {
            //Debug.Log(i);
            l_neighbors.Add(new List<int>());
            l_density.Add(0f);
            l_pressure.Add(0f);
        }


    }

}
