using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script referenced from https://www.youtube.com/watch?v=3Dw5d7PlcTM&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=4
    public class Heap<T> where T : IHeapItem<T>
    {
        T[] items;
        int currentItemCount;

        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        //Adds a node to the heap
        public void Add(T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        //Removes and returns the first node in the heap
        public T RemoveFirst()
        {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        //Changes the priority of a node in case there is a new path
        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public int Count { get { return currentItemCount; } }

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        //Sorts the placement of a new node
        void SortUp(T item)
        {
            //Calculate the parent of the node
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }

                else
                {
                    break;
                }
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }

        //Sorts the children of the selected node
        void SortDown(T item)
        {
            while (true)
            {
                //Calculate the child nodes 
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount)
                    {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }

                    }

                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }

                    else
                    {
                        return;
                    }
                }

                else
                {
                    return;
                }
            }
        }

        //Swap positions between two items 
        void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }

