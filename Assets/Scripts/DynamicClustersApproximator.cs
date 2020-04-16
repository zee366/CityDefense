using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool _verbose;

    private readonly object _lock = new object();
    private          Task   _task;

    private SmallWorld<float[], float> _graph;
    private HashSet<Transform>         _dataSources = new HashSet<Transform>();


    public void Start() {
        // Add all current as being tracked
        foreach ( Transform child in transform )
            RegisterTransform(child);

        // Starting building of graph interval loop
        StartCoroutine(GraphRebuildingLoop());
    }


    public Vector3 GetClosestApproximatedClusterPosition(Vector3 from) {
        // Can't query if not built yet
        if ( _graph == null ) return from;

        float[] queryPos = {from.x, from.y, from.z};

        IList<SmallWorld<float[], float>.KNNSearchResult> k_closest;
        lock ( _lock ) {
            k_closest = _graph.KNNSearch(queryPos, N_CLOSEST_FOR_CLUSTER);
        }

        // Averaging results positions
        Vector3 avg = Vector3.zero;
        for ( int i = 0; i < k_closest.Count; i++ )
            avg += new Vector3(k_closest[i].Item[0], k_closest[i].Item[1], k_closest[i].Item[2]);

        return avg / k_closest.Count;
    }


    /// <summary>
    /// Add the transform reference to the data set for graph building.
    /// </summary>
    /// <param name="transform"></param>
    public void RegisterTransform(Transform transform) {
        lock ( _lock ) {
            _dataSources.Add(transform);
        }
    }


    private IEnumerator GraphRebuildingLoop() {
        while ( true ) {
            // Convert positions to float arrays
            List<float[]> posAsVectors = new List<float[]>(_dataSources.Count);
            lock ( _lock ) {
                foreach ( Transform t in _dataSources )
                    posAsVectors.Add(new[] {t.position.x, t.position.y, t.position.z});
            }

            // Start rebuilding in a thread with all data
            _task = Task.Factory.StartNew(() => RebuildGraph(posAsVectors));

            yield return new WaitUntil(() => _task.IsCompleted);
        }
    }


    /// <summary>
    /// Expensive call, should be called only when necessary.
    /// Using HNSW graph search tool as in this article:
    /// https: //arxiv.org/ftp/arxiv/papers/1603/1603.09320.pdf
    /// </summary>
    private void RebuildGraph(List<float[]> vectors) {
        if ( _verbose )
            Debug.Log("Clusters: Rebuilding graph...");

        // Setting parameters that makes sens for the number or rioters (Might adjust)
        int numNeighbours = 15;
        var parameters = new SmallWorld<float[], float>.Parameters() {
            M           = numNeighbours,               // Max number of neighbours to connect with at each layer
            LevelLambda = 1 / Math.Log(numNeighbours), // Layer/Level logarithmic decrease factor
        };

        // Using CosineDistance calculation for approximation & speed only,
        // no need for precise Euclidean Distance calculation.
        SmallWorld<float[], float> graph = new SmallWorld<float[], float>(CosineDistance.NonOptimized);
        graph.BuildGraph(vectors, new System.Random(11), parameters);

        // Assigning new computed graph
        lock ( _lock ) {
            _graph = graph;
        }

        if ( _verbose )
            Debug.Log("Clusters: Done rebuilding graph.");
    }

}