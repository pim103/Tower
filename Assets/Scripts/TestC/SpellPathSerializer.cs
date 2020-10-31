﻿using System;
using System.IO;
using FullSerializer;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;

namespace TestC
{
    public class SpellPathSerializer : MonoBehaviour
    {
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private PathCreator emptyPathCreator;

        private string folderName = "SpellPathTemp";

        private void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

            TryToExportVertexPath();
        }

        private void TryToExportVertexPath()
        {
            Debug.Log("EXPORT PATH");

            fsSerializer serializer = new fsSerializer();
            serializer.TrySerialize(pathCreator.path.GetType(), pathCreator.path, out fsData data);
            File.WriteAllText(Application.dataPath + "/Data/" + folderName + "/vertexPath.json", fsJsonPrinter.CompressedJson(data));
        }

        private void TryToImportVertexPath()
        {
            Debug.Log("IMPORT PATH");
            
            string jsonPath = File.ReadAllText(Application.dataPath + "/Data/" + folderName + "/vertexPath.json");
            
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(jsonPath);

            VertexPath vertexPath = null;
            serializer.TryDeserialize(data, ref vertexPath);

            emptyPathCreator.path = vertexPath;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TryToImportVertexPath();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                TryToExportVertexPath();
            }
        }

        private void PrintPathData()
        {
            VertexPath vertexPath = pathCreator.path;
            
            Debug.Log(vertexPath.length);
        }
        
        void OnPathChanged()
        {
//            PrintPathData();
        }
    }
}