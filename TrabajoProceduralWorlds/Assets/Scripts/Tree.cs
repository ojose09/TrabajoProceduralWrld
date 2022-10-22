using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private const int IndexOfSquareChild = 0;
    private const int IndexOfCircleChild = 1;

    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private int totalLevels = 3;
    [SerializeField] private float initialSize = 5f;
    [SerializeField, Range(0, 1)] private float reductionPerLevel = 0.1f;

    private int currentLevel = 1;
    private Queue<GameObject> rootBranchesQueue = new Queue<GameObject>();

    private void Start()
    {
        GameObject rootBranch = Instantiate(branchPrefab, transform);
        ChangeBranchSize(rootBranch, initialSize);
        rootBranchesQueue.Enqueue(rootBranch);
        GenerateTree();
    }

    private void Update()
    {

    }
    private void GenerateTree()
    {
        if (currentLevel >= totalLevels)
        {
            return;
        }
        ++currentLevel;
        Debug.Log(currentLevel);

        float newSize = Mathf.Max(initialSize - initialSize * reductionPerLevel * (currentLevel - 1), 0.1f);
        var branchesCreatedThisCycle = new List<GameObject>();

        while (rootBranchesQueue.Count > 0)
        {
            var rootBranch = rootBranchesQueue.Dequeue();

            var leftBranch = CreateBranch(rootBranch, Random.Range(5f, 25f));
            var rightBranch = CreateBranch(rootBranch, -Random.Range(5f, 25f));

            ChangeBranchSize(leftBranch, newSize);
            ChangeBranchSize(rightBranch, newSize);

            branchesCreatedThisCycle.Add(leftBranch);
            branchesCreatedThisCycle.Add(rightBranch);
        }
        foreach (var newBranches in branchesCreatedThisCycle)
        {
            rootBranchesQueue.Enqueue(newBranches);
        }

        GenerateTree();
    }
    private GameObject CreateBranch(GameObject prevBranch, float relativeAngle)
    {
        GameObject newBranch = Instantiate(branchPrefab, transform);
        newBranch.transform.localPosition = prevBranch.transform.localPosition + prevBranch.transform.up * GetBranchLength(prevBranch);
        newBranch.transform.localRotation = prevBranch.transform.localRotation * Quaternion.Euler(0, 0, relativeAngle);

        return newBranch;
    }

    private void ChangeBranchSize(GameObject branchInstance, float newSize)
    {
        var square = branchInstance.transform.GetChild(IndexOfSquareChild);
        var circle = branchInstance.transform.GetChild(IndexOfCircleChild);

        //update the scale of the square
        var newScale = square.transform.localScale;
        newScale.y = newSize;
        square.transform.localScale = newScale;

        //update the position of the square
        var newPosition = square.transform.localPosition;
        newPosition.y = newSize / 2;
        square.transform.localPosition = newPosition;

        //update the position of the circle
        var newCirclePosition = circle.transform.localPosition;
        newCirclePosition.y = newSize;
        circle.transform.localPosition = newCirclePosition;
    }

    private float GetBranchLength(GameObject branchInstance)
    {
        return branchInstance.transform.GetChild(IndexOfSquareChild).localScale.y;
    }
}