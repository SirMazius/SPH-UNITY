using UnityEngine;

/*
    Aqui figuraran todas las propiedades del fluido
*/
public static class FluidProperties {

    public static float density; //<
    public static float mass;//<
    public static float buoyancy_diffusion; //< Gasses only
    public static float viscosity; //<
    public static float surfaceTension_factor; //<
    public static float surfaceTension_threshold; //<
    public static float gass_stiffness; //<
    public static float restitution; //<
    public static float support_radius;  //<
    public static float fluid_volume; //<
    public static float time_delta;

    public static int n_particles; //<
    public static int kernel_particles; //<

    public static void Initialize_fluid_properties(int _n_particles, int _kernel_particles, float _time_delta)
    {
        Build_water();

        n_particles = _n_particles;
        fluid_volume = n_particles * mass / density;
        kernel_particles = _kernel_particles;
        support_radius = Mathf.Pow((3 * fluid_volume * kernel_particles) / (4 * Mathf.PI * n_particles), 1f / 3f);
        surfaceTension_threshold = Mathf.Sqrt(density / kernel_particles);
        time_delta = _time_delta;
    }

    public static void Build_water()
    {
        density = 998.29f;
        mass = 0.02f;
        buoyancy_diffusion = 0f;
        viscosity = 3.5f;
        surfaceTension_factor = 0.0728f;

        gass_stiffness = 3f;
        restitution = 0f;
    }
}
