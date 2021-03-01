using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class population : MonoBehaviour
{
    // Start is called before the first frame update[Header("References")]
    public enemycontroller enemy;

    [Header("Controls")]
    public int initialPopulation ;
    [Range(0.0f, 1.0f)]
    public float mutationprobabilty = 0.055f;

    [Header("Crossover Controls")]
    public int bestAgentSelection = 0;
    public int worstAgentSelection = 2;
    public int numberToCrossover;

    private List<int> genePool = new List<int>();

    private int naturallySelected;

    public nn[] numpopulation;
    
    [System.Serializable] 
    public class storeddata{
        public int lastgeneration;
        public nn[] lastgenoms;
        public int bestagentselection;
        public storeddata(nn[] numpp,int crntgen,int bestAgentSelection)
        {
            int n=numpp.Length;
            lastgenoms=new nn[n];
            for(int i=0;i<n;i++)
            {
                lastgenoms[i]=numpp[i];
            }
            lastgeneration=crntgen;
            bestagentselection=bestAgentSelection;
        }

    }

    storeddata data;

    [Header("Public View")]
    public int currentGeneration;
    public int currentGenome = 0;
    bool fileexist=false;

    void OnEnable()
    {
        LoadData();
        CreatePopulation();
    }

    void OnDisable()
    {
        SaveData();
    }
    private void Start()
    {
        
    }

    private void CreatePopulation()
    {
        numpopulation = new nn[initialPopulation];
        initialFillPopulationWithRandomValues(numpopulation, 0);
        ResetToCurrentGenome();
    }

    private void ResetToCurrentGenome()
    {
        enemy.ResetWithNetwork(numpopulation[currentGenome]);
    }

    private void initialFillPopulationWithRandomValues (nn[] newPopulation, int startingIndex)
    {
        if(fileexist)
        {
            int n=data.lastgenoms.Length;
            for(int i=0;i<n;i++)
            {
                newPopulation[i]= data.lastgenoms[i];
            }
            currentGeneration=data.lastgeneration;
            bestAgentSelection=0;
            currentGenome=0;
            fileexist=false;
        }
        else
        {
            while (startingIndex < initialPopulation)
            {
                newPopulation[startingIndex] = new nn();
                newPopulation[startingIndex].Initialise(enemy.hiddenLayer, enemy.hiddenneurons);
                startingIndex++;
            }
            currentGenome=0;
        }
    }

    private void FillPopulationWithRandomValues (nn[] newPopulation, int startingIndex)
    {
       
        while (startingIndex < initialPopulation)
        {
            newPopulation[startingIndex] = new nn();
            newPopulation[startingIndex].Initialise(enemy.hiddenLayer, enemy.hiddenneurons);
            startingIndex++;
        }
        
    }
    public void Death (float fitness, nn network,float maxfitness)
    {

        if (currentGenome < numpopulation.Length -1)
        {
            if(fitness>maxfitness)
            {
                bestAgentSelection++;
            }
            numpopulation[currentGenome].fitness = fitness;
            currentGenome++;
            ResetToCurrentGenome();
        }
        else
        {
            RePopulate();
            //bestAgentSelection=0;
            
        }

    }
     private void RePopulate()
    {
        genePool.Clear();
        currentGeneration++;
        naturallySelected = 0;
        SortPopulation();

        nn[] newPopulation = PickBestPopulation();

        Crossover(newPopulation);
        Mutate(newPopulation);

        FillPopulationWithRandomValues(newPopulation, naturallySelected);

        numpopulation = newPopulation;

        currentGenome = bestAgentSelection;

        ResetToCurrentGenome();

    }
    private void Mutate (nn[] newPopulation)
    {

        for (int i = 0; i < naturallySelected; i++)
        {

            for (int c = 0; c < newPopulation[i].weights.Count; c++)
            {

                if (Random.Range(0.0f, 1.0f) < mutationprobabilty)
                {
                    newPopulation[i].weights[c] = MutateMatrix(newPopulation[i].weights[c]);
                }

            }

        }

    }
     Matrix<float> MutateMatrix (Matrix<float> A)
    {

        int randomPoints = Random.Range(1, (A.RowCount * A.ColumnCount) / 3);

        Matrix<float> C = A;

        for (int i = 0; i < randomPoints; i++)
        {
            int randomColumn = Random.Range(0, C.ColumnCount);
            int randomRow = Random.Range(0, C.RowCount);

            C[randomRow, randomColumn] = Mathf.Clamp(C[randomRow, randomColumn] + Random.Range(-1f, 1f), -1f, 1f);
        }

        return C;

    }
     private void Crossover (nn[] newPopulation)
    {
        for (int i = 0; i < numberToCrossover; i+=2)
        {
            int AIndex = i;
            int BIndex = i + 1;

            if (genePool.Count >= 1)
            {
                for (int l = 0; l < 100; l++)
                {
                    AIndex = genePool[Random.Range(0, genePool.Count)];
                    BIndex = genePool[Random.Range(0, genePool.Count)];

                    if (AIndex != BIndex)
                        break;
                }
            }

            nn Child1 = new nn();
            nn Child2 = new nn();

            Child1.Initialise(enemy.hiddenLayer, enemy.hiddenneurons);
            Child2.Initialise(enemy.hiddenLayer, enemy.hiddenneurons);

            Child1.fitness = 0;
            Child2.fitness = 0;


            for (int w = 0; w < Child1.weights.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    Child1.weights[w] = numpopulation[AIndex].weights[w];
                    Child2.weights[w] = numpopulation[BIndex].weights[w];
                }
                else
                {
                    Child2.weights[w] = numpopulation[AIndex].weights[w];
                    Child1.weights[w] = numpopulation[BIndex].weights[w];
                }

            }


            for (int w = 0; w < Child1.biases.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    Child1.biases[w] = numpopulation[AIndex].biases[w];
                    Child2.biases[w] = numpopulation[BIndex].biases[w];
                }
                else
                {
                    Child2.biases[w] = numpopulation[AIndex].biases[w];
                    Child1.biases[w] = numpopulation[BIndex].biases[w];
                }

            }

            newPopulation[naturallySelected] = Child1;
            naturallySelected++;

            newPopulation[naturallySelected] = Child2;
            naturallySelected++;

        }
    }
    private nn[] PickBestPopulation()
    {

        nn[] newPopulation = new nn[initialPopulation];

        for (int i = 0; i < bestAgentSelection; i++)
        {
            newPopulation[naturallySelected] = numpopulation[i];
            newPopulation[naturallySelected].fitness = 0;
            naturallySelected++;
            
            int f = Mathf.RoundToInt(numpopulation[i].fitness /10);

            for (int c = 0; c < f; c++)
            {
                genePool.Add(i);
            }

        }
        int last = numpopulation.Length - 1;
        for (int i = 0; i < worstAgentSelection; i++)
        {
            
            last -= i;

            int f = Mathf.RoundToInt(numpopulation[last].fitness /10);

            for (int c = 0; c < f; c++)
            {
                genePool.Add(last);
            }

        }

        return newPopulation;

    }
    private void SortPopulation()
    {
        for (int i = 0; i < numpopulation.Length; i++)
        {
            for (int j = i; j < numpopulation.Length; j++)
            {
                if (numpopulation[i].fitness < numpopulation[j].fitness)
                {
                    nn temp = numpopulation[i];
                    numpopulation[i] = numpopulation[j];
                    numpopulation[j] = temp;
                }
            }
        }

    }
    void LoadData()
    {
        string path=Application.persistentDataPath+"/playerAI.txt";
        if(File.Exists(path))
        {
            fileexist=true;
            BinaryFormatter formatter=new BinaryFormatter();
            FileStream stream=new FileStream (path,FileMode.Open);
            data=formatter.Deserialize(stream) as storeddata;
            stream.Close();
            
            
            
        }
        
        
    }

    void SaveData()
    {
        BinaryFormatter formatter=new BinaryFormatter();
        string path=Application.persistentDataPath+"/playerAI.txt";
        FileStream stream;
        if(File.Exists(path))
        {
            stream=new FileStream(path,FileMode.Open);
        }
        else
        {
            stream=new FileStream(path,FileMode.Create);
        }
        storeddata data=new storeddata(numpopulation,currentGeneration,bestAgentSelection);
        formatter.Serialize(stream,data);
        stream.Close();
    }
}
