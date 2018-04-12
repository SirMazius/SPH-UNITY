using System.Collections.Generic;
using UnityEngine;

public static class HashTable
{
    static List<int>[] base_array;

    const int prime1 = 73856093;
    const int prime2 = 19349663;
    const int prime3 = 83492791;

    static Vector3 bbMin, bbMax;
    static Vector3 r = new Vector3();
    static Vector3 v3h = new Vector3();
    static int size;

    static float l = FluidProperties.support_radius;
    public static void Initialize()
    {
        size = Next_prime(FluidProperties.n_particles * 2);

        base_array = new List<int>[size];

        for (int i = 0; i < size; i++)
            base_array[i] = new List<int>();
    }

    public static void Clean_table()
    {
        for (int i = 0; i < size; i++)
            base_array[i].Clear();
    }

    public static void Insert(List<Vector3> l_pos)
    {
        for (int i = 0; i < l_pos.Count; i++)
        {
            int index = ((int)(l_pos[i].x / l * prime1) ^ (int)(l_pos[i].y / l * prime2) ^ (int)(l_pos[i].z / l * prime3)) % size;
            base_array[index].Add(i);
        }
    }

	public static void Search_neighbors(List<Vector3> l_pos, List<List<int>> l_neighbors, int index)
    {
        r.x = l_pos[index].x / l * prime1;
        r.y = l_pos[index].y / l * prime2;
        r.z = l_pos[index].z / l * prime3;

        v3h.Set(FluidProperties.support_radius, FluidProperties.support_radius, FluidProperties.support_radius);

        bbMin.x = (r.x * (l_pos[index].x - v3h.x));
        bbMin.y = (r.y * (l_pos[index].y - v3h.y));
        bbMin.z = (r.z * (l_pos[index].z - v3h.z));

        bbMax.x = (r.x * (l_pos[index].x + v3h.x));
        bbMax.y = (r.y * (l_pos[index].y + v3h.y));
        bbMax.z = (r.z * (l_pos[index].z + v3h.z));

		//int count = 0;

        for (int i = (int)bbMin.x; i < (int)bbMax.x; i++)
        {
            for (int j = (int)bbMin.y; j < (int)bbMax.y; j++)
            {
                for (int k = (int)bbMin.z; k < (int)bbMax.z; k++)
                {
                    int index2 = ((int)(l_pos[i].x / l * prime1) ^ (int)(l_pos[i].y / l * prime2) ^ (int)(l_pos[i].z / l * prime3)) % size;
					l_neighbors.Add(base_array[index2]);
					/* Esto permite borrar elementos para evitar la comprobacion del radio en los kernels
					int lenght = l_neighbors[count].Count;
                    for (int position = 0; position < lenght; position++)
                    {
						if (Vector3.Magnitude(l_pos[index] - l_pos[l_neighbors[count][position]]) > FluidProperties.support_radius)
                        {
                            l_neighbors[count].RemoveAt(position); //< RemoveAt es caro de narices una cola seria mejor probablemente
                            lenght--;
                            position--;
                        }
                    }
					count++;
					*/
                }
            }
        }

    }



    private static int Next_prime(int number)
    {
        bool found = false;
        int prime = number;

        while (!found)
        {
            bool searching = true;
            prime++;

            if (prime == 2 || prime == 3)
            {
                found = true;
                return prime;
            }

            if (prime % 2 == 0 || prime % 3 == 0)
            {
                found = false;
                searching = false;
            }


            int divisor = 6;

            while (searching && divisor * divisor - 2 * divisor + 1 <= prime)
            {
                if (prime % (divisor - 1) == 0 || prime % (divisor + 1) == 0)
                {
                    found = false;
                    searching = false;
                }


                divisor += 6;
            }

            if (searching)
            {
                found = true;
            }

        }

        return prime;
    }
}
