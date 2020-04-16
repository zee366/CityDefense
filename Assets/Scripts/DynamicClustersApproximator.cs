using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HNSW.Net;
using UnityEngine;

/// <summary>
/// Keeps childs positions in memory,
/// update the Hierarchical Navigable Small World Graphs (HNSW) at each interval.
/// Give access to realtime, high level cluster information and query.
/// </summary>
public class DynamicClustersApproximator : MonoBehaviour {

    private const int N_CLOSEST_FOR_CLUSTER = 13; // Value for the KNN-search. Finding N closest to have a cluster approximation.

    [SerializeField]
    private float _clusterGraphUpdateInterval = 4f;

    [SerializeField]
    private bool _verbose;

    private CancellationTokenSource _taskToken;
    private Task                    _task;

    private SmallWorld<float[], float> _graph;
    private HashSet<Transform>         _dataSources = new HashSet<Transform>();


    public void Start() {
        // Add all current as being tracked
        foreach ( Transform child in transform )
            RegisterTransform(child);

        _taskToken = new CancellationTokenSource();
        _task = Task.Run(async () => // <- marked async
                         {
                             while ( true ) {
                                 RebuildGraph();
                                 await Task.Delay(100, _taskToken.Token); // <- await with cancellation
                             }
                         },
                         _taskToken.Token);


        // Graph building and updating thread
        Task taskA = new Task(TestTask);
        taskA.Start();

        StartCoroutine(RebuildGraphRoutine());
    }


    private void TestTask() {
        while ( true ) {
            // Setting parameters that makes sens for the number or rioters (Might adjust)
            int numNeighbours = 15;
            var parameters = new SmallWorld<float[], float>.Parameters() {
                M           = numNeighbours,               // Max number of neighbours to connect with at each layer
                LevelLambda = 1 / Math.Log(numNeighbours), // Layer/Level logarithmic decrease factor
            };

            // Convert positions to float arrays
            List<float[]> posAsVectors = new List<float[]>(_dataSources.Count);
            foreach ( Transform t in _dataSources )
                posAsVectors.Add(new[] {t.position.x, t.position.y, t.position.z});

            // Using CosineDistance calculation for approximation & speed only,
            // no need for precise Euclidean Distance calculation.
            SmallWorld<float[], float> graph = new SmallWorld<float[], float>(CosineDistance.NonOptimized);
            graph.BuildGraph(posAsVectors, new System.Random(11), parameters);
        }
    }


    /// <summary>
    /// Add the transform reference to the data set for graph building.
    /// </summary>
    /// <param name="transform"></param>
    public void RegisterTransform(Transform transform) {
        lock ( _dataSources ) {
            _dataSources.Add(transform);
        }
    }


    // public IEnumerator RebuildGraphRoutine() {
    //     while ( true ) {
    //         // TODO: Consider having a separate thread to compute graph
    //         // RebuildGraph();
    //
    //         yield return new WaitForSeconds(_clusterGraphUpdateInterval);
    //     }
    // }


    /// <summary>
    /// Expensive call, should be called only when necessary.
    /// Using HNSW graph search tool as in this article:
    /// https: //arxiv.org/ftp/arxiv/papers/1603/1603.09320.pdf
    /// </summary>
    private void RebuildGraph() {
        if ( _verbose )
            Debug.Log("Clusters: Rebuilding graph...");

        // Setting parameters that makes sens for the number or rioters (Might adjust)
        int numNeighbours = 15;
        var parameters = new SmallWorld<float[], float>.Parameters() {
            M           = numNeighbours,               // Max number of neighbours to connect with at each layer
            LevelLambda = 1 / Math.Log(numNeighbours), // Layer/Level logarithmic decrease factor
        };


        // Convert positions to float arrays
        List<float[]> posAsVectors = new List<float[]>(_dataSources.Count);
        lock ( _dataSources ) {
            foreach ( Transform t in _dataSources )
                posAsVectors.Add(new[] {t.position.x, t.position.y, t.position.z});
        }

        // Using CosineDistance calculation for approximation & speed only,
        // no need for precise Euclidean Distance calculation.
        SmallWorld<float[], float> graph = new SmallWorld<float[], float>(CosineDistance.NonOptimized);
        graph.BuildGraph(posAsVectors, new System.Random(11), parameters);

        // Assigning new computed graph
        lock ( _graph ) {
            _graph = graph;
        }

        if ( _verbose )
            Debug.Log("Clusters: Done rebuilding graph.");
    }


    private void OnDestroy() { _taskToken.Cancel(); }

}